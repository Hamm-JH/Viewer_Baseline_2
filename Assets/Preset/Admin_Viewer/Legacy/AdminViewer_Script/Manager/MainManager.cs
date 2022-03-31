using Manager.Definition;
using MGS.UCamera;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Manager
{
    [System.Serializable]
    public struct UrlSet
    {
        public string bridgeListURL;
        public string modelURL;
        public string addressURL;
        public string damagedURL;
        public string recoverURL;
        public string imageURL;
        public string historyURL;
        public string reportURL;
    }

    public class MainManager : MonoBehaviour
    {
        #region Singleton

        private static MainManager _instance;

        public static MainManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MainManager>();
                }
                return _instance;
            }
        }

        #endregion

        #region 변수

        [SerializeField] private ForwardRendererData renderData;
        public ForwardRendererData RenderSetting
        {
            get
            {
                if (renderData == null)
                {
                    renderData = Resources.Load<ForwardRendererData>("ForwardRenderer");
                }
                return renderData;
            }
        }

        [SerializeField] private ViewSceneStatus sceneStatus;
        [SerializeField] private Control.Type controlType;
        [SerializeField] private Definition.UseCase appUseCase;
        [SerializeField] private string[] args;
        [SerializeField] private string baseURL;

        public ViewSceneStatus SceneStatus
        {
            get => sceneStatus;
            // TODO : Scene 변경시 이벤트 실행
            set
            {
#if UNITY_EDITOR
                EditDebug.PrintStatusChangeRoutine($"Status change : {value.ToString()}");
#endif
                sceneStatus = value;
                ChangeSceneStatus(value);   // 상태코드별 상태변경

				if ((int)value > 2)
				{


					//Debug.Log((int)value);
					StartCoroutine(UIManager.Instance.SetIndicators(
						RuntimeData.RootContainer.Instance.IssueObjectList,
						value));
				}
			}
        }

        public Control.Type ControlType
        {
            get => controlType;
            set => controlType = value;
        }

        public UseCase AppUseCase { get => appUseCase; }

        //===================================================

        #region API

        [Header("URLs, import query")]
        [SerializeField] private string bridgeCode;
        [SerializeField] private string fileExtension;

        public string BridgeCode
        {
            get => bridgeCode;
            set => bridgeCode = value;
        }

        public string FileExtension
        {
            get => fileExtension;
            set => fileExtension = value;
        }

        [SerializeField] private UrlSet urlSet;

		#endregion

		#region Resources

		[Header("handling resources")]
        //public InVisibleOption visibleOption;
        public string bridgeType;

        //===================================================

        [SerializeField] private AroundAlignCamera mainCameraController;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform mainCameraTransform;

        /// <summary>
        /// url 모음
        /// </summary>

        public AroundAlignCamera MainCameraController
        {
            get => mainCameraController;
            set => mainCameraController = value;
        }

        public Camera MainCamera
        {
            get => mainCamera;
            set => mainCamera = value;
        }

        public Transform MainCamTransform
        {
            get => mainCameraTransform;
            set => mainCameraTransform = value;
        }

		//===================================================

		public Material DSRMat;
        public Material OutlineMat;

        //===================================================

        public Transform bottomGridPanel;

        //===================================================

        public string strReportURL;

        private bool isMapOn;

        #endregion

        #region Debug

        [Header("Debug")]
        [SerializeField] private string debugBridgeCode;
        [SerializeField] private string debugTunnelCode;

        #endregion

        //public Dim.DimScript dimcode;
        public string liningMaterial;

        [Header("키맵 세팅용 변수")]
        /// <summary>
        /// 키맵 세팅용 재귀실행 확인변수
        /// </summary>
        public bool isRecursived;
        

        #endregion

        #region 초기화
        private void Awake()
        {
            isRecursived = false;

            //args = Environment.GetCommandLineArgs();
            string hostName = Application.absoluteURL;

            string[] urlArgs = hostName.Split('/');

            baseURL = "";
            if (hostName != null && urlArgs.Length > 2)
            {
                baseURL = string.Format($"{urlArgs[0]}//{urlArgs[2]}");
            }
            else
            {
#if UNITY_EDITOR

                baseURL = "http://wesmart.synology.me:45001";   // 디버깅용
                //baseURL = "http://wesmart.synology.me:30051";   // 디버깅용
#else
                Debug.Log("요청받은 URL이 온전하지 않음");
                Application.Quit();
#endif
            }

            string _bridgeListURL = "/api/tunnel/search?app=y";
            string _modelURL = "/3dmodel/";
            string _addressURL = "/api/tunnel/search?cdTunnel=";
            string _damagedURL = "/api/tunnel/damage/state?cdTunnel=";
            string _recoverURL = "/api/tunnel/recover/state?cdTunnel=";
            string _imageURL = "/api/common/file/dn?";
            string _historyURL = "/api/tunnel/damageDailyHistory?";
            string _reportURL = "/api/tunnel/report?";


            SceneStatus = ViewSceneStatus.Awake;

            urlSet = new UrlSet();
#if UNITY_EDITOR

            urlSet.bridgeListURL = $"{baseURL}{_bridgeListURL}";
            urlSet.modelURL = $"{baseURL}{_modelURL}";
            urlSet.addressURL = $"{baseURL}{_addressURL}";
            urlSet.damagedURL = $"{baseURL}{_damagedURL}";
            urlSet.recoverURL = $"{baseURL}{_recoverURL}";
            urlSet.imageURL = $"{baseURL}{_imageURL}";
            urlSet.historyURL = $"{baseURL}{_historyURL}";
            urlSet.reportURL = $"{baseURL}{_reportURL}";

            strReportURL = urlSet.reportURL;
#else
            urlSet.bridgeListURL = $"{baseURL}{_bridgeListURL}";
            urlSet.addressURL = $"{baseURL}{_addressURL}";
            urlSet.damagedURL = $"{baseURL}{_damagedURL}";
            urlSet.recoverURL = $"{baseURL}{_recoverURL}";
            urlSet.imageURL = $"{baseURL}{_imageURL}";
            urlSet.historyURL = $"{baseURL}{_historyURL}";
            urlSet.reportURL = $"{baseURL}{_reportURL}";
            
            strReportURL = urlSet.reportURL;
#endif

            JSONManager.Instance.Initialize(urlSet);

        }

        // Start is called before the first frame update
        void Start()
        {
            //RuntimeData.RootContainer.Instance.cached3DInstance = null;

#region Initialize Control interface

            InitializeControlInterface(ControlType);

#endregion

            BridgeCode = "";
            string uri = "";


            UnityEngine.Rendering.Universal.ForwardRendererData obj = RenderSetting;

            int index = obj.rendererFeatures.Count;
            for (int i = 0; i < index; i++)
            {
                obj.rendererFeatures[i].SetActive(true);
			}

#if UNITY_EDITOR
			//JSONManager.Instance.LoadObjectList(urlSet.bridgeListURL);

			//20211202-00000223
			//20211202-00000283
			//20211202-00000235
			//20211202-00000283

			//string _urlQuery = "http://wesmart.synology.me:45001/unity/admin_viewer/index.html?cdTunnel=20211214-00000001";  //20211202-00000265
			//string _urlQuery = "http://wesmart.synology.me:45001/unity/admin_viewer/index.html?cdTunnel=20211202-00000283"; // 아크터널 ?
			//string _urlQuery = "http://wesmart.synology.me:45001/unity/admin_viewer/index.html?cdTunnel=20211202-00000259"; // (박스터널 4련)
            string _urlQuery = "http://wesmart.synology.me:45001/unity/admin_viewer/index.html?cdTunnel=20211202-00000265"; // 아크터널 ?

            string[] _cdBridge = _urlQuery.Split('=');
            string __modelURL = "/3dmodel/";
            _cdBridge = _cdBridge[1].Split('&');
            BridgeCode = _cdBridge[0];

            uri = string.Format($"{baseURL}{__modelURL}{BridgeCode}.{FileExtension}");
            //Debug.Log($"Bridge code : {BridgeCode}");
            //Debug.Log($"uri : {uri}");

            Request(new Manager.Request(Type.JSON, RequestCode.JSON_ObjectCall), uri);
            Request(new Manager.Request(Type.JSON, RequestCode.JSON_IssueCall), BridgeCode);

            //http://wesmart.synology.me:45001/unity/admin_viewer/index.html?cdTunnel=20211118-00000001
#else
            string urlQuery = Application.absoluteURL;
            string[] cdBridge = urlQuery.Split('=');
            string _modelURL = "/3dmodel/";
            cdBridge = cdBridge[1].Split('&');
            BridgeCode = cdBridge[0];

            Debug.Log($"model URL : {baseURL}{_modelURL}");
            Debug.Log($"model URI : {baseURL}{_modelURL}{bridgeCode}.{FileExtension}");

            uri = string.Format($"{baseURL}{_modelURL}{BridgeCode}.{FileExtension}");
            
            Request(new Manager.Request(Type.JSON, RequestCode.JSON_ObjectCall), uri);
            Request(new Manager.Request(Type.JSON, RequestCode.JSON_IssueCall), BridgeCode);
            
            //WebManager.ViewMap("F");
#endif
        }

		private void Update()
		{
            //Debug.Log(UnityEngine.Random.Range(0, 1));
        }

		public void EditorInitialize()
        {
#if UNITY_EDITOR
            List<RuntimeData.BridgeList> bridgeList = RuntimeData.RootContainer.Instance.bridgeCodeList;
            int index = bridgeList.Count;
            string uri = "";

            if(AppUseCase == UseCase.Bridge)
			{
                BridgeCode = debugBridgeCode; // 30051용 // 터널 모델
			}
            else if(AppUseCase == UseCase.Tunnel)
			{
                BridgeCode = debugTunnelCode;
            }

            uri = string.Format($"{urlSet.modelURL}{BridgeCode}.{FileExtension}");

            Request(new Manager.Request(Type.JSON, RequestCode.JSON_ObjectCall), uri);
            Request(new Manager.Request(Type.JSON, RequestCode.JSON_IssueCall), BridgeCode);
#endif
        }

