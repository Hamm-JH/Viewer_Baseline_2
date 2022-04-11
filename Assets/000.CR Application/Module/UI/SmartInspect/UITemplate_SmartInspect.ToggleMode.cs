using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
	using Definition;
	using Management;
	using UnityEngine;
	using View;

    public partial class UITemplate_SmartInspect : AUI
    {
		/// <summary>
		/// 모든 객체를 다시 켜기
		/// </summary>
		public void Event_Mode_ShowAll()
		{
			ContentManager.Instance.Reset_ModelObject();
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
