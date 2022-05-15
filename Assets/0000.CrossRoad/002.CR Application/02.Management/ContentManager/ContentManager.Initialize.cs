using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
    using Bearroll.UltimateDecals;
    using Definition;
	using Module;
    using Module.Interaction;
    using Module.Item;
    using Module.UI;

    public partial class ContentManager : IManager<ContentManager>
	{
		bool dmgComp = false;
		bool rcvComp = false;
		bool itfComp = false;
		bool modComp = false;
		//bool evtComp = false;

		/// <summary>
		/// UIManager 완료 시점 파악을 위한 메서드
		/// 1 dmg, 2 rcv, 3 interface 4 model 5 event 완료 체크
		/// </summary>
		/// <param name="_index"></param>
		public void CompCheck(int _index)
		{
			switch(_index)
			{
				case 1:	dmgComp = true;	break;
				case 2: rcvComp = true; break;
				case 3: itfComp = true; break;
				case 4: modComp = true; break;
				//case 5:	evtComp = true; break;
			}

			if(dmgComp && rcvComp && itfComp && modComp/* && evtComp*/)
			{
				_Interaction.LoadModuleComplete();
			}
		}

		private Dictionary<ModuleID, bool> m_mChecker;

		/// <summary>
		/// 컨텐츠 관리자가 시작될때 초기화 실행
		/// </summary>
		private void OnInitialize()
		{
			// 컨텐츠 관리자 전달
			MainManager.Instance.SetContentManager(this, OnUpdate_System);
		}

		/// <summary>
		/// 주관리자에서 초기화에 필요한 정보를 얻어옴
		/// </summary>
		/// <param name="_cManagement"></param>
		/// <param name="_cData"></param>
		private void OnUpdate_System(Definition.Data.CoreManagement _cManagement, Definition.Data.CoreData _cData)
		{
			// CoreManagement, CoreData를 받아온다.
			// 생성할 모듈, 기능 목록을 받아서 초기화 수행한다.

			// 1. 모듈 생성, 컨텐츠 관리자 자식에 할당
			// 2. 생성된 모듈에 기능코드 할당 
			// 3. 각 모듈에 기능 실행 (Do) 진행
			// 4. 각 모듈에서 필요 데이터 알아서 가져가기

			//UD_Manager.instance.DoDestroy();

			// 1. 모듈 리스트 생성
			List<AModule> mods = CreateModules(_cData.ModuleLists, _cData.FunctionCodes);
			Modules.AddRange(mods);

			// 1-1. 모듈 완성 확인변수 초기화
			m_mChecker = CreateChecker(Modules);
			
			// 2. 모듈 부모배치
			SetParentModules(Modules);

			// 3. 모듈 시작
			InitModules(Modules);

			// 4. 컨테이너 아이템 초기화 시행
			InitContainerItems();
		}

		#region Step 1
		/// <summary>
		/// 모듈 리스트 생성
		/// </summary>
		/// <param name="_modules"></param>
		/// <param name="_functions"></param>
		/// <returns></returns>
		private List<AModule> CreateModules(List<ModuleID> _modules, List<FunctionCode> _functions)
		{
			List<AModule> mods = new List<AModule>();

			_Module.Create_List(_modules, _functions, out mods);

			return mods;
		}

		private Dictionary<ModuleID, bool> CreateChecker(List<AModule> _modules)
        {
			Dictionary<ModuleID, bool> result = new Dictionary<ModuleID, bool>();

			_modules.ForEach(x =>
			{
				result.Add(x.ID, false);
			});

			return result;
        }
		#endregion

		#region Step 2
		/// <summary>
		/// 모듈 리스트에 등록된 모듈 부모배치
		/// </summary>
		/// <param name="_modules"></param>
		private void SetParentModules(List<AModule> _modules)
		{
			foreach(AModule mod in _modules)
			{
				mod.transform.SetParent(this.transform);
			}
		}
		#endregion

		#region Step 3
		/// <summary>
		/// 모듈 리스트 요소별 동작 시작
		/// </summary>
		/// <param name="_modules"></param>
		private void InitModules(List<AModule> _modules)
		{
			foreach(AModule mod in _modules)
			{
				InitModule(mod);
			}
		}
		#endregion

		#region Step 4

		private void InitContainerItems()
		{
			m_container.InitContainer();
		}

		#endregion

		/// <summary>
		/// 모듈 리스트 단일 요소 동작 시작
		/// </summary>
		/// <param name="_module"></param>
		private void InitModule(AModule _module)
		{
			_module.OnStart();
		}

		/// <summary>
		/// 모듈 초기화 완료시 완료 결과 보고
		/// </summary>
		/// <param name="_id"></param>
		public void CheckInitModuleComplete(ModuleID _id)
        {
			bool isFinish = true;

			// 현재 받아온 아이디 값이 체커에 존재하는 키인지 확인
			if(m_mChecker.ContainsKey(_id))
            {
				// 존재한다면 value을 true로 변경
				m_mChecker[_id] = true;
            }
			else
            {
				throw new Definition.Exceptions.ModuleNotInstantiated(_id);
            }

			// 모듈 결과 확인 리스트 순회
			foreach(bool result in m_mChecker.Values)
            {
				// 모듈 완료 결과 
				isFinish = isFinish && result;
            }

			// 모든 모듈의 초기화가 완료된 경우
			if(isFinish)
            {
				Debug.Log($"----- 모든 모듈 초기화가 완료됨. -----");
				Initialize_AfterModuleComplete();
			}
        }

		/// <summary>
		/// 모듈 초기화 완료 후에 초기화 단계
		/// </summary>
		private void Initialize_AfterModuleComplete()
        {
			PlatformCode pCode = MainManager.Instance.Platform;
			
			if(Platforms.IsDemoAdminViewer(pCode))
            {
				Debug.Log("모듈 초기화후 초기화");
            }
			else if(Platforms.IsDemoWebViewer(pCode))
            {
				Module_Items item = Module<Module_Items>(ModuleID.Item);
				if(item != null)
                {
					item.OnAfterInitialize();
                }
            }
			else if(Platforms.IsSmartInspectPlatform(pCode))
            {
				// Interaction 모듈을 찾는다.
				Module_Interaction interaction = Module<Module_Interaction>(ModuleID.Interaction);
				if (interaction != null)
                {
					// SmartInspect 인스턴스를 찾는다.
					UITemplate_SmartInspect ui;
					foreach(var aui in interaction.UiInstances)
                    {
						if (Utilities.Objects.TryGetValue(aui.gameObject, out ui))
                        {
							// 초기화 후 초기화를 시작한다.
							ui.Initialize_AfterModuleInitialize();
                        }
                    }
                }
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
        }
	}
}
