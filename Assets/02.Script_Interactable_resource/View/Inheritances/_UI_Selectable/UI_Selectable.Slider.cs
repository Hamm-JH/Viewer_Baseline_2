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
		/// 모델 투명도 변경
		/// </summary>
		/// <param name="_value"></param>
		public void Event_Model_Transparency(float _value)
		{
			Debug.Log($"Event_Model_Transparency {_value}");

			ContentManager.Instance.Set_Model_Transparency(_value);
		}

		/// <summary>
		/// 아이콘 크기 변경
		/// </summary>
		/// <param name="_value"></param>
		public void Event_Icon_Scale(float _value)
		{
			Debug.Log($"Event_Icon_Scale {_value}");

			ContentManager.Instance.Set_Issue_Scale(_value);
		}
	}
}
