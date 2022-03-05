using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Module;

	public partial class ContentManager : IManager<ContentManager>
	{
		

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

			// 1. 모듈 리스트 생성
			List<AModule> mods = CreateModules(_cData.ModuleLists, _cData.FunctionCodes);
			Modules.AddRange(mods);

			// 2. 모듈 부모배치
			SetParentModules(Modules);

			// 3. 모듈 시작
			InitModules(Modules);
		}

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

		/// <summary>
		/// 모듈 리스트 단일 요소 동작 시작
		/// </summary>
		/// <param name="_module"></param>
		private void InitModule(AModule _module)
		{
			_module.OnStart();
		}
	}
}
