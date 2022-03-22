using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using MODBS_Library;

namespace Indicator
{
    public class BPM2_Indicator : AIndicator
    {
        Manager.ViewSceneStatus _sceneStatus;

        [Header("main title")]
        [SerializeField] private Image mainTitleImage;
        [SerializeField] private TextMeshProUGUI mainTitleText;
        [SerializeField] private TextMeshProUGUI mainTitleCount;
        [SerializeField] private Image mainTitleLine;

        [Header("set element")]
        public RectTransform rootElementPanel;
        public List<Element.TableElement> element;
        private int issueIndex;
        private int subIssueIndex;

        // Only VPD status
        [SerializeField] private TextMeshProUGUI userLogText;
        public Element.IssueElement issueElement;

        [SerializeField] private GameObject closeButton;

        private void Awake()
        {
            element = new List<Element.TableElement>();
        }

        public override void SetPanelElements(List<AIssue> _issue)
        {
            //Debug.Log("BPM2 panel set element");

            // 씬 상태코드 업데이트
            _sceneStatus = Manager.MainManager.Instance.SceneStatus;

            // 넘어온 손상정보의 개수를 구한다.
            issueIndex = GetElementIndex(_issue);
            if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            {
                subIssueIndex = GetIssueIndex<Issue.ReinforcementIssue>(_issue);
            }

            //ActiveCloseButton();
            SetTitleText();
            ClearElements();
            SetElements(_issue);

            if(_sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            {

            }

            //Manager.UIManager.Instance.GetRoutineCode(IndicatorType.BPM2);
        }

        #region element index
        private int GetElementIndex(List<AIssue> _issue)
        {
            int result = 0;

            if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
            {
                result = GetIssueIndex<Issue.DamagedIssue>(_issue);
            }
            else if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
            {
                result = GetIssueIndex<Issue.ReinforcementIssue>(_issue);
            }
            else if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
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
                if (_issue[i].GetType().Equals(typeof(T)))
                {
                    result++;
                }
            }

            return result;
        }
        #endregion

        private void SetTitleImage(Manager.ViewSceneStatus _status, Image _target)
        {
            switch (_status)
            {
                case Manager.ViewSceneStatus.ViewPartDamage:
                    _target.sprite = UI.IssueConverter.GetMainIcon();
                    _target.color = UI.Colors.Instance.Set(UI.Colors.Palette.White);
                    break;

                case Manager.ViewSceneStatus.ViewPart2R:
                    _target.sprite = UI.IssueConverter.GetMainIcon<Issue.ReinforcementIssue>();
                    _target.color = UI.Colors.Instance.Set(UI.Colors.Palette.CustomPurple1);
                    //_target.color = UI.IssueConverter.GetMainCountColor<Issue.ReinforcementIssue>();
                    break;
            }
        }

        private void SetTitleText(Manager.ViewSceneStatus _status, TextMeshProUGUI _target, string mainTxt = "", string selected = "", int count = 0)
        {
            switch (_status)
            {
                case Manager.ViewSceneStatus.ViewPartDamage:
                    {
                        string selectedColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomCyan1);
                        string countColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomRed1);
                        string mainColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomGrey);

                        selected = MasterTemplate.FinalReplace(selected);
                        string _selected = $"<color=#{selectedColorCode}>{selected}</color>";
                        //string _count = string.Format("<color=#{0}>{1}</color>", countColorCode, string.Format("{0:000}", count));
                        string _main = $"<color=#{mainColorCode}>{Tunnel.TunnelConverter.GetName(mainTxt)} ({_selected})</color>";

                        _target.text = _main;
                    }
                    break;

