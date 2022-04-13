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
        /// Mode Dmg 활성화
        /// </summary>
        public void ModDmg_Active()
        {
            //m_moduleElements.m_dmgElement.m_dmgCount;     // 부재별 손상정보 패널  // 켜야됨
            //m_moduleElements.m_dmgElement.m_dmgList;      // 부재별 손상정보 리스트 패널  // 켜야됨
            //m_moduleElements.m_dmgElement.m_dmgInformation;   // 부재 자세한 정보 패널 // 꺼야됨

            //MDmg_ToggleLBar(0, true);     // 손상정보 패널 동기화
            //MDmg_ToggleLBar(1, true);     // 손상정보 리스트 패널 동기화
            //MDmg_ToggleLBar(2, false);    // objStatus 패널 동기화

            ModDmg_TogglePanel(0, true);
            //m_moduleElements.m_dmgElement.m_dmgCount.SetActive(false);
            //MDmg_ToggleLBar(0, true);

            ModDmg_TogglePanel(1, true);
            ModDmg_TogglePanel(2, false);
            ModDmg_TogglePanel(3, false);
        }

        /// <summary>
        /// index //
        /// 0 : dmgCount //
        /// 1 : dmgList //
        /// 2 : detailInfo //
        /// 3 : objStatus
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isOn"></param>
        private void ModDmg_TogglePanel(int index, bool isOn)
        {
            GameObject toggleRoot = _ModDmg_GetToggleRoot(index);

            // 모드 활성화시 초기에 꺼야하는 패널들 끄기
            toggleRoot.SetActive(isOn);

            // 끄는 패널과 Leftbar 상태 동기화
            _ModDmg_Set_Leftbar(index, isOn);
        }

        /// <summary>
        /// index //
        /// 0 : dmgCount //
        /// 1 : dmgList //
        /// 2 : detailInfo //
        /// 3 : objStatus
        /// </summary>
        /// <param name="index"></param>
        private GameObject _ModDmg_GetToggleRoot(int index)
        {
            GameObject result = null;

            switch(index)
            {
                case 0:
                    result = m_moduleElements.m_dmgElement.m_dmgCount;
                    break;

                case 1:
                    result = m_moduleElements.m_dmgElement.m_dmgList;
                    break;

                case 2:
                    result = m_moduleElements.m_dmgElement.m_dmgInformation;
                    break;

                case 3:
                    result = m_general.m_objStatus.root;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Leftbar의 이미지 상태를 변경한다.
        /// index // 
        /// 0 : dmgCount //
        /// 1 : dmgList //
        /// 3 : objStatus 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isOn"></param>
        private void _ModDmg_Set_Leftbar(int index, bool isOn)
        {
            int replacedIndex = -1;

            switch(index)
            {
                case 0: replacedIndex = 0; break;
                case 1: replacedIndex = 1; break;
                case 3: replacedIndex = 2; break;
            }

            if(replacedIndex != -1)
            {
                MDmg_ToggleLBar(replacedIndex, isOn);
            }
        }
    }
}
