using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Definition;
    using UnityEngine;
    using UnityEngine.UI;
    using View;

    public partial class UITemplate_SmartInspect : RootUI
    {
        /// <summary>
        /// module adm toggle total maintenance panel
        /// </summary>
        private void MAdm_LBar_1ToggleTotalMaintenance()
		{
            bool isOn = !m_moduleElements.m_admElement.m_TotalMaintenance.activeSelf;

            m_moduleElements.m_admElement.m_TotalMaintenance.SetActive(isOn);

            MAdm_ToggleLBar(0, isOn);
		}

        /// <summary>
        /// module adm toggle maintenance timeline panel
        /// </summary>
        private void MAdm_LBar_2ToggleMaintenanceTimeline()
		{
            bool isOn = !m_moduleElements.m_admElement.m_MaintenanceTimeline.activeSelf;

            m_moduleElements.m_admElement.m_MaintenanceTimeline.SetActive(isOn);

            MAdm_ToggleLBar(1, isOn);
		}

        /// <summary>
        /// module adm toggle statusList panel
        /// </summary>
        private void MAdm_LBar_3ToggleInfo()
		{
            bool isOn = !m_general.m_objStatus.root.activeSelf;
            m_general.m_objStatus.root.SetActive(isOn);

            MAdm_ToggleLBar(2, isOn);
		}

        /// <summary>
        /// module adm print report excel
        /// </summary>
        private void MAdm_LBar_4PrintReport()
		{
            Debug.LogError("print report 작업 예정");
		}


        private void MAdm_ToggleLBar(int index, bool isOn)
		{
            Image img_bg = null;
            Image img_main = null;

            switch(index)
			{
                case 0:
                    img_main = m_moduleElements.m_admElement.m_leftbar[0].btn_menu.image;
                    img_bg = m_moduleElements.m_admElement.m_leftbar[0].imgs[0];
                    break;

                case 1:
                    img_main = m_moduleElements.m_admElement.m_leftbar[1].btn_menu.image;
                    img_bg = m_moduleElements.m_admElement.m_leftbar[1].imgs[0];
                    break;

                case 2:
                    img_main = m_moduleElements.m_admElement.m_leftbar[2].btn_menu.image;
                    img_bg = m_moduleElements.m_admElement.m_leftbar[2].imgs[0];
                    break;
            }

            if(img_bg == null || img_main == null)
			{
                throw new Definition.Exceptions.ImagesNotAssigned();
			}

            m_uiResources.m_adm_leftbar.SetImage(img_bg, img_main, index, isOn);
		}
    }
}
