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
		public int ID { get; set; }

		void OnStart(FunctionCode _code);
	}
}
