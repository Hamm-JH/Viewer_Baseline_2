using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Indicator;
using System.IO;
using AdminViewer.API;
//using MODBS_Library;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton

        private static UIManager instance;

        public static UIManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<UIManager>() as UIManager;
                }
                return instance;
            }
        }

        #endregion

        [System.Serializable]
        public struct Env
        {
            public Capture L1_Capture;
            public Capture L2_Capture;
            public Capture L3_Capture;

            public State5_BP1_Indicator L1_CaptureTarget;
            public State5_BP2_Indicator L2_CaptureTarget;

            public string L1_Result;
            public string L2_Result;
            public string L3_Result;
        }

        [System.Serializable]
        public struct DrawingEnv
		{
            public bool isCapEnd_fr;
            public bool isCapEnd_ba;
            public bool isCapEnd_le;
            public bool isCapEnd_ri;
            public bool isCapEnd_to;
            public bool isCapEnd_bo;

            public string fr0_Result;
            public string ba0_Result;
            public string le0_Result;
            public string ri0_Result;
            public string to0_Result;
            public string bo0_Result;

            public string fr_Result;
            public string ba_Result;
            public string le_Result;
            public string ri_Result;
            public string to_Result;
            public string bo_Result;
        }

        [System.Serializable]
        public struct Popups
		{
            public GameObject PopupDrawingNoti;
		}
        //==========================

        public Env env;
        public DrawingEnv drawingEnv;
        public Popups popups;

        public Transform rootViewIssueMesh;

        public Dictionary<Indicator.IndicatorType, RectTransform> panelDic;

        //[SerializeField] private Button dimLineSwitchButton;
        //[SerializeField] private Button graphToggleButton;
        //[SerializeField] private Button gridToggleButton;
        //[SerializeField] private Button reportPrintButton;

        [Header("Toggle Buttons")]
        public bool mpm1Toggle;
        public bool mpm2Toggle;
        public bool graphToggle;
        public bool bpm1Toggle;
        public bool bpm2Toggle;
        public bool gridToggle;

        [HideInInspector] public bool isCapReady1;
        [HideInInspector] public bool isCapReady2;

        public bool isCapEnd1;
        public bool isCapEnd2;
        public bool isCapEnd3;

        private string imgName1;
        private string imgName2;

        private string img1Base64;
        private string img2Base64;

        public ButtonDimView _btnDimView;
        //public Indicator.Element.KeymapElement _keymapElement;
        
        private bool isDimLineVisible;  // DimLine 생성상태 확인변수

        [Header("UI Panels")]
        [SerializeField] private RectTransform KeymapPanel;

        //==========================

        public Dictionary<Indicator.IndicatorType, AIndicator> indicatorDic;
        public Dictionary<Indicator.IndicatorType, bool> indicatorRoutineCheck;

        #region panel & indicators

        [Header("Panels")]
        [SerializeField] private RectTransform TP_Panel;

        [SerializeField] private RectTransform BPM1_Panel;
        [SerializeField] public  RectTransform BPM2_Panel;
        [SerializeField] private RectTransform BPMM_Panel;

        [SerializeField] private RectTransform ImP_Panel;
        [SerializeField] private RectTransform BP_Panel;

        [SerializeField] private RectTransform State5_BP1_Panel;
        [SerializeField] private RectTransform State5_BP2_Panel;
        [SerializeField] private RectTransform State5_MP1_Panel;

        [SerializeField] private RectTransform locationPanel;
        [SerializeField] private RectTransform picturePanel;

        //============

        [Header("Indicators")]
        [SerializeField] private AIndicator TP_Indicator;

        [SerializeField] private AIndicator BPM1_Indicator;
        [SerializeField] private AIndicator BPM2_Indicator;
        [SerializeField] private AIndicator BPMM_Indicator;

        [SerializeField] private AIndicator ImP_Indicator;
        [SerializeField] private AIndicator BP_Indicator;

        [SerializeField] private AIndicator State5_BP1_Indicator;
        [SerializeField] private AIndicator State5_BP2_Indicator;
        [SerializeField] private AIndicator State5_MP1_Indicator;

        [SerializeField] private AIndicator location_Indicator;
        [SerializeField] private AIndicator mainPicture_Indicator;

        #endregion

        private void Awake()
        {
            isDimLineVisible = false;
            indicatorRoutineCheck = new Dictionary<IndicatorType, bool>();
        }

        private void Start()
        {
            SetPanel(ViewSceneStatus.Awake);

            {
                panelDic = new Dictionary<Indicator.IndicatorType, RectTransform>();

                panelDic.Add(IndicatorType.TP, TP_Panel);

                panelDic.Add(IndicatorType.BPM1, BPM1_Panel);
                panelDic.Add(IndicatorType.BPM2, BPM2_Panel);

                panelDic.Add(IndicatorType.BPMM, BPMM_Panel);

                panelDic.Add(IndicatorType.ImP, ImP_Panel);
                panelDic.Add(IndicatorType.BP, BP_Panel);

                panelDic.Add(IndicatorType.State5_BP1, State5_BP1_Panel);
                panelDic.Add(IndicatorType.State5_BP2, State5_BP2_Panel);
                panelDic.Add(IndicatorType.State5_MP1, State5_MP1_Panel);

                panelDic.Add(IndicatorType.Location, locationPanel);
                panelDic.Add(IndicatorType.mainPicture, picturePanel);

            } // 패널 Dic 초기화

            {
                indicatorDic = new Dictionary<IndicatorType, AIndicator>();

                indicatorDic.Add(IndicatorType.TP, TP_Indicator);

                indicatorDic.Add(IndicatorType.BPM1, BPM1_Indicator);
                indicatorDic.Add(IndicatorType.BPM2, BPM2_Indicator);

                indicatorDic.Add(IndicatorType.BPMM, BPMM_Indicator);

                indicatorDic.Add(IndicatorType.ImP, ImP_Indicator);
                indicatorDic.Add(IndicatorType.BP, BP_Indicator);

                indicatorDic.Add(IndicatorType.State5_BP1, State5_BP1_Indicator);
                indicatorDic.Add(IndicatorType.State5_BP2, State5_BP2_Indicator);
                indicatorDic.Add(IndicatorType.State5_MP1, State5_MP1_Indicator);

                indicatorDic.Add(IndicatorType.Location, location_Indicator);
                indicatorDic.Add(IndicatorType.mainPicture, mainPicture_Indicator);
            } // 표시기 Dic 초기화

            {
                ClearMesh();
            } // ViewMaintainance mesh 초기화
        }

        #region Hovering

        [Header("호버링 팝업 패널")]
        [SerializeField] RectTransform hoverObj;
        [SerializeField] HoverPanel hoverPanel;

        public void ToggleHoverPos(bool isOn, Vector3 position = default(Vector3))
        {
            hoverObj.gameObject.SetActive(isOn);

            if(isOn)
            {
                hoverObj.position = new Vector3(position.x, position.y+2, position.z);
            }
            else
            {
                hoverPanel.SetText("");
            }
        }

        public void SetHoverText(string value)
        {
            hoverPanel.SetText(value);
        }

        #endregion

        #region Buttons

        /// <summary>
        /// 치수선 표시 변경 세팅
        /// SceneStatus시에 상태변경
        /// </summary>
        /// <param name="isOn"></param>
        //public void SetDimLine(bool isOn)
        //{
        //    // 치수선 ON 상태일 경우
        //    if(isOn)
        //    {
        //        dimLineSwitchButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/on_btn_01");

        //        // TODO : 치수선 객체 표시 코드 실행
        //    }
        //    // 치수선 OFF 상태일 경우
        //    else
        //    {
        //        dimLineSwitchButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/off_btn_02");
        //        // TODO : 치수선 끄기코드 실행
        //    }
        //}

        /// <summary>
        /// 치수선 버튼 이미지 스위치
        /// </summary>
        public void SwitchDimLineButton()
        {
            isDimLineVisible = !isDimLineVisible;

            SwitchDimLine();

        }

        private void SwitchDimLine()
        {
            // TODO :  치수선 On/Off 메서드 호출
            // 변수 기준 스프라이트 변경
            // DimLine 참이된 경우
            //if (isDimLineVisible)
            //{
            //    dimLineSwitchButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/ruler");
            //}
            //// DimLine 거짓이된 경우
            //else
            //{
            //    dimLineSwitchButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/ruler02");
            //}
        }

        /// <summary>
        /// State 1 : 위치도 On / Off
        /// </summary>
        public void SwitchLocationPanel()
        {
            bool isOn = !locationPanel.gameObject.activeSelf;
            locationPanel.gameObject.SetActive(isOn);

            string result = isOn ? "T" : "F";
#if UNITY_EDITOR
#else
            WebManager.ViewMap(result);
#endif
        }

        /// <summary>
        /// State 1 : 전경사진 On / Off
        /// </summary>
        public void SwitchMainPicturePanel()
        {
            picturePanel.gameObject.SetActive(picturePanel.gameObject.activeSelf ? false : true);
        }

        public void SetButton(ViewSceneStatus sceneStatus)
        {
            // PREV : 메서드 삭제준비

            ButtonOff();

            switch(sceneStatus)
            {
                case ViewSceneStatus.Ready:
                case ViewSceneStatus.ViewAllDamage:
                    break;

                case ViewSceneStatus.ViewPartDamage:
                    SwitchDimLine();
                    break;

                case ViewSceneStatus.ViewPart2R:
                    SwitchDimLine();
                    break;

                case ViewSceneStatus.ViewMaintainance:
                    mpm1Toggle = true;
                    mpm2Toggle = true;
                    graphToggle = true;
                    GraphToggleCheck();

                    bpm1Toggle = true;
                    bpm2Toggle = true;
                    gridToggle = true;
                    GridToggleCheck();
                    break;
            }
        }

        private void ButtonOff()
        {
            mpm1Toggle = false;
            mpm2Toggle = false;
            graphToggle = false;

            bpm1Toggle = false;
            bpm2Toggle = false;
            gridToggle = false;
        }

        #region 좌측 토글 버튼(MPM1,MPM2/BPM1,BPM2)
        public void GraphToggleCheck()
        {
            if (!mpm1Toggle)
            {
                if (!mpm2Toggle)
                {
                    graphToggle = false;
                }
                else
                {
                    graphToggle = true;
                }
            }
            else
            {
                graphToggle = true;
            }
        }

        public void ViewGraphPanel()
        {
            if(panelDic[IndicatorType.State5_BP1].gameObject.activeSelf || panelDic[IndicatorType.State5_BP2].gameObject.activeSelf)
            {
                panelDic[IndicatorType.State5_BP1].gameObject.SetActive(false);
                panelDic[IndicatorType.State5_BP2].gameObject.SetActive(false);
            }
            else
            {
                panelDic[IndicatorType.State5_BP1].gameObject.SetActive(true);
                panelDic[IndicatorType.State5_BP2].gameObject.SetActive(true);
            }
        }

        public void GridToggleCheck()
        {
            if (!bpm1Toggle)
            {
                if (!bpm2Toggle)
                {
                    gridToggle = false;
                }
                else
                {
                    gridToggle = true;
                }
            }
            else
            {
                gridToggle = true;
            }
        }

        public void ViewGridPanel()
        {
            panelDic[IndicatorType.State5_MP1].gameObject.SetActive(!panelDic[IndicatorType.State5_MP1].gameObject.activeSelf);
        }

        #region 버튼 마우스 호버(Pointer In/Out) 이벤트
        public void InViewButtonHoverEvent(int num)
        {
            //if (num == 0)
            //    graphToggleButton.transform.GetChild(1).gameObject.SetActive(true);
            //else if (num == 1)
            //    gridToggleButton.transform.GetChild(1).gameObject.SetActive(true);
        }

        public void OutViewButtonHoverEvent(int num)
        {
            //if (num == 0)
            //    graphToggleButton.transform.GetChild(1).gameObject.SetActive(false);
            //else if (num == 1)
            //    gridToggleButton.transform.GetChild(1).gameObject.SetActive(false);
        }
		#endregion
		#endregion

		// TODO 0721
		#region 도면 보고서 출력 버튼

        public void DrawingReportDownloadButton()
		{
            // 3, 4번째 상태일 경우 다운로드 실행
            bool _dimView = RuntimeData.RootContainer.Instance.dimensionView;
            if(_dimView)
			{
                Manager.ViewSceneStatus _status = Manager.MainManager.Instance.SceneStatus;
                if(_status == ViewSceneStatus.ViewPartDamage || _status == ViewSceneStatus.ViewPart2R)
			    {
                    // 치수선이 켜진 상태에서 작동하도록 만들어야함 (조건)
                    // 부재명을 받아야함.
                    //string partname = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
                    //partname = BridgeCodeConverter.ConvertCode(partname, MODBS_Library.OutOption.AdView_MP2_Indicator);

                    StartCoroutine(OnReadyToDrawingCapture());
			    }
			}
            else
			{
                popups.PopupDrawingNoti.SetActive(true);
			}
		}

        private IEnumerator OnReadyToDrawingCapture()
		{
            string partname = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
            //partname = BridgeCodeConverter.ConvertCode(partname, MODBS_Library.OutOption.AdView_MP2_Indicator);

            bool isDmg = isDmgSceneStatue();

            ResetDrawing();

            yield return new WaitForEndOfFrame();

            // 캡쳐 코드 예시용 (fr)
			{
                // 타겟 카메라 1개
                // 레퍼런스 문자열
                // 레퍼런스 조건값

                // 타겟 인덱스 정수값
                // 0 1 2 3 4 5

                StartCoroutine(CaptureDrawing(RuntimeData.RootContainer.Instance.captureResource.camfr, 0, "FR"));
                StartCoroutine(CaptureDrawing(RuntimeData.RootContainer.Instance.captureResource.camba, 1, "BA"));
                StartCoroutine(CaptureDrawing(RuntimeData.RootContainer.Instance.captureResource.camle, 2, "LE"));
                StartCoroutine(CaptureDrawing(RuntimeData.RootContainer.Instance.captureResource.camri, 3, "RE"));
                StartCoroutine(CaptureDrawing(RuntimeData.RootContainer.Instance.captureResource.camto, 4, "TO"));
                StartCoroutine(CaptureDrawing(RuntimeData.RootContainer.Instance.captureResource.cambo, 5, "BO"));
            }

            while (true)
			{
                if(isEndDrawingCapture(drawingEnv))
				{
                    // 모든 작업이 끝남 다음 단계 진행을 위해 반복문 break
                    break;
				}

                yield return new WaitForEndOfFrame();
			}

            Debug.Log("6 Drawing image complete");

            yield return new WaitForEndOfFrame();

			#region 최종 전달 메서드
#if UNITY_EDITOR
			/*
             * OnReadyToDrawingPrint
             * 1 : 현재 선택된 부재명
             * 2 : 앞면 도면 front
             * 3 : 뒷면 도면 back
             * 4 : 좌면 도면 left
             * 5 : 우면 도면 right
             * 6 : 윗면 도면 top
             * 7 : 밑면 도면 bottom
             * 8 : 앞면 이미지 front
             * 9 : 뒷면 이미지 back
             * 10 : 좌면 이미지 left
             * 11 : 우면 이미지 right
             * 12 : 윗면 이미지 top
             * 13 : 밑면 이미지 bottom
             * 14 : 손상/보수 선택 bool
             */
#else
            WebManager.OnReadyToDrawingPrint(
                _f1: partname,
                _f2:  drawingEnv.fr_Result,
                _f3:  drawingEnv.ba_Result,
                _f4:  drawingEnv.le_Result,
                _f5:  drawingEnv.ri_Result,
                _f6:  drawingEnv.to_Result,
                _f7:  drawingEnv.bo_Result,
                _f8:  drawingEnv.fr0_Result,
                _f9:  drawingEnv.ba0_Result,
                _f10: drawingEnv.le0_Result,
                _f11: drawingEnv.ri0_Result,
                _f12: drawingEnv.to0_Result,
                _f13: drawingEnv.bo0_Result,
                _f14: isDmg
                );
#endif
            #endregion
        }

		private void ResetDrawing()
		{
            drawingEnv.isCapEnd_fr = false;
            drawingEnv.isCapEnd_ba = false;
            drawingEnv.isCapEnd_le = false;
            drawingEnv.isCapEnd_ri = false;
            drawingEnv.isCapEnd_to = false;
            drawingEnv.isCapEnd_bo = false;

            drawingEnv.fr0_Result = "";
            drawingEnv.ba0_Result = "";
            drawingEnv.le0_Result = "";
            drawingEnv.ri0_Result = "";
            drawingEnv.to0_Result = "";
            drawingEnv.bo0_Result = "";

            drawingEnv.fr_Result = "";
            drawingEnv.ba_Result = "";
            drawingEnv.le_Result = "";
            drawingEnv.ri_Result = "";
            drawingEnv.to_Result = "";
            drawingEnv.bo_Result = "";
        }

        private bool isEndDrawingCapture(DrawingEnv env)
		{
            bool result = true;

            result = result
                && env.isCapEnd_fr
                && env.isCapEnd_ba
                && env.isCapEnd_le
                && env.isCapEnd_ri
                && env.isCapEnd_to
                && env.isCapEnd_bo;

            return result;
		}

        private bool isDmgSceneStatue()
		{
            bool result = true;

            Manager.ViewSceneStatus _status = Manager.MainManager.Instance.SceneStatus;
            if(_status == ViewSceneStatus.ViewPartDamage)
			{
                result = true;
			}
            else if(_status == ViewSceneStatus.ViewPart2R)
			{
                result = false;
			}

            return result;
		}

        private IEnumerator CaptureDrawing(Camera targetCam, int index, string layerName)
		{
			{
                targetCam.cullingMask = 1 << LayerMask.NameToLayer(layerName);

                yield return new WaitForEndOfFrame();

                RenderTexture currentTexture = RenderTexture.active;
                RenderTexture.active = targetCam.targetTexture;

                Rect rect = new Rect(0, 0, targetCam.targetTexture.width, targetCam.targetTexture.height);

                byte[] imgByte;
                Texture2D _tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);


                _tex.ReadPixels(rect, 0, 0, false);
                _tex.Apply();

                RenderTexture.active = currentTexture;

                imgByte = _tex.EncodeToPNG();
                DestroyImmediate(_tex);

                string base64 = Convert.ToBase64String(imgByte);

                if (index == 0)
                {
                    drawingEnv.fr0_Result = base64;
                }
                else if (index == 1)
                {
                    drawingEnv.ba0_Result = base64;
                }
                else if (index == 2)
                {
                    drawingEnv.le0_Result = base64;
                }
                else if (index == 3)
                {
                    drawingEnv.ri0_Result = base64;
                }
                else if (index == 4)
                {
                    drawingEnv.to0_Result = base64;
                }
                else if (index == 5)
                {
                    drawingEnv.bo0_Result = base64;
                }
			}

            yield return new WaitForEndOfFrame();

			{
                targetCam.cullingMask = 1 << LayerMask.NameToLayer(layerName) | 1 << 0;

                yield return new WaitForEndOfFrame();

                RenderTexture currentTexture = RenderTexture.active;
                RenderTexture.active = targetCam.targetTexture;

                Rect rect = new Rect(0, 0, targetCam.targetTexture.width, targetCam.targetTexture.height);

                byte[] imgByte;
                Texture2D _tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);


                _tex.ReadPixels(rect, 0, 0, false);
                _tex.Apply();

                RenderTexture.active = currentTexture;

                imgByte = _tex.EncodeToPNG();
                DestroyImmediate(_tex);

                string base64 = Convert.ToBase64String(imgByte);
                
                if (index == 0)
                {
                    drawingEnv.fr_Result = base64;
                    drawingEnv.isCapEnd_fr = true;
                }
                else if (index == 1)
                {
                    drawingEnv.ba_Result = base64;
                    drawingEnv.isCapEnd_ba = true;
                }
                else if (index == 2)
                {
                    drawingEnv.le_Result = base64;
                    drawingEnv.isCapEnd_le = true;
                }
                else if (index == 3)
                {
                    drawingEnv.ri_Result = base64;
                    drawingEnv.isCapEnd_ri = true;
                }
                else if (index == 4)
                {
                    drawingEnv.to_Result = base64;
                    drawingEnv.isCapEnd_to = true;
                }
                else if (index == 5)
                {
                    drawingEnv.bo_Result = base64;
                    drawingEnv.isCapEnd_bo = true;
                }
            }

            //---


            yield break;
        }

		#endregion

		#region 보고서 출력 버튼

		public void ReportDownloadButton()
        {
            // 5번째 상태일 경우 다운로드 실행
            Manager.ViewSceneStatus _status = Manager.MainManager.Instance.SceneStatus;
            if(_status == ViewSceneStatus.ViewMaintainance)
            {
                StartCoroutine(OnReadyToCapture());

                //StartCoroutine(ReadyToCapture());

                // 표 이미지 2장 추가
                //CaptureImages();

                //string _URL = $"{Manager.MainManager.Instance.strReportURL}{"cdBridge="}{Manager.MainManager.Instance.BridgeCode}";
                //Application.OpenURL(_URL);
            }
        }

        private IEnumerator OnReadyToCapture()
        {
            isCapEnd1 = false;
            isCapEnd2 = false;
            isCapEnd3 = false;

            env.L1_Result = "";
            env.L2_Result = "";
            env.L3_Result = "";

            yield return new WaitForEndOfFrame();

            StartCoroutine(env.L1_Capture.GetImage(this, env.L1_CaptureTarget.TogglesBeforeCapture));
            StartCoroutine(env.L2_Capture.GetImage(this, env.L2_CaptureTarget.TogglesBeforeCapture));
            StartCoroutine(env.L3_Capture.GetImage(this));

            while(true)
            {
                if(isCapEnd1 && isCapEnd2 && isCapEnd3)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            //Debug.Log(env.L3_Result);

            Debug.Log("Done************");
            // TODO 0616
#if UNITY_EDITOR
#else
                WebManager.OnReadyToPrint(
                    _fName1: env.L1_Result,
                    _fName2: env.L2_Result,
                    _fName3: env.L3_Result
                    );
#endif
        }

        private IEnumerator ReadyToCapture()
        {
            isCapReady1 = false;
            isCapReady2 = false;

            Indicator.State5_BP1_Indicator _indic1 = indicatorDic[IndicatorType.State5_BP1] as Indicator.State5_BP1_Indicator;
            Indicator.State5_BP2_Indicator _indic2 = indicatorDic[IndicatorType.State5_BP2] as Indicator.State5_BP2_Indicator;

            isCapReady1 = true;
            StartCoroutine(_indic2.ReadyCapture());

            while(true)
            {
                yield return new WaitForEndOfFrame();
                if (isCapReady1 && isCapReady2) break;
            }
            //yield return new WaitUntil(() => isCapReady1 && isCapReady2);

            CaptureImages();

            yield break;
        }

        private void CaptureImages()
        {
            List<RectTransform> panels = new List<RectTransform>();
            panels.Add(panelDic[IndicatorType.State5_BP1]);
            panels.Add(panelDic[IndicatorType.State5_BP2]);

            isCapEnd1 = false;
            isCapEnd2 = false;

            imgName1 = "";
            imgName2 = "";

            img1Base64 = "";
            img2Base64 = "";

            int index = panels.Count;
            for (int i = 0; i < index; i++)
            {
                StartCoroutine(CreateScreenshot(panels[i], i));
            }

            //panelDic[IndicatorType.State5_BP1]
            //panelDic[IndicatorType.State5_BP2]
        }

        private IEnumerator CreateScreenshot(RectTransform _target, int _index)
        {
            yield return new WaitForEndOfFrame();

            #region set value

            string fileLocation = SetFileLocation();
            string fileName = SetFilename(_index);
            if (_index == 0) imgName1 = fileName;
            else if (_index == 1) imgName2 = fileName;

            //string fileRoute =  $@"{fileLocation}\{fileName}";
            string fileRoute = SetFileRoute(_target, _index, fileLocation);

            //Debug.Log($"file Route : {fileRoute}");

            Camera targetCamera = Manager.MainManager.Instance.MainCamera;
            #endregion

            if(!Directory.Exists(fileLocation))     Directory.CreateDirectory(fileLocation);

            //Debug.Log(Application.persistentDataPath);
            //Debug.Log(Application.dataPath);
            //Debug.Log(fileLocation);
            //Debug.Log(SetRelativePath(fileLocation));

            // 캡쳐 전 불투명화
            _target.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            yield return new WaitForEndOfFrame();

            RenderTexture currentTexture = RenderTexture.active;
            RenderTexture.active = targetCamera.targetTexture;

            byte[] imgByte;     // 스크린샷을 Byte로 저장. Texture2D use
            Texture2D tex = new Texture2D(int.Parse(_target.rect.width.ToString()), int.Parse(_target.rect.height.ToString()), TextureFormat.RGB24, false);

            Debug.Log($"target width : {_target.rect.width.ToString()}");
            Debug.Log($"target height : {_target.rect.height.ToString()}");
            Debug.Log($"target rect.x : {_target.rect.x}");
            Debug.Log($"target rect.y : {_target.rect.y}");
            Debug.Log($"target pos.x : {_target.position.x}");
            Debug.Log($"target pos.y ; {_target.position.y}");

            float xPos = _target.rect.x + _target.position.x;
            float yPos = _target.rect.y + _target.position.y;

            tex.ReadPixels(new Rect(xPos, yPos, _target.rect.width, _target.rect.height), 0, 0);

            tex.Apply();

            RenderTexture.active = currentTexture;  // ?

            // Encode texture into PNG
            imgByte = tex.EncodeToPNG();
            DestroyImmediate(tex);

            string base64Image = "";
            base64Image = Convert.ToBase64String(imgByte);  // 이미지 base64 변환

            //Debug.Log(base64Image);

            File.WriteAllBytes(fileRoute, imgByte);

            // 캡쳐 전 투명화 복구
            _target.GetComponent<Image>().color = new Color(1, 1, 1, 0.85f);

            if (_index == 0)
            {
                isCapEnd1 = true;
                img1Base64 = base64Image;
            }
            else if (_index == 1)
            {
                isCapEnd2 = true;
                img2Base64 = base64Image;
            }

            if (isCapEnd1 && isCapEnd2)
            {
//#if UNITY_EDITOR
//#else
//                WebManager.OnReadyToPrint(
//                    _fName1: img1Base64,
//                    _fName2: img2Base64);
//#endif
            }
        }

        private string SetAbsolutePath(string absolutePath)
        {
            return absolutePath;
        }

        private string SetRelativePath(string absolutePath)
        {
            string result = absolutePath;

            result = result.Replace(Application.dataPath, ".");

            Debug.Log(result);

            return result;
        }

        private string SetFileLocation()
        {
            string appPath = Application.dataPath;
            string inFolderName = @"/CaptureImages";
            //return @"D:\GLTFBuild\GraphImages";
            return appPath + inFolderName;
        }

        private string SetFilename(int _index)
        {
            string result = "";

            string bridgeCode = Manager.MainManager.Instance.BridgeCode;

            DateTime nowTime = DateTime.Now;
            string dateCode = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}",
                nowTime.Year, nowTime.Month, nowTime.Day,
                nowTime.Hour, nowTime.Minute);

            string fileCode = "";
            if(_index == 0)
            {
                fileCode = "F1";
            }
            else if(_index == 1)
            {
                fileCode = "F2";
            }

            result = $"{bridgeCode},{dateCode},{fileCode}.png";

            return result;
        }

        

        private string SetFileRoute(RectTransform _target, int _index, string _fileLocation)
        {
            string fileRoute = "";

            string bridgeCode = Manager.MainManager.Instance.BridgeCode;

            // 파일 위치 지정
            //string fileLocation = @"D:\GLTFBuild\GraphImages";

            string fileName = string.Format(@"\{0}", SetFilename(_index)); // @"\" + "1" + _index + ".png"; //SetFilename(_index);
            //string fileName = string.Format(@"\{0}.png", $"{bridgeCode}_GraphImage_{string.Format("{0:00}", _index)}", _target.gameObject.name + _index);

            fileRoute = _fileLocation + fileName;

            return fileRoute;
        }

