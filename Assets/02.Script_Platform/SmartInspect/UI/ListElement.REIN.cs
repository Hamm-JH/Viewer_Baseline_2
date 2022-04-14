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
        public void REIN_Init()
        {
            if (m_template == Template.PartCount)
            {
                PartCount_Init();
            }
            else if (m_template == Template.IssueList)
            {
                REIN_IssueList_Init();
            }
            else
            {
                throw new System.Exception("template not defined");
            }
        }

        private void REIN_IssueList_Init()
        {

        }
    }
}
