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
		/// 레이어 코드 기반으로 내부 데이터 갱신
		/// </summary>
		/// <param name="_codes"></param>
		public abstract void Set(List<LayerCode> _codes);
	}
}
