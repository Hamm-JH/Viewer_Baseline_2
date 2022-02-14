using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;

	public class EventData
	{
		private IInteractable m_element;
		private InputEventType m_eventType;

		/// <summary>
		/// 상호작용 가능한 요소
		/// </summary>
		public IInteractable Element { get => m_element; set => m_element=value; }

		/// <summary>
		/// 발생한 이벤트의 형식
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		public EventData(IInteractable _target, InputEventType _mainEventType)
		{
			Element = _target;
			EventType = _mainEventType;
		}


		public static bool IsEqual(EventData A, EventData B)
		{
			if(A.Element.Target == B.Element.Target)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