#endregion

        // TODO 0105
        public void SetKeymapToOrbit()
		{
            MainManager.Instance.isRecursived = false;
            ViewSceneStatus _stat = MainManager.Instance.SceneStatus;   // set 프로퍼티를 건드리지 않고 get 프로퍼티로 현재 상태 받아옴
            MainManager.Instance.SceneStatus = _stat;                   // set 프로퍼티로 status 재실행 (무한루프 방지용)
		}

#endregion

#region Panels
        public void SetPanel(ViewSceneStatus sceneStatus)
        {
            PanelOff();
            switch (sceneStatus)
            {
                case ViewSceneStatus.Ready:
                    {
                        TP_Panel.gameObject.SetActive(true);

                        locationPanel.gameObject.SetActive(true);
                        picturePanel.gameObject.SetActive(true);
                    }
                    break;

                case ViewSceneStatus.ViewAllDamage:
                    {
                        TP_Panel.gameObject.SetActive(true);
                        BPMM_Panel.gameObject.SetActive(true);
                    }
                    break;

                case ViewSceneStatus.ViewPartDamage:
                    {
                        TP_Panel.gameObject.SetActive(true);

                        BPM1_Panel.gameObject.SetActive(true);
                        BPM2_Panel.gameObject.SetActive(true);

                        KeymapPanel.gameObject.SetActive(true);
                    }
                    break;

                case ViewSceneStatus.ViewPart2R:
                    {
                        TP_Panel.gameObject.SetActive(true);

                        BPM1_Panel.gameObject.SetActive(true);
                        BPM2_Panel.gameObject.SetActive(true);

                        KeymapPanel.gameObject.SetActive(true);
                    }
                    break;

                case ViewSceneStatus.ViewMaintainance:
                    {
                        TP_Panel.gameObject.SetActive(true);

                        State5_BP1_Panel.gameObject.SetActive(true);
                        State5_BP2_Panel.gameObject.SetActive(true);
                        State5_MP1_Panel.gameObject.SetActive(true);
                    }
                    break;
            }
        }

        /// <summary>
        /// 모든 패널 Off
        /// </summary>
        private void PanelOff()
        {
            locationPanel.gameObject.SetActive(false);
            picturePanel.gameObject.SetActive(false);

            KeymapPanel.gameObject.SetActive(false);
            
            TP_Panel.gameObject.SetActive(false);
            BPM1_Panel.gameObject.SetActive(false);
            BPM2_Panel.gameObject.SetActive(false);
            BPMM_Panel.gameObject.SetActive(false);

            ImP_Panel.gameObject.SetActive(false);

            State5_BP1_Panel.gameObject.SetActive(false);
            State5_BP2_Panel.gameObject.SetActive(false);
            State5_MP1_Panel.gameObject.SetActive(false);
        }
