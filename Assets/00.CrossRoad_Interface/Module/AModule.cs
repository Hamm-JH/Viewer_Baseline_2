using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
	using Definition;

	public abstract class AModule : MonoBehaviour, IModule
	{
		protected ModuleID id;
		//protected FunctionCode m_currentFunction;
        private List<FunctionCode> m_functions;

        public ModuleID ID { get => id; set => id = value; }
        //public FunctionCode Function { get => m_currentFunction; set => m_currentFunction=value; }

        public List<FunctionCode> Functions { get => m_functions; set => m_functions = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_code"></param>

        public void OnCreate(ModuleID _id, FunctionCode _code)
		{
			id = _id;
			//m_currentFunction = _code;

			if(m_functions == null) m_functions = new List<FunctionCode>();

			// 리스트에서 동일한 코드를 갖고있지 않은 경우에만 할당
			if(!m_functions.Contains(_code))
            {
				m_functions.Add(_code);
            }
		}

		/// <summary>
		/// functionCode에 따라 모듈 실행
		/// </summary>
		public abstract void OnStart();
	}
}
