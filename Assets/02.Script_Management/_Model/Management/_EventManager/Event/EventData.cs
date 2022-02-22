using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using UnityEngine.EventSystems;
	using View;

	/// <summary>
	/// 접근 데이터와 데이터에 접근하기 위한 메서드 원형 선언
	/// </summary>
	[System.Serializable]
	public abstract class EventData
	{
		protected IInteractable m_element;
		protected InputEventType m_eventType;
		private Status status;

		/// <summary>
		/// 상호작용 가능한 요소
		/// </summary>
		public IInteractable Element { get => m_element; set => m_element=value; }

		/// <summary>
		/// 발생한 이벤트의 형식
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		/// <summary>
		/// 이벤트 처리 결과
		/// </summary>
		public Status StatusCode { get => status; set => status=value; }

		[Header("Private")]
		protected GameObject m_selected3D = null;
		protected RaycastHit m_hit = default(RaycastHit);
		protected List<RaycastResult> m_results = new List<RaycastResult>();

		public GameObject Selected3D { get => m_selected3D; set => m_selected3D=value; }
		public RaycastHit Hit { get => m_hit; set => m_hit=value; }
		public List<RaycastResult> Results { get => m_results; set => m_results=value; }



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
		public abstract void DoEvent();
		
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
