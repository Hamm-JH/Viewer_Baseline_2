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
		protected int id = (int)Definition.ModuleID.AWebAPI;

		public int ID { get => id; set => id = value; }

		void Start()
		{
			Debug.Log($"{this.name} start");
		}
	}
}
