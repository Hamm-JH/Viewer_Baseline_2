using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Events;

	/// <summary>
	/// 이벤트 관리
	/// </summary>
	public partial class EventManager : IManager<EventManager>
	{
		[SerializeField] EventData selectedEvent;
		[SerializeField] GameObject cacheDownObj;

		public void OnEvent(EventData currEvent)
		{
			if (currEvent.EventType == Definition.InputEventType.Input_clickDown
				|| currEvent.EventType == Definition.InputEventType.Input_clickFailureUp) return;

			// 선택된 이벤트 상태가 없는 경우, 아무 동작을 수행하지 않는 더미 인스턴스를 생성한다.
			if(selectedEvent == null)
			{
				selectedEvent = new EventData(null, Definition.InputEventType.NotDef);
			}

			// 이전에 선택된 대상이 없을 경우
			if (selectedEvent.Element == null)
			{
				if(currEvent.Element != null)
				{
					UpdateNewEvent(currEvent);
				}
				return;
			}

			//Debug.Log(1);


			if (currEvent.Element == null)
			{
				DeselectEvent(selectedEvent);
				return;
			}

			//Debug.Log("");

			DeselectEvent(selectedEvent);
			UpdateNewEvent(currEvent);
		}

		/// <summary>
		/// 선택 이벤트 실행구간
		/// </summary>
		/// <param name="currEvent"></param>
		private void UpdateNewEvent(EventData currEvent)
		{
			selectedEvent = currEvent;

			// 마우스 클릭 다운 단계
			if(selectedEvent.EventType == Definition.InputEventType.Input_clickDown)
			{
				cacheDownObj = selectedEvent.Element.Target;
			}
			// 마우스 클릭 끝 (가능함)
			else if(selectedEvent.EventType == Definition.InputEventType.Input_clickSuccessUp)
			{
				selectedEvent.Element.OnSelect();
			}
			// 마우스 클릭 끝 (불가능함)
			else if(selectedEvent.EventType == Definition.InputEventType.Input_clickFailureUp)
			{
				cacheDownObj = null;
			}
		}

		/// <summary>
		/// 선택 해제 이벤트 실행구간
		/// </summary>
		/// <param name="selectedEvent"></param>
		private void DeselectEvent(EventData selectedEvent)
		{
			if (selectedEvent.EventType == Definition.InputEventType.NotDef) return;		// 정의되지 않은 이벤트 패스

			// 마우스 클릭 다운 단계
			if (selectedEvent.EventType == Definition.InputEventType.Input_clickDown)
			{
				//cacheDownObj = selectedEvent.Element.Target;
			}
			// 마우스 클릭 끝 (가능함)
			else if (selectedEvent.EventType == Definition.InputEventType.Input_clickSuccessUp)
			{
				//selectedEvent.Element.OnSelect();
				selectedEvent.Element.OnDeselect();
			}
			// 마우스 클릭 끝 (불가능함)
			else if (selectedEvent.EventType == Definition.InputEventType.Input_clickFailureUp)
			{
				//cacheDownObj = null;
			}


			selectedEvent = null;
		}
	}
}
