using Definition;
using SmartInspect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : RootUI
    {
        /// <summary>
        /// Mode Adm 활성화
        /// </summary>
        public void ModAdm_Active()
		{
            ModAdm_ResetBasePosition();

            ModAdm_TogglePanel(0, true);
            ModAdm_TogglePanel(1, true);
            ModAdm_TogglePanel(2, false);
        }

        private void ModAdm_ResetBasePosition()
        {
            if (!m_moduleElements.m_admElement.root.activeSelf) return;

            m_moduleElements.m_admElement.m_issueCountPanel.ReturnBasePosition();
            m_moduleElements.m_admElement.m_timelinePanel.ReturnBasePosition();
        }

        /// <summary>
        /// index //
        /// 0 : Total Maintenance //
        /// 1 : Maintenance Timeline //
        /// 2 : objStatus //
        /// 3 : Report
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isOn"></param>
        private void ModAdm_TogglePanel(int index, bool isOn)
		{
            GameObject toggleRoot = _ModAdm_GetToggleRoot(index);

            toggleRoot.SetActive(isOn);

            _ModAdm_Set_Leftbar(index, isOn);
        }

        private GameObject _ModAdm_GetToggleRoot(int index)
		{
            GameObject result = null;

            switch(index)
			{
                case 0:
                    result = m_moduleElements.m_admElement.m_TotalMaintenance;
                    break;

                case 1:
                    result = m_moduleElements.m_admElement.m_MaintenanceTimeline;
                    break;

                case 2:
                    result = m_general.m_objStatus.root;
                    break;
            }

            return result;
		}

        private void _ModAdm_Set_Leftbar(int index, bool isOn)
		{
            MAdm_ToggleLBar(index, isOn);

        }

		#region TimeLine

        private void Ins_MAdm_TL_SelectYear1(Interactable _setter)
		{
            // SmartInspect.UI_Resources에서 UI 토글 준비
            // 리소스 준비 필요..

            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_YearTemplate(_index: 0);

            int tIndex = 0;
            _Ins_MAdm_TL_toggleYear(tIndex);
		}

        private void Ins_MAdm_TL_SelectYear5(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_YearTemplate(_index: 1);

            int tIndex = 1;
            _Ins_MAdm_TL_toggleYear(tIndex);
        }

        private void Ins_MAdm_TL_SelectYear10(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_YearTemplate(_index: 2);

            int tIndex = 2;
            _Ins_MAdm_TL_toggleYear(tIndex);
        }

        private void Ins_MAdm_TL_SelectYear50(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_YearTemplate(_index: 3);

            int tIndex = 3;
            _Ins_MAdm_TL_toggleYear(tIndex);
        }

        private void _Ins_MAdm_TL_toggleYear(int _tIndex)
        {
            List<RProcessMenu> menus = m_moduleElements.m_admElement.m_timelinePanel.m_yearButtons;

            int index = menus.Count;
            for (int i = 0; i < index; i++)
            {
                bool isHighlight = false;
                if(i == _tIndex)    isHighlight = true;

                m_uiResources.m_adm_timeline.SetImage(menus[i].btn_menu.GetComponent<Image>(), menus[i].txts[0], i, isHighlight);
            }

        }



        private void Ins_MAdm_TL_SelectALL(Interactable _setter)
		{
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_IssueIndex(_index: 0);
        }

        private void Ins_MAdm_TL_SelectCrack(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_IssueIndex(_index: 1);
        }

        private void Ins_MAdm_TL_SelectSpalling(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_IssueIndex(_index: 2);
        }

        private void Ins_MAdm_TL_SelectEfflorescence(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_IssueIndex(_index: 3);
        }

        private void Ins_MAdm_TL_SelectBreakage(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_IssueIndex(_index: 4);
        }


        private void Ins_MAdm_TL_PrevYear(Interactable _setter)
		{
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_PrevYear();
        }

        private void Ins_MAdm_TL_NextYear(Interactable _setter)
        {
            m_moduleElements.m_admElement.m_timelinePanel.m_graphFeeds[0].Update_NextYear();
        }

        #endregion
    }
}
