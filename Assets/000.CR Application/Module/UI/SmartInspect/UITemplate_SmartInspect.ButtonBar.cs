using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Definition;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;
    using View;

    public partial class UITemplate_SmartInspect : AUI
    {
        private void ButtonBar_OnSelect(Interactable _ui)
        {
            UI_Selectable ui = (UI_Selectable)_ui;

            ButtonBar_OnSelect(ui.Data.m_toggleIndex);
        }

        private void ButtonBar_OnSelect(int index)
        {
            Image img_bg = null;
            Image img_icon = null;

            int lIndex = m_bottomBar.m_btnMenu_bottomBar.Count;
            for (int i = 0; i < lIndex; i++)
            {
                bool isTarget = i == index ? true : false;

                img_bg = m_bottomBar.m_btnMenu_bottomBar[i].btn_menu.GetComponent<Image>();
                img_icon = m_bottomBar.m_btnMenu_bottomBar[i].imgs[0];

                m_uiResources.m_bottomBar.SetColor_btnBar(img_bg, img_icon, i, isTarget);

            }
        }
        
        private void ButtonBar_MoveUI(Interactable _ui)
        {
            UI_Selectable ui = (UI_Selectable)_ui;

            if(ui.Data.m_isMovedDown)
            {
                ui.Data.m_isMovedDown = false;
                ui.ChildPanel.transform.DOMoveY(43, 1);
            }
            else
            {
                ui.Data.m_isMovedDown = true;
                ui.ChildPanel.transform.DOMoveY(-96, 1);
            }
            //Debug.Log(ui.ChildPanel.transform.position);
            // 43 up
            // -96 down
        }
    }
}
