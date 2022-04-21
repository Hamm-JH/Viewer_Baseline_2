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

		public void Event_Mode_HideIsolate(Inspect_EventType _type)
        {
			UIEventType type = _type == Inspect_EventType.BtnBar_05_Hide ? UIEventType.Mode_Hide : UIEventType.Mode_Isolate;

			ToggleType toggle = type == UIEventType.Mode_Hide ? ToggleType.Hide : ToggleType.Isolate;
			//ToggleType toggle = _type == Inspect_EventType.BtnBar_05_Hide ? ToggleType.Hide : ToggleType.Isolate;

			ContentManager.Instance.Toggle_ModelObject(type, toggle);
		}
    }
}
