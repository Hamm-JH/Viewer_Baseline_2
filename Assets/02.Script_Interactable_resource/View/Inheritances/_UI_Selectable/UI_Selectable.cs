using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Definition;
	using UnityEngine.UI;

	public partial class UI_Selectable : Interactable
	{
		public override GameObject Target
		{
			get => gameObject;
		}

		public override List<GameObject> Targets => throw new System.NotImplementedException();

		Button m_btn;
		Slider m_slider;

		[SerializeField] UIEventType eventType;

		/// <summary>
		/// 자식 패널 객체
		/// </summary>
		[SerializeField] GameObject childPanel;

		/// <summary>
		/// ui 효과 요소
		/// </summary>
		[SerializeField] List<GameObject> uiFXs;

		private void Start()
		{
			if(gameObject.TryGetComponent<Button>(out m_btn))
			{
				m_btn.onClick.AddListener(new UnityAction(OnSelect));
			}

			if(gameObject.TryGetComponent<Slider>(out m_slider))
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
			Debug.Log($"OnSelect : {this.name}");

			ConditionalBranch(eventType);
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
				case UIEventType.Toggle:
				case UIEventType.Viewport_ViewMode:
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
					break;

				case UIEventType.OrthoView_Perspective:
					Event_ToggleOrthoView(false);
					break;

				case UIEventType.Mode_Hide:
					Event_Mode_Hide();
					break;

				case UIEventType.Mode_Isolate:
					Event_Mode_Isolate();
					break;

				case UIEventType.Fit_Center:
					FitCenter();
					break;
			}
		}

		#endregion

	}
}
