using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;

	[System.Serializable]
	public class EventData_Empty : AEventData
	{
		public EventData_Empty()
		{
			EventType = InputEventType.NotDef;
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			Debug.Log("OnProcess Empty");
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			Debug.Log("DoEvent Empty");
		}
	}
}
