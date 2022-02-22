using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Management;

	public partial class UI_Selectable : Interactable
	{
		/// <summary>
		/// ������ ��ü�� ��Ȱ��ȭ (������)
		/// </summary>
		public void Event_Mode_Hide()
		{
			ContentManager.Instance.Toggle_ModelObject(Definition.ToggleType.Hide);
		}

		/// <summary>
		/// ������ ��ü �̿��� ��ü���� ��Ȱ��ȭ
		/// </summary>
		public void Event_Mode_Isolate()
		{
			ContentManager.Instance.Toggle_ModelObject(Definition.ToggleType.Isolate);
		}
	}
}
