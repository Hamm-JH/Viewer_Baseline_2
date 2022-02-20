using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
	/// <summary>
	/// 웹 API 모듈
	/// Front, Back
	/// </summary>
	public partial class Module_WebAPI : MonoBehaviour, IModule
	{
		protected int id = (int)Definition.ModuleID.WebAPI;

		public int ID { get => id; set => id = value; }

		public void OnStart(FunctionCode _code)
		{
			throw new System.NotImplementedException();
		}

		void Start()
		{
			Debug.Log($"{this.name} start");
		}
	}
}
