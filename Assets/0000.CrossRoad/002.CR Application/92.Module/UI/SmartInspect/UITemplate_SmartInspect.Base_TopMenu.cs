using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : RootUI
    {
        private void TopMenu_ToggleProfile()
        {
            m_basePanel.m_profilePopup.profileBox.SetActive(
                !m_basePanel.m_profilePopup.profileBox.activeSelf);
        }
    }
}
