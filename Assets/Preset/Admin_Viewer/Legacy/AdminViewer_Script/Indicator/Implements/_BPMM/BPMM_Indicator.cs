using Issue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Indicator
{
    public class BPMM_Indicator : AIndicator
    {
        Manager.ViewSceneStatus _sceneStatus;

        [Header("main title")]
        [SerializeField] private Image mainTitleImage;
        [SerializeField] private TextMeshProUGUI mainTitleText;
        [SerializeField] private TextMeshProUGUI mainTitleCount;

        [Header("set element")]
        public RectTransform rootElementPanel;
        public Element.AElement element;
        private int issueIndex;

        public override void SetPanelElements(List<AIssue> _issue)
        {
            //Debug.Log("BPMM panel set element");

            _sceneStatus = Manager.MainManager.Instance.SceneStatus;
            List<Issue.AIssue> _damageList = RuntimeData.RootContainer.Instance.IssueObjectList;
            int _damageCount = 0;
            for(int i = 0; i < _damageList.Count; i++)
            {
                if (_damageList[i].GetType().Equals(typeof(Issue.DamagedIssue)))
                    _damageCount++;
            }
            issueIndex = _damageCount;

            SetTitleText();
            ClearElements();
            SetElements(_issue);

        }

        protected override void SetTitleText()
        {
            if(_sceneStatus.Equals(Manager.ViewSceneStatus.ViewAllDamage))
            {
                mainTitleImage.sprite = UI.IssueConverter.GetMainIcon<Issue.DamagedIssue>();
                // PREV 삭제 준비
                //mainTitleText.text = string.Format($"{UI.IssueConverter.GetMainTitle<Issue.DamagedIssue>()}   <color=#FD116E>{issueIndex}</color>");
                mainTitleText.text = string.Format($"{UI.IssueConverter.GetMainTitle<Issue.DamagedIssue>()}");

                mainTitleCount.text = string.Format("{0:000}", issueIndex);
                mainTitleCount.color = UI.IssueConverter.GetMainCountColor<Issue.DamagedIssue>();
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
            if(_sceneStatus.Equals(Manager.ViewSceneStatus.ViewAllDamage))
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Indicator/BPMM_TableElement"));
                obj.transform.SetParent(rootElementPanel);
                element = obj.GetComponent<Element.TableElement>();

                element.SetElement(_issue, UI.PanelType.BPMM);
            }

            //Debug.Log("BPMM set element");

            Manager.UIManager.Instance.GetRoutineCode(IndicatorType.BPMM);
        }

        
    }
}
