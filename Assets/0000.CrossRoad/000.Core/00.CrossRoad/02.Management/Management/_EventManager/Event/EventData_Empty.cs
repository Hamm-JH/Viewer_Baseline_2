using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;

	[System.Serializable]
	public class EventData_Empty : AEventData
	{
		/// <summary>
		/// 비어있는 이벤트
		/// </summary>
		public EventData_Empty()
		{
			EventType = InputEventType.NotDef;
		}

		/// <summary>
		/// 이벤트 전처리
		/// </summary>
		/// <param name="_mList">모듈 리스트</param>
		public override void OnProcess(List<ModuleCode> _mList)
		{
			Debug.Log("OnProcess Empty");
		}

		/// <summary>
		/// 이벤트 후처리
		/// </summary>
		/// <param name="_sEvents">현재 이벤트 상태</param>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			Debug.Log("DoEvent Empty");
		}
	}
}
