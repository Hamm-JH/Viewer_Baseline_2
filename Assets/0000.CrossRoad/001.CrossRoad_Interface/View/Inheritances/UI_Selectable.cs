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
	using AdminViewer.Tunnel;
	using static Platform.Bridge.Bridges;
    using static SmartInspect.ListElement;
    using SmartInspect;
    using Platform.Tunnel;
    using Platform.Bridge;

    public partial class UI_Selectable : Interactable,
		IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{

		[System.Serializable]
		public class Datas
		{
			[Header("ButtonBar switch")]
			public bool m_isMovedDown;

			/// <summary>
			/// ButtonBar 사용
			/// </summary>
			public int m_toggleIndex;

			/// <summary>
			/// 이벤트 발생을 전달하는 리스트 인스턴스
			/// </summary>
			public ListElement m_issueListElement;

			/// <summary>
			/// 이벤트 발생시 UI 상태를 변경하기 위한 리스트 인스턴스
			/// </summary>
			public ListElement m_resourceListElement;

			public BridgeCode m_bridgeIssueCode;
			public TunnelCode m_tunnelCode;

			[Header("Image Panel")]
			[SerializeField] RectTransform thisRect;
			public bool m_isEnlarged = false;

			/// <summary>
			/// 이미지 확장
			/// </summary>
			public void EnlargeImage()
            {
				// 축소 단계
				if(m_isEnlarged)
                {
					m_isEnlarged = false;
					RepositionImage();

				}
				else
                {
					m_isEnlarged = true;
					Enlarge();

				}
            }

			/// <summary>
			/// 이미지 위치 변경
			/// </summary>
			private void RepositionImage()
            {
				thisRect.localPosition = new Vector2(148, 94);
            }

			/// <summary>
			/// 확대
			/// </summary>
			private void Enlarge()
            {
				thisRect.position = new Vector2(1600, 900);
            }
		}

		[System.Serializable]
		public class Resource
        {
			public bool m_useResource;
			public Color m_onDefault = Color.white;
			public Color m_onSelect = Color.white;
        }


		public override GameObject Target
		{
			get => gameObject;
		}

		public override List<GameObject> Targets => throw new System.NotImplementedException();

		Button m_btn;
		Slider m_slider;

		[SerializeField] AUI m_rootUI;
		[SerializeField] EventCode m_EventCodes;
		[SerializeField] UIEventType eventType;
		[SerializeField] Inspect_EventType inspect_eventType;
		[SerializeField] Datas m_data;
		[SerializeField] Resource m_resource;
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
			Button btn;
			Slider slider;

			// 버튼이 존재한다면, 버튼의 클릭 이벤트를 Conditional Branch에 연결한다.
			if (gameObject.TryGetComponent<Button>(out btn))
			{
				m_btn = btn;
				btn.onClick.AddListener(new UnityAction(OnSelect));
			}

			// 슬라이더가 존재한다면, 슬라이더의 슬라이딩 이벤트를 Conditional Branch에 연결한다.
			if (gameObject.TryGetComponent<Slider>(out slider))
			{
				m_slider = slider;
				slider.onValueChanged.AddListener(new UnityAction<float>(OnChangeValue));
			}
		}

        #region Button

		/// <summary>
		/// 선택 해제
		/// </summary>
        public override void OnDeselect()
		{
			Debug.Log($"OnDeselect : {this.name}");
		}

		/// <summary>
		/// 선택 해제
		/// </summary>
		public override void OnSelect()
		{
			if(eventType == UIEventType.Null)
            {
				Debug.LogError($"{this.name} needs event");
				return;
            }
			//Debug.Log(this.name);

			m_rootUI.GetUIEvent(eventType, this);
			m_rootUI.GetUIEvent(Inspect_eventType, this);
			Toggle_Status();

			m_EventCodes.OnSelect(m_rootUI, this);
		}

		/// <summary>
		/// 선택 해제
		/// </summary>
		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{

		}

		/// <summary>
		/// 상태 토글
		/// </summary>
		private void Toggle_Status()
        {
			if (!m_resource.m_useResource) return;

			//Data.m_resourceListElement;

			if (Data.m_resourceListElement != null)
			{
				Data.m_resourceListElement._CountData.m_elements.ForEach(x =>
				{
					x.m_data.m_partCount.btns_detail.ForEach(x =>
					{
						x.Toggle_btnColor(false);
					});

					x.m_data.m_dmgWork.btns_ui.ForEach(x =>
					{
						x.Toggle_btnColor(false);
					});

					x.m_data.m_rcvWork.btns_ui.ForEach(x =>
					{
						x.Toggle_btnColor(false);
					});

					
				});
			}



			//if (Data.m_issueListElement != null)
            //{
			//	Data.m_issueListElement._CountData.m_elements.ForEach(x =>
			//	{
			//		x.m_data.m_dmgWork.btns_ui.ForEach(x =>
			//		{
			//			x.Toggle_btnColor(false);
			//		});
			//
			//		x.m_data.m_rcvWork.btns_ui.ForEach(x =>
			//		{
			//			x.Toggle_btnColor(false);
			//		});
			//	});
            //}


			Toggle_btnColor(true);
        }

		/// <summary>
		/// 버튼색 토글
		/// </summary>
		/// <param name="isOn"></param>
		public void Toggle_btnColor(bool isOn)
        {
			if (!m_resource.m_useResource) return;

			if (m_btn != null)
			{
				m_btn.GetComponent<Image>().color = isOn ? m_resource.m_onSelect : m_resource.m_onDefault;
			}
		}

        #endregion

        #region Slider

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
			m_rootUI.GetUIEvent(_value, Inspect_eventType, this);

			m_EventCodes.OnChangeValue(_value, m_rootUI, this);
		}

        #endregion

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
