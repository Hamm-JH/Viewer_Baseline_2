using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Management;
    using Platform.Bridge;
    using View;

    public partial class UITemplate_SmartInspect : RootUI
    {
        private void Ins_Panel_OnSelectCount(BridgeCode bCode)
        {
            // IssueList 패널 활성
            ModDmg_ToggleIssueList(true);
            ModRcv_ToggleIssueList(true);

            // IssueDetail 패널 비활성
            ModDmg_ToggleIssueDetail(false);

            PlatformCode pCode = MainManager.Instance.Platform;

            //if (Platforms.IsBridgePlatform(pCode))
            //{
            //    //ui.Data.m_bridgeIssueCode;
            //    ui.Data.m_issueListElement.ResetList(ui.Data.m_bridgeIssueCode);
            //}
            //else if (Platforms.IsTunnelPlatform(pCode))
            //{
            //    //ui.Data.m_tunnelCode;
            //    ui.Data.m_issueListElement.ResetList(ui.Data.m_tunnelCode);
            //}
            //else
            //{
            //    throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            //}
        }

        private void Ins_Panel_OnSelectCount(Interactable _setter)
		{
            // _setter -> UI_Selectable
            UI_Selectable ui = (UI_Selectable)_setter;

            //ui.Data

            Debug.Log("Hello count");

            // IssueList 패널 활성
            ModDmg_ToggleIssueList(true);
            ModRcv_ToggleIssueList(true);

            // IssueDetail 패널 비활성
            ModDmg_ToggleIssueDetail(false);

            PlatformCode pCode = MainManager.Instance.Platform;

            if(Platforms.IsBridgePlatform(pCode))
            {
                //ui.Data.m_bridgeIssueCode;
                ui.Data.m_issueListElement.ResetList(ui.Data.m_bridgeIssueCode);
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                //ui.Data.m_tunnelCode;
                ui.Data.m_issueListElement.ResetList(ui.Data.m_tunnelCode);
            }
            else
            {
                throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }

		}


    }
}
