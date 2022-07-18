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
        //public FunctionCode Function { get; set; }

		/// <summary>
		/// 모듈 기능, 아이템 리스트
		/// </summary>
        public List<FunctionCode> Functions { get; set; }

		/// <summary>
		/// 모듈 생성 시작
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="_code"></param>
		void OnCreate(ModuleID _id, FunctionCode _code);
	}
}