#endregion

#region ViewMaintainanceMesh

        public void ClearMesh()
        {
            int index = rootViewIssueMesh.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(rootViewIssueMesh.GetChild(i).gameObject);
            }
        }

#endregion

#region 표시기에 알맞은 정보 배치

        /// <summary>
        /// TODO Indicator : SceneStatus의 정보를 변경할때 또는 표시기의 정보를 바꿔야 할때 호출하는 메서드
        /// </summary>
        /// <param name="issues"></param>
        /// <param name="sceneStatus"></param>
        public IEnumerator SetIndicators(List<Issue.AIssue> issues, ViewSceneStatus sceneStatus)
        {
            yield return new WaitForEndOfFrame();

            switch(sceneStatus)
            {
                case ViewSceneStatus.ViewPartDamage:
                case ViewSceneStatus.ViewPart2R:
                    yield return new WaitUntil(() => RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform != null);
                    break;

                case ViewSceneStatus.ViewMaintainance:
                    break;
            }

            // 표시기 정보배치 완료 확인코드 비우기
            if(indicatorRoutineCheck != null)
            {
                indicatorRoutineCheck.Clear();
            }
            else
            {
                indicatorRoutineCheck = new Dictionary<IndicatorType, bool>();
            }

            gameObject.GetComponent<Canvas>().enabled = false;
            gameObject.GetComponent<CanvasScaler>().enabled = false;

            ClearMesh();

            // panel의 개수 받음
            int index = panelDic.Count;
            for (int i = 0; i < index; i++)
            {
                // dictionary 내부의 패널이 active 상태인지 확인
                if( panelDic[(IndicatorType)i].gameObject.activeSelf )
                {
#if UNITY_EDITOR
                    EditDebug.PrintPANELRoutine($"activated : {panelDic[(IndicatorType)i].name}");
#endif
                    indicatorRoutineCheck.Add((IndicatorType)i, false);         // 표시기 정보배치 완료 확인코드 추가
                    indicatorDic[(IndicatorType)i].SetPanelElements(issues);    // 손상/보수 정보 리스트 켜져있는 표시기에 전달
                }
                // active 상태가 아닌 객체 표시(확인되지 않는다)
                else
                {
#if UNITY_EDITOR
                    EditDebug.PrintPANELRoutine($"deActivated : {panelDic[(IndicatorType)i].name}");
#endif
                }
            }

            StartCoroutine(checkPanelSetRoutine(indicatorRoutineCheck));

            yield break;
        }

