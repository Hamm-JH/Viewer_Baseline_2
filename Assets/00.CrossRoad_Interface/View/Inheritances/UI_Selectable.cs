﻿using System.Collections;
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
	using AdminViewer.Tunnel;
	using static Platform.Bridge.Bridges;
    using static SmartInspect.ListElement;
    using SmartInspect;

    public partial class UI_Selectable : Interactable,
		IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[System.Serializable]
		public class Datas
		{
			/// <summary>
			/// ButtonBar 사용
			/// </summary>
			public int m_toggleIndex;

			public ListElement m_issueListElement;

			public CodeLv4 m_bridgeIssueCode;
			public TunnelCode m_tunnelCode;
		}

		public override GameObject Target
		{
			get => gameObject;
		}

		public override List<GameObject> Targets => throw new System.NotImplementedException();

		Button m_btn;
		Slider m_slider;

		[SerializeField] AUI m_rootUI;
		[SerializeField] UIEventType eventType;
		[SerializeField] Inspect_EventType inspect_eventType;
		[SerializeField] Datas m_data;
		//[SerializeField] UniqueUIEventType m_unique;

		public Datas Data
		{
			get => m_data;
			set => m_data = value;
		}

		public AUI RootUI
        {
			get => m_rootUI;
			set => m_rootUI = value;
        }

		public UIEventType EventType
        {
			get => eventType;
			set => eventType = value;
        }

        public Inspect_EventType Inspect_eventType 
		{ 
			get => inspect_eventType; 
			set => inspect_eventType = value; 
		}

        [Header("Linked UI Elements")]
		/// <summary>
		/// 자식 패널 객체
		/// </summary>
		//[SerializeField] GameObject childPanel;

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
		}

		public override void OnDeselect()
		{
			Debug.Log($"OnDeselect : {this.name}");
		}

		public override void OnSelect()
		{
			if(eventType == UIEventType.Null)
            {
				Debug.LogError($"{this.name} needs event");
				return;
            }

			m_rootUI.GetUIEvent(eventType, this);
			m_rootUI.GetUIEvent(Inspect_eventType, this);
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
			if (eventType == UIEventType.Null)
			{
				Debug.LogError($"{this.name} needs event");
				return;
			}

			m_rootUI.GetUIEvent(_value, eventType, this);
		}

		#region Enable Disable

		private void OnEnable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		private void OnDisable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
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
