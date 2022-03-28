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

			if (Selected3D != null)
			{
				m_focusEvent.Invoke(m_focus, m_focusDelta);
			}
			// 빈 공간을 누른 경우
			else if (m_results.Count == 0)
			{
				m_focusEvent.Invoke(m_focus, m_focusDelta);
			}
			// UI 객체를 누른 경우 m_results.Count != 0
			else
			{
				Debug.LogError("포커스시 키맵 UI를 포커싱하는지 확인 필요");
				ContentManager.Instance.Input_KeymapFocus(m_focus, m_focusDelta);
			}
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			StatusCode = Status.Update;
		}
	}
}
