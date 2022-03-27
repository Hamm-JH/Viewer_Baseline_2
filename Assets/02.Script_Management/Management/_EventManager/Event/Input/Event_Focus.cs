using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class Event_Focus : EventData_Input
	{
		private Camera m_camera;
		private GraphicRaycaster m_graphicRaycaster;
		private Vector3 m_focus;
		private float m_focusDelta;
		UnityEvent<Vector3, float> m_focusEvent;

		public Event_Focus(InputEventType _eventType, GameObject _obj, UnityEvent<GameObject> _event) : base(_eventType, _obj, _event)
		{
		}

		public Event_Focus(InputEventType _eventType,
			Vector3 _focus, float _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<Vector3, float> _event) : base(_eventType, _focus, _delta, _camera, _grRaycaster, _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_focus = _focus;
			m_focusDelta = _delta;
			m_focusEvent = _event;
		}

		public Event_Focus(InputEventType _eventType, int _btn, Vector3 _mousePos, Camera _camera, GraphicRaycaster _graphicRaycaster, UnityEvent<GameObject> _event) : base(_eventType, _btn, _mousePos, _camera, _graphicRaycaster, _event)
		{
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			m_selected3D = null;
			m_hit = default(RaycastHit);
			m_results = new List<RaycastResult>();

			Get_Collect3DObject(Input.mousePosition, out m_selected3D, out m_hit, out m_results);

			if (Selected3D != null)
			{
				m_focusEvent.Invoke(m_focus, m_focusDelta);
			}
			// �� ������ ���� ���
			else if (m_results.Count == 0)
			{
				m_focusEvent.Invoke(m_focus, m_focusDelta);
			}
			// UI ��ü�� ���� ��� m_results.Count != 0
			else
			{
				ContentManager.Instance.Input_KeymapFocus(m_focus, m_focusDelta);
			}
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			StatusCode = Status.Update;
		}
	}
}
