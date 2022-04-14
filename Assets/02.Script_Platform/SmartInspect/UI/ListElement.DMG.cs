using Definition;
using Management;
using Module.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    using Management.Content;
    using Module.Model;
    using Platform.Bridge;
    using Platform.Tunnel;

    /// <summary>
    /// SmartInspect의 UI 내부 리스트 요소 클래스
    /// </summary>
    public partial class ListElement
    {
        public void DMG_Init()
        {
            if(m_template == Template.PartCount)
            {
                PartCount_Init();
            }
            else if(m_template == Template.IssueList)
            {
                DMG_IssueList_Init();
            }
            else
            {
                throw new System.Exception("template not defined");
            }
        }

        

        private void DMG_IssueList_Init()
        {
            // 단일 이슈 단위로 정보를 배치하는 작업을 수행한다.

            // issue 리스트를 가져온다.
            List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory);

            // 가져온 리스트 개별 단위로 순회한다.
                // 단일 이슈에 대한 인스턴스 생성
                // 패킷 생성후 실행
        }

    }
}
