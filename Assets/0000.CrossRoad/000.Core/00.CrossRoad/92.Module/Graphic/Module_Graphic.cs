using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Graphic
{
	using Definition;
    using Management;

    public partial class Module_Graphic : AModule
	{
		// 스카이 박스 그래픽 컨트롤
		// 전체 그래픽 톤 관리

		// TODO :: CHECK :: Graphic 모듈에서 ColorTable 관리, 0517 : 그래픽 템플릿별 스타일 적용

		// Start is called before the first frame update
		void Start()
		{
			OnCreate(ModuleID.Graphic, FunctionCode.Graphic);
		}

		/// <summary>
		/// 초기화 시작
		/// </summary>
		public override void OnStart()
		{
			Debug.LogWarning($"{this.GetType().ToString()} OnStart");

			ContentManager.Instance.CheckInitModuleComplete(ID);
		}
	}
}
