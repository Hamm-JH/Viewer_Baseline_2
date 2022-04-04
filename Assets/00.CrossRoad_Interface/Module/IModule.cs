using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
	using Definition;

	/// <summary>
	/// ���� ������ ����� ���̽� �������̽�
	/// </summary>
	public interface IModule
	{
		/// <summary>
		/// ����� ID
		/// </summary>
		public ModuleID ID { get; set; }

		/// <summary>
		/// ����� ���� ���
		/// </summary>
		public FunctionCode Function { get; set; }

		void OnCreate(ModuleID _id, FunctionCode _code);
	}
}
