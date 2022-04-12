using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// module dmg toggle damagedInfo panel
        /// </summary>
        private void MDmg_LBar_1ToggleDInfo()
        {
            // 손상정보 부재별 손상정보 토글
            m_moduleElements.m_dmgElement.m_dmgCount.SetActive(
                !m_moduleElements.m_dmgElement.m_dmgCount.activeSelf);
        }

        /// <summary>
        /// module dmg toggle damagedList panel
        /// </summary>
        private void MDmg_LBar_2ToggleDList()
        {
            // 손상목록 리스트 패널 토글
            m_moduleElements.m_dmgElement.m_dmgList.SetActive(
                !m_moduleElements.m_dmgElement.m_dmgList.activeSelf);
        }

        /// <summary>
        /// module dmg toggle statusList panel
        /// </summary>
        private void MDmg_LBar_3ToggleSInfo()
        {
            m_general.m_objStatus.root.SetActive(!m_general.m_objStatus.root.activeSelf);
        }

        /// <summary>
        /// 손상 목록에서 더보기를 누르면, 아래의 상세 정보창을 토글링한다.(예시 교대01)
        /// </summary>
        public void MDmg_4ToggleDInformation()
        {
            // 자세한 정보창(예시 교대01 창) On/Off
            m_moduleElements.m_dmgElement.m_dmgInformation.SetActive(
                !m_moduleElements.m_dmgElement.m_dmgInformation.activeSelf);
        }
    }
}
