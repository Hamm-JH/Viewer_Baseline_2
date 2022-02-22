using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using Definition;
	using Management;

	/// <summary>
	/// 모델 처리 모듈 
	/// Import, Export
	/// </summary>
	public partial class Module_Model : AModule
	{
		[SerializeField] private GameObject model;
		[SerializeField] private Bounds centerBounds;
		[SerializeField] private List<GameObject> m_modelObjects;

		protected GameObject Model { get => model; set => model=value; }
		public Bounds CenterBounds { get => centerBounds; }
		public List<GameObject> ModelObjects { get => m_modelObjects; set => m_modelObjects=value; }

		/// <summary>
		/// 모델 받아오기
		/// </summary>
		public void OnImport(string URI)
		{
			InImport(URI);
		}

		/// <summary>
		/// 모델 출력하기
		/// </summary>
		public void OnExport()
		{
			InExport();
		}

		//public override void OnCreate(ModuleID _id, FunctionCode _code)
		//{
		//	m_currentFunction = _code;
		//}

		public override void OnStart()
		{
			//Debug.LogError($"{this.GetType().ToString()} Run");

			if(Function == FunctionCode.Model_Import)
			{
				// 모델 임포트를 위해 컨텐츠 관리자에 URI 수집시 실행시킬 메서드를 보낸다.
				ContentManager.Instance.RunModule_ModelImport(OnImport);
			}
			else if(Function == FunctionCode.Model_Export)
			{

			}
		}
	}
}
