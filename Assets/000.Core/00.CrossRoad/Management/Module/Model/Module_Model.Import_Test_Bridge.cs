using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using View;
	using System.Linq;
	using Management;
	using TMPro;
	using UnityEngine.UI;
	using Legacy.UI;
    using Platform.Bridge;

    /// <summary>
    /// 템플릿
    /// </summary>
    public partial class Module_Model : AModule
	{
		[Header("Bridge")]
		[SerializeField] GameObject m_bRootObj;
		[SerializeField] GameObject m_bRoot3D;
        [SerializeField] GameObject m_bRoot2D;
		[SerializeField] GameObject[] m_bridgeTopParts;
		[SerializeField] GameObject[] m_bridgeBottomParts;

		public void InitializeObjectBridge(GameObject _root)
		{
			m_bRootObj = _root.transform.GetChild(0).gameObject;
			m_bRoot3D = null;
            m_bRoot2D = null;

			int index = m_bRootObj.transform.childCount;
			for (int i = 0; i < index; i++)
			{
				string name = m_bRootObj.transform.GetChild(i).name;
				switch (name)
				{
					case "RA":
					case "RCT":
					case "PSCI":
					case "RCS":
						m_bRoot3D = m_bRootObj.transform.GetChild(i).gameObject;
						break;

					default:
                        m_bRoot2D = m_bRootObj.transform.GetChild(i).gameObject;
						break;
				}
			}

			// 치수선 삭제
			//Destroy(dim);
            Initialize2DObject(m_bRoot2D);

			StartCoroutine(Initialize3DObject(m_bRoot3D));

			
		}

        bool routineCheck3D;


        private IEnumerator Initialize3DObject(GameObject _root3DObject)
		{
			//manager.RootObject.transform.parent.position = new Vector3(0, 0, 0);

			// 지역 리스트 초기화
			List<GameObject> bridgeTopParts = new List<GameObject>();
			List<GameObject> bridgeBottomParts = new List<GameObject>();


			// 지역 리스트에 요소 할당
			int index = _root3DObject.transform.childCount;
			for (int i = 0; i < index; i++)
			{
				// todo 0222
				if (_root3DObject.transform.GetChild(i).name.Substring(0, 2) == "SP")
				{
					bridgeTopParts.Add(_root3DObject.transform.GetChild(i).gameObject);
				}
				else if (_root3DObject.transform.GetChild(i).name.Substring(0, 2) == "AP")
				{
					bridgeBottomParts.Add(_root3DObject.transform.GetChild(i).gameObject);
				}
			}

			//// 리스트 전역 배열 변환
			m_bridgeTopParts = (from obj in bridgeTopParts orderby obj.name select obj).ToArray<GameObject>();
			m_bridgeBottomParts = (from obj in bridgeBottomParts orderby obj.name select obj).ToArray<GameObject>();

			//// 다음 메서드 실행
			SetObjectsMaterialNCollider(_root3DObject);

			yield return new WaitForEndOfFrame();

            routineCheck3D = true;
            InitializeCheck();

			//ContentManager.Instance.SetCameraCenterPosition();

			//ContentManager.Instance.CompCheck(4);

			yield break;
		}

		private void SetObjectsMaterialNCollider(GameObject _root3DObject)
		{
            Materials.Init(MaterialType.Default);

			#region 변수 선언부
			// 3D 객체 아래의 모든 자식객체 수집
			Transform[] objTransforms = _root3DObject.gameObject.GetComponentsInChildren<Transform>();

			// 자식객체 중에 조건에 맞는 (material을 가진 객체의 조건) 객체 수집
			Transform[] selected_objTransforms = (from obj in objTransforms where obj.name.Split(',').Length > 1 select obj).ToArray<Transform>();

			// 선택 가능한 객체 모아둠
			ModelObjects = new List<GameObject>();

			Dictionary<string, int> multiSplitIndex = new Dictionary<string, int>();
            #endregion

            #region 모든 객체의 material, collider를 할당한다.

            GraphicCode gCode = MainManager.Instance.Graphic;
            BridgeCode bCode = BridgeCode.Null;
            BridgeCodeDetail bCodeDetail = BridgeCodeDetail.Null;


			// Material을 할당해야 하는 객체에 Material, Collider 할당
			// [반복] material을 가진 모든 자식객체
			int index1 = selected_objTransforms.Length;
			for (int i = 0; i < index1; i++)
			{
				// 현재 index 객체의 문자열 인자값 할당
				string[] splitObjectString = selected_objTransforms[i].name.Split(',');

				string name = splitObjectString[0];
                bCode = Bridges.GetPartCode(name);
                bCodeDetail = Bridges.GetPartCodeDetail(name);

                //Debug.Log($"Tag:: {name}");
                //Debug.Log($"Tag:: {bCode}");    // 다 잘뜸
                //Debug.Log($"Tag:: {bCodeDetail}");
                //Debug.Log($"Tag:: {name.Split('_').Length}");   // 모두 4

                // [조건] 분할 문자열이 2개 : (객체명 문자열 1, material 정보 1) 객체가 한 개의 Material을 필요로 하는 경우
                if (splitObjectString.Length == 2)
				{
                    //----------
                    Bridge_Materials.Set(selected_objTransforms[i].GetComponent<MeshRenderer>(), gCode, selected_objTransforms[i].name);
                    //Bridge_Materials.Set(selected_objTransforms[i].GetComponent<MeshRenderer>(), gCode, bCode, bCodeDetail);
                    //selected_objTransforms[i].GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Default);

                    selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
					selected_objTransforms[i].gameObject.AddComponent<Obj_Selectable>();
					ModelObjects.Add(selected_objTransforms[i].gameObject);
				}
				// [조건] 분할 문자열이 2개 이상: (객체명 문자열 1, material 정보 n) 객체가 여러 개의 Material을 필요로 하는 경우
				else
				{
					// Dictionary에 확인
					int stringIndex = 0;

					// [조건] Dictionary에 객체명 키값 검색시 키가 있는 경우 : 첫 번째 객체가 아닌 경우
					if (multiSplitIndex.TryGetValue(splitObjectString[0], out stringIndex).Equals(true))
					{
						multiSplitIndex[splitObjectString[0]]++;

                        //----------
                        Bridge_Materials.Set(selected_objTransforms[i].GetComponent<MeshRenderer>(), gCode, selected_objTransforms[i].name);
                        //Bridge_Materials.Set(selected_objTransforms[i].GetComponent<MeshRenderer>(), gCode, bCode, bCodeDetail);
                        //selected_objTransforms[i].GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Default);

                        selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
						selected_objTransforms[i].gameObject.AddComponent<Obj_Selectable>();
						ModelObjects.Add(selected_objTransforms[i].gameObject);
					}
					// 해당 키값이 없으면 첫 번째 검색된 객체임
					else
					{
						// Dictionary 삭제 (의도치 않은 오류가 발생해서 Dictionary 키를 한번씩 날려버리는 코드 작성)
						// 교각의 받침 02 03 라인이 2번째 교각에 있을때
						// 교각의 받침 03 04 라인이 3번째 교각에 있다. 이때 03 라인의 이름이 중복되는 문제 발생
						multiSplitIndex.Clear();

						// Dictionary에 새 키 할당
						multiSplitIndex.Add(splitObjectString[0], stringIndex);
						multiSplitIndex[splitObjectString[0]]++;

                        //----------
                        Bridge_Materials.Set(selected_objTransforms[i].GetComponent<MeshRenderer>(), gCode, selected_objTransforms[i].name);
                        //Bridge_Materials.Set(selected_objTransforms[i].GetComponent<MeshRenderer>(), gCode, bCode, bCodeDetail);
                        //selected_objTransforms[i].GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Default);

                        selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
						selected_objTransforms[i].gameObject.AddComponent<Obj_Selectable>();
						ModelObjects.Add(selected_objTransforms[i].gameObject);
					}
				}

				// 객체에 Meshrenderer가 존재할 경우 Meshrenderer 관리 리스트에 Meshrenderer 할당
				//MeshRenderers.Add(selected_objTransforms[i].GetComponent<MeshRenderer>());
			}

			multiSplitIndex.Clear();

			#endregion


		}

        private void InitializeCheck()
        {
            bool result =
                routineCheck2D &&
                routineCheck2D_11 && routineCheck2D_12 && routineCheck2D_13 &&
                routineCheck3D;

            if (result)
            {
                ContentManager.Instance.SetCameraCenterPosition();

                ContentManager.Instance.CompCheck(4);

                ContentManager.Instance.Container.m_dimView.Initial2DSet(m_bRootObj);
                    //DimViewManager.Instance.Initial2DSet(_2DObject: (GameObject)args[0]);
                //MaterialChange();
                //MainManager.Instance.Request(new Request(Type.DimView, RequestCode.DimView_3DCall), RootObject);
                //MainManager.Instance.Request(new Request(Type.DimView, RequestCode.DimView_2DCall), RootObject);

                //RuntimeData.RootContainer.Instance.isObjectRoutineEnd = true;
                //MainManager.Instance.InitializeRoutineCheck();
            }
        }

        #region 2D initialize

        bool routineCheck2D;
        bool routineCheck2D_11;
        bool routineCheck2D_12;
        bool routineCheck2D_13;

        GameObject dimObj;
        Transform[] dimTransforms;
        Transform[] mainLineTransforms;
        Transform[] subLineTransforms;

        float offset = 5f;

        private async void Initialize2DObject(GameObject root2D)
        {
            //yield return null;

            routineCheck2D    = false;
            routineCheck2D_11 = false;
            routineCheck2D_12 = false;
            routineCheck2D_13 = false;

            StartCoroutine(SetObject_Dimension(root2D));

            routineCheck2D = true;
            InitializeCheck();

            //yield break;
        }

        /// <summary>
        /// 1. 치수선 배열 검출
        /// </summary>
        /// <param name="root2D"></param>
        /// <returns></returns>
        private IEnumerator SetObject_Dimension(GameObject root2D)
        {
            yield return null;

            dimObj = root2D.transform.GetChild(0).gameObject;

            dimTransforms = dimObj.transform.GetComponentsInChildren<Transform>();

            // 1. 치수선 객체 배열 정렬
            Transform[] dimensionLineTransforms = (from obj in dimTransforms where obj.name.Contains("Line") select obj)
                .ToArray<Transform>(); // 모든 치수선들을 받아오는 배열
            mainLineTransforms = (from obj in dimensionLineTransforms where obj.name.Contains("Main") select obj)
                .ToArray<Transform>(); // 주 치수선 배열 정렬
            subLineTransforms = (from obj in dimensionLineTransforms where obj.name.Contains("sub") select obj)
                .ToArray<Transform>(); // 부 치수선 배열 정렬

            StartCoroutine(SetObject_DimensionLines(dimensionLineTransforms));


            // 2. 모든 외곽선값 표시객체 배열 정렬
            Transform[] outlineTransforms = (from obj in dimTransforms where obj.name.Split('_').Length > 3 select obj)
                .ToArray<Transform>();

            StartCoroutine(SetObject_OutLines(outlineTransforms));

            // 3. 모든 치수선값 표시 객체 배열 정렬
            Transform[] textMeshTransforms =
                (from obj in dimTransforms
                 where obj.name.Contains("Main") &&
                       obj.name.Split('_').Length == 3
                 select obj).ToArray<Transform>();

            StartCoroutine(SetObject_TextMeshes(textMeshTransforms));

            yield break;
        }

        /// <summary>
        /// 1-1. 치수선 material 할당
        /// </summary>
        /// <param name="dimLines"></param>
        /// <returns></returns>
        private IEnumerator SetObject_DimensionLines(Transform[] dimLines)
        {
            yield return null;
            //Task.Delay(1);6

            int index = dimLines.Length;
            MeshRenderer renderer;

            for (int i = 0; i < index; i++)
            {
                if (dimLines[i].TryGetComponent<MeshRenderer>(out renderer))
                {
                    renderer.material = MainManager.Instance.DimLineMat;
                }
            }

            routineCheck2D_11 = true;
            InitializeCheck();

            yield break;
        }

        /// <summary>
        /// 1-2. 외곽선 material 할당
        /// </summary>
        /// <param name="outLines"></param>
        /// <returns></returns>
        private IEnumerator SetObject_OutLines(Transform[] outLines)
        {
            yield return null;

            int _index = outLines.Length;
            MeshRenderer render;

            for (int i = 0; i < _index; i++)
            {
                if(outLines[i].TryGetComponent<MeshRenderer>(out render))
				{
                    outLines[i].GetComponent<MeshRenderer>().material = MainManager.Instance.OutlineMat;
				}
            }

            routineCheck2D_12 = true;
            InitializeCheck();

            yield break;
        }

        /// <summary>
        /// 1-3. TextMesh 할당
        /// </summary>
        /// <param name="textMeshes"></param>
        /// <returns></returns>
        private IEnumerator SetObject_TextMeshes(Transform[] textMeshes)
        {
            yield return null;

            int _index = 0;

            TextMeshPro tmPro;
            string[] arguments;
            GameObject textInstance = new GameObject("textInstance");
            textInstance.AddComponent<TextMeshPro>();
            ContentSizeFitter fit = textInstance.AddComponent<ContentSizeFitter>();
            fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            List<GameObject> texts = new List<GameObject>();

            _index = textMeshes.Length;
            for (int i = 0; i < _index; i++)
            {
                arguments = textMeshes[i].name.Split('_');

                textInstance.transform.position = textMeshes[i].position; // text 기본위치 설정
                textInstance.transform.eulerAngles = GetTextMeshRotation(textMeshes[i]);

                // tmPro 설정단계
                tmPro = textInstance.GetComponent<TextMeshPro>();
                tmPro.text = string.Format("{0:N0}", int.Parse(arguments[1]));
                tmPro.fontSize = 2;
                tmPro.fontStyle = FontStyles.Bold;
                tmPro.alignment = TextAlignmentOptions.Center;
                tmPro.color = new Color(1f, 0.8759048f, 0, 1);

                GameObject tempObj = Instantiate(textInstance, textInstance.transform.position,
                    textInstance.transform.rotation);
                tempObj.transform.SetParent(textMeshes[i]); // text 객체 SetParent
                tempObj.transform.localPosition = new Vector3(0, 0, 0); // text 0, 0, 0 정렬
                tempObj.transform.localPosition = GetTextMeshPosition(textMeshes[i]); // text 위치 정렬

                texts.Add(tempObj);

                //yield return new WaitForEndOfFrame();

                //RectTransform _rect = tempObj.GetComponent<RectTransform>();

                //BoxCollider coll = tempObj.AddComponent<BoxCollider>();
                //coll.center = new Vector3(0, 0, 0);
                //coll.size = new Vector3(_rect.rect.width, _rect.rect.height, 0.1f);

                //_rect.
                //if (i % 100 == 0)
                //{
                //    yield return null;
                //}
            }

            yield return new WaitForEndOfFrame();

            _index = texts.Count;
            for (int i = 0; i < _index; i++)
            {
                RectTransform _rect = texts[i].GetComponent<RectTransform>();

                OffsetFitter offsetFitter = texts[i].AddComponent<OffsetFitter>();
                offsetFitter.Init(texts[i].transform.localPosition);

                BoxCollider coll = texts[i].AddComponent<BoxCollider>();
                coll.center = new Vector3(0, 0, 0);
                coll.size = new Vector3(_rect.rect.width+0.1f, _rect.rect.height, 0.1f);
                coll.isTrigger = true;

                Rigidbody rigid = texts[i].AddComponent<Rigidbody>();
                rigid.useGravity = false;

                //texts[i].transform.localPosition; // offset용
            }

            // 할당이 끝난 textInstance 삭제
            Destroy(textInstance, 0.1f);

            routineCheck2D_13 = true;
            InitializeCheck();

            yield break;
        }

        private Vector3 GetTextMeshRotation(Transform target)
        {
            string parentName = target.parent.name.Substring(0, 2);
            string textPosition = target.name.Split('_')[2];
            string partName = target.parent.parent.name.Substring(0, 6);

            switch (parentName)
            {
                case "TO":
                    return new Vector3(90, -90, 180);

                case "BO":
                    return new Vector3(-90, 180, -90);

                case "FR":
                    return new Vector3(0, 180, 0);

                case "BA":
                    return new Vector3(0, 0, 0);

                case "RE":
                    switch (textPosition)
                    {
                        case "Top":
                        case "Bottom":
                            return new Vector3(0, -90, 0);

                        case "Left":
                        case "Right":
                            return new Vector3(180, 90, 180);
                    }

                    return new Vector3(0, -90, 0);

                case "LE":
                    return new Vector3(0, 90, 0);
            }

            return new Vector3(0, 0, 0);
        }

        private Vector3 GetTextMeshPosition(Transform target)
        {
            string parentName = target.parent.name.Substring(0, 2);
            string textPosition = target.name.Split('_')[2];

            switch (parentName)
            {
                case "TO":
                    {
                        switch (textPosition)
                        {
                            case "Top":
                                return new Vector3(0, 0, -offset);

                            case "Bottom":
                                return new Vector3(0, 0, offset);

                            case "Left":
                                return new Vector3(offset, 0, 0);

                            case "Right":
                                return new Vector3(-offset, 0, 0);
                        }
                    }
                    break;

                case "BO":
                    {
                        switch (textPosition)
                        {
                            case "Top":
                                return new Vector3(0, 0, offset);

                            case "Bottom":
                                return new Vector3(0, 0, -offset);

                            case "Left":
                                return new Vector3(offset, 0, 0);

                            case "Right":
                                return new Vector3(-offset, 0, 0);
                        }
                    }
                    break;

                case "FR":
                    {
                        switch (textPosition)
                        {
                            case "Top":
                                return new Vector3(0, offset, 0);

                            case "Bottom":
                                return new Vector3(0, -offset, 0);

                            case "Left":
                                return new Vector3(-offset, 0, 0);

                            case "Right":
                                return new Vector3(offset, 0, 0);
                        }
                    }
                    break;

                case "BA":
                    {
                        switch (textPosition)
                        {
                            case "Top":
                                return new Vector3(0, offset, 0);

                            case "Bottom":
                                return new Vector3(0, -offset, 0);

                            case "Left":
                                return new Vector3(offset, 0, 0);

                            case "Right":
                                return new Vector3(-offset, 0, 0);
                        }
                    }
                    break;

                case "LE":
                    {
                        switch (textPosition)
                        {
                            case "Top":
                                return new Vector3(0, offset, 0);

                            case "Bottom":
                                return new Vector3(0, -offset, 0);

                            case "Left":
                                return new Vector3(0, 0, -offset);

                            case "Right":
                                return new Vector3(0, 0, offset);
                        }
                    }
                    break;

                case "RE":
                    {
                        switch (textPosition)
                        {
                            case "Top":
                                return new Vector3(0, offset, 0);

                            case "Bottom":
                                return new Vector3(0, -offset, 0);

                            case "Left":
                                return new Vector3(0, 0, offset);

                            case "Right":
                                return new Vector3(0, 0, -offset);
                        }
                    }
                    break;
            }

            return new Vector3(0, 0, 0);
        }

        private Vector3 GetTextColliderSize(Transform target)
        {
            Vector3 result = default(Vector3);

            string parentName = target.parent.name.Substring(0, 2);
            string textPosition = target.name.Split('_')[2];

            RectTransform rect = target.GetComponent<RectTransform>();
            float width = rect.rect.width;
            float height = rect.rect.height;

            switch (parentName)
            {
                case "TO":
                    {
                        switch (textPosition)
                        {
                            case "Top":
                                return new Vector3(0, 0, -offset);

                            case "Bottom":
                                return new Vector3(0, 0, offset);

                            case "Left":
                                return new Vector3(offset, 0, 0);

                            case "Right":
                                return new Vector3(-offset, 0, 0);
                        }
                    }
                    break;

                case "BO":

                    break;

                case "FR":

                    break;

                case "BA":

                    break;

                case "LE":

                    break;

                case "RE":

                    break;
            }



            return result;
        }

        #endregion
    }
}
