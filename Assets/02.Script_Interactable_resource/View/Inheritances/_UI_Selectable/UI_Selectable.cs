using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using Management;
	using Module.UI;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public partial class UI_Selectable : Interactable,
		IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		public override GameObject Target
		{
			get => gameObject;
		}

		public override List<GameObject> Targets => throw new System.NotImplementedException();

		public GameObject ChildPanel { get => childPanel; set => childPanel=value; }

		Button m_btn;
		Slider m_slider;

		[SerializeField] AUI m_rootUI;
		[SerializeField] UIEventType eventType;
		[SerializeField] UniqueUIEventType m_unique;

		[Header("Linked UI Elements")]
		/// <summary>
		/// 자식 패널 객체
		/// </summary>
		[SerializeField] GameObject childPanel;

		/// <summary>
		/// ui 효과 요소
		/// </summary>
		[SerializeField] List<GameObject> m_uiFXs;

		/// <summary>
		/// ui 호버링 연결 토글요소
		/// </summary>
		[SerializeField] List<GameObject> m_uiHoverElements;

		/// <summary>
		/// 표면 버튼에 반응하는 변수
		/// </summary>
		[Header("Test Values")]
		public string t_surfaceCode;

		private void Start()
		{
			// 버튼이 존재한다면, 버튼의 클릭 이벤트를 Conditional Branch에 연결한다.
			if (gameObject.TryGetComponent<Button>(out m_btn))
			{
				m_btn.onClick.AddListener(new UnityAction(OnSelect));
			}

			// 슬라이더가 존재한다면, 슬라이더의 슬라이딩 이벤트를 Conditional Branch에 연결한다.
			if (gameObject.TryGetComponent<Slider>(out m_slider))
			{
				m_slider.onValueChanged.AddListener(new UnityAction<float>(OnChangeValue));
			}
			//m_btn.
		}

		public override void OnDeselect()
		{
			Debug.Log($"OnDeselect : {this.name}");
		}

		public override void OnSelect()
		{
			//Debug.Log($"OnSelect : {this.name}");

			ConditionalBranch(eventType);
		}

		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{

		}

		/// <summary>
		/// 슬라이더 값 변경시 실행
		/// </summary>
		/// <param name="_value"></param>
		public override void OnChangeValue(float _value)
		{
			ConditionalBranch(_value, eventType);
		}

		#region Conditional Branch

		/// <summary>
		/// Slider Branch
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_eventType"></param>
		private void ConditionalBranch(float _value, UIEventType _eventType)
		{
			m_rootUI.GetUIEvent(_value, _eventType, this);
		}

		/// <summary>
		/// Button Branch
		/// </summary>
		/// <param name="_eventType"></param>
		private void ConditionalBranch(UIEventType _eventType)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			//bool isDemoAdminViewer = false;
			//if (Platforms.IsDemoAdminViewer(pCode)) isDemoAdminViewer = true;

			m_rootUI.GetUIEvent(_eventType, this);

			//// 데모용 관리자뷰어 모드일때
			//if (isDemoAdminViewer)
			//{
			//	// 루트 UI에서 이벤트 분배
			//	m_rootUI.GetUIEvent(_eventType, this);
			//}
			//// 잠시 교량, 터널용 이벤트 뒤로 두기
			//else
			//{
			//	m_rootUI.GetUIEvent(_eventType, this);
			//}
		}

		#endregion

		#region Enable Disable

		private void OnEnable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));

			//if (m_unique == UniqueUIEventType.SetChild_Highlight)
			//{
			//	((Image)m_btn.targetGraphic).sprite = m_btn.spriteState.disabledSprite;
			//}
		}

		private void OnDisable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));

			//if (m_unique == UniqueUIEventType.SetChild_Highlight)
			//{
			//	((Image)m_btn.targetGraphic).sprite = m_btn.spriteState.disabledSprite;
			//}
		}

		#endregion

		#region Mouse region

		/// <summary>
		/// UI 제대로 클릭시 발동?
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerUp(PointerEventData eventData)
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		/// <summary>
		/// 마우스 포인터 진입시 발동
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerEnter(PointerEventData eventData)
		{

			m_uiHoverElements.ForEach(x => x.SetActive(true));
		}

		/// <summary>
		/// 마우스 포인터 나가면 발동
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerExit(PointerEventData eventData)
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		#endregion
	}
}
