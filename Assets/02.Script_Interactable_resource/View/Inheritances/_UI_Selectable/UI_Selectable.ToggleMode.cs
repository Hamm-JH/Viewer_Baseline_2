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
		/// <summary>
		/// ������ ��ü�� ��Ȱ��ȭ (������)
		/// </summary>
		public void Event_Mode_HideIsolate(UIEventType _type)
		{
			ToggleType toggle = _type == UIEventType.Mode_Hide ? ToggleType.Hide : ToggleType.Isolate;
			ContentManager.Instance.Toggle_ModelObject(_type, toggle);
		}
	}
}
