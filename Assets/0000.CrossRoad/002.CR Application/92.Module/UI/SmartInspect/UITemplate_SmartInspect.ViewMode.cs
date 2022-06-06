using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
	using Definition;
	using Management;
	using UnityEngine;
	using View;

    public partial class UITemplate_SmartInspect : RootUI
    {
		/// <summary>
		/// 모두 보이기
		/// </summary>
		public void Event_View_Home()
		{
			//ContentManager.Instance.SetCameraCenterPosition();

			ModDmg_ResetBasePosition();
			ModRcv_ResetBasePosition();
			ModAdm_ResetBasePosition();

			//ContentManager.Instance.Reset_ModelObject();
		}

		private void Event_Toggle_ViewMode(GameObject ChildPanel)
		{
			bool toggle = false;

			if (ChildPanel != null)
			{
				toggle = !(bool)ChildPanel.activeSelf;

				ChildPanel.SetActive(toggle);

				//foreach (GameObject obj in m_uiFXs)
				//{
				//	obj.SetActive(toggle);
				//}
			}
		}
	}
}
