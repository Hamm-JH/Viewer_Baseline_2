using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Indicator
{
    public enum ImP_option
    {
        Null,
        All,
        Damage,
        DamageInDay,
        Recover,
        Reinforcement
    }

    [System.Serializable]
    public class ImageIndex
    {
        public string date;    // 날짜
        public Issue.IssueCode issueCode;   // 손상정보
        public string description;          // 설명
        public ImP_option imgOption;        // 출력옵션
        public string issueOrderCode;       // 손상코드

        public string fgroup;
        public List<Dictionary<string, string>> imgList;
        public int requestIndex;

        public ImageIndex()
        {
            date = "";
            issueCode = IssueCode.Null;
            description = "";
            imgOption = ImP_option.Null;
            issueOrderCode = "";

            fgroup = "";
            imgList = new List<Dictionary<string, string>>();
            requestIndex = -1;
        }

        public ImageIndex(ImageIndex clone, int index)
        {
            date = clone.date;
            issueCode = clone.issueCode;
            description = clone.description;
            imgOption = clone.imgOption;
            issueOrderCode = clone.issueOrderCode;

            fgroup = clone.fgroup;
            imgList = clone.imgList;
            requestIndex = index;
        }

        public ImageIndex(string _date, Issue.IssueCode _issueCode, string _description, ImP_option _imgOption, string _issueOrderCode, 
            string _fgroup, List<Dictionary<string, string>> _imgList)
        {
            date = _date;
            issueCode = _issueCode;
            description = _description;
            imgOption = _imgOption;
            issueOrderCode = _issueOrderCode;

            fgroup = _fgroup;
            imgList = _imgList;
            requestIndex = -1;
        }
    }

    public class ImP_Indicator : AIndicator, IDragHandler, IScrollHandler
    {
        [System.Serializable]
        public struct TagElement
        {
            public GameObject obj;
            public Image icon;
            public Image underLine;
            public TextMeshProUGUI text;
        }

        [Header("UI, 이벤트 관리요소")]
        [SerializeField] private Scrollbar scrollBar;
        [SerializeField] private UI.DragCall dragCache;
        
        [Header("이미지")]
        // 이미지의 수
        public int imgIndex;

        // 입력 : 이미지별 정보 수집 인스턴스 리스트
        public List<ImageIndex> imageIndexList;
        
        // 출력 : 이미지별 UI 표출 인스턴스 리스트
        public List<Element.Imp_element> imageList;

        // 이미지 분류에 활용할 관리정보
        AIssue targetIssue;

        // 이미지 패널 진입시 타이틀 정보창 표시, 관련 이미지 표시 분류를 위한 옵션
        ImP_option viewOption;

        // 사진 확대 인스턴스
        public GameObject enlargeImage;

        [SerializeField] List<TagElement> tags;

        [Header("All tag")]
        [SerializeField] Image allIcon;
        [SerializeField] Image allLine;
        [SerializeField] TextMeshProUGUI allText;

        [Header("Dmg tag")]
        [SerializeField] Image dmgIcon;
        [SerializeField] Image dmgLine;
        [SerializeField] TextMeshProUGUI dmgText;

        [Header("rcv tag")]
        [SerializeField] Image rcvIcon;
        [SerializeField] Image rcvLine;
        [SerializeField] TextMeshProUGUI rcvText;


        public float overDescriptionHeight;

        #region Singleton

        private static ImP_Indicator instance;

        public static ImP_Indicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ImP_Indicator>() as ImP_Indicator;
                }
                return instance;
            }
        }

        #endregion

        private void Awake()
        {
            dragCache = new UI.DragCall();
            imageList = new List<Element.Imp_element>();
        }

        #region 이미지 패널 시작
        public void SetPanelElements(AIssue _issue, ImP_option initOption)
        {
            Manager.UIManager.Instance.GetComponent<Canvas>().enabled = false;
            Manager.UIManager.Instance.GetComponent<CanvasScaler>().enabled = false;

            targetIssue = _issue;
            viewOption = initOption;

            ClearElements(); // 있던 요소 지우기
            RequestImages(targetIssue, initOption); // 
            SetViewOption(viewOption);  // 타이틀 세팅 변경 (없애기)
        }
        #endregion

        #region  request data import

        protected override void ClearElements()
        {
            ElementPanel.localPosition = new Vector3(0, 0, 0);

            int index = ElementPanel.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(ElementPanel.GetChild(i).gameObject);
            }

            imageList.Clear();
        }

        private void RequestImages(AIssue _issue, ImP_option option)
        {
            // 교량코드 필요
            string bridgeCode = Manager.MainManager.Instance.BridgeCode;
            // 특정 부재명
            string partName = _issue.BridgePartName;

            Debug.Log(bridgeCode);
            Debug.Log(partName);

            Manager.JSONManager.Instance.LoadHistory(
                Manager.JSONLoadType.HistoryImage,
                bridgeCode: bridgeCode,
                query: $"&cdTunnelParts={_issue.BridgePartName}",
                issue: _issue,
                option: option
                );

            //Manager.JSONManager.Instance.LoadHistory(
            //    Manager.JSONLoadType.HistoryImage,
            //    bridgeCode: bridgeCode,
            //    query: $"&cdBridgeParts=SP02_DS_AP",
            //    restrict: "20201106-00000005"
            //    );
        }



        public void SetViewOption(ImP_option _viewOption)
        {
            AlloffOption();

            //Color onColor = new Color(0xb8 / 255f, 0xc6 / 255f, 0xd3 / 255f, 1);
            Color onColor = new Color(0x50 / 255f, 0x5a / 255f, 0x66 / 255f, 1);

            switch (_viewOption)
            {
                case ImP_option.All:
                    tags[0].obj.SetActive(true);
                    tags[0].icon.color = onColor;
                    tags[0].underLine.color = onColor;
                    tags[0].text.color = onColor;
                    break;

                case ImP_option.Damage:
                case ImP_option.DamageInDay:
                    tags[1].obj.SetActive(true);
                    tags[1].icon.color = onColor;
                    tags[1].underLine.color = onColor;
                    tags[1].text.color = onColor;
                    break;

                case ImP_option.Recover:
                    tags[2].obj.SetActive(true);
                    tags[2].icon.color = onColor;
                    tags[2].underLine.color = onColor;
                    tags[2].text.color = onColor;
                    break;
            }
        }

        private void AlloffOption()
        {
            int index = tags.Count;
            for (int i = 0; i < index; i++)
            {
                tags[i].obj.SetActive(false);
            }
            //Color offColor = new Color(0xb8 / 255f, 0xc6 / 255f, 0xd3 / 255f, 1);
            //allIcon.color = offColor;
            //allLine.color = offColor;
            //allText.color = offColor;

            //dmgIcon.color = offColor;
            //dmgLine.color = offColor;
            //dmgText.color = offColor;

            //rcvIcon.color = offColor;
            //rcvLine.color = offColor;
            //rcvText.color = offColor;
        }   

        #endregion

        #region import datas

        public void SetImages(List<ImageIndex> _imageList)
        {
            imageIndexList = _imageList;

            Dictionary<string, int> dateIndex = new Dictionary<string, int>();
            Dictionary<string, int> yearIndex = new Dictionary<string, int>();

            int index = imageIndexList.Count;
            for (int i = 0; i < index; i++)
            {
                int _index = imageIndexList[i].imgList.Count;
                for (int ii = 0; ii < _index; ii++)
                {
                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/ImP_ImgContent"));
                    Element.Imp_element element = obj.GetComponent<Element.Imp_element>();


                    ImageIndex imgIndex = new ImageIndex(
                        clone: imageIndexList[i],
                        index: ii
                        );

                    //element.SetElement(imgIndex, true);
                    //Debug.Log(imgIndex.date);
                    string year = imgIndex.date.Split('-')[0];

                    bool isFirstDate = !dateIndex.ContainsKey(imgIndex.date);
                    bool isFirstYear = !yearIndex.ContainsKey(year);

                    if (isFirstDate) dateIndex.Add(imgIndex.date, 0);
                    if (isFirstYear) yearIndex.Add(year, 0);

                    //if (dateIndex.ContainsKey(imgIndex.date))
                    //{
                    //    element.SetElement(imgIndex, false);
                    //}
                    //else
                    //{
                    //    dateIndex.Add(imgIndex.date, 0);
                    //    element.SetElement(imgIndex, true);
                    //}

                    element.SetElement(
                        _imgIndex: imgIndex,
                        isFirstDate: isFirstDate,
                        isFirstYear: isFirstYear
                        );

                    imageList.Add(element);

                    obj.transform.SetParent(ElementPanel);
                }
            }

            Debug.Log("^^^^^^^^ SetImages");
            overDescriptionHeight = 0;

            index = imageList.Count;
            for (int i = 0; i < index; i++)
            {
                float indexOverTextHeight = GetOverTextHeight(imageList[i]);
                
                if(indexOverTextHeight > 0)
                {
                    if(overDescriptionHeight <= indexOverTextHeight)
                    {
                        overDescriptionHeight = indexOverTextHeight;
                    }
                }
            }

            imgIndex = _imageList.Count;
            SetScrollBar();
            ResizingDescription();
            //TagData(viewOption);

            Manager.UIManager.Instance.GetComponent<Canvas>().enabled = true;
            Manager.UIManager.Instance.GetComponent<CanvasScaler>().enabled = true;
        }

        #endregion

        private float GetOverTextHeight(Element.Imp_element indexElement)
        {
            float result = 0;

            float panelHeight = indexElement.description.GetComponent<RectTransform>().rect.height;
            float indexTextHeight = indexElement.description.preferredHeight;

            result = indexTextHeight - panelHeight;

            return result;
        }

        private void SetScrollBar()
        {
            scrollBar.value = 0;

            dragCache.Set(
                _direction: UI.DragDirection.X,
                _vector: UI.DragVector.LeftToRight,
                _method: UI.DragMethod.ScrollBar,
                _target: ElementPanel
                );

            UI.Drag.InitializeScrollBar_barSize(
                scrollBar: scrollBar,
                dragCall: dragCache
                );
        }

        public void TagData(ImP_option _option)
        {
            viewOption = _option;

            SetViewOption(viewOption);
            ChangeOnViewList(viewOption);
        }

        private void ChangeOnViewList(ImP_option _option)
        {
            int index = imageList.Count;
            for (int i = 0; i < index; i++)
            {
                if(_option.Equals(ImP_option.All))
                {
                    imageList[i].gameObject.SetActive(true);
                }
                else if(_option.Equals(imageList[i].DataOption))
                {
                    imageList[i].gameObject.SetActive(true);
                }
                else
                {
                    imageList[i].gameObject.SetActive(false);
                }
            }
        }

        private void ResizingDescription()
        {
            for(int i = 0; i < imageList.Count; i++)
            {
                imageList[i].GetComponent<Indicator.Element.Imp_element>().SetDescriptionLength();
            }
        }

        #region #test
        /// <summary>
        /// 테스트용
        /// </summary>
        /// <param name="_issue"></param>
        public override void SetPanelElements(List<AIssue> _issue)
        {
            //Debug.Log("ImP panel debug");

            //// TODO 1214 debug
            //AIssue issue = RuntimeData.RootContainer.Instance.RootIssueObject.GetChild(2).GetComponent<AIssue>();

            //ClearElements();

            //SetPanelElements(issue, ImP_option.Damage);
            ////RequestImages(issue, ImP_option.All);

            //Manager.UIManager.Instance.GetRoutineCode(IndicatorType.ImP);
        }

        protected override void SetElements(List<AIssue> _issue)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetTitleText()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Events
        /// <summary>
        /// 이미지 패널 드래그시 이벤트 발생
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            float velocity = Input.GetAxis("Mouse X");

            dragCache.Set(
                _direction: UI.DragDirection.X,
                _vector: UI.DragVector.LeftToRight,
                _method: UI.DragMethod.MouseWheel,
                _moveVelocity: velocity,
                _target: ElementPanel
                );

            UI.Drag.OnControl(dragCache);

            UI.Drag.UpdateScrollBar_barValue(scrollBar, dragCache);

            //GetMargin();

            if(overDescriptionHeight > 0)
            {
                velocity = Input.GetAxis("Mouse Y");

                dragCache.Set(
                    _direction: UI.DragDirection.TemplateY,
                    _vector: UI.DragVector.TopToBottom,
                    _method: UI.DragMethod.MouseWheel,
                    _moveVelocity: velocity,
                    _target: ElementPanel,
                    _overTextHeight: overDescriptionHeight
                    );

                

                UI.Drag.OnControl(dragCache);
            }
            
        }

        private void GetMargin()
        {
            if(imageList.Count != 0)
            {
                Vector4 v4 = imageList[0].description.margin;

                Debug.Log(imageList[0].description.preferredHeight);
                Debug.Log(imageList[0].description.renderedHeight);

                Debug.Log($"Left : {v4.x}");
                Debug.Log($"Top : {v4.y}");
                Debug.Log($"Right : {v4.z}");
                Debug.Log($"Bottom : {v4.w}");
            }
        }


        public void OnScroll(PointerEventData eventData)
        {
            RuntimeData.RootContainer.Instance.IsScrollInPanel = true;

            float velocity = -Input.GetAxis("Mouse ScrollWheel");

            dragCache.Set(
                _direction: UI.DragDirection.X,
                _vector: UI.DragVector.LeftToRight,
                _moveVelocity: velocity,
                _method: UI.DragMethod.MouseWheel,
                _target: ElementPanel,
                _dragResist: 1f
                );

            UI.Drag.OnControl(dragCache);

            UI.Drag.UpdateScrollBar_barValue(scrollBar, dragCache);
        }

        public void HorizontalScrollValueChange()
        {
            float scrollBarValue = scrollBar.value;

            dragCache.Set(
                _direction: UI.DragDirection.X,
                _vector: UI.DragVector.LeftToRight,
                _moveVelocity: scrollBarValue,
                _method: UI.DragMethod.ScrollBar,
                _target: ElementPanel
                );

            UI.Drag.OnControl(dragCache);
        }

        #endregion

        public void EnlargePopupClose()
        {
            enlargeImage.SetActive(false);
        }
    }
}
