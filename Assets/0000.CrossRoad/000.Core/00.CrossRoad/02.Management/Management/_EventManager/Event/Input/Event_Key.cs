using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Management.Events.Inputs
{
	public class Event_Key : EventData_Input
	{
		public List<KeyData> m_keys;
		UnityEvent<List<KeyData>> m_keyEvent;

		/// <summary>
		/// 키보드 입력 이벤트 생성자
		/// </summary>
		/// <param name="_eventType">이벤트 분류</param>
		/// <param name="_kData">키보드 리스트</param>
		/// <param name="_camera">카메라</param>
		/// <param name="_grRaycaster">그래픽 레이캐스터</param>
		/// <param name="_event">키보드 리스트를 필요로 하는 이벤트</param>
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

		/// <summary>
		/// 이벤트 전처리
		/// </summary>
		/// <param name="_mList">모듈 리스트</param>
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

		/// <summary>
		/// 이벤트 후처리
		/// </summary>
		/// <param name="_sEvents">현재 이벤트 리스트</param>
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
	}
}
