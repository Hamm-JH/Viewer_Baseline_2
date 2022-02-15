using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;

	public class EventData_Empty : EventData
	{
		public EventData_Empty()
		{
			EventType = InputEventType.NotDef;
		}

		public override void OnProcess(GameObject _cObj)
		{
			Debug.Log("OnProcess Empty");
		}
	}
}
