using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Layer
{
	public abstract class ALayer : AModule
	{

		/// <summary>
		/// 레이어 첫 생성시 실행
		/// </summary>
		public abstract void OnCreate();


		/// <summary>
		/// 레이어 변수에 해당 레이어 할당시 실행
		/// </summary>
		public abstract void LayerIn();

		/// <summary>
		/// 레이어 업데이트시 실행
		/// </summary>
		public abstract void LayerUpdate();

		/// <summary>
		/// 레이어 제거시 실행
		/// </summary>
		public abstract void LayerOut();

		public override void OnCreate(ModuleID _id, FunctionCode _code)
		{
			throw new System.NotImplementedException();
		}
	}
}
