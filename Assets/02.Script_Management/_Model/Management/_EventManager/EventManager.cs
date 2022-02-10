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

		public void OnEvent(EventData currEvent)
		{
			//Debug.Log("In");

			// 이전에 선택된 대상이 없을 경우
			if (selectedEvent == null)
			{
				if(currEvent != null)
				{
					UpdateNewEvent(currEvent);
				}
				return;
			}

			//Debug.Log(1);

			if (currEvent == null)
			{
				DeselectEvent(selectedEvent);
				return;
			}

			//Debug.Log("");

			DeselectEvent(selectedEvent);
			UpdateNewEvent(currEvent);
		}

		private void UpdateNewEvent(EventData currEvent)
		{
			selectedEvent = currEvent;

			selectedEvent.target.OnSelect();
		}

		private void DeselectEvent(EventData selectedEvent)
		{
			selectedEvent.target.OnDeselect();

			selectedEvent = null;
		}
	}
}