#region 컨트롤 인터페이스에 따라 컨트롤러 다르게 생성
        private void InitializeControlInterface(Control.Type controlType)
        {
            GameObject Controller = new GameObject("Controller");

            //Controller.AddComponent(Control.ImportManager.GetControllerComponent(controlType));

            Controller.transform.SetParent(transform);
        }
        #endregion

        #endregion

        public Bounds rootBounds;

#region Status Change

        /// <summary>
        /// Scene의 상태코드 프로퍼티가 변할때 실행되는 코드
        /// </summary>
        /// <param name="status"></param>
        private void ChangeSceneStatus(ViewSceneStatus status)
        {
            RuntimeData.RootContainer.Instance.selectedInstance = null;
            RuntimeData.RootContainer.Instance.cachedInstance = null;
            //UIManager.Instance._keymapElement.clickedTransform = null;
            UIManager.Instance.SetButton(status);   // 버튼 상태변경
            UIManager.Instance.SetPanel(status);    // 패널 상태변경
            SwitchOptionalStatus(status);           // 특정 SceneStatus에서 실행되는 코드 모음
            ObjectManager.Instance.Set3DObject();

            if(status == ViewSceneStatus.ViewPartDamage || status == ViewSceneStatus.ViewPart2R)
			{
                Transform tr = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform;
                if(tr != null)
				{
                    Debug.Log(tr);
                    // 치수선 배치
                    //Dim.DimScript.Instance.OnInput(tr);
				}
			}
            else
			{
                if((int)status > 2)
				{
                    // 치수선 객체 전체끄기
                    //Dim.DimScript.Instance.AllViewSet(false);
				}
			}
        }

        /// <summary>
        /// 특정 SceneStatus에서 실행되는 코드
        /// </summary>
        /// <param name="status"></param>
        private void SwitchOptionalStatus(ViewSceneStatus status)
        {
            switch (status)
            {
                case ViewSceneStatus.Ready:
#if UNITY_EDITOR
#else
                    if(!isMapOn)
                    {
                        isMapOn = true;
                        WebManager.InitializeMap("T");
                    }
                    WebManager.ViewMap("T");
#endif
                    Debug.Log("CreateMap True / argument : T");
                    InitializeValue();
                    //VisibleSwitch();
                    Manager.SceneChangeMenu.Instance.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.BridgeModel);

                    float _minPosY = 0;
                    if(MainManager.Instance.appUseCase == UseCase.Bridge)
					{
                        _minPosY = RuntimeData.RootContainer.Instance.Root3DObject.GetComponent<Bridge.ObjectData>().MinVector3.y;

                    }
                    else if(MainManager.Instance.appUseCase == UseCase.Tunnel)
					{
                        _minPosY = Data.Viewer.Cache.Instance.models.rootBound.min.y - 1;
					}

                    bottomGridPanel.position = new Vector3(0, _minPosY, 0);
                    break;

                case ViewSceneStatus.ViewAllDamage:
#if UNITY_EDITOR
#else
                    isMapOn = false;
                    WebManager.ViewMap("F");
#endif
                    //Debug.Log("CreateMap False / argument : F");
                    VisibleSwitch();
                    break;

                case ViewSceneStatus.ViewPartDamage:
                case ViewSceneStatus.ViewPart2R:
                case ViewSceneStatus.ViewMaintainance:
#if UNITY_EDITOR
#else
                    isMapOn = false;
                    WebManager.ViewMap("F");
#endif
                    //Debug.Log("CreateMap False / argument : F");
                    break;
            }
        }

        /// <summary>
        /// 보여야할 교량을 변경할때 실행하는 코드
        /// </summary>
        /// <returns></returns>
        private void VisibleSwitch()
        {
            switch (SceneStatus)
            {
                case ViewSceneStatus.Ready:

                case ViewSceneStatus.ViewAllDamage:
                    RuntimeData.RootContainer.Instance.skewBridge.SetActive(false);
                    RuntimeData.RootContainer.Instance.curveBridge.SetActive(false);
                    RuntimeData.RootContainer.Instance.interactableBridge.SetActive(false);
                    RuntimeData.RootContainer.Instance.curveSkewBridge.SetActive(false);
                    if(MainManager.Instance.AppUseCase == UseCase.Bridge)
					{
                        RuntimeData.RootContainer.Instance.RootObject.SetActive(true);
					}
                    else if(MainManager.Instance.AppUseCase == UseCase.Tunnel)
					{
                        Data.Viewer.Cache.Instance.models.ModelName.SetActive(true);
					}
                    //ObjectManager.Instance.ResetCamPosition(RuntimeData.RootContainer.Instance.Root3DObject, visibleOption);
                    break;
            }
        }

