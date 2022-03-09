using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
	using Definition;

	/// <summary>
	/// 모듈 관리코드
	/// </summary>
	public static class _Module
	{
		/// <summary>
		/// 리스트 단위로 모듈 생성
		/// </summary>
		/// <param name="_modules"></param>
		/// <param name="_functions"></param>
		public static void Create_List(List<ModuleID> _modules, List<FunctionCode> _functions, out List<AModule> _objs)
		{
			_objs = new List<AModule>();

			foreach(ModuleID id in _modules)
			{
				FunctionCode function = Match_Single(id, _functions);

				// 기능 코드가 Null이 아닐때만 생성 시작
				if(function != FunctionCode.Null)
				{
					AModule mod = Create(id, function);
					mod.OnCreate(id, function);
					_objs.Add(mod);
				}
			}
		}

		/// <summary>
		/// 기능 생성 시작
		/// </summary>
		/// <param name="_module"></param>
		/// <param name="_function"></param>
		public static AModule Create(ModuleID _module, FunctionCode _function)
		{
			GameObject obj = new GameObject(_module.ToString());

			AModule mod = SetModule(obj, _module);

			return mod;
		}

		private static AModule SetModule(GameObject _obj, ModuleID _module)
		{
			AModule mod = null;

			if(_module == ModuleID.Model)
			{
				mod = _obj.AddComponent<Model.Module_Model>();
			}
			else if(_module == ModuleID.Interaction)
			{
				mod = _obj.AddComponent<Interaction.Module_Interaction>();
			}
			else if(_module == ModuleID.WebAPI)
			{
				mod = _obj.AddComponent<WebAPI.Module_WebAPI>();
			}
			else if(_module == ModuleID.Graphic)
			{
				mod = _obj.AddComponent<Graphic.Module_Graphic>();
			}
			else if(_module == ModuleID.Item)
			{
				mod = _obj.AddComponent<Item.Module_Items>();
			}
			else
			{
				throw new System.Exception($"Module code not creatable :: id : {_module.ToString()}");
			}

			return mod;
		}

		#region Matching - ModuleID :: FunctionCode

		/// <summary>
		/// 모듈에 맞는 기능을 반환해준다.
		/// </summary>
		/// <param name="_module"></param>
		/// <param name="_functions"></param>
		/// <returns></returns>
		public static FunctionCode Match_Single(ModuleID _module, List<FunctionCode> _functions)
		{
			foreach(FunctionCode function in _functions)
			{
				if(IsMatch(_module, function))
				{
					return function;
				}
			}
			return FunctionCode.Null;
		}

		/// <summary>
		/// 기능 코드가 모듈에 맞는 코드인가?
		/// </summary>
		/// <param name="_module"></param>
		/// <param name="_function"></param>
		/// <returns></returns>
		private static bool IsMatch(ModuleID _module, FunctionCode _function)
		{
			bool result = false;

			if(_module == ModuleID.Model)
			{
				switch(_function)
				{
					case FunctionCode.Model_Export:
					case FunctionCode.Model_Import:
						return true;
				}
			}
			else if(_module == ModuleID.Interaction)
			{
				switch(_function)
				{
					case FunctionCode.Interaction_3D:
					case FunctionCode.Interaction_UI:
						return true;
				}
			}
			else if(_module == ModuleID.WebAPI)
			{
				switch(_function)
				{
					case FunctionCode.API_Front:
					case FunctionCode.API_Back:
						return true;
				}
			}
			else if(_module == ModuleID.Item)
			{
				switch(_function)
				{
					case FunctionCode.Item_LocationGuide:
						return true;
				}
			}

			return result;
		}
		#endregion
	}
}
