using Definition;
using Management;
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
            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsBridgePlatform(pCode))
            {
                // issue 리스트를 가져온다.
                List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory, _countData.m_bCode);
                SetIssueList(2, issues);
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory, _countData.m_tCode);
                SetIssueList(2, issues);
            }
            else
            {
                throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
            //SetIssueList(2, issues);
        }

        private void REIN_Image_Init()
        {
            Debug.Log("REIN_Image_Init");
        }
    }
}
