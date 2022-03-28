using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
	using UnityEngine.Events;
	using UnityEngine.UI;

	public class Event_ClickFailureUp : EventData_Input
	{
		//public Camera m_camera;
		//public GraphicRaycaster m_grRaycaster;
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;


		public Event_ClickFailureUp(InputEventType _eType,
			int _btn, Vector3 _mousePos,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_clickPosition = _mousePos;
			m_clickEvent = _event;
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			if( _sEvents.ContainsKey(InputEventType.Input_clickDown))
			{
				_sEvents.Remove(InputEventType.Input_clickDown);
			}
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			StatusCode = Status.Update;
		}
	}
}
