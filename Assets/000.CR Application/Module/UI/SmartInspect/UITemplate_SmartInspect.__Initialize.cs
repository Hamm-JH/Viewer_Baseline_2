using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using Management;
    using View;


    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// 모듈 세팅이 완료된 시점에서 초기화를 수행한다.
        /// </summary>
        public void Initialize_AfterModuleInitialize()
        {
            ContentManager.Instance._API.RequestAddressData(API_GetAddress);

            Initialize_UI_Panels();
            
        }

        /// <summary>
        /// API에서 주소값을 받아오면 주소 데이터 기반으로 초기화를 수행한다.
        /// </summary>
        /// <param name="_data"></param>
        private void Initialize_API_GetAddress(AAPI _data)
        {
            // 다시 언박싱
            DAddress data = (DAddress)_data;

            Header_SetObjectName(data.nmTunnel);    // 시설물명
            General_SetAddress(data.nmAddress);     // 주소 할당

            API_RequestTexture(data.mp_fid, data.mp_ftype, data.mp_fgroup, API_getMainTexture);     // 텍스처 가져오기
        }

        private void Initialize_UI_Panels()
        {
            _Initialize_UI_Dmg_Panels();
            _Initialize_UI_Rcv_Panels();

            m_moduleElements.m_admElement.m_issueCountPanel.m_barCharts.ForEach(x =>
            {
                x.Init();
            });

            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds.ForEach(x =>
            {
                x.Init();
            });
        }

        private void _Initialize_UI_Dmg_Panels()
        {
            m_moduleElements.m_dmgElement.m_issueCountPanels.ForEach(x =>
            {
                x.m_listElement.Init(null, null, x.m_pBar, x.m_pBarText,
                    m_moduleElements.m_dmgElement.m_issueListPanels[0].m_listElement
                    );
            });

            m_moduleElements.m_dmgElement.m_issueListPanels.ForEach(x =>
            {
                x.m_listElement.Init(x.title, x.baseTitleName, 
                    m_moduleElements.m_dmgElement.m_issueCountPanels[0].m_pBar,
                    m_moduleElements.m_dmgElement.m_issueCountPanels[0].m_pBarText,
                    m_moduleElements.m_dmgElement.m_issueListPanels[0].m_listElement);
            });
        }

        private void _Initialize_UI_Rcv_Panels()
        {
            List<SmartInspect.IssueCountPanel> cPanels = m_moduleElements.m_rcvElement.m_issueCountPanels;
            int index = cPanels.Count;
            for (int i = 0; i < index; i++)
            {
                cPanels[i].m_listElement.Init(
                    null, null, cPanels[i].m_pBar, cPanels[i].m_pBarText,
                    m_moduleElements.m_rcvElement.m_issueListPanels[i].m_listElement
                    );
            }

            List<SmartInspect.IssueListPanel> iPanels = m_moduleElements.m_rcvElement.m_issueListPanels;
            index = iPanels.Count;
            for (int i = 0; i < index; i++)
            {
                iPanels[i].m_listElement.Init(iPanels[i].title, iPanels[i].baseTitleName,
                    m_moduleElements.m_rcvElement.m_issueCountPanels[i].m_pBar,
                    m_moduleElements.m_rcvElement.m_issueCountPanels[i].m_pBarText,
                    iPanels[i].m_listElement);
            }
        }
    }
}
