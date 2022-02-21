using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;

	[System.Serializable]
	public abstract class AUI : MonoBehaviour, IModule
	{
		protected ModuleID id = Definition.ModuleID.Prop_UI;
		protected FunctionCode m_currentFunction;

		public ModuleID ID { get => id; set => id = value; }
		public FunctionCode Function { get => m_currentFunction; set => m_currentFunction = value; }

		public void OnCreate(ModuleID _id, FunctionCode _code)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 레이어 코드 기반으로 내부 데이터 갱신
		/// </summary>
		/// <param name="_codes"></param>
		//public abstract void Set(List<LayerCode> _codes);
	}
}
