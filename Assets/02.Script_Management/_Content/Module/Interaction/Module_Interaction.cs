using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Interaction
{
	using Definition;
	using UnityEngine.UI;

	/// <summary>
	/// 상호작용 처리 모듈
	/// 3D, UI
	/// </summary>
	public partial class Module_Interaction : AModule
	{
		public Canvas rootCanvas;
		public GraphicRaycaster grRaycaster;

		//public override void OnCreate(ModuleID _id, FunctionCode _code)
		//{
		//	Debug.Log($"{this.GetType()} OnStart");
		//}

		public override void Run()
		{
			Debug.LogError($"{this.GetType().ToString()} Run");
		}
	}
}
