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
        private void Panel_SetDetail(Interactable _setter)
        {
            // 디테일 패널 가져옴
            IssueDetailPanel detail = m_moduleElements.m_dmgElement.m_issueDetailPanels[0];

            UI_Selectable ui = (UI_Selectable)_setter;

            // 패널이 없는경우
            if(ui.ChildPanel != null)
            {
                RecordElement element;
                if(ui.ChildPanel.TryGetComponent<RecordElement>(out element))
                {
                    detail.root.SetActive(true);
                    detail.m_titlePartName.text = element.m_issue.__PartName;

                    detail.m_width.text = element.m_issue.Width.Length <= 1 ? $"{element.m_issue.Width}" : $"{element.m_issue.Width}000";
                    detail.m_height.text = element.m_issue.Height.Length <= 1 ? $"{element.m_issue.Height}" : $"{element.m_issue.Height}000";
                    detail.m_depth.text = element.m_issue.Depth.Length <= 1 ? $"{element.m_issue.Depth}" : $"{element.m_issue.Depth}000";

                    detail.m_description.text = element.m_issue.DmgDescription;
                    
                }
            }
        }
    }
}
