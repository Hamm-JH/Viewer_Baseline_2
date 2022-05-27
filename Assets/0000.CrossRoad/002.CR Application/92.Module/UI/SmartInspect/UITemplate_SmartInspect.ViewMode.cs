using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
	using Definition;
	using Management;
	using UnityEngine;
	using View;

    public partial class UITemplate_SmartInspect : AUI
    {
		/// <summary>
		/// 모두 보이기
		/// </summary>
		public void Event_View_Home()
		{
			ContentManager.Instance.SetCameraCenterPosition();

			ModDmg_ResetBasePosition();
			ModRcv_ResetBasePosition();
			ModAdm_ResetBasePosition();

			ContentManager.Instance.Reset_ModelObject();
		}

		public void Event_Toggle_ChildPanel(int index, GameObject ChildPanel)
		{
			// 자식 객체 토글
			// 이 인스턴스가 가진 childPanel을 보내 토글 끄기 대상에서 제외한다.

			TogglePanelList(index, ChildPanel);
			//m_rootUI.TogglePanelList(index, ChildPanel);
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

		private void Event_Toggle_ViewMode(UIEventType _eventType)
		{
			MainManager.Instance.UpdateCameraMode(_eventType);
			ContentManager.Instance.SetCameraCenterPosition(_eventType);
		}
	}
}
