using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeData
{
    [System.Serializable]
    public class SelectedObject
    {
        public object item;
        public Transform itemTransform;

        public SelectedObject() { }
        public SelectedObject(object _item, Transform _itemTransform)
        {
            item = _item;
            itemTransform = _itemTransform;
        }

        public void Set(object _item, Transform _itemTransform)
        {
            item = _item;
            itemTransform = _itemTransform;
        }
    }

    [System.Serializable]
    public class BridgeList
    {
        public string bridgeCode;
        public string address;

        public void Set(string _bridgeCode, string _address)
        {
            bridgeCode = _bridgeCode;
            address = _address;
        }
    }

    [System.Serializable]
    public class SubCam
    {
        public Transform rootTransform;
        public Transform camTransform;
        public Camera subCamera;
    }

    public class RootContainer : MonoBehaviour
    {
        #region Singleton
        private static RootContainer instance;

        public static RootContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<RootContainer>() as RootContainer;
                }
                return instance;
            }

        }
        #endregion

        [System.Serializable]
        public struct DrawingCaptureResource
		{
            public RenderTexture texfr;
            public RenderTexture texba;
            public RenderTexture texle;
            public RenderTexture texri;
            public RenderTexture texto;
            public RenderTexture texbo;

            public Camera camfr;
            public Camera camba;
            public Camera camle;
            public Camera camri;
            public Camera camto;
            public Camera cambo;
        }

        public EventSystem eventSystem;

        // 전경사진 파일내용
        private string mainFid;         public string MainFid       { get => mainFid; set => mainFid = value; }
        private string mainFType;       public string MainFType     { get => mainFType; set => mainFType = value; }
        private string mainFGroup;      public string MainFGroup    { get => mainFGroup; set => mainFGroup = value; }


        [Header("교량코드 리스트")]
        public List<BridgeList> bridgeCodeList;

        [Header("L3 이미지 카메라 위치")]
        public SubCam subCamL3;

        [Header("Select information")]
        public SelectedObject selectedInstance;
        public SelectedObject cachedInstance;
        public SelectedObject cached3DInstance;
        public SelectedObject cachedIssueInstance;

        [Header("Camera")]
        public MGS.UCamera.AroundAlignCamera mainCam;
        public MGS.UCamera.AroundAlignCamera subCam;
        //public UnityTemplateProjects.SimpleCameraController subFreeCam;
        public Transform mainCamTarget;
        public Transform subCamTarget;

        [Header("Global status")]
        public bool dimensionView;
        public bool recordView;

        [Header("Bridge objects")]
        public GameObject RootObject;
        public GameObject Root2DObject;
        public GameObject Root3DObject;

        [Header("Bridge part contained dictionary")]
        //public Dictionary<MODBS_Library.CodeLv4, bool> partDics;

        [Header("Address")]
        public string Address;
        public string BridgeName;

        [Header("Optional objects")]
        public Transform rootTempObj;
        public Transform rootDimBox;
        public Transform rootRecordIssue;
        public Transform rootViewIssue;

        [Header("Issue objects")]
        public Transform RootIssueObject;
        public List<Issue.AIssue> IssueObjectList;

        [Header("Cache bridge objects")]
        public GameObject skewBridge;
        public GameObject curveBridge;
        public GameObject interactableBridge;
        public GameObject curveSkewBridge;

        public float panningExtensionBorder;
        public float dragSensitivity;
        public bool isDragInvert;
        public bool IsScrollInPanel { get; set; }

        [HideInInspector] public bool isObjectRoutineEnd;   // 객체 생성루틴 종료 확인
        [HideInInspector] public bool isIssueRoutineEnd;    // 손상/보수 루틴 종료 확인

        [Header("6면 도면 이미지")]
        public DrawingCaptureResource captureResource;

        private void Awake()
        {
            bridgeCodeList = new List<BridgeList>();

            selectedInstance = null;
            cachedInstance = null;
            cached3DInstance = null;

            dimensionView = false;
            recordView = false;

            IssueObjectList = new List<Issue.AIssue>();

            panningExtensionBorder = 7f;
            dragSensitivity = 1f;
            isDragInvert = true;
            IsScrollInPanel = false;

            isObjectRoutineEnd = false;
            isIssueRoutineEnd = false;

            InitPartDics();

            StartCoroutine(ScrollPanelControl());
        }

        /// <summary>
        /// 교량에 포함된 부재들을 확인하는 사전
        /// </summary>
        private void InitPartDics()
        {
            //partDics = new Dictionary<MODBS_Library.CodeLv4, bool>();

            //int index = Enum.GetValues(typeof(MODBS_Library.CodeLv4)).Length;
            //for (int i = 0; i < index; i++)
            //{
            //    partDics.Add((MODBS_Library.CodeLv4)i, false);
            //}
        }

        private IEnumerator ScrollPanelControl()
        {
            while (true)
            {
                yield return new WaitUntil(() => IsScrollInPanel == true);

                yield return new WaitForSeconds(1f);

                IsScrollInPanel = false;

                //Debug.Log("Scroll");

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
