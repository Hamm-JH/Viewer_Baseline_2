using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;

	/// <summary>
	/// 접근 데이터와 데이터에 접근하기 위한 메서드 원형 선언
	/// </summary>
	public abstract class EventData
	{
		protected IInteractable m_element;
		protected InputEventType m_eventType;

		/// <summary>
		/// 상호작용 가능한 요소
		/// </summary>
		public IInteractable Element { get => m_element; set => m_element=value; }

		/// <summary>
		/// 발생한 이벤트의 형식
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		//public EventData() { }

		//public EventData(IInteractable _target, InputEventType _mainEventType)
		//{
		//	Element = _target;
		//	EventType = _mainEventType;
		//}

		/// <summary>
		/// 이벤트 처리 메서드
		/// </summary>
		public abstract void OnProcess(GameObject _cObj);

		/// <summary>
		/// 이벤트 내부처리 완료후 외부 이벤트 발산처리
		/// </summary>
		public void Do()
		{

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
