using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using System.Linq;
	using UnityEngine.EventSystems;
	using View;

	/// <summary>
	/// 접근 데이터와 데이터에 접근하기 위한 메서드 원형 선언
	/// </summary>
	[System.Serializable]
	public abstract class EventData
	{
		//protected IInteractable m_element;
		protected List<IInteractable> m_elements;
		protected InputEventType m_eventType;
		private Status status;

		/// <summary>
		/// 상호작용 가능한 요소
		/// </summary>
		//public IInteractable Element { get => m_element; set => m_element=value; }
		public List<IInteractable> Elements { get => m_elements; set => m_elements=value; }

		public List<GameObject> objects;

		/// <summary>
		/// 발생한 이벤트의 형식
		/// </summary>
		public InputEventType EventType { get => m_eventType; set => m_eventType=value; }

		/// <summary>
		/// 이벤트 처리 결과
		/// </summary>
		public Status StatusCode { get => status; set => status=value; }

		//----------------------------------------------------------------------------------------------

		[Header("Private 3D")]
		protected GameObject m_selected3D = null;
		protected RaycastHit m_hit = default(RaycastHit);
		protected List<RaycastResult> m_results = new List<RaycastResult>();
		// 마우스 버튼 번호
		protected int m_btn;

		public GameObject Selected3D { get => m_selected3D; set => m_selected3D=value; }
		public RaycastHit Hit { get => m_hit; set => m_hit=value; }
		public List<RaycastResult> Results { get => m_results; set => m_results=value; }
		public int BtnIndex { get => m_btn; set => m_btn=value; }

		//----------------------------------------------------------------------------------------------

		[Header("Private UI")]
		protected UIEventType m_uiEventType;
		protected ToggleType m_toggleType;

		public UIEventType UiEventType { get => m_uiEventType; set => m_uiEventType=value; }
		public ToggleType ToggleType { get => m_toggleType; set => m_toggleType=value; }
		


		/// <summary>
		/// 이벤트 처리 메서드
		/// </summary>
		public abstract void OnProcess(GameObject _cObj);


		/// <summary>
		/// 이벤트 내부처리 완료후 외부 이벤트 발산처리
		/// </summary>
		public abstract void DoEvent();
		public abstract void DoEvent(List<GameObject> _objs);

		public abstract void DoEvent(Dictionary<InputEventType, EventData> _sEvents);
		
		public static bool IsEqual(EventData A, EventData B)
		{
			if(A.Elements.Last().Target == B.Elements.Last().Target)
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
