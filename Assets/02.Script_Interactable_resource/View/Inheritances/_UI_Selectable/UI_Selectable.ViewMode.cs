using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using Management;
	using UnityEngine.UI;

	public partial class UI_Selectable : Interactable
	{
		/// <summary>
		/// ��� ���̱�
		/// </summary>
		public void Event_View_Home()
		{
			ContentManager.Instance.SetCameraCenterPosition();
		}

		public void Event_Toggle_ChildPanel(int index)
		{
			// �ڽ� ��ü ���
			// �� �ν��Ͻ��� ���� childPanel�� ���� ��� ���� ��󿡼� �����Ѵ�.

			m_rootUI.TogglePanelList(index, ChildPanel);
		}

		private void Event_Toggle_ViewMode()
		{
			bool toggle = false;

			if (ChildPanel != null)
			{
				toggle = !(bool)ChildPanel.activeSelf;
				
				ChildPanel.SetActive(toggle);

				foreach(GameObject obj in m_uiFXs)
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
