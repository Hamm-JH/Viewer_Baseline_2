using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
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

		Button m_btn;
		Slider m_slider;

		[SerializeField] AUI m_rootUI;
		[SerializeField] UIEventType eventType;
		[SerializeField] UniqueUIEventType m_unique;

		[Header("Linked UI Elements")]
		/// <summary>
		/// �ڽ� �г� ��ü
		/// </summary>
		[SerializeField] GameObject childPanel;

		/// <summary>
		/// ui ȿ�� ���
		/// </summary>
		[SerializeField] List<GameObject> m_uiFXs;

		/// <summary>
		/// ui ȣ���� ���� ��ۿ��
		/// </summary>
		[SerializeField] List<GameObject> m_uiHoverElements;

		/// <summary>
		/// ǥ�� ��ư�� �����ϴ� ����
		/// </summary>
		[Header("Test Values")]
		[SerializeField] string t_surfaceCode;

		private void Start()
		{
			// ��ư�� �����Ѵٸ�, ��ư�� Ŭ�� �̺�Ʈ�� Conditional Branch�� �����Ѵ�.
			if(gameObject.TryGetComponent<Button>(out m_btn))
			{
				m_btn.onClick.AddListener(new UnityAction(OnSelect));
			}

			// �����̴��� �����Ѵٸ�, �����̴��� �����̵� �̺�Ʈ�� Conditional Branch�� �����Ѵ�.
			if(gameObject.TryGetComponent<Slider>(out m_slider))
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

		public override void OnDeselect<T>(T t)
		{
			
		}

		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{
			
		}

		/// <summary>
		/// �����̴� �� ����� ����
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
			switch(_eventType)
			{
				case UIEventType.Slider_Model_Transparency:
					Event_Model_Transparency(_value);
					break;

				case UIEventType.Slider_Icon_Scale:
					Event_Icon_Scale(_value);
					break;
			}
		}

		/// <summary>
		/// Button Branch
		/// </summary>
		/// <param name="_eventType"></param>
		private void ConditionalBranch(UIEventType _eventType)
		{
			switch(_eventType)
			{
				case UIEventType.View_Home:
					// �ʱ� ȭ������ ����
					Event_View_Home();
					Event_Toggle_ChildPanel(1);
					break;

				case UIEventType.Toggle:
				case UIEventType.Viewport_ViewMode:
					Event_Toggle_ViewMode();
					break;

				case UIEventType.Toggle_ChildPanel1:
					// ChildPanel 1�� ���
					Event_Toggle_ChildPanel(1);
					Event_Toggle_ViewMode();
					break;

				case UIEventType.Viewport_ViewMode_ISO:
				case UIEventType.Viewport_ViewMode_TOP:
				case UIEventType.Viewport_ViewMode_SIDE:
				case UIEventType.Viewport_ViewMode_BOTTOM:
					Event_Toggle_ViewMode(_eventType);
					break;

				case UIEventType.OrthoView_Orthogonal:
					Event_ToggleOrthoView(true);
					Event_Toggle_ViewMode();
					break;

				case UIEventType.OrthoView_Perspective:
					Event_ToggleOrthoView(false);
					Event_Toggle_ViewMode();
					break;

					// ��ü ��� �ٽ� �ѱ�
				case UIEventType.Mode_ShowAll:
					Event_Mode_ShowAll();
					Event_Toggle_ChildPanel(1);
					break;

				case UIEventType.Mode_Hide:
				case UIEventType.Mode_Isolate:
					Event_Mode_HideIsolate(_eventType);
					Event_Toggle_ChildPanel(1);
					break;

				case UIEventType.Test_Surface:
					Event_Legacy_ChangeCameraDirection();
					break;
			}
		}



		#endregion

		#region Enable Disable

		private void OnEnable()
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));

			if (m_unique == UniqueUIEventType.SetChild_Highlight)
			{
				((Image)m_btn.targetGraphic).sprite = m_btn.spriteState.disabledSprite;
			}
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
		/// UI ����� Ŭ���� �ߵ�?
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerUp(PointerEventData eventData)
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		/// <summary>
		/// ���콺 ������ ���Խ� �ߵ�
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			
			m_uiHoverElements.ForEach(x => x.SetActive(true));
		}

		/// <summary>
		/// ���콺 ������ ������ �ߵ�
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerExit(PointerEventData eventData)
		{
			m_uiHoverElements.ForEach(x => x.SetActive(false));
		}

		#endregion
	}
}
