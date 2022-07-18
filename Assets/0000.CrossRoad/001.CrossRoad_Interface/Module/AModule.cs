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

		/// <summary>
		/// ��� ID
		/// </summary>
        public ModuleID ID { get => id; set => id = value; }

		/// <summary>
		/// ��� ����Ʈ
		/// </summary>
        public List<FunctionCode> Functions { get => m_functions; set => m_functions = value; }

        /// <summary>
        /// ��� ����
        /// </summary>
        /// <param name="_id">��� �з�</param>
        /// <param name="_code">��� �ڵ�</param>
        public void OnCreate(ModuleID _id, FunctionCode _code)
		{
			id = _id;
			//m_currentFunction = _code;

			if(m_functions == null) m_functions = new List<FunctionCode>();

			// ����Ʈ���� ������ �ڵ带 �������� ���� ��쿡�� �Ҵ�
			if(!m_functions.Contains(_code))
            {
				m_functions.Add(_code);
            }
		}

		/// <summary>
		/// functionCode�� ���� ��� ����
		/// </summary>
		public abstract void OnStart();
	}
}