                case Manager.ViewSceneStatus.ViewPart2R:
                    {
                        string selectedColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomCyan1);
                        string countColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomPurple1);
                        string mainColorCode = UI.Colors.Instance.GetStringCode(UI.Colors.Palette.CustomGrey);

                        selected = MasterTemplate.FinalReplace(selected);
                        string _selected = $"<color=#{selectedColorCode}>{selected}</color>";
                        string _count = string.Format("<color=#{0}>{1}</color>", countColorCode, string.Format("{0:000}", count));
                        string _main = $"<color=#{mainColorCode}>{Tunnel.TunnelConverter.GetName(mainTxt)} ({_selected}) {_count} 건</color>";

                        _target.text = _main;
                    }
                    break;
            }
        }

        protected override void SetTitleText()
        {
            string selectedName = "1";

            userLogText.enabled = false;

            if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
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

                // 선택 부재명 변환
                selectedName = _partName;
                if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Bridge)
				{
                    //selectedName = BridgeCodeConverter.ConvertCode(selectedName, MODBS_Library.OutOption.AdView_MP2_Indicator);
				}
                else if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Tunnel)
				{
                    Debug.LogError("BPM2_Indicator.cs // 1101 187 이름 변경");
				}
                #endregion

                SetTitleImage(_sceneStatus, mainTitleImage);

                SetTitleText(_sceneStatus, mainTitleText,
                    mainTxt: UI.IssueConverter.GetMainTitle(),
                    selected: selectedName,
                    count: _damageCount
                    );

                mainTitleImage.enabled = true;
                mainTitleText.enabled = true;
                mainTitleCount.enabled = false;
                mainTitleLine.enabled = true;
                mainTitleLine.enabled = false;

                //mainTitleImage.sprite = UI.IssueConverter.GetMainIcon();
                //mainTitleText.text = string.Format($"{selectedName} {UI.IssueConverter.GetMainTitle()}");

                if (Indicator.BPM1_Indicator.Instance.issueIndex == 0)
                    userLogText.enabled = false;
                else if (Indicator.BPM1_Indicator.Instance.issueIndex > 0)
                    userLogText.enabled = true;
            }
            else if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
            {
                #region 변수 할당
                // 보강정보 리스트를 할당한다.
                List<Issue.AIssue> _reinList = RuntimeData.RootContainer.Instance.IssueObjectList;

                // 현재 선택된 부재명을 할당한다.
                string _partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

                // 선택된 부재에 존재하는 보수정보의 개수를 받아둔다.
                int _reinCount = 0;
                for (int i = 0; i < _reinList.Count; i++)
                {
                    if (_reinList[i].GetType().Equals(typeof(Issue.ReinforcementIssue)) && _reinList[i].BridgePartName == _partName)
                        _reinCount++;
                }

                // 선택된 부재 이름을 한글로 변환한다.
                selectedName = _partName;
                if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Bridge)
				{
                    //selectedName = BridgeCodeConverter.ConvertCode(selectedName, MODBS_Library.OutOption.AdView_MP2_Indicator);
				}
                else if(Manager.MainManager.Instance.AppUseCase == Manager.Definition.UseCase.Tunnel)
				{
                    Debug.LogError("BPM2_Indicator.cs // 1101 238 이름 변경");
				}
                #endregion

                SetTitleImage(_sceneStatus, mainTitleImage);

                SetTitleText(_sceneStatus, mainTitleText,
                    mainTxt: UI.IssueConverter.GetMainTitle<Issue.ReinforcementIssue>(),
                    selected: selectedName,
                    count: _reinCount
                    );

                mainTitleImage.enabled = true;
                mainTitleText.enabled = true;
                mainTitleCount.enabled = false;
                mainTitleLine.enabled = true;
                mainTitleLine.enabled = false;
                
                //mainTitleImage.sprite = UI.IssueConverter.GetMainIcon<Issue.ReinforcementIssue>();
                //mainTitleImage.color = UI.IssueConverter.GetMainCountColor<Issue.ReinforcementIssue>();
                //mainTitleText.text = string.Format($"{selectedName} {UI.IssueConverter.GetMainTitle<Issue.ReinforcementIssue>()}");
                //mainTitleCount.text = string.Format("{0:000}", issueIndex);
                //mainTitleCount.color = UI.IssueConverter.GetMainCountColor<Issue.ReinforcementIssue>();

            }
            //else if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            //{
            //    mainTitleImage.enabled = true;
            //    mainTitleText.enabled = true;
            //    mainTitleCount.enabled = true;
            //    mainTitleLine.enabled = true;

            //    List<Issue.AIssue> _recoverList = RuntimeData.RootContainer.Instance.IssueObjectList;
            //    int _recoverCount = 0;
            //    for (int i = 0; i < _recoverList.Count; i++)
            //    {
            //        if (_recoverList[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
            //            _recoverCount++;
            //    }
            //    issueIndex = _recoverCount;
            //    mainTitleImage.sprite = UI.IssueConverter.GetMainIcon<Issue.RecoveredIssue>();
            //    mainTitleText.text = string.Format($"{UI.IssueConverter.GetMainTitle<Issue.RecoveredIssue>()}");
            //    mainTitleImage.color = UI.IssueConverter.GetMainCountColor<Issue.RecoveredIssue>();
                
            //    mainTitleCount.text = string.Format("{0:000}", issueIndex);
            //    mainTitleCount.color = UI.IssueConverter.GetMainCountColor<Issue.RecoveredIssue>();

            //    mainTitleLine.enabled = true;
            //    mainTitleLine.color = UI.IssueConverter.GetMainCountColor<Issue.RecoveredIssue>();

            //    List<Issue.AIssue> _reinforcementList = RuntimeData.RootContainer.Instance.IssueObjectList;
            //    int _reinforcementCount = 0;
            //    for (int i = 0; i < _reinforcementList.Count; i++)
            //    {
            //        if (_reinforcementList[i].GetType().Equals(typeof(Issue.RecoveredIssue)))
            //            _reinforcementCount++;
            //    }
            //    issueIndex = _reinforcementCount;
            //}
        }

        protected override void ClearElements()
        {
            int index = rootElementPanel.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(rootElementPanel.GetChild(i).gameObject);
            }

            element.Clear();
            issueElement = null;
        }

        protected override void SetElements(List<AIssue> _issue)
        {
            Element.TableElement _element;

            if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewPartDamage))
            {
                // 이미지 패널 생성

                GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPM2_VPD_TableElement"));
                obj.transform.SetParent(rootElementPanel);
                issueElement = obj.GetComponent<Element.IssueElement>();

                InitializeIssue();
            }
            else if (_sceneStatus.Equals(Manager.ViewSceneStatus.ViewPart2R))
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPM2_VP2R_TableElement"));
                obj.transform.SetParent(rootElementPanel);
                _element = obj.GetComponent<Element.TableElement>();

                _element.SetElement(_issue, UI.PanelType.BPM2);

                element.Add(_element);
            }
            //else if (sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            //{
            //    {
            //        GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPM2_VP2R_TableElement"));
            //        obj.transform.SetParent(rootElementPanel);
            //        _element = obj.GetComponent<Element.TableElement>();

            //        _element.SetElement(_issue, UI.PanelType.BPM2, 1);

            //        element.Add(_element);
            //    }

            //    {
            //        GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPM1_VP2R_TableElement"));
            //        obj.transform.SetParent(rootElementPanel);
            //        _element = obj.GetComponent<Element.TableElement>();

            //        _element.SetElement(_issue, UI.PanelType.BPM2, 2);

            //        element.Add(_element);
            //    }
            //}

            //if(sceneStatus.Equals(Manager.ViewSceneStatus.ViewMaintainance))
            //    ChangeTag(1);
            //TagElement(1);

            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.BPM2);
        }

        #region View part damage event

        private void InitializeIssue()
        {
            List<AIssue> issueList = RuntimeData.RootContainer.Instance.IssueObjectList;
            AIssue targetIssue = null;

            string targetPartName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;

            int index = issueList.Count;
            for (int i = 0; i < index; i++)
            {
                if (issueList[i].BridgePartName == targetPartName)
                {
                    targetIssue = issueList[i];
                    break;
                }
            }

            if (targetIssue != null)
            {
                SetIssueData(targetIssue);
            }
        }

        public void SetIssueData(Issue.AIssue _issue)
        {
            SetUserLog(_issue);
            if(issueElement != null)
            {
                issueElement.GetIssueData(_issue);
            }
        }

        private void SetUserLog(Issue.AIssue _issue)
        {
            string date = "";
            string user = "";

            if (_issue.GetType().Equals(typeof(Issue.DamagedIssue)))
            {
                Issue.DamagedIssue dmgIssue = _issue as Issue.DamagedIssue;

                date = dmgIssue.DTCheck;
                user = dmgIssue.NmUser;
            }
            else if (_issue.GetType().Equals(typeof(Issue.RecoveredIssue)))
            {
                Issue.RecoveredIssue rcvIssue = _issue as Issue.RecoveredIssue;

                date = rcvIssue.DTCheck;
                user = rcvIssue.NmUser;
            }

            userLogText.text = $"{date} {user}";
        }

        #endregion

        //public void ChangeTag(int index)
        //{
        //    //TagTitle(index);
        //    //TagElement(index);
        //}

        //private void TagTitle(int index)
        //{
        //    if(index == 1)
        //    {
        //        SetTitleImage(Manager.MainManager.Instance.SceneStatus, mainTitleImage);

        //        //mainTitleImage.color = new Color(1, 1, 1, 1);

        //        //List<Issue.AIssue> _damageList

        //        mainTitleText.color = new Color(1, 1, 1, 1);
        //        mainTitleText.text = string.Format($"{UI.IssueConverter.GetMainTitle<Issue.RecoveredIssue>()}   <color=#19FADF>{issueIndex}</color>");
        //        //mainTitleCount.color = UI.IssueConverter.GetMainCountColor<Issue.RecoveredIssue>();
        //        mainTitleLine.color = UI.IssueConverter.GetMainCountColor<Issue.RecoveredIssue>();
        //    }
        //    else if(index == 2)
        //    {
        //        mainTitleImage.color = new Color(30 / 255f, 30 / 255f, 30 / 255f, 1);
        //        mainTitleText.color = new Color(30 / 255f, 30 / 255f, 30 / 255f, 1);
        //        mainTitleText.text = string.Format($"{UI.IssueConverter.GetMainTitle<Issue.RecoveredIssue>()}   <color=#1E1E1E>{issueIndex}</color>");
        //        mainTitleCount.color = new Color(30 / 255f, 30 / 255f, 30 / 255f, 1);
        //        mainTitleLine.color = new Color(30 / 255f, 30 / 255f, 30 / 255f, 1);
        //    }
        //}

        //private void TagElement(int index)
        //{
        //    int _index = element.Count;
        //    if(_index >= 2)
        //    {
        //        if(index == 1)
        //        {
        //            element[0].gameObject.SetActive(true);
        //            element[1].gameObject.SetActive(false);
        //        }
        //        else if(index == 2)
        //        {
        //            element[0].gameObject.SetActive(false);
        //            element[1].gameObject.SetActive(true);
        //        }
        //    }
        //}

        public void ClosePanel()
        {
            Manager.UIManager.Instance.bpm2Toggle = false;
            Manager.UIManager.Instance.GridToggleCheck();
            this.gameObject.SetActive(false);
        }

        private void ActiveCloseButton()
        {
            if (_sceneStatus != Manager.ViewSceneStatus.ViewMaintainance)
                closeButton.SetActive(false);
            else
                closeButton.SetActive(true);
        }
    }
}
