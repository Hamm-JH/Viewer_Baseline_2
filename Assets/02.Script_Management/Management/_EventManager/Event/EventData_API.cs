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
	public class EventData_API : EventData
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

		public override void OnProcess(GameObject _cObj, List<ModuleCode> _mList)
		{
			switch(EventType)
			{
				// �� �������� ���� �̺�Ʈ�� ó������ �ʰ�, ������ �ִ� EventData_Input :: clickSuccessUp �̺�Ʈ�� ������ �����Ѵ�.
				case InputEventType.API_SelectObject:
				case InputEventType.API_SelectIssue:
					{
						Status _success = Status.Skip;

						EventManager.Instance.OnEvent(new EventData_Input(
							_eventType: InputEventType.Input_clickSuccessUp,
							_obj: m_obj,
							_event: m_clickEvent
							));

						StatusCode = _success;
					} 
					break;
			}
		}

		public override void DoEvent()
		{
			switch(EventType)
			{
				// ��ŵ
				case InputEventType.API_SelectObject:
					break;
			}
		}

		public override void DoEvent(List<GameObject> _objs)
		{
			Debug.Log("DoEvent Empty");
		}

		public override void DoEvent(Dictionary<InputEventType, EventData> _sEvents)
		{
			throw new System.NotImplementedException();
		}
	}
}
