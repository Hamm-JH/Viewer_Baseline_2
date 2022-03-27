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

		public EventData_API(InputEventType _eventType, GameObject _obj,
			UnityEvent<GameObject> _event)
		{
			EventType = _eventType;
			m_obj = _obj;
			m_clickEvent = _event;
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			switch(EventType)
			{
				// 이 영역에서 선택 이벤트를 처리하지 않고, 기존에 있던 EventData_Input :: clickSuccessUp 이벤트로 동작을 전달한다.
				case InputEventType.API_SelectObject:
				case InputEventType.API_SelectIssue:
					{

						EventManager.Instance.OnEvent(new EventData_Input(
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

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			switch(EventType)
			{
				// 스킵
				case InputEventType.API_SelectObject:
				case InputEventType.API_SelectIssue:
					{
						EventManager.Instance.OnEvent(new EventData_Input(
							_eventType: InputEventType.Input_clickSuccessUp,
							_obj: m_obj,
							_event: m_clickEvent
							));
					}
					break;
			}
		}
	}
}
