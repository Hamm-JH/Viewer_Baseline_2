using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
    using Management.Content;
    using Module.Interaction;
    using Module.UI;
    using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class Event_Focus : EventData_Input
	{
		//private Camera m_camera;
		//private GraphicRaycaster m_grRaycaster;
		private Vector3 m_focus;
		private float m_focusDelta;
		UnityEvent<Vector3, float> m_focusEvent;

		public Event_Focus(InputEventType _eventType,
			Vector3 _focus, float _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<Vector3, float> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_focus = _focus;
			m_focusDelta = _delta;
			m_focusEvent = _event;
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			m_selected3D = null;
			m_hit = default(RaycastHit);
			m_results = new List<RaycastResult>();

			Get_Collect3DObject(Input.mousePosition, out m_selected3D, out m_hit, out m_results);

			float _delta = m_focusDelta;
			PlatformCode pCode = MainManager.Instance.Platform;
			if (Platforms.IsDemoWebViewer(pCode)) 
			{
				_delta = m_focusDelta;
			}
			else if(Platforms.IsSmartInspectPlatform(pCode))
            {
				float zoomSensitivity = ((SmartInspectManager)(SmartInspectManager.Instance)).setting.ZoomSensitivity;

				//Module_Interaction interaction = ContentManager.Instance.Module<Module_Interaction>();

				//UITemplate_SmartInspect inspect = (UITemplate_SmartInspect)interaction.UiInstances[0];

				_delta = m_focusDelta * zoomSensitivity;
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }

			if (Selected3D != null)
			{
				m_focusEvent.Invoke(m_focus, _delta);
			}
			// �� ������ ���� ���
			else if (m_results.Count == 0)
			{
				m_focusEvent.Invoke(m_focus, _delta);
			}
			// UI ��ü�� ���� ��� m_results.Count != 0
			else
			{
				pCode = MainManager.Instance.Platform;

				if(Platforms.IsDemoAdminViewer(pCode))
                {
					Debug.LogError("��Ŀ���� Ű�� UI�� ��Ŀ���ϴ��� Ȯ�� �ʿ�");
					ContentManager.Instance.Input_KeymapFocus(m_focus, _delta);
                }
			}
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			StatusCode = Status.Update;
		}
	}
}
