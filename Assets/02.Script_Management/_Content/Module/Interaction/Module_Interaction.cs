using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Interaction
{
	using UnityEngine.UI;

	/// <summary>
	/// 상호작용 처리 모듈
	/// 3D, UI
	/// </summary>
	public partial class Module_Interaction : MonoBehaviour, IModule
	{
		protected int id = (int)Definition.ModuleID.AInteraction;

		public int ID { get => id; set => id = value; }

		public Canvas rootCanvas;
		public GraphicRaycaster grRaycaster;

		void Start()
		{
			Debug.Log($"{this.name} start");
		}
	}
}
