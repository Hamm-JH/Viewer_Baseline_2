using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    /// <summary>
    /// SmartInspect의 UI 내부 리스트 요소 클래스
    /// </summary>
    public partial class ListElement
    {
        private void SetIssueList(int _rIndex, List<Definition._Issue.Issue> _issues)
        {
            int index = 1;

            if(titleText != null)
            {
                titleText.text = $"{titleName} {_issues.Count}건";
            }


            _countData.m_pBar.ChangeValue(  (int)(((float)_issues.Count / _countData.m_maxIssueCount) * 100)  );
            _countData.m_pBar_Text.text = $"{_issues.Count}";

            foreach (Definition._Issue.Issue issue in _issues)
            {
                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("UI/SmartInspect/Inspect_Records"), m_contentRoot);
                RecordElement element = obj.GetComponent<RecordElement>();

                Packet_Record packet = new Packet_Record(_rIndex, index++, issue, _countData.m_tgElement, m_rootUI);

                element.Init(packet);
            }
        }
    }
}
