using Definition;
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
        /// module dmg toggle damagedInfo panel
        /// </summary>
        private void MDmg_LBar_1ToggleDInfo()
        {
            bool isOn = !m_moduleElements.m_dmgElement.m_dmgCount.activeSelf;
            // 손상정보 부재별 손상정보 토글
            m_moduleElements.m_dmgElement.m_dmgCount.SetActive(isOn);

            MDmg_ToggleLBar(0, isOn);
        }

        /// <summary>
        /// module dmg toggle damagedList panel
        /// </summary>
        private void MDmg_LBar_2ToggleDList()
        {
            bool isOn = !m_moduleElements.m_dmgElement.m_dmgList.activeSelf;
            // 손상목록 리스트 패널 토글
            m_moduleElements.m_dmgElement.m_dmgList.SetActive(isOn);

            MDmg_ToggleLBar(1, isOn);
        }

        /// <summary>
        /// module dmg toggle statusList panel
        /// </summary>
        private void MDmg_LBar_3ToggleSInfo()
        {
            bool isOn = !m_general.m_objStatus.root.activeSelf;
            m_general.m_objStatus.root.SetActive(isOn);

            MDmg_ToggleLBar(2, isOn);
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

        /// <summary>
        /// index ::
        /// 0 : dInfo, 1 : dList, 2 : sInfo
        /// </summary>
        /// <param name="index"></param>
        private void MDmg_ToggleLBar(int index, bool isOn)
        {
            Image img_bg = null;
            Image img_main = null;

            switch(index)
            {
                case 0:
                    //img_bg = m_moduleElements.m_dmgElement.m_leftbar[0].btn_menu.image;
                    //img_main = m_moduleElements.m_dmgElement.m_leftbar[0].imgs[0];
                    img_main = m_moduleElements.m_dmgElement.m_leftbar[0].btn_menu.image;
                    img_bg = m_moduleElements.m_dmgElement.m_leftbar[0].imgs[0];
                    break;

                case 1:
                    img_main = m_moduleElements.m_dmgElement.m_leftbar[1].btn_menu.image;
                    img_bg = m_moduleElements.m_dmgElement.m_leftbar[1].imgs[0];
                    break;

                case 2:
                    img_main = m_moduleElements.m_dmgElement.m_leftbar[2].btn_menu.image;
                    img_bg = m_moduleElements.m_dmgElement.m_leftbar[2].imgs[0];
                    break;
            }

            if(img_bg == null || img_main == null)
            {
                throw new Definition.Exceptions.ImagesNotAssigned();
            }

            m_uiResources.m_dmg_leftbar.SetImage(img_bg, img_main, index, isOn);
        }
    }
}
