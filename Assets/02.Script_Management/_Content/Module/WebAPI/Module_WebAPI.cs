using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	/// <summary>
	/// 웹 API 모듈
	/// Front, Back
	/// </summary>
	public partial class Module_WebAPI : AModule
	{
		//public override void OnCreate(ModuleID _id, FunctionCode _code)
		//{
		//	throw new System.NotImplementedException();
		//}

		public override void OnStart()
		{
			Debug.LogError($"{this.GetType().ToString()} Run");
		}
	}
}
