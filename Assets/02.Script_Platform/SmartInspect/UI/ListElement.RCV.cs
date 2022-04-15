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
        public void RCV_Init()
        {
            if (m_template == Template.PartCount)
            {
                PartCount_Init();
            }
            else if (m_template == Template.IssueList)
            {
                RCV_IssueList_Init();
            }
            else
            {
                throw new System.Exception("template not defined");
            }
        }

        private void RCV_IssueList_Init()
        {
            // issue 리스트를 가져온다.
            List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory);

            SetIssueList(2, issues);
        }

        private void RCV_Image_Init()
        {
            Debug.Log("RCV_Image_Init");
        }
    }
}
