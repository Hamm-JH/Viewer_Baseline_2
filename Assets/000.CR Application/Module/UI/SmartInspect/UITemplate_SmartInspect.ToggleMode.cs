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
		/// ��� ��ü�� �ٽ� �ѱ�
		/// </summary>
		public void Event_Mode_ShowAll()
		{
			ContentManager.Instance.Reset_ModelObject();
		}

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