#region Used only SceneStatus : Ready

        /// <summary>
        /// 객체 생성 루틴과 손상/보수 정보 생성 루틴의 종료를 확인한다.
        /// 모두 종료된 경우, SceneStatus.Ready 상태를 만들어준다.
        /// </summary>
        public void InitializeRoutineCheck()
        {
#region Debug
#if UNITY_EDITOR
            EditDebug.PrintINITIALIZERoutine($"ObjectRoutine : {RuntimeData.RootContainer.Instance.isObjectRoutineEnd}");
            EditDebug.PrintINITIALIZERoutine($"IssueRoutine : {RuntimeData.RootContainer.Instance.isIssueRoutineEnd}");
#endif
#endregion

            if (RuntimeData.RootContainer.Instance.isObjectRoutineEnd
                && RuntimeData.RootContainer.Instance.isIssueRoutineEnd)
            {
                SceneStatus = ViewSceneStatus.Ready;
                //EventClassifier.Instance.currentCache =
                //    new EventCache(typeof(Manager.SceneChangeMenu),
                //                   Manager.SceneChangeMenu.Instance,
                //                   Manager.SceneChangeMenu.Instance.transform,
                //                   ViewSceneStatus.Ready);
            }
        }

        /// <summary>
        /// Status Ready
        /// </summary>
        private void InitializeValue()
        {
            Control.AContoller.Instance.DragSensitivity = RuntimeData.RootContainer.Instance.dragSensitivity;
            Control.AContoller.Instance.IsDragInverted = RuntimeData.RootContainer.Instance.isDragInvert;
        }

