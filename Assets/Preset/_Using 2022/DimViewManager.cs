//using MGS.UCamera;
using Management;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UniJSON;
using UnityEngine;

namespace Legacy.Items
{
    public class DimViewManager : MonoBehaviour
    {
        public enum parseCode
        {
            TO,
            BO,
            FR,
            BA,
            LE,
            RE
        }

        public enum ViewRotations
        {
            //ALL,
            Top,
            Bottom,
            Front,
            Back,
            Left,
            Right
        }

        #region Singleton

        private static DimViewManager instance;

        public static DimViewManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<DimViewManager>() as DimViewManager;
                }
                return instance;
            }
        }

        #endregion

        #region 변수 선언부

        public ViewRotations viewRotations;

        public Transform objLevel1;
        public List<Transform> objLevel2;
        public List<Transform> objLevel3;

        public Transform dimLevel1;
        public Transform dimLevel2;
        public List<Transform> dimLevel3;
        public List<Transform> dimLevel4;
        public List<Transform> dimLevel5;

        public Vector3 selectBound;
        public Transform targetCenter;

        //public AroundAlignCamera aroundAlignCam;

        //2D 모드 카메라 조작을 위한 변수 선언
        //public AroundCamera aroundCamera;
        //public AroundCamera viewCamera;
        public bool selectViewMode = false;

        #endregion

        #region 임시로 만든 LateUpdate() 

        //private void LateUpdate()
        //{
        //    if(selectObj.Length == 1 && selectObj.ElementAt(0) != null)
        //    {
        //        ViewSet(selectObj[0]);
        //        DimSet(selectObj[0], viewRotations);
        //    }
        //    else if(selectObj.Length == 0)
        //    {
        //        ViewClear();
        //        DimClear();
        //        dimShow = false;
        //    }
        //}

        #endregion

        #region initialSet : BridgeManager.cs 작업 완료 후 넘겨받은 bridgetype으로 교량 Level 구조를 단계별로 리스트화
        public void Initial3DSet(GameObject _3DObject)
        {
            objLevel1 = GameObject.Find(_3DObject.transform.GetChild(0).name).transform;
            //3D 모델 리스트화
            for (int level2Count = 0; level2Count < objLevel1.childCount; level2Count++)
            {
                objLevel2.Add(objLevel1.GetChild(level2Count).transform);
                for (int level3Count = 0; level3Count < objLevel2[level2Count].childCount; level3Count++)
                {
                    objLevel3.Add(objLevel2[level2Count].GetChild(level3Count).transform);
                }
            }

            #region (임시)3D모드에서 연석 MeshRenderer 처리
            //부재 mesh renderer off
            //if (mainManager.WebManager.IsRegisterMode == false)
            //{
            //    foreach(Transform item in objLevel3)
            //    {
            //        if(item.name.Contains("CL,") || item.name.Contains("CR,"))
            //        {
            //            item.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //        }
            //    }
            //}
            //else if (mainManager.WebManager.IsRegisterMode != false)
            //{
            //    foreach (Transform item in objLevel3)
            //    {
            //        if (item.name.Contains("CL,") || item.name.Contains("CR,"))
            //        {
            //            item.gameObject.GetComponent<MeshRenderer>().enabled = true;
            //        }
            //    }
            //}

            //GRRenderObjs mesh renderer off
            //if (mainManager.WebManager.IsRegisterMode == false)
            //{
            //    foreach (Transform item in objLevel2)
            //    {
            //        if (item.name.Contains("GRRenderObjs"))
            //        {
            //            for (int cnt = 0; cnt < item.childCount; cnt++)
            //            {
            //                item.GetChild(cnt).GetComponent<MeshRenderer>().enabled = false;
            //            }
            //        }
            //    }
            //}
            //else if (mainManager.WebManager.IsRegisterMode != false)
            //{
            //    foreach (Transform item in objLevel2)
            //    {
            //        if (item.name.Contains("GRRenderObjs"))
            //        {
            //            for (int cnt = 0; cnt < item.childCount; cnt++)
            //            {
            //                item.GetChild(cnt).GetComponent<MeshRenderer>().enabled = true;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        public void Initial2DSet(GameObject _2DObject)
        {
            //치수선 리스트화
            dimLevel2 = GameObject.Find("DimObj").transform;
            dimLevel1 = dimLevel2.parent;
            for (int level3Count = 0; level3Count < dimLevel2.childCount; level3Count++)
            {
                dimLevel3.Add(dimLevel2.GetChild(level3Count).transform);
                for (int level4Count = 0; level4Count < dimLevel3[level3Count].childCount; level4Count++)
                {
                    dimLevel4.Add(dimLevel3[level3Count].GetChild(level4Count).transform);
                    for (int level5Count = 0; level5Count < dimLevel3[level3Count].GetChild(level4Count).childCount; level5Count++)
                    {
                        dimLevel5.Add(dimLevel3[level3Count].GetChild(level4Count).GetChild(level5Count).transform);
                    }
                }
            }

            SetLayerLevel5();

            dimLevel2.gameObject.SetActive(false);
        }
		#endregion

		#region Dim Level 5 Layer set

        private void SetLayerLevel5()
		{
            int index = dimLevel5.Count;
			for (int i = 0; i < index; i++)
			{
                string name = dimLevel5[i].name.Split('_')[0];

                Transform[] children = dimLevel5[i].GetComponentsInChildren<Transform>();

                int _index = children.Length;
				for (int ii = 0; ii < _index; ii++)
				{
                    children[ii].gameObject.layer = LayerMask.NameToLayer(name);
				}
			}

		}

		#endregion

		#region 임시로 만든 View 동작 버튼
		//public void Confirm()
		//{
		//    if (SelectCheck(selectObj[0]))
		//    {
		//        ViewSet(selectObj[0]);
		//        DimSet(selectObj[0], viewRotations);
		//    }
		//    else
		//        Debug.Log("오브젝트의 선택이 잘못되었습니다.");
		//}
		#endregion

		public void ViewSet(Transform selectObj)
        {
            //AroundCamera.cs의 selectViewMode 제어
            selectViewMode = true;
            //aroundCamera.selectViewMode = selectViewMode;
            //RenderSettings.skybox = Resources.Load("gradient_sky01") as Material;

            //3D 모델 3Level 비활성화
            for (int level3Count = 0; level3Count < objLevel3.Count; level3Count++)
            {
                objLevel3[level3Count].gameObject.SetActive(false);
            }

            //선택한 3D 모델만 활성화
            objLevel3.Find(x => x.name == selectObj.name).gameObject.SetActive(true);
        }

        public void ViewClear()
        {
            //AroundCamera.cs의 selectViewMode 제어
            selectViewMode = false;
            //aroundCamera.selectViewMode = selectViewMode;
            //RenderSettings.skybox = Resources.Load("gradient_sky02") as Material;


            for (int level3Count = 0; level3Count < objLevel3.Count; level3Count++)
            {
                objLevel3[level3Count].gameObject.SetActive(true);
            }
        }

        public void DimSet(Transform selectObj, ViewRotations viewRotations, bool dimShow)
        {
            try
            {
                dimLevel2.gameObject.SetActive(true);

                //치수선 5level 비활성화
                for (int level5Count = 0; level5Count < dimLevel5.Count; level5Count++)
                {
                    dimLevel5[level5Count].gameObject.SetActive(false);
                }

                parseCode pCode = parseCode.TO;
                switch (viewRotations)
                {
                    case ViewRotations.Top: pCode = parseCode.TO; break;
                    case ViewRotations.Bottom: pCode = parseCode.BO; break;
                    case ViewRotations.Front: pCode = parseCode.FR; break;
                    case ViewRotations.Back: pCode = parseCode.BA; break;
                    case ViewRotations.Left: pCode = parseCode.LE; break;
                    case ViewRotations.Right: pCode = parseCode.RE; break;
                }

                // dimlevel 5 Transform
                Transform[] targetTransforms =
                    (from obj in dimLevel5
                     where obj.parent.name.Contains(selectObj.name.Substring(0, 9)) &&
                     obj.name.Contains(pCode.ToString()) == true
                     select obj).ToArray<Transform>();

                targetTransforms[0].gameObject.SetActive(true);

                switch (viewRotations)
                {
                    case ViewRotations.Top: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Bottom: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Front: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Back: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Left: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Right: SelectObjFocus(selectObj, viewRotations); break;
                }
            }
            catch
            {
                switch (viewRotations)
                {
                    case ViewRotations.Top: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Bottom: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Front: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Back: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Left: SelectObjFocus(selectObj, viewRotations); break;
                    case ViewRotations.Right: SelectObjFocus(selectObj, viewRotations); break;
                }
            }

        }

        public void DimClear()
        {
            dimLevel2.gameObject.SetActive(false);
        }

        private static Bounds CalculateLocalBounds(Transform transform)
        {
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Bounds bounds = new Bounds(transform.position, Vector3.zero);
            foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }
            Vector3 localCenter = bounds.center - transform.position;
            bounds.center = localCenter;
            transform.rotation = currentRotation;
            return bounds;
        }

        public void SelectObjFocus(Transform selectObj, ViewRotations viewRotations)
        {
            Bounds _b = ContentManager.Instance._CenterBounds;

            targetCenter.position = _b.center;

            //switch (viewRotations)
            //{
            //    case ViewRotations.Top:
            //        aroundAlignCam.DirectionAngle(0);
            //        break;

            //    case ViewRotations.Bottom:
            //        aroundAlignCam.DirectionAngle(1);
            //        break;

            //    case ViewRotations.Front:
            //        aroundAlignCam.DirectionAngle(2);
            //        break;

            //    case ViewRotations.Back:
            //        aroundAlignCam.DirectionAngle(3);
            //        break;

            //    case ViewRotations.Left:
            //        aroundAlignCam.DirectionAngle(4);
            //        break;

            //    case ViewRotations.Right:
            //        aroundAlignCam.DirectionAngle(5);
            //        break;
            //}
        }
    }
}


