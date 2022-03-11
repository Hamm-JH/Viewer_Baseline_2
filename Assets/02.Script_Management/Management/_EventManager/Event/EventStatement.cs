using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;

	[System.Serializable]
	public class EventStatement : EventData
	{
		/// <summary>
		/// 모듈 스택
		/// </summary>
		[SerializeField] List<ModuleCode> m_moduleList;

		[SerializeField] GameObject m_pinModeObj;
		[SerializeField] GameObject m_pinModePin;

		public List<ModuleCode> ModuleList
		{ 
			get => m_moduleList; 
			set => m_moduleList=value; 
		}

		public GameObject CacheObject
		{
			get => m_pinModeObj;
			set => m_pinModeObj = value;
		}

		public GameObject CachePin
		{
			get => m_pinModePin;
			set => m_pinModePin = value;
		}

		public EventStatement()
		{
			EventType = InputEventType.Statement;
			m_moduleList = new List<ModuleCode>();
		}

		

		public override void OnProcess(GameObject _cObj, List<ModuleCode> _mList)
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

		public override void DoEvent(Dictionary<InputEventType, EventData> _sEvents)
		{
			throw new System.NotImplementedException();
		}

		public void Destroy_CacheObject()
		{
			GameObject.Destroy(CacheObject);
			CacheObject = null;
		}

		public void Destroy_CachePin()
		{
			GameObject.Destroy(CachePin);
			CachePin = null;
		}

	}
}
