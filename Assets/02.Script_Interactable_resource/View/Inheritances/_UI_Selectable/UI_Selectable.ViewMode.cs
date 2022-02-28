using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using Management;

	public partial class UI_Selectable : Interactable
	{
		
		public void Event_Toggle_ChildPanel(int index)
		{
			// 자식 객체 토글
			// 이 인스턴스가 가진 childPanel을 보내 토글 끄기 대상에서 제외한다.

			m_rootUI.TogglePanelList(index, childPanel);
		}

		private void Event_Toggle_ViewMode()
		{

			if(childPanel != null)
			{
				bool toggle = !(bool)childPanel.activeSelf;
				
				childPanel.SetActive(toggle);

				foreach(GameObject obj in uiFXs)
				{
					obj.SetActive(toggle);
				}
			}

		}

		private void Event_Toggle_ViewMode(UIEventType _eventType)
		{
			MainManager.Instance.UpdateCameraMode(_eventType);
			ContentManager.Instance.SetCameraCenterPosition(_eventType);
		}
	}
}
