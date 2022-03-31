using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MODBS_Library;
using UnityEngine.UI;

namespace Manager
{
    public class PositionManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public Camera myCamera;
        public GameObject damagedTarget;
        public GameObject recoveredTarget;
        public Transform circleParent;
        public RectTransform[] skewList;
        public RectTransform[] curveList;
        public RectTransform[] interactableList;
        public RectTransform[] curveSkewList;
        private RectTransform _canvasRect;
        public Transform pointList;

        public GameObject bridgeHoverIcon;

        /// <summary>
        /// bridgeType : 0 = Skew
        ///              1 = Curve
        ///              2 = Interactable
        /// </summary>
        public int bridgeNum;
        private static PositionManager _instance;

        public static PositionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PositionManager>();
                }
                return _instance;
            }
        }
        void Start()
        {
            _canvasRect = circleParent.GetComponent<RectTransform>();
        }

        public void Update()
        {
            //return;
            UpdatePosition(bridgeNum);
        }

        // 시작할때 교량형상에 맞게 해당 교량만 활성화
        public void StartBridgeType(int bridgeNum)
        {
            Transform bridge = GameObject.FindGameObjectWithTag("BridgeType").transform;
            Transform pointList = this.transform;
            switch (bridgeNum)
            {
                case 0: // Skew
                    {
                        bridge.GetChild(0).gameObject.SetActive(true);
                        pointList.GetChild(0).gameObject.SetActive(true);
                        bridgeHoverIcon.SetActive(true);
                    }
                    break;
                case 1: // Curve
                    {
                        bridge.GetChild(1).gameObject.SetActive(true);
                        pointList.GetChild(1).gameObject.SetActive(true);
                        bridgeHoverIcon.SetActive(true);
                    }
                    break;
                case 2: // Interactable
                    {
                        bridge.GetChild(2).gameObject.SetActive(true);
                        pointList.GetChild(2).gameObject.SetActive(true);
                        bridgeHoverIcon.SetActive(true);
                        // TODO : Interactable의 경우 교량의 사이즈에 맞게 pointlist 재배치
                        //SetInsteractablePoint(pointList);
                    }
                    break;
                case 3: // CurveSkew
                    {
                        bridge.GetChild(3).gameObject.SetActive(true);
                        pointList.GetChild(3).gameObject.SetActive(true);
                        bridgeHoverIcon.SetActive(true);
                    }
                    break;
            }
        }

        // 교량에 길이(가변)에 맞게 손상/보수 아이콘을 재배치
        public void SetInsteractablePoint(Transform pointList)
        {
            //Transform interactablePointList = pointList.GetChild(0).transform;
            //Transform[] damageList = new Transform[17];
            //Transform[] recoverList = new Transform[17];
            //List<Transform> listDS = new List<Transform>();
            //int countDS = 0;

            //float minX = 0;
            //float minY = 0;
            //float minZ = 0;
            //float maxX = 0;
            //float maxY = 0;
            //float maxZ = 0;
            //float totalX = 0;
            //float betweenDisX = 0;

            //// 리스트를 손상/보수 두가지로 나눔
            //for (int i = 0; i < 17; i++)
            //{
            //    damageList[i] = pointList.GetChild(0).GetChild(i).transform;
            //    recoverList[i] = pointList.GetChild(0).GetChild(i + 17).transform;
            //}

            //// DS=포장 리스트 생성
            ////for(int i = 0; i < Manager.DimViewManager.Instance.objLevel3.Count; i++)
            ////{
            ////    if (Manager.DimViewManager.Instance.objLevel3[i].name.Contains("DS"))
            ////    {
            ////        listDS.Add(Manager.DimViewManager.Instance.objLevel3[i]);
            ////        countDS++;
            ////    }
            ////}

            //Bounds _b = MainManager.Instance.rootBounds;

            //// 포장면을 기준으로 min, max 값 할당
            //minX = _b.min.x;
            //minY = _b.min.y;
            //minZ = _b.min.z;
            //maxX = _b.max.x;
            //maxY = _b.max.y;
            //maxZ = _b.max.z;

            ////minX = listDS[0].GetComponent<Bridge.ObjectData>().MinVector3.x;
            ////minY = listDS[0].GetComponent<Bridge.ObjectData>().MinVector3.y;
            ////minZ = listDS[0].GetComponent<Bridge.ObjectData>().MinVector3.z;
            ////maxX = listDS[countDS - 1].GetComponent<Bridge.ObjectData>().MaxVector3.x;
            ////maxY = listDS[countDS - 1].GetComponent<Bridge.ObjectData>().MaxVector3.y;
            ////maxZ = listDS[countDS - 1].GetComponent<Bridge.ObjectData>().MaxVector3.z;
            //// 3-1-2. 해당 교량의 부재별 손상/보수 건이 몇건인지 확인
            //// 3-1-3. 손상/보수 건이 없거나 0개인 부재는 표시 객체를 비활성화
            //// 3-1-4. 받아온 좌표값을 바탕으로 손상/보수 position 객체의 위치를 지정
            //totalX = maxX - minX;

            //// CodeLv4의 0번이 null = 0 으로 설정되어 있어서 damageList[0]을 제외하고 처리
            //// null = 0을 제외하면 손상, 보수 각각 11개의 데이터
            //betweenDisX = totalX / (damageList.Length - 2); // 리스트 전체 개수가 12이므로 필요한 i 값은 1~11까지 10개 
            //for (int i = 1; i < damageList.Length; i++)
            //{
            //        damageList[i].position = new Vector3(minX + (betweenDisX * (i - 1)), maxY, maxZ / 4);
            //}
            //for (int i = 1; i < recoverList.Length; i++)
            //{
            //        recoverList[i].position = new Vector3(minX + (betweenDisX * (i - 1)), minY, minZ / 4);
            //}
        }

        public void CheckIssueCount(Dictionary<char, Dictionary<CodeLv4, int>> _index, string name)
        {
            for(int i = 0; i < bridgeHoverIcon.transform.childCount; i++)
            {
                bridgeHoverIcon.transform.GetChild(i).gameObject.SetActive(true);
            }
            //Debug.Log(_index);
            //Debug.Log(name);
            bridgeNum = 0;
            //StartBridgeType(bridgeNum);

            SetInsteractablePoint(pointList);
            int Index1 = _index['D'].Keys.Count;
            if (bridgeNum == 0)
            {
                for (int i = 0; i < Index1; i++)
                {
                    if (i == 17) break;

                    string debugTxt = "";
                    if (_index['D'][(CodeLv4)i] != 0)
                    {
                        //skewList[i].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        debugTxt = "D";
                        skewList[i].GetChild(0).GetChild(2).GetComponent<Text>().text = _index['D'][(CodeLv4)i].ToString();
                    }
                    else
                        skewList[i].gameObject.SetActive(false);

                    if (_index['R'][(CodeLv4)i] != 0)
                    {
                        debugTxt = "R";
                        //skewList[i + 12].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        skewList[i + 17].GetChild(0).GetChild(2).GetComponent<Text>().text = _index['R'][(CodeLv4)i].ToString();
                    }
                    else
                        skewList[i + 17].gameObject.SetActive(false);
                }
                this.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if (bridgeNum == 1)
            {
                for (int i = 0; i < Index1; i++)
                {
                    if (_index['D'][(CodeLv4)i] != 0)
                    {
                        //curveList[i].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        curveList[i].GetChild(0).GetChild(2).GetComponent<Text>().text = _index['D'][(CodeLv4)i].ToString();
                    }
                    else
                        curveList[i].gameObject.SetActive(false);

                    if (_index['R'][(CodeLv4)i] != 0)
                    {
                        //curveList[i + 12].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        curveList[i + 17].GetChild(0).GetChild(2).GetComponent<Text>().text = _index['R'][(CodeLv4)i].ToString();
                    }
                    else
                        curveList[i + 17].gameObject.SetActive(false);
                }
                this.transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (bridgeNum == 2)    // 교량형상을 보여주는 방식이 변경되어 bridgeNum을 2로 강제 할당 중
            {
                for (int i = 0; i < Index1; i++)
                {
                    if (_index['D'][(CodeLv4)i] != 0)
                    {
                        //interactableList[i].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        interactableList[i].GetChild(0).GetChild(1).GetComponent<Text>().text = _index['D'][(CodeLv4)i].ToString();
                    }
                    else
                        interactableList[i].gameObject.SetActive(false);

                    if (_index['R'][(CodeLv4)i] != 0)
                    {
                        //interactableList[i + 12].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        interactableList[i + 12].GetChild(0).GetChild(1).GetComponent<Text>().text = _index['R'][(CodeLv4)i].ToString();
                    }
                    else
                        interactableList[i + 17].gameObject.SetActive(false);
                }
                this.transform.GetChild(2).gameObject.SetActive(false);
            }
            else if (bridgeNum == 3)
            {
                for (int i = 0; i < Index1; i++)
                {
                    if (_index['D'][(CodeLv4)i] != 0)
                    {
                        //curveSkewList[i].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        curveSkewList[i].GetChild(0).GetChild(2).GetComponent<Text>().text = _index['D'][(CodeLv4)i].ToString();
                    }
                    else
                        curveSkewList[i].gameObject.SetActive(false);

                    if (_index['R'][(CodeLv4)i] != 0)
                    {
                        //curveSkewList[i + 12].GetChild(0).GetChild(0).GetComponent<Text>().text = MODBS_Library.LevelCodeConverter.ConvertLv4String((CodeLv4)i);
                        curveSkewList[i + 17].GetChild(0).GetChild(2).GetComponent<Text>().text = _index['R'][(CodeLv4)i].ToString();
                    }
                    else
                        curveSkewList[i + 17].gameObject.SetActive(false);
                }
                this.transform.GetChild(3).gameObject.SetActive(false);
            }
            //if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
			//{
			//}
            //else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
			//{
            //    Debug.LogError("PositionManager.cs // 1101 259");
			//}
        }

        // 카메라 move에 맞춰서 position 객체의 world좌표를 기준으로 viewport 좌표를 갱신
        public void UpdatePosition(int bridgeNum)
        {
            switch (bridgeNum)
            {
                case 0: // skew
                    {
						for (int i = 0; i < skewList.Length; i++)
						{
							Transform worldObject = transform.GetChild(0).GetChild(i);
							Vector2 viewportPosition = myCamera.WorldToViewportPoint(worldObject.position);
							Vector2 worldObjectScreenPosition = new Vector2(
								((viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
								((viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));
							skewList[i].anchoredPosition = worldObjectScreenPosition;
						}
					}
                    break;
                case 1: //curve
                    {
                        //for (int i = 0; i < curveList.Length; i++)
                        //{
                        //    Transform worldObject = transform.GetChild(1).GetChild(i);
                        //    Vector2 viewportPosition = myCamera.WorldToViewportPoint(worldObject.position);
                        //    Vector2 worldObjectScreenPosition = new Vector2(
                        //        ((viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
                        //        ((viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));
                        //    curveList[i].anchoredPosition = worldObjectScreenPosition;
                        //}
                    }
                    break;
                case 2: //interactable
                    {
                        //for (int i = 0; i < interactableList.Length; i++)
                        //{
                        //    Transform worldObject = transform.GetChild(2).GetChild(i);
                        //    Vector2 viewportPosition = myCamera.WorldToViewportPoint(worldObject.position);
                        //    Vector2 worldObjectScreenPosition = new Vector2(
                        //        ((viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
                        //        ((viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));
                        //    interactableList[i].anchoredPosition = worldObjectScreenPosition;
                        //}
                    }
                    break;
                case 3: //curveSkew
                    {
                        //for (int i = 0; i < curveSkewList.Length; i++)
                        //{
                        //    Transform worldObject = transform.GetChild(3).GetChild(i);
                        //    Vector2 viewportPosition = myCamera.WorldToViewportPoint(worldObject.position);
                        //    Vector2 worldObjectScreenPosition = new Vector2(
                        //        ((viewportPosition.x * _canvasRect.sizeDelta.x) - (_canvasRect.sizeDelta.x * 0.5f)),
                        //        ((viewportPosition.y * _canvasRect.sizeDelta.y) - (_canvasRect.sizeDelta.y * 0.5f)));
                        //    curveSkewList[i].anchoredPosition = worldObjectScreenPosition;
                        //}
                    }
                    break;
            }
        }
        public void ClearIssueData()
        {
            for(int i = 0; i < bridgeHoverIcon.transform.childCount; i++)
            {
                bridgeHoverIcon.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}