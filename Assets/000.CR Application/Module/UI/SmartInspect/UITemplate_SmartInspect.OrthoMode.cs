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
        private void OrthoMode_OnSelect(Interactable _ui)
        {
            UI_Selectable ui = (UI_Selectable)_ui;

            OrthoMode_OnSelect(ui.Data.m_toggleIndex);
        }

        private void OrthoMode_OnSelect(int index)
        {
            Image img_bg = null;
            Image img_icon = null;

            int lIndex = m_bottomBar.m_orthographic.m_btnMenu_orthoMode.Count;
            for (int i = 0; i < lIndex; i++)
            {
                bool isTarget = i == index ? true : false;

                img_bg = m_bottomBar.m_orthographic.m_btnMenu_orthoMode[i].btn_menu.GetComponent<Image>();
                img_icon = m_bottomBar.m_orthographic.m_btnMenu_orthoMode[i].imgs[0];

                m_uiResources.m_bottomBar.SetColor_btnOrthoMode(img_bg, img_icon, i, isTarget);
            }
        }
    }
}