#endregion

#region 각 표시기의 정보배치 완료 코드 받기

        public void GetRoutineCode(IndicatorType type)
        {
            if(indicatorRoutineCheck.ContainsKey(type))
            {
                indicatorRoutineCheck[type] = true;
            }
        }

#endregion

#region 각 표시기 정보배치 완료시 실행 코루틴 코드

        private IEnumerator checkPanelSetRoutine(Dictionary<IndicatorType, bool> _routineCheck)
        {
            yield return new WaitForEndOfFrame();

            IndicatorType[] types = _routineCheck.Keys.ToArray<IndicatorType>();

            int index = types.Length;
            for (int i = 0; i < index; i++)
            {
                if(_routineCheck.ContainsKey(types[i]))
                {
                    yield return new WaitUntil(() => _routineCheck[types[i]] == true);
                }
            }

            bool result = true;
            for (int i = 0; i < index; i++)
            {
                result = result && _routineCheck[types[i]];
            }

            if(result == true)
            {
                gameObject.GetComponent<Canvas>().enabled = true;
                gameObject.GetComponent<CanvasScaler>().enabled = true;
            }

            // TODO : 스크린샷 기능 적용 테스트
            //ScreenCaptureManager.Instance.ScreenCapture();

            yield break;
        }

#endregion
    }
}
