using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        private void Viewport_OnSelect(Interactable _ui)
        {
            UI_Selectable ui = (UI_Selectable)_ui;

            Viewport_OnSelect(ui.Data.m_toggleIndex);
        }

        private void Viewport_OnSelect(int index)
        {
            Image img_bg = null;
            Image img_icon = null;

            int lIndex = m_bottomBar.m_viewport.m_btnMenu_viewMode.Count;
            for (int i = 0; i < lIndex; i++)
            {
                bool isTarget = i == index ? true : false;

                img_bg = m_bottomBar.m_viewport.m_btnMenu_viewMode[i].btn_menu.GetComponent<Image>();
                img_icon = m_bottomBar.m_viewport.m_btnMenu_viewMode[i].imgs[0];

                m_uiResources.m_bottomBar.SetColor_btnViewMode(img_bg, img_icon, index, isTarget);
            }
        }
    }
}
