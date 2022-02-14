using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;

	public abstract class AUI : MonoBehaviour, IModule
	{
		protected int id = (int)Definition.ModuleID.AUI;
		public int ID { get => id; set => id = value; }

		/// <summary>
		/// ���̾� �ڵ� ������� ���� ������ ����
		/// </summary>
		/// <param name="_codes"></param>
		public abstract void Set(List<LayerCode> _codes);
	}
}
