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
        /// module rcv toggle repairList panel
        /// </summary>
        private void MRcv_LBar_1RepairList()
        {
            bool isToggle = !m_moduleElements.m_rcvElement.m_rcvCount.activeSelf;

            m_moduleElements.m_rcvElement.m_rcvCount.SetActive(isToggle);
            m_moduleElements.m_rcvElement.m_rcvList.SetActive(isToggle);
        }

        /// <summary>
        /// module rcv toggle ReinforcedList panel
        /// </summary>
        private void MRcv_LBar_2ReinforcedList()
        {
            bool isToggle = !m_moduleElements.m_rcvElement.m_reinCount.activeSelf;

            m_moduleElements.m_rcvElement.m_reinCount.SetActive(isToggle);
            m_moduleElements.m_rcvElement.m_reinList.SetActive(isToggle);
        }

        /// <summary>
        /// module rcv toggle StatusInfo panel
        /// </summary>
        private void MRcv_LBar_3StatusInfo()
        {
            m_general.m_objStatus.root.SetActive(!m_general.m_objStatus.root.activeSelf);
        }

        /// <summary>
        /// module rcv toggle Dimension panel
        /// </summary>
        private void MRcv_LBar_4Dimension()
        {
            // TODO 4 Dimension 메서드 작업예정
            Debug.LogError($"4 Dimension 메서드 작업예정");
        }

        /// <summary>
        /// module rcv toggle DrawingPrint panel
        /// </summary>
        private void MRcv_LBar_5DrawingPrint()
        {
            // TODO 5 DrawingPrint 메서드 작업예정
            Debug.LogError($"5 DrawingPrint 메서드 작업예정");
        }
    }
}
