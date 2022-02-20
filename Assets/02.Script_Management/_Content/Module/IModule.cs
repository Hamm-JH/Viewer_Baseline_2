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
		public int ID { get; set; }

		void OnStart(FunctionCode _code);
	}
}
