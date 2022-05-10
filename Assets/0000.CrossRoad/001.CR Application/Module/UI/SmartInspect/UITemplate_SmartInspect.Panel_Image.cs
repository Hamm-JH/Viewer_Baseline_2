using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using SmartInspect;
    using View;

    public partial class UITemplate_SmartInspect : AUI
    {
        private void Panel_SetImage(Interactable _setter)
        {
            // 이미지 패널 가져옴
            ImagePopup imgPopup = m_general.m_imgPopup;

            UI_Selectable ui = (UI_Selectable)_setter;

            if(ui.ChildPanel != null)
            {
                RecordElement element;
                if(ui.ChildPanel.TryGetComponent<RecordElement>(out element))
                {
                    imgPopup.root.SetActive(true);
                    imgPopup.m_date.text = element.m_issue.DateDmg;

                    imgPopup.m_issueImagePanel.Init(element.m_issue);
                }
            }
        }
    }
}
