 using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using MODBS_Library;

namespace Indicator
{
    public class BPM1_Indicator : AIndicator
    {
        //Manager.ViewSceneStatus sceneStatus;

        [Header("main title")]
        [SerializeField] private Image mainTitleImage;
        [SerializeField] private TextMeshProUGUI mainTitleText;
        [SerializeField] private TextMeshProUGUI mainTitleCount;

        [Header("set element")]
        public RectTransform rootElementPanel;
        public Element.AElement element;
        public int issueIndex;

        [SerializeField] private GameObject closeButton;
        #region Singleton

        private static BPM1_Indicator _instance;

        public static BPM1_Indicator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<BPM1_Indicator>();
                }
                return _instance;
            }
        }

        #endregion
        public override void SetPanelElements(List<AIssue> _issue)
        {
            //Debug.Log("BPM1 panel set element");

            sceneStatus = Manager.MainManager.Instance.SceneStatus;
            issueIndex = GetElementIndex(_issue);

            //ActiveCloseButton();
            SetTitleText();
            ClearElements();
            SetElements(_issue);
            
        }

        #region element index
        private int GetElementIndex(List<AIssue> _issue)
        {
            int result = 0;

            if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
            {
                result = GetIssueIndex<Issue.DamagedIssue>(_issue);
            }
            else if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
            {
                result = GetIssueIndex<Issue.RecoveredIssue>(_issue);
            }
            else if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            {
                result = GetIssueIndex<Issue.DamagedIssue>(_issue);
            }

            return result;
        }

        private int GetIssueIndex<T>(List<AIssue> _issue) where T : Issue.AIssue
        {
            int result = 0;

            int index = _issue.Count;
            for (int i = 0; i < index; i++)
            {
                if(_issue[i].GetType().Equals(typeof(T)))
                {
                    result++;
                }
            }

            return result;
        }
        #endregion

        private void SetTitleImage(Manager.ViewSceneStatus _status, Image _target)
        {
            switch(_status)
            {
                // 손상정보 보기 단계
                case Manager.ViewSceneStatus.ViewPartDamage:
                    {
                        _target.sprite = UI.IssueConverter.GetMainIcon<Issue.DamagedIssue>();
                        _target.color = UI.Colors.Instance.Set(UI.Colors.Palette.White);
                    }
                    break;

                // 보수/보강정보 보기 단계
                case Manager.ViewSceneStatus.ViewPart2R:
                    {
                        _target.sprite = UI.IssueConverter.GetMainIcon<Issue.RecoveredIssue>();
                        _target.color = UI.Colors.Instance.Set(UI.Colors.Palette.CustomBlue1);
                    }
                    break;

                // 정보 요약단계
                case Manager.ViewSceneStatus.ViewMaintainance:

                    break;
            }
        }

        private void SetTitleText(Manager.ViewSceneStatus _status, TextMeshProUGUI _target, string mainTxt = "", string selected = "", int count = 0)
        {
            switch(_status)
            {
                // 손상정보 보기 단계
                case Manager.ViewSceneStatus.ViewPartDamage:
                    {
                        string selectedColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomCyan1);
                        string countColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomRed1);
                        string mainColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomGrey);

                        selected = MasterTemplate.FinalReplace(selected);
                        string _selected = $"<color=#{selectedColorCode}>{selected}</color>";
                        //Debug.Log(_selected);
                        string _count = string.Format("<color=#{0}>{1}</color>", countColorCode, string.Format("{0:000}", count));
                        string _main = $"<color=#{mainColorCode}>{Tunnel.TunnelConverter.GetName(mainTxt)} ({_selected}) {_count} 건</color>";

                        _target.text = _main;
                    }
                    break;

                case Manager.ViewSceneStatus.ViewPart2R:
                    {
                        string selectedColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomCyan1);
                        string countColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomBlue1);
                        string mainColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomGrey);

                        selected = MasterTemplate.FinalReplace(selected);
                        string _selected = $"<color=#{selectedColorCode}>{selected}</color>";
                        string _count = string.Format("<color=#{0}>{1}</color>", countColorCode, string.Format("{0:000}", count));
                        string _main = $"<color=#{mainColorCode}>{Tunnel.TunnelConverter.GetName(mainTxt)} ({_selected}) {_count} 건</color>";

                        _target.text = _main;
                    }
                    break;

                case Manager.ViewSceneStatus.ViewMaintainance:

                    break;
            }
        }

        protected override void SetTitleText()
        {
            string selectedName = "1";

            // 현재 Scene 상태가 손상 보기 상태일 경우
            if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
            {
                #region 변수 할당
                // 손상정보 리스트를 할당한다.
                List<Issue.AIssue> _damageList = RuntimeData.RootContainer.Instance.IssueObjectList;

                // 현재 선택된 부재명을 할당한다.
                string _partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

                // 선택된 부재에 존재하는 손상정보의 개수를 받아둔다.
                int _damageCount = 0;
                for (int i = 0; i < _damageList.Count; i++)
                {
                    if (_damageList[i].GetType().Equals(typeof(Issue.DamagedIssue)) && _damageList[i].BridgePartName == _partName)
                        _damageCount++;
                }

                // 손상정보의 개수를 할당한다.
                issueIndex = _damageCount;

                // 선택된 부재 이름을 한글로 변환한다.
                selectedName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
                if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Bridge)
				{
                    //selectedName = BridgeCodeConverter.ConvertCode(selectedName, MODBS_Library.OutOption.AdView_MP2_Indicator);
                    MasterTemplate.FinalReplace(selectedName);
				}
                else if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Tunnel)
				{
                    // 가져온 이름 나중에 이름 변환 테이블로 변환
                    Debug.LogError("BPM1_Indicator.cs // 1101 198 이름변환");
				}
                //Debug.Log(selectedName);
                #endregion

                SetTitleImage(sceneStatus, mainTitleImage);

                SetTitleText(sceneStatus, mainTitleText,
                    mainTxt: UI.IssueConverter.GetMainTitle<Issue.DamagedIssue>(),
                    selected: selectedName,
                    count: issueIndex
                    );

                if (issueIndex == 0)
                    Manager.UIManager.Instance.BPM2_Panel.GetComponent<BPM2_Indicator>().rootElementPanel.gameObject.SetActive(false);
                else if(issueIndex > 0)
                    Manager.UIManager.Instance.BPM2_Panel.GetComponent<BPM2_Indicator>().rootElementPanel.gameObject.SetActive(true);
            }
            else if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
            {
                #region 변수 할당
                // 보수정보 리스트를 할당한다.
                List<Issue.AIssue> _recoverList = RuntimeData.RootContainer.Instance.IssueObjectList;

                // 현재 선택된 부재명을 할당한다.
                string _partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

                // 선택된 부재에 존재하는 보수정보의 개수를 받아둔다.
                int _recoverCount = 0;
                for (int i = 0; i < _recoverList.Count; i++)
                {
                    if (_recoverList[i].GetType().Equals(typeof(Issue.RecoveredIssue)) && _recoverList[i].BridgePartName == _partName)
                        _recoverCount++;
                }

                // 보수정보의 개수를 할당한다.
                issueIndex = _recoverCount;

                // 선택된 부재 이름을 한글로 변환한다.
                selectedName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
                if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Bridge)
				{
                    //selectedName = BridgeCodeConverter.ConvertCode(selectedName, MODBS_Library.OutOption.AdView_MP2_Indicator);
				}
                else if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Tunnel)
				{
                    Debug.LogError("BPM1_Indicator.cs // 1101 245 이름 변경");
                    selectedName = Tunnel.TunnelConverter.GetName(selectedName);
				}
                
                #endregion

                SetTitleImage(sceneStatus, mainTitleImage);

                SetTitleText(sceneStatus, mainTitleText,
                    mainTxt: UI.IssueConverter.GetMainTitle<Issue.RecoveredIssue>(),
                    selected: selectedName,
                    count: issueIndex
                    );

                //mainTitleImage.sprite = UI.IssueConverter.GetMainIcon<Issue.RecoveredIssue>();
                //mainTitleText.text = string.Format($"{selectedName} {UI.IssueConverter.GetMainTitle<Issue.RecoveredIssue>()}");
                //mainTitleImage.color = UI.IssueConverter.GetMainCountColor<Issue.RecoveredIssue>();
                
                //mainTitleCount.text = string.Format("{0:000}", issueIndex);
                //mainTitleCount.color = UI.IssueConverter.GetMainCountColor<Issue.RecoveredIssue>();
            }
            else if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            {
                List<Issue.AIssue> _damageList = RuntimeData.RootContainer.Instance.IssueObjectList;
                string _partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
                int _damageCount = 0;
                for (int i = 0; i < _damageList.Count; i++)
                {
                    if (_damageList[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                        _damageCount++;
                }
                issueIndex = _damageCount;
                mainTitleImage.sprite = UI.IssueConverter.GetMainIcon<Issue.DamagedIssue>();
                mainTitleText.text = string.Format($"{UI.IssueConverter.GetMainTitle<Issue.DamagedIssue>()}   <color=#FD116E>{issueIndex}</color>");
                //mainTitleCount.text = $"{issueIndex}";
                mainTitleCount.text = "";
                mainTitleCount.color = UI.IssueConverter.GetMainCountColor<Issue.DamagedIssue>();
                Manager.UIManager.Instance.BPM2_Panel.GetComponent<BPM2_Indicator>().rootElementPanel.gameObject.SetActive(true);
            }
        }

        protected override void ClearElements()
        {
            int index = rootElementPanel.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(rootElementPanel.GetChild(i).gameObject);
            }
        }

        protected override void SetElements(List<AIssue> _issue)
        {
            if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPM1_TableElement"));
                obj.transform.SetParent(rootElementPanel);
                element = obj.GetComponent<Element.TableElement>();

                element.SetElement(_issue, UI.PanelType.BPM1);
            }
            else if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPM1_VP2R_TableElement"));
                obj.transform.SetParent(rootElementPanel);
                element = obj.GetComponent<Element.TableElement>();

                element.SetElement(_issue, UI.PanelType.BPM1);
            }
            else if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPM1_TableElement"));
                obj.transform.SetParent(rootElementPanel);
                element = obj.GetComponent<Element.TableElement>();

                element.SetElement(_issue, UI.PanelType.BPM1);
            }

            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.BPM1);
        }

        public void ClosePanel()
        {
            Manager.UIManager.Instance.bpm1Toggle = false;
            Manager.UIManager.Instance.GridToggleCheck();
            this.gameObject.SetActive(false);
        }

        private void ActiveCloseButton()
        {
            if (sceneStatus != Manager.ViewSceneStatus.ViewMaintainance)
                closeButton.SetActive(false);
            else
                closeButton.SetActive(true);
        }
    }
}
