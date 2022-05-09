using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
	using Definition;

	/// <summary>
	/// 실행 가능한 모듈의 베이스 인터페이스
	/// </summary>
	public interface IModule
	{
		/// <summary>
		/// 모듈의 ID
		/// </summary>
		public ModuleID ID { get; set; }

		/// <summary>
		/// 모듈의 현재 기능
		/// </summary>
		public FunctionCode Function { get; set; }

		void OnCreate(ModuleID _id, FunctionCode _code);
	}
}
