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
	public abstract class AEventData
	{
		protected List<IInteractable> m_elements;
		protected InputEventType m_eventType;
		private Status status;

		/// <summary>
		/// 입력 상태 싱글인지 멀티인지 확인
		/// </summary>
		public InputStatus inputStatus = InputStatus.Single;

		/// <summary>
		/// 상호작용 가능한 요소
		/// </summary>
		//public IInteractable Element { get => m_element; set => m_element=value; }
		public List<IInteractable> Elements { get => m_elements; set => m_elements=value; }

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

		/// <summary>
		/// 선택된 객체
		/// </summary>
		public GameObject Selected3D { get => m_selected3D; set => m_selected3D=value; }

		/// <summary>
		/// 3d ray에 맞은 객체 관리
		/// </summary>
		public RaycastHit Hit { get => m_hit; set => m_hit=value; }

		/// <summary>
		/// UI ray에 맞은 객체 리스트 관리
		/// </summary>
		public List<RaycastResult> Results { get => m_results; set => m_results=value; }

		//----------------------------------------------------------------------------------------------

		[Header("Private UI")]
		protected UIEventType m_uiEventType;
		protected ToggleType m_toggleType;

		/// <summary>
		/// UI 이벤트 분류
		/// </summary>
		public UIEventType UiEventType { get => m_uiEventType; set => m_uiEventType=value; }
		
		/// <summary>
		/// UI 토글 여부 분류
		/// </summary>
		public ToggleType ToggleType { get => m_toggleType; set => m_toggleType=value; }
		
		/// <summary>
		/// 이벤트 전처리 메서드
		/// </summary>
		/// <param name="_mList">이벤트 전처리</param>
		public abstract void OnProcess(List<ModuleCode> _mList);

		/// <summary>
		/// 이벤트 후처리 메서드
		/// </summary>
		/// <param name="_sEvents">현재 이벤트 상태</param>
		public abstract void DoEvent(Dictionary<InputEventType, AEventData> _sEvents);

		/// <summary>
		/// AdminViewer 키맵 선택 이벤트
		/// </summary>
		/// <param name="_hits">UI 선택된 객체 리스트</param>
		protected bool IsClickOnKeymap(List<RaycastResult> _hits)
		{
			bool result = false;

			if (_hits.Count != 0)
			{
				_hits.ForEach(x =>
				{
					if (x.gameObject.name.Contains("Keymap"))
					{
						result = true;
					}
				});
			}

			return result;
		}
    }
}
