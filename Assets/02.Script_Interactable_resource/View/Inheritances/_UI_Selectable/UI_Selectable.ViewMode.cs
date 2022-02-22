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
