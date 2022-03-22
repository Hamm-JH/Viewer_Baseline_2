using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using _02.Scripts._OutlineTest;
using UnityEngine;
using UnityEngine.UI;
//using MODBS_Library;
using System.Threading.Tasks;
using MGS.UCamera;
using TMPro;
using UI;

namespace Manager
{
    public partial class ObjectManager : MonoBehaviour
    {
        #region Singleton

        private static ObjectManager instance;

        public static ObjectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ObjectManager>();
                    if (instance == null)
                    {
                        return null;
                    }

                    return instance;
                }

                return instance;
            }
        }

        #endregion

        #region 변수

        public GameObject RootObject;
        public GameObject Root3DObject;
        public GameObject Root2DObject;

        private bool routineCheck2D;
        private bool routineCheck2D_11;
        private bool routineCheck2D_12;
        private bool routineCheck2D_13;

        private bool routineCheck3D;
        private bool routineCheck3D_21;
        private bool routineCheck3D_23;
        private bool routineCheck3D_24;

        //===================================================

        [SerializeField] List<GameObject> bridgeTopParts;
        [SerializeField] List<GameObject> bridgeBottomParts;

        Transform[] objTransforms; // 3D 객체의 모든 Tansform들
        Transform[] selected_objTransforms; // 자식객체 중에 조건에 맞는 (material을 가진 객체의 조건) 객체 수집

        //===================================================

        [SerializeField] GameObject dimObj;
        private Transform SelectedDimTransform;

        Transform[] dimTransforms; // 2D 객체의 모든 Transform들
        Transform[] mainLineTransforms;
        Transform[] subLineTransforms;

        public DataTable historyTable;

        float offset = 14f;

        #endregion

        #region 속성

        public List<GameObject> BridgeTopParts
        {
            get => bridgeTopParts;
            set => bridgeTopParts = value;
        }

        public List<GameObject> BridgeBottomParts
        {
            get => bridgeBottomParts;
            set => bridgeBottomParts = value;
        }

        #endregion

        #region Set 3D object

        public void Set3DObject()
        {
            Manager.ViewSceneStatus sceneStatus = MainManager.Instance.SceneStatus;

            SetObjMaterial(sceneStatus);

            switch (sceneStatus)
            {
                case ViewSceneStatus.Ready:
                    {
                        GameObject _bridgeRootObj = null;
                        if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
						{
                            _bridgeRootObj = RuntimeData.RootContainer.Instance.Root3DObject;
						}
                        else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
						{
                            _bridgeRootObj = Data.Viewer.Cache.Instance.models.ModelName;
						}
                        else
						{
                            throw new Exception("App Type Error");
						}

                        BridgeRoot _bridgeRoot;
                        if(!_bridgeRootObj.TryGetComponent<BridgeRoot>(out _bridgeRoot))
                        {
                            _bridgeRootObj.AddComponent<BridgeRoot>();
                        }
                        _bridgeRoot = _bridgeRootObj.GetComponent<BridgeRoot>();
                        SetCamera(); // 카메라 설정
                        _bridgeRoot.SetIssueData();
                        SetDimLines(false);
                        ClearDimBox();
                        ClearTemp3DObj();
                        ClearIssueRecords();
                        ClearViewMaintainance();
                        IssueObjDefaultColor();

                        if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
						{
                            int index = selected_objTransforms.Length;
                            for (int i = 0; i < index; i++)
                            {
                                Collider _coll;
                                Bridge.ObjectData objData = selected_objTransforms[i].GetComponent<Bridge.ObjectData>();

                                objData.gameObject.layer = 0;
                                objData.GetComponent<MeshRenderer>().enabled = true;
                                objData.GetComponent<MeshRenderer>().material = objData.DefaultMaterial;

                                ////=========================

                                //// TODO : 1123
                                //selected_objTransforms[i].GetComponent<MeshRenderer>().material =
                                //    Resources.Load<UnityEngine.Material>($"Materials/Hololens/XRay");

                                ////=========================

                                if (objData.TryGetComponent<Collider>(out _coll))
                                {
                                    objData.GetComponent<Collider>().enabled = true;
                                }

                                objData.IsSelected = false;
                            }
						}
                        else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
						{
                            Debug.LogError("ObjectManager.cs // 1101 167");
						}
                    }
                    break;

                case ViewSceneStatus.ViewAllDamage:
                case ViewSceneStatus.ViewMaintainance:
                    {
                        // 3D 객체 모두 보이기
                        // - 3D 레이어 Default
                        // - 3D MeshRenderer On
                        // - 3D Collider On
                        // - Keymap 카메라 Off
                        SetDimLines(false);
                        ClearDimBox();
                        ClearTemp3DObj();
                        ClearIssueRecords();
                        ClearViewMaintainance();
                        IssueObjDefaultColor();
                        Manager.PositionManager.Instance.ClearIssueData(); // 손상/보수 아이콘 표시 비활성화
                        RuntimeData.RootContainer.Instance.dimensionView = true;
                        Manager.EventClassifier.Instance.SwitchDimensionView();

                        SetCamera(); // 카메라 설정
                        IssueManager.Instance.DisplayIssue();

                        

                        if (MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
						{
                            int index = selected_objTransforms.Length;
                            for (int i = 0; i < index; i++)
                            {
                                Collider _coll;
                                Bridge.ObjectData objData = selected_objTransforms[i].GetComponent<Bridge.ObjectData>();

                                objData.gameObject.layer = 0;
                                objData.GetComponent<MeshRenderer>().enabled = true;
                                objData.GetComponent<MeshRenderer>().material = objData.DefaultMaterial;

                                ////=========================

                                //// TODO : 1123
                                //selected_objTransforms[i].GetComponent<MeshRenderer>().material =
                                //    Resources.Load<UnityEngine.Material>($"Materials/Hololens/XRay");

                                ////=========================

                                if (objData.TryGetComponent<Collider>(out _coll))
                                {
                                    objData.GetComponent<Collider>().enabled = true;
                                }

                                objData.IsSelected = false;
                            }
						}
                        else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
						{
                            Debug.LogError("ObjectManager.cs // 1101 223");


						}

                        

                    }
                    break;

                case ViewSceneStatus.ViewPartDamage:
                case ViewSceneStatus.ViewPart2R:
                    {
                        // 3D 객체 변경
                        // - 3D 레이어 Keymap
                        // - 3D MeshRenderer On
                        // - 3D Collider On
                        // - Keymap 카메라 On
                        SetDimLines(false);
                        ClearDimBox();
                        ClearTemp3DObj();
                        ClearIssueRecords();
                        ClearViewMaintainance();
                        IssueObjDefaultColor();
                        Manager.PositionManager.Instance.ClearIssueData();

                        SetCamera(); // 카메라 설정
                        IssueManager.Instance.DisplayIssue();

                        SetObjMaterial(sceneStatus);

                        //Manager.JSONManager.Instance.LoadHistory(MainManager.Instance.BridgeCode,
                        //    $"&cdBridgeParts={RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name}");

                        if (MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
						{
							int index = selected_objTransforms.Length;
							for (int i = 0; i < index; i++)
							{
								Collider _coll;
								Bridge.ObjectData objData = selected_objTransforms[i].GetComponent<Bridge.ObjectData>();

								objData.gameObject.layer = 10; // layer keymap
								objData.GetComponent<MeshRenderer>().enabled = true;
								objData.GetComponent<MeshRenderer>().material = objData.DefaultMaterial;
								if (objData.TryGetComponent<Collider>(out _coll))
								{
									objData.GetComponent<Collider>().enabled = true;
								}
							}
						}
                        else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
						{
                            //Debug.LogError("ObjectManager.cs // 1101 270");

                            Data.Viewer.Cache.Instance.SetOn(10, mode: 1);
						}

                        GameObject tempObj = CreateTemporary3DObject(); // 임시객체 할당, 반환

                        if (tempObj != null)
                        {
                            CreateDimensionCube();
                        }
                    }
                    break;
            }

            
        }

        private float GetTargetDistance(Bounds _bound)
        {
            float maxValue = 0f;

            if (maxValue <= _bound.size.x)
            {
                maxValue = _bound.size.x;
            }
            if (maxValue <= _bound.size.y)
            {
                maxValue = _bound.size.y;
            }
            if (maxValue <= _bound.size.z)
            {
                maxValue = _bound.size.z;
            }

            return maxValue;
        }

        #region Camera

        private void SetCamera()
        {
            AroundAlignCamera mainAroundCam = RuntimeData.RootContainer.Instance.mainCam;
            AroundAlignCamera subAroundCam = RuntimeData.RootContainer.Instance.subCam;

            Camera mainCam = RuntimeData.RootContainer.Instance.mainCam.GetComponent<Camera>();
            Camera subCam = RuntimeData.RootContainer.Instance.subCam.GetComponent<Camera>();
            //UnityTemplateProjects.SimpleCameraController subFreeCam = subCam.GetComponent<UnityTemplateProjects.SimpleCameraController>();

            Transform _mainTarget = RuntimeData.RootContainer.Instance.mainCamTarget;
            Transform _subTarget = RuntimeData.RootContainer.Instance.subCamTarget;

            mainAroundCam.distanceRange = new Range(10, 1000);

            ViewSceneStatus sceneStatus = MainManager.Instance.SceneStatus;
            bool isRecursived = MainManager.Instance.isRecursived;

            // 서브카메라의 설정
            //SetSubCamera(_stat: sceneStatus, 
            //    orbitCam: subAroundCam, orbitTarget: _subTarget,
            //    freeCam: subFreeCam, camTarget: subCam, isStatRecursived: isRecursived);

            switch (sceneStatus)
            {
                case ViewSceneStatus.Ready:             // 상태 1
                case ViewSceneStatus.ViewAllDamage:     // 상태 2
                case ViewSceneStatus.ViewMaintainance:  // 상태 5
                    {
                        Bounds __bound;
                        if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
					    {
                            Bridge.ObjectData _obj = ObjectManager.Instance.Root3DObject.GetComponent<Bridge.ObjectData>();
                            __bound = _obj.Bound;

                            //_mainTarget.position = _obj.CenterBound;
                            _mainTarget.position = __bound.center;

                            // L3 카메라 루트위치 변경
                            RuntimeData.RootContainer.Instance.subCamL3.rootTransform.position = __bound.center;

                            float maxValue = 0f;
                            maxValue = __bound.size.x;

                            maxValue = Mathf.Max(Mathf.Max(__bound.size.x, __bound.size.y), __bound.size.z);

                            mainAroundCam.targetDistance = maxValue;
                            RuntimeData.RootContainer.Instance.subCamL3.camTransform.localPosition = new Vector3(0, 0, -maxValue);
					    }
                        else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
					    {
                            //__bound = Data.Viewer.Cache.Instance.models.rootBound;
                            __bound = MainManager.Instance.rootBounds;

                            _mainTarget.position = __bound.center;

                            // L3 카메라 루트위치 변경
                            RuntimeData.RootContainer.Instance.subCamL3.rootTransform.position = __bound.center;

                            //float maxValue = 0f;
                            //maxValue = __bound.size.x;

                            int segCount = Data.Viewer.Cache.Instance.models.Segments.Count;
                            float maxValue = Mathf.Max(Mathf.Max(__bound.size.x, __bound.size.y), __bound.size.z);

                            mainAroundCam.targetDistance = maxValue / 2;
                            RuntimeData.RootContainer.Instance.subCamL3.camTransform.localPosition = new Vector3(0, 0, -maxValue);
                        }

                        mainCam.cullingMask = -1;
                        mainAroundCam.isCanControl = true;
					}
                    break;

                case ViewSceneStatus.ViewPartDamage:    // 상태 3
                case ViewSceneStatus.ViewPart2R:        // 상태 4
                    {
                        if(RuntimeData.RootContainer.Instance.cached3DInstance != null)
					    {
                            if (RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform == null)
                            {
                                return;
                            }
                            if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
						    {
                                if (RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.GetComponent<Bridge.ObjectData>() == null)
                                {
                                    return;
                                }
						    }
					    }
                        else
					    {
                            return;
					    }

                        //Debug.Log(_mainTarget.position);
                        //Debug.Log(RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform);
                        //Debug.Log(RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.GetComponent<Bridge.ObjectData>());
                        //Debug.Log(RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.GetComponent<Bridge.ObjectData>().CenterBound);

                        Vector3 mainPos = new Vector3();
                        Vector3 subPos = new Vector3();
                        Bounds _bound = new Bounds();
                        //int segCount = 1;

                        if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
					    {
                            mainPos = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform
                                .GetComponent<Bridge.ObjectData>().Bound.center;
                            subPos = ObjectManager.Instance.Root3DObject.GetComponent<Bridge.ObjectData>().CenterBound;
                            _bound = ObjectManager.instance.Root3DObject.GetComponent<Bridge.ObjectData>().Bound;
                        }
                        else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
					    {
                            MeshRenderer renderer;
                            if(RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.TryGetComponent<MeshRenderer>(out renderer))
						    {
                                //segCount = Data.Viewer.Cache.Instance.models.Segments.Count;

                                mainPos = renderer.bounds.center;
                                subPos = MainManager.Instance.rootBounds.center;
                                _bound = Data.Viewer.Cache.Instance.models.rootBound;
						    }
					    }

                        _mainTarget.position = mainPos;
                        //////_subTarget.position = subPos;


                        {
                            float maxValue = 0f;
                            maxValue = Mathf.Max(Mathf.Max(_bound.size.x, _bound.size.y), _bound.size.z);

                            mainAroundCam.targetDistance = maxValue / 3;
                            //////subAroundCam.targetDistance = maxValue / 1.8f;
                        }

                        //subAroundCam.targetDistance = ObjectManager.instance.Root3DObject.GetComponent<Bridge.ObjectData>().Bound;

                        {
                            List<SystemEnum.Layer> mainLayers = new List<SystemEnum.Layer>();
                            mainLayers.Add(SystemEnum.Layer.Default);
                            mainLayers.Add(SystemEnum.Layer.Ignore_Raycast);
                            mainLayers.Add(SystemEnum.Layer.Overlay);
                            mainLayers.Add(SystemEnum.Layer.PPV);
                            mainLayers.Add(SystemEnum.Layer.TransparentFX);
                            mainLayers.Add(SystemEnum.Layer.UI);
                            mainLayers.Add(SystemEnum.Layer.Water);
                            mainLayers.Add(SystemEnum.Layer.FR);
                            mainLayers.Add(SystemEnum.Layer.BA);
                            mainLayers.Add(SystemEnum.Layer.LE);
                            mainLayers.Add(SystemEnum.Layer.RE);
                            mainLayers.Add(SystemEnum.Layer.TO);
                            mainLayers.Add(SystemEnum.Layer.BO);
                            mainLayers.Add(SystemEnum.Layer.Issue);

                            List<SystemEnum.Layer> subLayers = new List<SystemEnum.Layer>();
                            subLayers.Add(SystemEnum.Layer.Keymap);
                            subLayers.Add(SystemEnum.Layer.Issue);

                            mainCam.cullingMask = Layer.Index.GetLayerMask(mainLayers.ToArray());
                            subCam.cullingMask = Layer.Index.GetLayerMask(subLayers.ToArray());
                        }

                        mainAroundCam.isCanControl = true;
                        subAroundCam.isCanControl = true;
                        //subFreeCam.isCanControl = true;

                        //mainCam.UpdateView();
                        //subCam.UpdateView();
                    }
                    break;
            }
        }

        #endregion

        #region Event : 치수선 켜기/끄기

        public void SetDimensionView(bool isOn)
        {
            //bool isDimView = RuntimeData.RootContainer.Instance.dimensionView;

            SetDimLines(isOn);
            SetActiveDimCube(isOn);
        }

        #endregion

        #region dimension line

        public void SetDimLines(bool isOn)
        {
            if (SelectedDimTransform != null)
            {
                Transform lv3Transform = SelectedDimTransform.parent;
                Transform lv4Transform = SelectedDimTransform;

                lv3Transform.gameObject.SetActive(isOn);
                lv4Transform.gameObject.SetActive(isOn);
            }
        }

        #endregion

        #region Temp Objects

        #region Temp 3D Obj

        /// <summary>
        /// 현재 선택된 3D 객체를 기준으로 임시표시객체 생성
        /// </summary>
        /// <returns></returns>
        private GameObject CreateTemporary3DObject()
        {
            if (RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform == null)
            {
                Debug.Log("3D 캐싱객체 할당이 안되어 있습니다.");
                return null;
            }

            GameObject obj =
                Instantiate<GameObject>(RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.gameObject);
            GameObject origin = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.gameObject;
            obj.transform.position = origin.transform.position;
            obj.transform.localScale = origin.transform.lossyScale;
            obj.transform.rotation = origin.transform.rotation;
            //obj.transform.eulerAngles = new Vector3(0, 0, 0);

            Transform[] transforms = obj.GetComponentsInChildren<Transform>();
            int index = transforms.Length;
            for (int i = 0; i < index; i++)
            {
                transforms[i].gameObject.layer = (int)SystemEnum.Layer.Default;
            }

            MeshRenderer[] renderers = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform
                .GetComponentsInChildren<MeshRenderer>(); /* obj.GetComponentsInChildren<MeshRenderer>();*/
            index = renderers.Length;
            for (int i = 0; i < index; i++)
            {
                renderers[i].material = Resources.Load<Material>("Materials/Damage/selection");
                renderers[i].enabled = true;
            }

            Collider[] colliders = obj.GetComponentsInChildren<Collider>();
            index = colliders.Length;
            for (int i = 0; i < index; i++)
            {
                colliders[i].enabled = false;
            }

            Bridge.ObjectData[] objData = obj.GetComponentsInChildren<Bridge.ObjectData>();
            foreach (Bridge.ObjectData objIndex in objData)
            {
                Destroy(objIndex);
            }

            obj.transform.SetParent(RuntimeData.RootContainer.Instance.rootTempObj);

            return obj;
        }

        private void ClearTemp3DObj()
        {
            Transform _root3DObj = RuntimeData.RootContainer.Instance.rootTempObj;
            int index = _root3DObj.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(_root3DObj.GetChild(i).gameObject);
            }
        }

        #endregion

        #region DimensionCube

        private void CreateDimensionCube()
        {
            List<Transform> dimLines = new List<Transform>();// DimViewManager.Instance.dimLevel4;

            //Dim.DimScript.Instance.OnInput(RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform);

            //GameObject selectedDim = Dim.DimScript.Instance.selectedList.Find(x => x.transform.parent.name == "Dim");

            //List<GameObject> sList = Dim.DimScript.Instance.selectedList;
            //int index = sList.Count;
			//for (int i = 0; i < index; i++)
			//{
            //    Debug.Log(sList[i].name);
			//}

            SelectedDimTransform = null;
			//SelectedDimTransform = selectedDim.transform;

			CreateDimBox(SelectedDimTransform, RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform);

			foreach (Transform part in dimLines)
            {
                if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
				{
                    string partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name.Split(',')[0];
                    if (part.name == partName)
                    {
                        SelectedDimTransform = part;
                        //CreateDimBox(SelectedDimTransform,
                        //    RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform);
                        return;
                    }
				}
                else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
				{
                    string partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
                    if(part.name == partName)
					{
                        CreateDimBox(SelectedDimTransform,
                            RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform);
                        return;
                    }

                }
            }
        }

        private void CreateDimBox(Transform m_partDimTrans, Transform cached3D)
        {
            GameObject dimbox = new GameObject(m_partDimTrans.name);
            dimbox.transform.SetParent(RuntimeData.RootContainer.Instance.rootDimBox);
            dimbox.AddComponent<DimBox>().CreateWall(m_partDimTrans, cached3D);

            SetDimLines(RuntimeData.RootContainer.Instance.dimensionView);
            SetActiveDimCube(RuntimeData.RootContainer.Instance.dimensionView);
        }

        private void ClearDimBox()
        {
            Transform _rootDimBox = RuntimeData.RootContainer.Instance.rootDimBox;
            int index = _rootDimBox.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(_rootDimBox.GetChild(i).gameObject);
            }

            RuntimeData.RootContainer.Instance.captureResource.camfr = null;
            RuntimeData.RootContainer.Instance.captureResource.camba = null;
            RuntimeData.RootContainer.Instance.captureResource.camle = null;
            RuntimeData.RootContainer.Instance.captureResource.camri = null;
            RuntimeData.RootContainer.Instance.captureResource.camto = null;
            RuntimeData.RootContainer.Instance.captureResource.cambo = null;
        }

        /// <summary>
        /// 3D DimCube 켜고 끄기
        /// </summary>
        /// <param name="isOn"></param>
        public void SetActiveDimCube(bool isOn)
        {
            Transform rootDimCube = RuntimeData.RootContainer.Instance.rootDimBox;
            int index = rootDimCube.childCount;
            for (int i = 0; i < index; i++)
            {
                rootDimCube.GetChild(i).gameObject.SetActive(isOn);
            }
        }

        #endregion

        #region Issue records

        private void ClearIssueRecords()
        {
            Transform _rootIssueRecords = RuntimeData.RootContainer.Instance.rootRecordIssue;
            int index = _rootIssueRecords.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(_rootIssueRecords.GetChild(i).gameObject);
            }
        }

        #endregion

        #region View issue

        private void ClearViewMaintainance()
        {
            ViewSceneStatus sceneStatus = MainManager.Instance.SceneStatus;
            switch (sceneStatus)
            {
                case ViewSceneStatus.ViewMaintainance:

                    break;

                default:
                    {
                        Transform _rootViewIssue = RuntimeData.RootContainer.Instance.rootViewIssue;
                        int index = _rootViewIssue.childCount;
                        for (int i = 0; i < index; i++)
                        {
                            Destroy(_rootViewIssue.GetChild(i).gameObject);
                        }
                    }
                    break;
            }
        }

        #endregion

        #endregion

        #region Issue 오브젝트 색상 초기화
        private void IssueObjDefaultColor()
        {
            List<Issue.AIssue> _issueList = RuntimeData.RootContainer.Instance.IssueObjectList;

            for (int i = 0; i < _issueList.Count; i++)
            {
                if (_issueList[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                    _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(255 / 255f, 45 / 255f, 45 / 255f));
                else if (_issueList[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
                    _issueList[i].GetComponent<MeshRenderer>().material.SetColor("Color01", new Color(45 / 255f, 94 / 255f, 255 / 255f));
            }
        }
        #endregion

        #endregion

        #region Initialize

        public void InitializeObject(GameObject rootObject)
        {
            rootObject.transform.position = new Vector3(0, 0, 0);
            RootObject = rootObject.transform.GetChild(0).gameObject;
            RuntimeData.RootContainer.Instance.RootObject = RootObject;

            //CodeLv1 lv1Code;

            //for (int i = 0; i < RootObject.transform.childCount; i++)
            //{
            //    if (Enum.TryParse<CodeLv1>(RootObject.transform.GetChild(i).name, out lv1Code))
            //    {
            //        Root3DObject = RootObject.transform.GetChild(i).gameObject;
            //        RuntimeData.RootContainer.Instance.Root3DObject = Root3DObject;
            //        //StartCoroutine(Initialize3DObject(Root3DObject));
            //        Initialize3DObject(Root3DObject);
            //    }
            //    else
            //    {
            //        Root2DObject = RootObject.transform.GetChild(i).gameObject;
            //        RuntimeData.RootContainer.Instance.Root2DObject = Root2DObject;
            //        //StartCoroutine(Initialize2DObject(Root2DObject));
            //        Initialize2DObject(Root2DObject);
            //    }
            //}
        }

        #endregion

        #region Initialize check

        private void InitializeCheck()
        {
#if UNITY_EDITOR
#endif

            bool result =
                routineCheck2D &
                routineCheck2D_11 & routineCheck2D_12 & routineCheck2D_13 &
                routineCheck3D &
                routineCheck3D_21 & routineCheck3D_23 & routineCheck3D_24;
            if (result)
            {
                MaterialChange();
                MainManager.Instance.Request(new Request(Type.DimView, RequestCode.DimView_3DCall), RootObject);
                MainManager.Instance.Request(new Request(Type.DimView, RequestCode.DimView_2DCall), RootObject);

                RuntimeData.RootContainer.Instance.isObjectRoutineEnd = true;
                MainManager.Instance.InitializeRoutineCheck();
            }
        }

        #region Texture 사용 부재 Material 세팅

        private void MaterialChange()
        {
            int GRindex = 0;
            float matScale = 0;

            Bridge.ObjectData objData = new Bridge.ObjectData();
            foreach (Transform matTransform in selected_objTransforms)
            {
                if (matTransform.childCount > 0)
                {
                    objData = matTransform.GetComponent<Bridge.ObjectData>();
                    matScale = objData.Bound.max.x - objData.Bound.min.x;
                }
                else
                    objData = matTransform.GetComponent<Bridge.ObjectData>();

                // 스틸 난간
                if (matTransform.name.Contains("GRS_L"))
                {
                    GRindex++;
                    switch (GRindex)
                    {
                        case 1:
                            Material mat = Resources.Load<Material>("Materials/GRS_L/GR_TO");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 1, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 2:
                            mat = Resources.Load<Material>("Materials/GRS_L/GR_BA");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 1, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 3:
                            mat = Resources.Load<Material>("Materials/GRS_L/GR_RE");
                            mat.SetVector("_ScaleTiling", new Vector4(3.1f, 3.1f, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 4:
                            mat = Resources.Load<Material>("Materials/GRS_L/GR_FR");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 1, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 5:
                            mat = Resources.Load<Material>("Materials/GRS_L/GR_LE");
                            mat.SetVector("_ScaleTiling", new Vector4(3.1f, 3.1f, 0, 0));
                            objData.DefaultMaterial = mat;
                            GRindex = 0;
                            break;
                    }
                }

                if (matTransform.name.Contains("GRS_R"))
                {
                    GRindex++;
                    switch (GRindex)
                    {
                        case 1:
                            Material mat = Resources.Load<Material>("Materials/GRS_R/GR_TO");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 1, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 2:
                            mat = Resources.Load<Material>("Materials/GRS_R/GR_FR");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 1, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 3:
                            mat = Resources.Load<Material>("Materials/GRS_R/GR_LE");
                            mat.SetVector("_ScaleTiling", new Vector4(3.1f, 3.1f, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 4:
                            mat = Resources.Load<Material>("Materials/GRS_R/GR_BA");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 1, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 5:
                            mat = Resources.Load<Material>("Materials/GRS_R/GR_RE");
                            mat.SetVector("_ScaleTiling", new Vector4(3.1f, 3.1f, 0, 0));
                            objData.DefaultMaterial = mat;
                            GRindex = 0;
                            break;
                    }
                }

                // 콘크리트 난간
                if (matTransform.name.Contains("GRC"))
                {
                    GRindex++;
                    switch (GRindex)
                    {
                        case 1:
                            Material mat = Resources.Load<Material>("Materials/GRC_L/GRC_TO");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 1, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 2:
                            mat = Resources.Load<Material>("Materials/GRC_L/GRC_BA");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 2.4f, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 3:
                            mat = Resources.Load<Material>("Materials/GRC_L/GRC_RE");
                            mat.SetVector("_ScaleTiling", new Vector4(3.1f, 3.1f, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 4:
                            mat = Resources.Load<Material>("Materials/GRC_L/GRC_FR");
                            mat.SetVector("_ScaleTiling", new Vector4(matScale, 2.4f, 0, 0));
                            objData.DefaultMaterial = mat;
                            break;
                        case 5:
                            mat = Resources.Load<Material>("Materials/GRC_L/GRC_LE");
                            mat.SetVector("_ScaleTiling", new Vector4(3.1f, 3.1f, 0, 0));
                            objData.DefaultMaterial = mat;
                            GRindex = 0;
                            break;
                    }
                }

                // 포장
                if (matTransform.name.Contains("AP_"))
                {
                    Material mat = Resources.Load<Material>("Materials/Bridge/01");
                    objData.DefaultMaterial = mat;
                }

                // 받침 탄성
                if (matTransform.name.Contains("BE_RUP"))
                {
                    if (matTransform.childCount > 0)
                    {
                        Material mat = Resources.Load<Material>("Materials/Bridge/04");
                        objData.DefaultMaterial = mat;
                    }
                    else
                    {
                        Material mat = Resources.Load<Material>("Materials/Bridge/05");
                        objData.DefaultMaterial = mat;
                    }
                }
                // 받침 포트
                if (matTransform.name.Contains("BE_POT"))
                {
                    if (matTransform.childCount > 0)
                    {
                        Material mat = Resources.Load<Material>("Materials/Bridge/05");
                        objData.DefaultMaterial = mat;
                    }
                    else
                    {
                        Material mat = Resources.Load<Material>("Materials/Bridge/04");
                        objData.DefaultMaterial = mat;
                    }
                }

                // 신축이음
                if (matTransform.name.Contains("JI_"))
                {
                    if (matTransform.name.Contains("MN,MN"))
                    {
                        if (matTransform.childCount > 0)
                        {
                            objData = matTransform.GetComponent<Bridge.ObjectData>();
                            matScale = objData.Bound.max.z - objData.Bound.min.z;
                        }
                        else
                            objData = matTransform.GetComponent<Bridge.ObjectData>();

                        GRindex++;
                        if (GRindex == 2)
                        {
                            Material mat = Resources.Load<Material>("Materials/JI/JI_MN");
                            mat.SetVector("_ScaleTiling", new Vector4(4, matScale * 40, 0, 0));
                            objData.DefaultMaterial = mat;
                        }
                        else
                        {
                            Material mat = Resources.Load<Material>("Materials/Bridge/01");
                            objData.DefaultMaterial = mat;
                            if (GRindex == 6) GRindex = 0;
                        }
                    }

                    if (matTransform.name.Contains("RL,RL"))
                    {
                        GRindex++;
                        if (GRindex == 2)
                        {
                            Material mat = Resources.Load<Material>("Materials/JI/JI_RL");
                            mat.SetVector("_ScaleTiling", new Vector4(4, matScale * 40, 0, 0));
                            objData.DefaultMaterial = mat;
                        }
                        else
                        {
                            Material mat = Resources.Load<Material>("Materials/Bridge/01");
                            objData.DefaultMaterial = mat;
                            if (GRindex == 6) GRindex = 0;
                        }
                    }

                    if (matTransform.name.Contains("TP,TP"))
                    {
                        if (matTransform.childCount > 0)
                        {
                            objData = matTransform.GetComponent<Bridge.ObjectData>();
                            matScale = objData.Bound.max.z - objData.Bound.min.z;
                        }
                        else
                            objData = matTransform.GetComponent<Bridge.ObjectData>();

                        GRindex++;
                        if (GRindex == 2)
                        {
                            Material mat = Resources.Load<Material>("Materials/JI/JI_TP");
                            mat.SetVector("_ScaleTiling", new Vector4(4, matScale * 40, 0, 0));
                            objData.DefaultMaterial = mat;
                        }
                        else
                        {
                            Material mat = Resources.Load<Material>("Materials/Bridge/01");
                            objData.DefaultMaterial = mat;
                            if (GRindex == 6) GRindex = 0;
                        }
                    }

                    if (matTransform.name.Contains("NB,NB"))
                    {
                        if (matTransform.childCount > 0)
                        {
                            objData = matTransform.GetComponent<Bridge.ObjectData>();
                            matScale = objData.Bound.max.z - objData.Bound.min.z;
                        }
                        else
                            objData = matTransform.GetComponent<Bridge.ObjectData>();

                        GRindex++;
                        if (GRindex == 2)
                        {
                            Material mat = Resources.Load<Material>("Materials/JI/JI_NRM");
                            mat.SetVector("_ScaleTiling", new Vector4(4, matScale * 40, 0, 0));
                            objData.DefaultMaterial = mat;
                        }
                        else
                        {
                            Material mat = Resources.Load<Material>("Materials/Bridge/01");
                            objData.DefaultMaterial = mat;
                            if (GRindex == 6) GRindex = 0;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region 3D initialize

        private async void Initialize3DObject(GameObject root3D)
        {
            //Task.Delay(1);
            //yield return null;

            //await 

            routineCheck3D = false;
            routineCheck3D_21 = false;
            routineCheck3D_23 = false;

            StartCoroutine(SetObject_bridgeParts(root3D));
            StartCoroutine(SetObject_Mat_Collider(root3D));

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("3D async");
#endif

            routineCheck3D = true;
            InitializeCheck();

            //yield break;
        }

        /// <summary>
        /// 1. bridgeParts 설정
        /// </summary>
        /// <param name="root3D"></param>
        /// <returns></returns>
        private IEnumerator SetObject_bridgeParts(GameObject root3D)
        {
            yield return null;

            BridgeTopParts = new List<GameObject>();
            BridgeBottomParts = new List<GameObject>();

            int index = root3D.transform.childCount;
            for (int i = 0; i < index; i++)
            {
                //if (LevelCodeConverter.ConvertLv3Code(root3D.transform.GetChild(i).name.Substring(0, 2)) == CodeLv3.SP)
                //{
                //    BridgeTopParts.Add(root3D.transform.GetChild(i).gameObject);
                //}
                //else if (LevelCodeConverter.ConvertLv3Code(root3D.transform.GetChild(i).name.Substring(0, 2)) ==
                //         CodeLv3.AP)
                //{
                //    BridgeBottomParts.Add(root3D.transform.GetChild(i).gameObject);
                //}
            }

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("1. SetObject_bridgeParts");
#endif

            yield break;
        }

        /// <summary>
        /// 2. Material, Collider 설정 
        /// </summary>
        /// <param name="root3D"></param>
        /// <returns></returns>
        private IEnumerator SetObject_Mat_Collider(GameObject root3D)
        {
            yield return null;

            #region 변수 초기화

            int index = 0; // 반복문 총 길이 캐싱변수

            // 3D 객체 아래의 모든 자식객체 수집
            objTransforms = root3D.gameObject.GetComponentsInChildren<Transform>();

            // 자식객체 중에 조건에 맞는 (material을 가진 객체의 조건) 객체 수집
            selected_objTransforms = (from obj in objTransforms where obj.name.Contains(',')/* Split(',').Length > 1*/ select obj)
                .ToArray<Transform>();

            //yield break;

            Dictionary<string, int> multiSplitIndex = new Dictionary<string, int>();

            #endregion

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("2. SetObject_Mat_Collider");
#endif

            #region ObjectData 코드 할당

            index = objTransforms.Length;
            for (int i = 0; i < index; i++)
            {
                objTransforms[i].gameObject.AddComponent<Bridge.ObjectData>();
            }

            #endregion

            #region 모든 객체의 material, collider 할당

            index = selected_objTransforms.Length;
            for (int i = 0; i < index; i++)
            {
                string[] splitObjectString = selected_objTransforms[i].name.Split(',');

                // [조건] 분할 문자열이 2개 : (객체명 문자열 1 / material 정보 1) 객체가 한 개의 Material을 필요로 하는 경우
                if (splitObjectString.Length == 2)
                {
                    selected_objTransforms[i].GetComponent<MeshRenderer>().material =
                        Resources.Load<UnityEngine.Material>($"Materials/Bridge/{splitObjectString[1]}");

                    selected_objTransforms[i].GetComponent<Bridge.ObjectData>().DefaultMaterial =
                        selected_objTransforms[i].GetComponent<MeshRenderer>().material;

                    //// TODO : 1123
                    //selected_objTransforms[i].GetComponent<MeshRenderer>().material =
                    //    Resources.Load<UnityEngine.Material>($"Materials/Hololens/XRay");

                    selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
                }
                // [조건] 분할 문자열이 2개 이상: (객체명 문자열 1, material 정보 n) 객체가 여러 개의 Material을 필요로 하는 경우
                else if (splitObjectString.Length > 2)
                {
                    // Dictionary에 확인
                    int stringIndex = 0;

                    // 조건 : Dictionary에 객체명 키값 검색시 키가 있는 경우 : 첫 번째 객체가 아닌 경우
                    if (multiSplitIndex.TryGetValue(splitObjectString[0], out stringIndex).Equals(true))
                    {
                        multiSplitIndex[splitObjectString[0]]++;

                        selected_objTransforms[i].GetComponent<MeshRenderer>().material =
                            Resources.Load<Material>(
                                $"Materials/{splitObjectString[multiSplitIndex[splitObjectString[0]]].Substring(0, 1)}/" +
                                $"{splitObjectString[multiSplitIndex[splitObjectString[0]]]}");

                        selected_objTransforms[i].GetComponent<Bridge.ObjectData>().DefaultMaterial =
                            selected_objTransforms[i].GetComponent<MeshRenderer>().material;

                        //// TODO : 1123
                        //selected_objTransforms[i].GetComponent<MeshRenderer>().material =
                        //    Resources.Load<UnityEngine.Material>($"Materials/Hololens/XRay");
                    }
                    // Dictionary 내부에 해당 키값이 없으면 첫 번째 검색된 객체.
                    else
                    {
                        multiSplitIndex.Clear();

                        // Dictionary에 새 키 할당
                        multiSplitIndex.Add(splitObjectString[0], stringIndex);
                        multiSplitIndex[splitObjectString[0]]++;

                        selected_objTransforms[i].GetComponent<MeshRenderer>().material = Resources.Load<Material>(
                            $"Materials/{splitObjectString[multiSplitIndex[splitObjectString[0]]].Substring(0, 1)}/" +
                            $"{splitObjectString[multiSplitIndex[splitObjectString[0]]]}");

                        selected_objTransforms[i].GetComponent<Bridge.ObjectData>().DefaultMaterial =
                            selected_objTransforms[i].GetComponent<MeshRenderer>().material;

                        // TODO : 1123
                        //selected_objTransforms[i].GetComponent<MeshRenderer>().material =
                        //    Resources.Load<UnityEngine.Material>($"Materials/Hololens/XRay");

                        selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
                    }

                    selected_objTransforms[i].GetComponent<Bridge.ObjectData>().DefaultMaterial =
                        selected_objTransforms[i].GetComponent<MeshRenderer>().material;
                }
            }

            multiSplitIndex.Clear();

            #endregion

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("2-1. SetObject_Bounds_CenterVector");
#endif
            SetObject_Bounds_CenterVector(root3D.transform);
            // ObjectData 할당 종료 후 Bound Center값 받아서 L3 root 위치변경
            StartCoroutine(SetPanningBounds(root3D));
            StartCoroutine(SetPartDics());

            yield break;
        }

        /// <summary>
        /// 2-1. ObjectData에 Bounds와 중심 벡터 구함
        /// </summary>
        /// <param name="_objects"></param>
        private void SetObject_Bounds_CenterVector(Transform _objects)
        {

            #region 변수

            int index = 0;

            Bridge.ObjectData objData;
            Bridge.ObjectData childObjData;

            List<Vector3> minVector = new List<Vector3>();
            List<Vector3> maxVector = new List<Vector3>();

            Vector3 minVectorResult = new Vector3(0, 0, 0);
            Vector3 maxVectorResult = new Vector3(0, 0, 0);

            #endregion

            #region ObjectData 설정

            // 현재 GameObject가 objectData를 갖고있는지 확인
            if (_objects.TryGetComponent<Bridge.ObjectData>(out objData))
            {
                // 자식 객체들에 재귀 실행. (자식 객체가 없다면 반복문이 실행되지 않는다.)
                index = _objects.transform.childCount;
                for (int i = 0; i < index; i++)
                {
                    SetObject_Bounds_CenterVector(_objects.transform.GetChild(i));
                }

                objData.ParentName = _objects.transform.parent.name;

                Collider _collider;
                if (index != 0)
                {
                    if (_objects.TryGetComponent<Collider>(out _collider))
                    {
                        objData = _objects.GetComponent<Bridge.ObjectData>();
                        objData.Bound = _collider.bounds;
                        objData.CenterBound = objData.Bound.center;
                    }
                    // 자식 객체가 아닌 경우
                    else
                    {
                        objData = _objects.GetComponent<Bridge.ObjectData>();

                        index = _objects.transform.childCount;

                        // 자식 객체의 최소 최대 벡터 수집
                        for (int i = 0; i < index; i++)
                        {
                            childObjData = _objects.transform.GetChild(i).GetComponent<Bridge.ObjectData>();

                            minVector.Add(childObjData.MinVector3);
                            maxVector.Add(childObjData.MaxVector3);
                        }

                        // 최소 최대 벡터값 계산
                        for (int i = 0; i < index; i++)
                        {
                            // 벡터값 비교, 할당
                            if (i != 0)
                            {
                                minVectorResult.Set(
                                    (minVectorResult.x > minVector[i].x) ? minVector[i].x : minVectorResult.x,
                                    (minVectorResult.y > minVector[i].y) ? minVector[i].y : minVectorResult.y,
                                    (minVectorResult.z > minVector[i].z) ? minVector[i].z : minVectorResult.z
                                );

                                maxVectorResult.Set(
                                    (maxVectorResult.x < maxVector[i].x) ? maxVector[i].x : maxVectorResult.x,
                                    (maxVectorResult.y < maxVector[i].y) ? maxVector[i].y : maxVectorResult.y,
                                    (maxVectorResult.z < maxVector[i].z) ? maxVector[i].z : maxVectorResult.z
                                );
                            }
                            // 초기 할당 코드
                            else
                            {
                                minVectorResult = minVector[i];
                                maxVectorResult = maxVector[i];
                            }
                        }

                        // Bounds 생성시 center 좌표, 영역값을 구해서 넣어준다.
                        objData.Bound = new Bounds((minVectorResult + maxVectorResult) / 2,
                            (maxVectorResult - minVectorResult));

                        objData.Bound = objData.Bound;
                        objData.CenterBound = objData.Bound.center;

                        // 계산용 리스트 비움
                        minVector.Clear();
                        maxVector.Clear();
                    }
                }
                else
                {
                    if (_objects.TryGetComponent<Collider>(out _collider))
                    {
                        objData = _objects.GetComponent<Bridge.ObjectData>();
                        objData.Bound = _collider.bounds;
                        objData.CenterBound = objData.Bound.center;
                    }
                }
            }
            else
            {
                // 자식 객체들에 재귀 실행
                index = _objects.transform.childCount;
                for (int i = 0; i < index; i++)
                {
                    SetObject_Bounds_CenterVector(_objects.transform.GetChild(i));
                }
            }

            #endregion

            routineCheck3D_21 = true;
            InitializeCheck();

        }

        /// <summary>
        /// 2-3. 카메라 패닝위치 변경
        /// </summary>
        /// <param name="root3D"></param>
        /// <returns></returns>
        private IEnumerator SetPanningBounds(GameObject root3D)
        {
            yield return new WaitUntil(() => routineCheck3D_21 == true);

            Bounds rootBounds = root3D.GetComponent<Bridge.ObjectData>().Bound;

            Bounds panningBounds = new Bounds(rootBounds.center, rootBounds.size * 5f);

            // 패닝 경계값을 필요한 위치에 할당
            //manager.InputManager.SelectionController.mouseControl.draggingBound = dragBounds;

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("2-3. SetPanningBounds");
#endif

            routineCheck3D_23 = true;
            InitializeCheck();

            yield break;
        }

        /// <summary>
        /// 2-4. 탑재된 부재 확인변수 갱신
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetPartDics()
        {
            yield return null;

            int index = selected_objTransforms.Length;

            for (int i = 0; i < index; i++)
            {
                if (selected_objTransforms[i].name.Split('_').Length >= 3)
                {
                    string arg = selected_objTransforms[i].name.Split('_')[1];
                    CodeLv4 codeLv4 = CodeLv4.Null;

                    if (Enum.TryParse<CodeLv4>(arg, out codeLv4))
                    {
                        //RuntimeData.RootContainer.Instance.partDics[codeLv4] = true;
                    }
                }
            }

            routineCheck3D_24 = true;
            InitializeCheck();

            yield break;
        }

        #endregion

        #region 카메라 위치할당

        /// <summary>
        /// 카메라 바라보는 시작위치 지정
        /// </summary>
        /// <param name="root3D"></param>
        /// <returns></returns>
        //public void ResetCamPosition(GameObject root3D)
        //{
        //    Bridge.ObjectData objData = root3D.GetComponent<Bridge.ObjectData>();

        //    float maxValue = 0f;
        //    Vector3 centerPosition = objData.Bound.center;

        //    maxValue = (maxValue < objData.Bound.size.x) ? objData.Bound.size.x : maxValue;
        //    maxValue = (maxValue < objData.Bound.size.y) ? objData.Bound.size.y : maxValue;
        //    maxValue = (maxValue < objData.Bound.size.z) ? objData.Bound.size.z : maxValue;

        //    float targetDistance = maxValue + maxValue / 10 - 3;

        //    MainManager.Instance.MainCameraController.targetDistance = targetDistance;
        //    MainManager.Instance.MainCamTransform.position = centerPosition;

        //    Bounds _panningBounds = new Bounds(
        //        objData.Bound.center,
        //        objData.Bound.size * RuntimeData.RootContainer.Instance.panningExtensionBorder
        //    );

        //    Control.AContoller.Instance.PanningBounds = _panningBounds;
        //}

        public void ResetCamPosition(GameObject rootObj, InVisibleOption visibleOption)
        {
            switch (visibleOption)
            {
                case InVisibleOption.Skew:
                case InVisibleOption.Curve:
                case InVisibleOption.Interactable:
                    //{
                    //    Collider _collider;
                    //    Bounds _bound;

                    //    List<Vector3> minVector = new List<Vector3>();
                    //    List<Vector3> maxVector = new List<Vector3>();

                    //    Vector3 resultMinVector;
                    //    Vector3 resultMaxVector;

                    //    // 자식 객체들을 반복
                    //    int index = rootObj.transform.childCount;
                    //    for (int i = 0; i < index; i++)
                    //    {
                    //        // 자식 객체들을 돌면서 Collider 가진 객체 확인
                    //        if (rootObj.transform.GetChild(i).TryGetComponent<Collider>(out _collider))
                    //        {
                    //            // collider 객체에서 bounds의 minVector, maxVector 수집
                    //            minVector.Add(_collider.bounds.min);
                    //            maxVector.Add(_collider.bounds.max);
                    //        }
                    //    }

                    //    // 최소, 최대값 계산
                    //    resultMinVector = GetMinVector(minVector);
                    //    resultMaxVector = GetMaxVector(maxVector);

                    //    _bound = new Bounds(
                    //        (resultMaxVector + resultMinVector) / 2, (resultMaxVector - resultMinVector));

                    //    float maxValue = 0f;
                    //    float targetDistance = 0f;

                    //    maxValue = (maxValue < _bound.size.x) ? _bound.size.x : maxValue;
                    //    maxValue = (maxValue < _bound.size.y) ? _bound.size.y : maxValue;
                    //    maxValue = (maxValue < _bound.size.z) ? _bound.size.z : maxValue;

                    //    targetDistance = maxValue + maxValue / 10 - 3;

                    //    MainManager.Instance.MainCameraController.targetDistance = targetDistance;
                    //    MainManager.Instance.MainCamTransform.position = _bound.center;

                    //    Bounds _panningBounds = new Bounds(
                    //        _bound.center,
                    //        _bound.size * RuntimeData.RootContainer.Instance.panningExtensionBorder);

                    //    Control.AContoller.Instance.PanningBounds = _panningBounds;
                    //}
                    break;

                    //case InVisibleOption.Interactable:
                    //    ResetCamPosition(Root3DObject);
                    //    break;
            }
        }

        private Vector3 GetMinVector(List<Vector3> minVector)
        {
            Vector3 _minVector = new Vector3(0, 0, 0);

            int index = minVector.Count;
            for (int i = 0; i < index; i++)
            {
                if (i == 0) _minVector = minVector[0];
                else
                {
                    _minVector = new Vector3(
                        (_minVector.x > minVector[i].x) ? minVector[i].x : _minVector.x,
                        (_minVector.y > minVector[i].y) ? minVector[i].y : _minVector.y,
                        (_minVector.z > minVector[i].z) ? minVector[i].z : _minVector.z
                    );
                }
            }

            return _minVector;
        }

        private Vector3 GetMaxVector(List<Vector3> maxVector)
        {
            Vector3 _maxVector = new Vector3(0, 0, 0);

            int index = maxVector.Count;
            for (int i = 0; i < maxVector.Count; i++)
            {
                if (i == 0) _maxVector = maxVector[0];
                else
                {
                    _maxVector = new Vector3(
                        (_maxVector.x < maxVector[i].x) ? maxVector[i].x : _maxVector.x,
                        (_maxVector.y < maxVector[i].y) ? maxVector[i].y : _maxVector.y,
                        (_maxVector.z < maxVector[i].z) ? maxVector[i].z : _maxVector.z
                    );
                }
            }

            return _maxVector;
        }

        #endregion

        #region 2D initialize

        private async void Initialize2DObject(GameObject root2D)
        {
            //yield return null;

            routineCheck2D = false;
            routineCheck2D_11 = false;
            routineCheck2D_12 = false;
            routineCheck2D_13 = false;

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("2D async");
#endif

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
            Transform[] outlineTransforms = (from obj in dimTransforms where obj.name.Split('/').Length == 3 select obj).ToArray<Transform>();

            StartCoroutine(SetObject_OutLines(outlineTransforms));

            // 3. 모든 치수선값 표시 객체 배열 정렬
            Transform[] textMeshTransforms =
                (from obj in dimTransforms
                 where obj.name.Contains("Main") &&
                       obj.name.Split('_').Length == 3
                 select obj).ToArray<Transform>();

            StartCoroutine(SetObject_TextMeshes(textMeshTransforms));

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("1. SetObject_Dimension");
#endif

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
            //Task.Delay(1);

            int index = dimLines.Length;
            MeshRenderer renderer;

            //Parallel.For(0, dimLines.Length,
            //       index => {
            //           if (dimLines[index].TryGetComponent<MeshRenderer>(out renderer))
            //           {
            //               renderer.material = MainManager.Instance.DSRMat;
            //           }
            //       });

            for (int i = 0; i < index; i++)
            {
                if (dimLines[i].TryGetComponent<MeshRenderer>(out renderer))
                {
                    renderer.material = MainManager.Instance.DSRMat;
                }
            }

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("1-1. SetObject_DimensionLines");
#endif

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

            //Parallel.For(0, _index,
            //    index =>
            //    {
            //        outLines[index].GetComponent<MeshRenderer>().material = MainManager.Instance.DSRMat;
            //    });

            for (int i = 0; i < _index; i++)
            {
                outLines[i].GetComponent<MeshRenderer>().material = MainManager.Instance.OutlineMat;
            }

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("1-2. SetObject_OutLines");
#endif

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

            //Parallel.For(0, _index,
            //    index =>
            //    {
            //        arguments = textMeshes[index].name.Split('_');

            //        textInstance.transform.position = textMeshes[index].position; // text 기본위치 설정
            //        textInstance.transform.eulerAngles = GetTextMeshRotation(textMeshes[index]);

            //        // tmPro 설정단계
            //        tmPro = textInstance.GetComponent<TextMeshPro>();
            //        tmPro.text = arguments[1];
            //        tmPro.fontSize = 2;
            //        tmPro.fontStyle = FontStyles.Bold;
            //        tmPro.alignment = TextAlignmentOptions.Center;
            //        tmPro.color = new Color(1f, 0.8759048f, 0, 1);

            //        GameObject tempObj = Instantiate(textInstance, textInstance.transform.position,
            //            textInstance.transform.rotation);
            //        tempObj.transform.SetParent(textMeshes[index]); // text 객체 SetParent
            //        tempObj.transform.localPosition = new Vector3(0, 0, 0); // text 0, 0, 0 정렬
            //        tempObj.transform.localPosition = GetTextMeshPosition(textMeshes[index]); // text 위치 정렬
            //    });

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
                coll.size = new Vector3(_rect.rect.width, _rect.rect.height, 0.1f);
                coll.isTrigger = true;

				Rigidbody rigid = texts[i].AddComponent<Rigidbody>();
				rigid.useGravity = false;

				//texts[i].transform.localPosition; // offset용
			}

            // 할당이 끝난 textInstance 삭제
            Destroy(textInstance, 0.1f);

#if UNITY_EDITOR
            EditDebug.PrintOBJRoutine("1-3. SetObject_TextMeshes");
#endif

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

            switch(parentName)
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