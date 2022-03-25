using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;

	[System.Serializable]
	public class EventStatement : AEventData
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

		public override void OnProcess(List<ModuleCode> _mList)
		{
			Debug.Log("OnProcess Empty");
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			throw new System.NotImplementedException();
		}

		public void Destroy_CacheObject()
		{
			// 여기 저장된 대상은 모델 객체임
			//GameObject.Destroy(CacheObject);
			CacheObject = null;
		}

		public void Destroy_CachePin()
		{
			// 여기 저장된 대상은 Cache Pin 객체
			GameObject.Destroy(CachePin);
			CachePin = null;
		}

	}
}
