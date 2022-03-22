using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Indicator.Element
{
    public class TableElement : AElement, IDragHandler, IScrollHandler
    {
        public RectTransform rootElementPanel;
        [SerializeField] private Scrollbar scrollBar;
        public List<UI.ColumnType> columnDefineList;    // Column 정의 리스트
        public List<Image> columnImageList;             // Column 이미지 리스트
        public List<TMPro.TextMeshProUGUI> columnTextList;  // Colun 문자열 리스트

        private List<Issue.AIssue> issueList;
        private UI.DragCall dragCache;

        private UI.PanelType panelType;                 // TableElement를 생성한 패널
        private Manager.ViewSceneStatus sceneStatus;    // 현재 SceneStatus
        private UI.targetIssue targetIssue;             // 호출 패널과 SceneStatus를 조합해 현재 표시해야할 객체를 정의하는 변수

        private int numOfPanelIndex;

        [SerializeField] private List<AElement> inRecords;

        private void Awake()
        {
            dragCache = new UI.DragCall();
        }

        public override void SetElement(params object[] arg)
        {
            if (arg.Length >= 1 && arg[0].GetType().Equals(typeof(List<Issue.AIssue>)))
            {
                issueList = arg[0] as List<Issue.AIssue>;
            }
            if (arg.Length >= 2 && arg[1].GetType().Equals(typeof(UI.PanelType)))
            {
                panelType = (UI.PanelType)arg[1];
            }
            if (arg.Length >= 3 && arg[2].GetType().Equals(typeof(int)))
            {
                numOfPanelIndex = (int)arg[2];
            }

            sceneStatus = Manager.MainManager.Instance.SceneStatus;

            targetIssue = UI.IssueConverter.GetTargetIssue(panelType: panelType, sceneStatus: sceneStatus);

            // targetIssue가 R2, Image가 아닌경우 아래 실행
            if (targetIssue != UI.targetIssue.R2)
            {
                ClearRecords();
                SetColumnList();
                CreateRecords(issueList);
                SetScrollBar();
            }
            else
            {
                ClearRecords();
                SetColumnList(numOfPanelIndex);
                CreateRecords(issueList, numOfPanelIndex);
                SetScrollBar();
            }
        }

        #region 초기화

        /// <summary>
        /// Table 객체에 존재할수도 있는 내부 단일 요소 정리
        /// </summary>
        private void ClearRecords()
        {
            int index = rootElementPanel.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(rootElementPanel.GetChild(i).gameObject);
            }

            if(inRecords != null)
            {
                inRecords.Clear();
            }
            else
            {
                inRecords = new List<AElement>();
            }
        }

        /// <summary>
        /// 리스트 정의된 Column에 대한 초기화
        /// </summary>
        private void SetColumnList()
        {
            int index = columnDefineList.Count;
            for (int i = 0; i < index; i++)
            {
                //columnImageList[i].sprite = UI.IssueConverter.GetColumnIcon(columnDefineList[i]);
                columnImageList[i].gameObject.SetActive(false);

                if (targetIssue == UI.targetIssue.Damage)
                {
                    columnTextList[i].text = UI.IssueConverter.GetColumnName<Issue.DamagedIssue>(columnDefineList[i]);
                    //columnTextList[i].color = new Color(0, 191 / 255f, 1, 1);
                }
                else if (targetIssue == UI.targetIssue.Recover)
                {
                    columnTextList[i].text = UI.IssueConverter.GetColumnName<Issue.RecoveredIssue>(columnDefineList[i]);
                    //columnTextList[i].color = new Color(0, 191 / 255f, 1, 1);
                }
                else if (targetIssue == UI.targetIssue.Reinforcement)
                {
                    columnTextList[i].text = UI.IssueConverter.GetColumnName<Issue.ReinforcementIssue>(columnDefineList[i]);
                    //columnTextList[i].color = new Color(0, 191 / 255f, 1, 1);
                }
            }
        }

        private void SetColumnList(int _index)
        {
            int index = columnDefineList.Count;
            for (int i = 0; i < index; i++)
            {
                //columnImageList[i].sprite = UI.IssueConverter.GetColumnIcon(columnDefineList[i]);
                columnImageList[i].gameObject.SetActive(false);

                switch (_index)
                {
                    case 1:
                        columnTextList[i].text = UI.IssueConverter.GetColumnName<Issue.RecoveredIssue>(columnDefineList[i]);
                        columnTextList[i].color = new Color(0, 191 / 255f, 1, 1);
                        break;

                    case 2:
                        columnTextList[i].text = UI.IssueConverter.GetColumnName<Issue.ReinforcementIssue>(columnDefineList[i]);
                        columnTextList[i].color = new Color(0, 191 / 255f, 1, 1);
                        break;
                }
            }
        }

        /// <summary>
        /// 스크롤바 내부 크기조정
        /// </summary>
        private void SetScrollBar()
        {
            scrollBar.value = 0;

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _method: UI.DragMethod.ScrollBar,
                _target: rootElementPanel
                );

            UI.Drag.InitializeScrollBar_barSize(
                scrollBar: scrollBar,
                dragCall: dragCache);
        }
        #endregion

        #region 내부 요소 상태변경

        public void ChangeSelectionChildState(AElement record)
        {
            int index = inRecords.Count;
            for (int i = 0; i < index; i++)
            {
                inRecords[i].ToggleRecordColors(inRecords[i] == record);
            }
        }

        #endregion

        #region 내부 요소 생성
        private void CreateRecords(List<Issue.AIssue> _issues)
        {
            int internalIndex = 0;

            if (panelType.Equals(UI.PanelType.BPMM))
            {
                if (Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewAllDamage)
                {
                    int index = _issues.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (_issues[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                        {
                            internalIndex++;

                            //Debug.Log("damaged issue");


                            GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPMM_RecordElement"));
                            obj.transform.SetParent(rootElementPanel);

                            BPMM_VAD_Record record = obj.GetComponent<BPMM_VAD_Record>();
                            record.SetElement(_issues[i], internalIndex);
                        }
                    }
                }
            }
            else if (panelType.Equals(UI.PanelType.BPM1))
            {
                if (Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewPartDamage)
                {
                    int index = _issues.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (_issues[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                        {
                            string selectedPartName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

                            if(selectedPartName == _issues[i].BridgePartName)
                            {
                                internalIndex++;

                                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM1_VPD_RecordElement"));
                                obj.transform.SetParent(rootElementPanel);

                                BPM1_VPD_Record record = obj.GetComponent<BPM1_VPD_Record>();
                                record.SetElement(_issues[i], internalIndex, transform.GetComponent<TableElement>());

                                // 로드 완료된 객체들의 리스트를 생성하기
                                inRecords.Add(record);
                            }
                        }
                    }
                }
                else if (Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewPart2R)
                {
                    int index = _issues.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (_issues[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
                        {
                            string selectedPartName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

                            if(selectedPartName == _issues[i].BridgePartName)
                            {
                                internalIndex++;

                                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM1_VP2R_RecordElement"));
                                obj.transform.SetParent(rootElementPanel);

                                BPM1_VP2R_Record record = obj.GetComponent<BPM1_VP2R_Record>();
                                record.SetElement(_issues[i], internalIndex, transform.GetComponent<TableElement>());

                                // 로드 완료된 객체들의 리스트를 생성하기
                                inRecords.Add(record);
                            }
                        }
                    }
                }
                else if (Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewMaintainance)
                {
                    int index = _issues.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (_issues[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                        {
                            internalIndex++;

                            GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM1_VPD_RecordElement"));
                            obj.transform.SetParent(rootElementPanel);

                            BPM1_VPD_Record record = obj.GetComponent<BPM1_VPD_Record>();
                            record.SetElement(_issues[i], internalIndex);
                        }
                    }
                }
            }
            else if (panelType.Equals(UI.PanelType.BPM2))
            {
                if (Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewPartDamage)
                {
                    int index = _issues.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (_issues[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                        {
                            //string selectedPartName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

                            //if(selectedPartName == _issues[i].BridgePartName)
                            //{
                            //    internalIndex++;

                            //    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM1_VPD_RecordElement"));
                            //    obj.transform.SetParent(rootElementPanel);

                            //    BPM1_VPD_Record record = obj.GetComponent<BPM1_VPD_Record>();
                            //    record.SetElement(_issues[i], internalIndex);
                            //}
                        }
                    }
                }
                else if (Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewPart2R)
                {
                    int index = _issues.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (_issues[i].GetType().Equals(typeof(Issue.ReinforcementIssue)))
                        {
                            string selectedPartName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

                            if(selectedPartName == _issues[i].BridgePartName)
                            {
                                internalIndex++;

                                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM2_VP2R_RecordElement"));
                                obj.transform.SetParent(rootElementPanel);

                                BPM2_VP2R_Record record = obj.GetComponent<BPM2_VP2R_Record>();
                                record.SetElement(_issues[i], internalIndex);
                            }
                        }
                    }
                }
                else if (Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewMaintainance)
                {
                    int index = _issues.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (_issues[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
                        {
                            internalIndex++;

                            GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM1_VP2R_RecordElement"));
                            obj.transform.SetParent(rootElementPanel);

                            BPM1_VP2R_Record record = obj.GetComponent<BPM1_VP2R_Record>();
                            record.SetElement(_issues[i], internalIndex);
                        }

                        else if (_issues[i].GetType().Equals(typeof(Issue.ReinforcementIssue)))
                        {
                            internalIndex++;

                            GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM2_VP2R_RecordElement"));
                            obj.transform.SetParent(rootElementPanel);

                            BPM2_VP2R_Record record = obj.GetComponent<BPM2_VP2R_Record>();
                            record.SetElement(_issues[i], internalIndex);
                        }
                    }
                }
            }
        }

        private void CreateRecords(List<Issue.AIssue> _issues, int index)
        {
            int internalIndex = 0;

            if(panelType.Equals(UI.PanelType.BPM2))
            {
                if(Manager.MainManager.Instance.SceneStatus == Manager.ViewSceneStatus.ViewMaintainance)
                {
                    int _index = _issues.Count;
                    for (int i = 0; i < _index; i++)
                    {
                        if(index == 1)
                        {
                            if(_issues[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
                            {
                                internalIndex++;

                                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM1_VP2R_RecordElement"));
                                obj.transform.SetParent(rootElementPanel);

                                BPM1_VP2R_Record record = obj.GetComponent<BPM1_VP2R_Record>();
                                record.SetElement(_issues[i], internalIndex);
                            }
                        }

                        if(index == 2)
                        {
                            if (_issues[i].GetType().Equals(typeof(Issue.ReinforcementIssue)))
                            {
                                internalIndex++;

                                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Indicator/BPM2_VP2R_RecordElement"));
                                obj.transform.SetParent(rootElementPanel);

                                BPM2_VP2R_Record record = obj.GetComponent<BPM2_VP2R_Record>();
                                record.SetElement(_issues[i], internalIndex);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// State 5 다중패널 생성 / 할당
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_issues"></param>
        public void CreateRecords<T>(List<Issue.AIssue> _issues ) where T : Issue.AIssue
        {
            ClearElementList(); // 기존 element 요소 지우기

            List<T> _table = GetIssueList<T>(_issues);

            int index = _table.Count;
            for (int i = 0; i < index; i++)
            {
                //Debug.Log(_table[i].GetType());

                // 할당된 객체의 수만큼 내부 element 생성
                // 생성할 내부요소 불러옴
                GameObject _element = GetIssueElement<T>();

                GameObject initElement = Instantiate<GameObject>(_element, rootElementPanel);

                initElement.GetComponent<Element.State5_Element>().SetElement<T>(_table[i], i, transform.GetComponent<TableElement>());
            }
        }

        #endregion

        #region State5

        private void ClearElementList()
        {
            int index = rootElementPanel.transform.childCount;
            for (int i = index-1; i >= 0; i--)
            {
                Destroy(rootElementPanel.transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 이슈별로 분류한 새 리스트를 받는다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_issues"></param>
        /// <returns></returns>
        private List<T> GetIssueList<T>(List<Issue.AIssue> _issues) where T : Issue.AIssue
        {
            List<T> _table = new List<T>();

            int index = _issues.Count;
            for (int i = 0; i < index; i++)
            {
                if(_issues[i].GetType() == typeof(T))
                {
                    _table.Add(_issues[i] as T);
                }
            }

            return _table;
        }

        private GameObject GetIssueElement<T>() where T : Issue.AIssue
        {
            GameObject obj;

            if(typeof(T) == typeof(Issue.DamagedIssue))
            {
                obj = Resources.Load<GameObject>("Indicator/State5_Dmg_Element");
            }
            else if(typeof(T) == typeof(Issue.RecoveredIssue))
            {
                obj = Resources.Load<GameObject>("Indicator/State5_Rcv_Element");
            }
            else if(typeof(T) == typeof(Issue.ReinforcementIssue))
            {
                obj = Resources.Load<GameObject>("Indicator/State5_Rein_Element");
            }
            else
            {
                obj = Resources.Load<GameObject>("Indicator/State5_Dmg_Element");
            }

            return obj;
        }

        #endregion


        #region Events

        public void OnDrag(PointerEventData eventData)
        {
            float velocity = Input.GetAxis("Mouse Y");

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _method: UI.DragMethod.MouseWheel,
                _moveVelocity: velocity,
                _target: rootElementPanel);

            UI.Drag.OnControl(dragCache);

            UI.Drag.UpdateScrollBar_barValue(scrollBar, dragCache);
        }

        public void OnScroll(PointerEventData eventData)
        {
            RuntimeData.RootContainer.Instance.IsScrollInPanel = true;

            float velocity = -Input.GetAxis("Mouse ScrollWheel");

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _moveVelocity: velocity,
                _method: UI.DragMethod.MouseWheel,
                _target: rootElementPanel,
                _dragResist: 1f
                );

            UI.Drag.OnControl(dragCache);

            // 휠 스크롤시 스크롤바 상태 갱신
            UI.Drag.UpdateScrollBar_barValue(scrollBar, dragCache);
        }

        public void VerticalScrollValueChange()
        {
            float scrollBarValue = scrollBar.value;

            dragCache.Set(
                _direction: UI.DragDirection.Y,
                _vector: UI.DragVector.TopToBottom,
                _moveVelocity: scrollBarValue,
                _method: UI.DragMethod.ScrollBar,
                _target: rootElementPanel
                );

            //UI.Drag.OnChangeScrollPosition(scrollBarValue, dragCache);

            UI.Drag.OnControl(dragCache);
        }

        #endregion
    }
}