#endregion

#endregion

#region Request
        public void Request(Request request, params object[] args)
        {
            switch (request.managerType)
            {
                case Type.Main:

                    break;

                case Type.JSON:
                    switch (request.requestCode)
                    {
                        // 3D GLTF 객체 호출
                        case RequestCode.JSON_ObjectCall:
                            {
                                SceneStatus = ViewSceneStatus.LoadObjects;
                                JSONManager.Instance.LoadObjectToJSON(URI: (string)args[0]);
                            }
                            break;

                        // 손상/보수정보 호출
                        case RequestCode.JSON_IssueCall:
                            {
                                JSONManager.Instance.LoadIssueToJSON(bridgeCode: (string)args[0]);
                            }
                            break;
                    }
                    break;

                case Type.Object:
                    switch (request.requestCode)
                    {
                        // 3D 객체 초기화
                        case RequestCode.Object_Initialize:
                            {
                                SceneStatus = ViewSceneStatus.InitializeObject;
                                if(AppUseCase == UseCase.Bridge)
								{
                                    ObjectManager.Instance.InitializeObject(rootObject: (GameObject)args[0]);
								}
                                else if(AppUseCase == UseCase.Tunnel)
								{
                                    ObjectManager.Instance.InitializeObjectTunnel(_root: (GameObject)args[0]);
								}
                            }
                            break;
                    }
                    break;

                case Type.DimView:
                    switch (request.requestCode)
                    {
                        case RequestCode.DimView_3DCall:
                            {
                                //DimViewManager.Instance.Initial3DSet(_3DObject: (GameObject)args[0]);
                            }
                            break;

                        case RequestCode.DimView_2DCall:
                            {
                                //DimViewManager.Instance.Initial2DSet(_2DObject: (GameObject)args[0]);
                            }
                            break;
                    }
                    break;
            }
        }
#endregion

        public void ToggleMap(GameObject target)
        {
            if(target.activeSelf)
            {
                
            }
        }
    }
}
