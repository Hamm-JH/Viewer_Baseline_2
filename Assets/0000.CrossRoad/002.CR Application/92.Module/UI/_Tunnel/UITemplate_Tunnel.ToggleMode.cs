using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
	using Management;
	using View;

	public partial class UITemplate_Tunnel : AUI
	{
		public UITemplate_Tunnel()
		{
		}

		/// <summary>
		/// 모든 객체를 다시 켜기
		/// </summary>
		public void Event_Mode_ShowAll()
		{
			ContentManager.Instance.Reset_ModelObject();
			m_sliders.ResetSlider_transparency();
			m_sliders.ResetSlider_scale();
		}

		/// <summary>
		/// 선택한 객체를 비활성화 (반투명)
		/// </summary>
		public void Event_Mode_HideIsolate(UIEventType _type)
		{
			ToggleType toggle = _type == UIEventType.Mode_Hide ? ToggleType.Hide : ToggleType.Isolate;
			ContentManager.Instance.Toggle_ModelObject(_type, toggle);
		}

		
	}
}
