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
		/// 웹 뷰어에 한정해서 사용하는 웹뷰어 웹 우측 탭 상태 업데이트 클래스
		/// </summary>
		[System.Serializable]
		public class State_DemoWebViewer
        {
            /// <summary>
            /// true : 손상 탭
            /// false : 보수 탭
            /// </summary>
            private bool isDmgTab = true;

            public bool IsDmgTab { get => isDmgTab; set => isDmgTab = value; }


			public State_DemoWebViewer()
            {
				isDmgTab = true;
            }

        }

		/// <summary>
		/// 모듈 스택
		/// </summary>
		[SerializeField] List<ModuleCode> m_moduleList;
		/// <summary>
		/// 모듈코드에 대응하는 모듈 상태 인스턴스를 저장하는 변수
		/// </summary>
		[SerializeField] Dictionary<ModuleCode, AModuleStatus> m_moduleStatus;

		/// <summary>
		/// WebViewer에서 관리하는 상태 변수
		/// </summary>
		[SerializeField] State_DemoWebViewer m_state_demoWebViewer;

		[SerializeField] GameObject m_pinModeObj;
		[SerializeField] GameObject m_pinModePin;

		/// <summary>
		/// 모듈 리스트
		/// </summary>
		public List<ModuleCode> ModuleList
		{ 
			get => m_moduleList; 
			set => m_moduleList=value; 
		}

		/// <summary>
		/// 모듈 코드에 대응하는 상태 인스턴스 
		/// </summary>
		public Dictionary<ModuleCode, AModuleStatus> ModuleStatus
		{
			get => m_moduleStatus;
			set => m_moduleStatus = value;
		}

		/// <summary>
		/// WebViewer에서 관리하는 상태 변수
		/// </summary>
		public State_DemoWebViewer State_demoWebViewer { get => m_state_demoWebViewer; set => m_state_demoWebViewer = value; }

		/// <summary>
		/// 데모 웹 뷰어에서 POI 등록 단계에서 이전 단계에서 선택된 객체를 저장
		/// </summary>
		public GameObject CacheObject
		{
			get => m_pinModeObj;
			set => m_pinModeObj = value;
		}

		/// <summary>
		/// 손상정보 등록 위치를 저장하는 POI
		/// </summary>
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
			m_state_demoWebViewer = new State_DemoWebViewer();
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
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 임시로 선택 객체를 저장하고 잇는 캐시 객체의 정보를 지운다.
		/// </summary>
		public void Destroy_CacheObject()
		{
			// 여기 저장된 대상은 모델 객체임
			//GameObject.Destroy(CacheObject);
			CacheObject = null;
		}

		/// <summary>
		/// 임시로 손상 위치를 저장하고 있는 POI 객체의 정보를 지운다.
		/// </summary>
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
		/// 관리자뷰어 :: 모듈코드에 대응하는 모듈 상태 인스턴스를 받는다.
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

		/// <summary>
		/// 웹 뷰어의 현재 상태를 반환
		/// </summary>
		/// <param name="_index">옵션 인덱스</param>
		/// <returns>웹 뷰어의 현재 상태</returns>
		public State_DemoWebViewer GetState_DemoWebViewer(int _index)
        {
			return State_demoWebViewer;
        }
	}
}
