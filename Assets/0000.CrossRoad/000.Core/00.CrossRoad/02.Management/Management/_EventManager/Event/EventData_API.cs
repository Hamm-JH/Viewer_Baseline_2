using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using System.Linq;
	using UnityEngine.Events;
	using View;

	[System.Serializable]
	public class EventData_API : AEventData
	{
		[Header("API :: SelectObject")]
		GameObject m_obj;
		UnityEvent<GameObject> m_clickEvent;

		/// <summary>
		/// API 이벤트 생성자
		/// </summary>
		/// <param name="_eventType">입력 이벤트 분류</param>
		/// <param name="_obj">선택된 3D 객체</param>
		/// <param name="_event">객체 선택 이벤트</param>
		public EventData_API(InputEventType _eventType, GameObject _obj,
			UnityEvent<GameObject> _event)
		{
			EventType = _eventType;
			m_obj = _obj;
			m_clickEvent = _event;
		}

		/// <summary>
		/// 이벤트 전처리
		/// </summary>
		/// <param name="_mList">모듈 리스트</param>
		public override void OnProcess(List<ModuleCode> _mList)
		{
			switch(EventType)
			{
				// 이 영역에서 선택 이벤트를 처리하지 않고, 기존에 있던 EventData_Input :: clickSuccessUp 이벤트로 동작을 전달한다.
				case InputEventType.API_SelectObject:
				case InputEventType.API_SelectIssue:
					{
						EventManager.Instance.OnEvent(new Events.Inputs.Event_SelectObject(
							_eventType: InputEventType.Input_clickSuccessUp,
							_obj: m_obj,
							_event: m_clickEvent
							));

						Status _success = Status.Skip;
						StatusCode = _success;
					} 
					break;
			}
		}

		/// <summary>
		/// 이벤트 후처리
		/// </summary>
		/// <param name="_sEvents">현재 이벤트 리스트</param>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			switch(EventType)
			{
				// 스킵
				case InputEventType.API_SelectObject:
				case InputEventType.API_SelectIssue:
					{
						//EventManager.Instance.OnEvent(new Events.Inputs.Event_SelectObject(
						//	_eventType: InputEventType.Input_clickSuccessUp,
						//	_obj: m_obj,
						//	_event: m_clickEvent
						//	));
					}
					break;
			}
		}
	}
}
