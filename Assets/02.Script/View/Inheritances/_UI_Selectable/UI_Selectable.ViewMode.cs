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
		private void Event_Toggle_ViewMode()
		{
			childPanel?.SetActive(!(bool)childPanel.activeSelf);
		}

		private void Event_Toggle_ViewMode(UIEventType _eventType)
		{
			MainManager.Instance.UpdateCameraMode(_eventType);
			ContentManager.Instance.SetCameraCenterPosition(_eventType);
		}
	}
}
