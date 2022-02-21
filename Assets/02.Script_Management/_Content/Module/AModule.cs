using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
	using Definition;

	public abstract class AModule : MonoBehaviour, IModule
	{
		protected ModuleID id;
		protected FunctionCode m_currentFunction;

		public ModuleID ID { get => id; set => id = value; }
		public FunctionCode Function { get => m_currentFunction; set => m_currentFunction = value; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_id"></param>
		/// <param name="_code"></param>
		
		public virtual void OnCreate(ModuleID _id, FunctionCode _code)
		{
			id = _id;
			m_currentFunction = _code;
		}

		/// <summary>
		/// functionCode에 따라 모듈 실행
		/// </summary>
		public abstract void Run();
	}
}
