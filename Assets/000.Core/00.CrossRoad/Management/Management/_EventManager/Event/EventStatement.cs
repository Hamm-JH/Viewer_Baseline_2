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
		/// <summary>
		/// 모듈코드에 대응하는 모듈 상태 인스턴스를 저장하는 변수
		/// </summary>
		[SerializeField] Dictionary<ModuleCode, AModuleStatus> m_moduleStatus;

		[SerializeField] GameObject m_pinModeObj;
		[SerializeField] GameObject m_pinModePin;

		public List<ModuleCode> ModuleList
		{ 
			get => m_moduleList; 
			set => m_moduleList=value; 
		}

		public Dictionary<ModuleCode, AModuleStatus> ModuleStatus
		{
			get => m_moduleStatus;
			set => m_moduleStatus = value;
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
			m_moduleStatus = new Dictionary<ModuleCode, AModuleStatus>();
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



		//----------------------

		/// <summary>
		/// 새 모듈을 생성한다.
		/// </summary>
		/// <param name="_mCode"></param>
		public void CreateNewModule(ModuleCode _mCode)
		{
			ModuleStatus.Add(_mCode, new AModuleStatus());
		}

		/// <summary>
		/// 모듈 상태값을 가져온다.
		/// </summary>
		/// <param name="_mCode"></param>
		public AModuleStatus GetModuleStatus(ModuleCode _mCode)
		{
			AModuleStatus mstat;
			if (TryGetModule(_mCode, out mstat))
			{
				return mstat;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 모듈 상태값을 새로 할당한다.
		/// </summary>
		/// <param name="_mCode"></param>
		/// <param name="_status"></param>
		/// <returns></returns>
		public bool SetModuleStatus(ModuleCode _mCode, ModuleStatus _status)
		{
			bool result = false;

			AModuleStatus mstat;
			if(TryGetModule(_mCode, out mstat))
			{
				mstat.m_moduleStatus = _status;
				result = true;
			}
			else
			{
				mstat = new AModuleStatus();
				ModuleStatus.Add(_mCode, mstat);
				mstat.m_moduleStatus = _status;
			}

			return result;
		}

		/// <summary>
		/// 모듈코드에 대응하는 모듈 상태 인스턴스를 받는다.
		/// </summary>
		/// <param name="_mCode"></param>
		/// <param name="_mStatus"></param>
		/// <returns></returns>
		public bool TryGetModule(ModuleCode _mCode, out AModuleStatus _mStatus)
		{
			bool result = false;
			_mStatus = null;

			if(ModuleStatus.TryGetValue(_mCode, out _mStatus))
			{
				result = true;
			}

			return result;
		}
		//public void IsContainsModule(ModuleCode, )

	}
}
