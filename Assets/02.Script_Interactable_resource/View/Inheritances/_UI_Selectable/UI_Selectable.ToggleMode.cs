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
		/// 선택한 객체를 비활성화 (반투명)
		/// </summary>
		public void Event_Mode_Hide()
		{
			ContentManager.Instance.Toggle_ModelObject(Definition.ToggleType.Hide);
		}

		/// <summary>
		/// 선택한 객체 이외의 객체들을 비활성화
		/// </summary>
		public void Event_Mode_Isolate()
		{
			ContentManager.Instance.Toggle_ModelObject(Definition.ToggleType.Isolate);
		}
	}
}
