using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Management.Events.Inputs
{
	public class Event_Key : AEventData
	{
		private Camera m_camera;
		private GraphicRaycaster m_grRaycaster;
		public List<KeyData> m_keys;
		UnityEvent<List<KeyData>> m_keyEvent;

		public Event_Key(InputEventType _eventType,
			List<KeyData> _kData,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<List<KeyData>> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_keys = _kData;
			m_keyEvent = _event;
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			// 현재 입력된 키가 1개 이상일 경우
			if (m_keys != null && m_keys.Count != 0)
			{
				// 키 데이터 업데이트
				if (_sEvents.ContainsKey(InputEventType.Input_key))
				{
					_sEvents[InputEventType.Input_key] = this;
				}
				else
				{
					_sEvents.Add(InputEventType.Input_key, this);
				}
			}
			else
			{
				// 기존 저장된 키 지우기
				if (_sEvents.ContainsKey(InputEventType.Input_key))
				{
					_sEvents.Remove(InputEventType.Input_key);
				}
			}

			m_keyEvent.Invoke(m_keys);
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			if (m_keys != null && m_keys.Count != 0)
			{
				StatusCode = Status.Update;
			}
			// KeyData 리스트가 null인 경우
			else
			{
				StatusCode = Status.Update;
			}
		}
	}
}
