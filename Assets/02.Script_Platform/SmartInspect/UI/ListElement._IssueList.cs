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

            foreach (Definition._Issue.Issue issue in _issues)
            {
                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("UI/SmartInspect/Inspect_Records"), m_contentRoot);
                RecordElement element = obj.GetComponent<RecordElement>();

                Packet_Record packet = new Packet_Record(_rIndex, index++, issue, m_rootUI);

                element.Init(packet);
            }
        }
    }
}
