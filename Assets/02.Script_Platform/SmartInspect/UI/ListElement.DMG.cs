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

            //Packet_Record packet = new Packet_Record(
            //    _rIndex: 0,
            //    _issue: null,
            //    _rootUI: m_rootUI);
        }

        private void PartCount_Init()
        {
            PlatformCode pCode = MainManager.Instance.Platform;
            if(Platforms.IsBridgePlatform(pCode))
            {
                // 분류 코드 리스트 갖고온다.
                List<Bridges.CodeLv4> cList = Bridges.GetCodeList();
                // 손상정보 리스트도 갖고온다.
                List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory);
                //SmartInspectManager.Instance.Module<Module_Model>(ModuleID.Model).DmgData;

                // 분류 코드 리스트 기반으로 요소 리스트를 생성한다.
                Dictionary<Bridges.CodeLv4, int> result = GetCountList_Bridge(cList, issues);

                SetCountList_Bridge(result);
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                // 분류 코드 리스트 갖고온다.
                List<AdminViewer.Tunnel.TunnelCode> cList = Tunnels.GetCodeList();
                // 손상정보 리스트도 갖고온다.
                List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory);
                //SmartInspectManager.Instance.Module<Module_Model>(ModuleID.Model).DmgData;

                // 분류 코드 리스트 기반으로 요소 리스트를 생성한다.
                Dictionary<AdminViewer.Tunnel.TunnelCode, int> result = GetCountList_Tunnel(cList, issues);

                SetCountList_Tunnel(result);
            }
            else
            {
                throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
        }

        private void DMG_IssueList_Init()
        {

        }

    }
}
