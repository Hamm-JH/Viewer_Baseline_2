using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Graphic
{
	using Definition;

	public partial class Module_Graphic : AModule
	{
		// 스카이 박스 그래픽 컨트롤
		// 전체 그래픽 톤 관리

		// Start is called before the first frame update
		void Start()
		{
			OnCreate(ModuleID.Graphic, FunctionCode.Graphic);
		}

		public override void OnStart()
		{
			Debug.LogWarning($"{this.GetType().ToString()} OnStart");
			
			// TODO 0222 : 그래픽 템플릿별 스타일 적용
		}
	}
}
