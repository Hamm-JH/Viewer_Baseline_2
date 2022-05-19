using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using Definition;
	using Management;
    using System.Linq;
    using static Module.WebAPI.Module_WebAPI;

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
		public Bounds CenterBounds
		{ 
			get => centerBounds; 
			set => centerBounds = value; 
		}
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

		public override void OnStart()
		{
			//Debug.LogError($"{this.GetType().ToString()} Run");

			if(Functions.First() == FunctionCode.Model_Import)
			{
				// 모델 임포트를 위해 컨텐츠 관리자에 URI 수집시 실행시킬 메서드를 보낸다.
				ContentManager.Instance.RunModule_ModelImport(OnImport);
				//ContentManager.Instance.CheckInitModuleComplete(ID);		// 내부에서 처리함
			}
			else if(Functions.First() == FunctionCode.Model_Export)
			{
				ContentManager.Instance.CheckInitModuleComplete(ID);
			}
		}

		public void OnAfterInitialize()
        {
			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsDemoWebViewer(pCode))
            {

            }
        }
	}
}
