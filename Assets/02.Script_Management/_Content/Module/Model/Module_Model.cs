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

		protected GameObject Model { get => model; set => model=value; }
		public Bounds CenterBounds { get => centerBounds; }

		void Start()
		{
			Debug.Log($"{this.name} started");
		}

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

		public override void Run()
		{
			Debug.LogError($"{this.GetType().ToString()} Run");

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
