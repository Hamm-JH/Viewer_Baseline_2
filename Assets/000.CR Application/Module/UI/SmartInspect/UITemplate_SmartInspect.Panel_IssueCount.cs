using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Management;
    using View;

    public partial class UITemplate_SmartInspect : AUI
    {
        private void Ins_Panel_OnSelectCount(Interactable _setter)
		{
            // _setter -> UI_Selectable
            UI_Selectable ui = (UI_Selectable)_setter;

            //ui.Data

            Debug.Log("Hello count");

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
