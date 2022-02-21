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

		[SerializeField] UIEventType eventType;

		[SerializeField] GameObject childPanel;

		private void Start()
		{
			Button btn;
			if(gameObject.TryGetComponent<Button>(out btn))
			{
				btn.onClick.AddListener(new UnityAction(OnSelect));
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

		private void ConditionalBranch(UIEventType _eventType)
		{
			switch(_eventType)
			{
				case UIEventType.Toggle:
				case UIEventType.Toggle_ViewMode:
					Event_Toggle_ViewMode();
					break;

				case UIEventType.Toggle_ViewMode_ISO:
				case UIEventType.Toggle_ViewMode_TOP:
				case UIEventType.Toggle_ViewMode_SIDE:
				case UIEventType.Toggle_ViewMode_BOTTOM:
					Event_Toggle_ViewMode(_eventType);
					break;

				case UIEventType.Fit_Center:
					FitCenter();
					break;
			}
		}
	}
}
