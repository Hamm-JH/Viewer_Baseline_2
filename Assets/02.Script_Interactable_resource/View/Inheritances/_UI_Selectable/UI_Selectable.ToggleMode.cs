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
		public void Event_Mode_Hide(UIEventType _type)
		{
			ContentManager.Instance.Toggle_ModelObject(_type, Definition.ToggleType.Hide);
		}

		/// <summary>
		/// ������ ��ü �̿��� ��ü���� ��Ȱ��ȭ
		/// </summary>
		public void Event_Mode_Isolate(UIEventType _type)
		{
			ContentManager.Instance.Toggle_ModelObject(_type, Definition.ToggleType.Isolate);
		}
	}
}
