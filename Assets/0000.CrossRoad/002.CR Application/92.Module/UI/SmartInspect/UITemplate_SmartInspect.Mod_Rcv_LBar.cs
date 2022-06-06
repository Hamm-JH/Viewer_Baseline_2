using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using UnityEngine.UI;
    using View;

    public partial class UITemplate_SmartInspect : RootUI
    {
        /// <summary>
        /// module rcv toggle repairList panel
        /// </summary>
        private void MRcv_LBar_1RepairList()
        {
            bool isToggle = !m_moduleElements.m_rcvElement.m_rcvCount.activeSelf;

            m_moduleElements.m_rcvElement.m_rcvCount.SetActive(isToggle);
            m_moduleElements.m_rcvElement.m_rcvList.SetActive(false);

            MRcv_ToggleLBar(0, isToggle);
        }

        /// <summary>
        /// module rcv toggle ReinforcedList panel
        /// </summary>
        private void MRcv_LBar_2ReinforcedList()
        {
            bool isToggle = !m_moduleElements.m_rcvElement.m_reinCount.activeSelf;

            m_moduleElements.m_rcvElement.m_reinCount.SetActive(isToggle);
            m_moduleElements.m_rcvElement.m_reinList.SetActive(isToggle);

            MRcv_ToggleLBar(1, isToggle);
        }

        /// <summary>
        /// module rcv toggle StatusInfo panel
        /// </summary>
        private void MRcv_LBar_3StatusInfo()
        {
            bool isOn = !m_general.m_objStatus.root.activeSelf;
            m_general.m_objStatus.root.SetActive(isOn);

            MRcv_ToggleLBar(2, isOn);
        }

        /// <summary>
        /// module rcv toggle Dimension panel
        /// </summary>
        private void MRcv_LBar_4Dimension()
        {
            Debug.LogError($"4 Dimension 메서드 작업예정");
            MRcv_ToggleLBar(3, false);
        }

        /// <summary>
        /// module rcv toggle DrawingPrint panel
        /// </summary>
        private void MRcv_LBar_5DrawingPrint()
        {
            Debug.LogError($"5 DrawingPrint 메서드 작업예정");
            MRcv_ToggleLBar(4, false);
        }

        /// <summary>
        /// index ::
        /// 0 : Repaired List // 1 : Damaged List // 2 : Status Info // 3 : Dimension // 4 : Drawing Print
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isOn"></param>
        private void MRcv_ToggleLBar(int index, bool isOn)
        {
            Image img_bg = null;
            Image img_main = null;

            switch(index)
            {
                case 0:
                    img_main = m_moduleElements.m_rcvElement.m_leftbar[0].btn_menu.image;
                    img_bg = m_moduleElements.m_rcvElement.m_leftbar[0].imgs[0];
                    break;

                    // TODO :: CHECK :: 삭제예정 (버튼 안씀)
                case 1:
                    img_main = m_moduleElements.m_rcvElement.m_leftbar[1].btn_menu.image;
                    img_bg = m_moduleElements.m_rcvElement.m_leftbar[1].imgs[0];
                    break;

                case 2:
                    img_main = m_moduleElements.m_rcvElement.m_leftbar[2].btn_menu.image;
                    img_bg = m_moduleElements.m_rcvElement.m_leftbar[2].imgs[0];
                    break;

                case 3:
                    img_main = m_moduleElements.m_rcvElement.m_leftbar[3].btn_menu.image;
                    img_bg = m_moduleElements.m_rcvElement.m_leftbar[3].imgs[0];
                    break;

                case 4:
                    img_main = m_moduleElements.m_rcvElement.m_leftbar[4].btn_menu.image;
                    img_bg = m_moduleElements.m_rcvElement.m_leftbar[4].imgs[0];
                    break;
            }

            if(index != 5)
            {
                if(img_bg == null || img_main == null)
                {
                    throw new Definition.Exceptions.ImagesNotAssigned();
                }
            }

            m_uiResources.m_rcv_leftbar.SetImage(img_bg, img_main, index, isOn);
        }
    }
}
