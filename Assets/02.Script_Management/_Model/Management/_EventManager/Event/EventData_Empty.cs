using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;

	[System.Serializable]
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

		public override void DoEvent()
		{
			Debug.Log("DoEvent Empty");
		}

		public override void DoEvent(List<GameObject> _objs)
		{
			Debug.Log("DoEvent Empty");
		}
	}
}
