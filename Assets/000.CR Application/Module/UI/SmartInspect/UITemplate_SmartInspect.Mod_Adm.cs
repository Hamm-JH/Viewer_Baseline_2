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
        /// Mode Adm 활성화
        /// </summary>
        public void ModAdm_Active()
		{
            ModAdm_TogglePanel(0, true);
            ModAdm_TogglePanel(1, true);
            ModAdm_TogglePanel(2, false);
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
    }
}
