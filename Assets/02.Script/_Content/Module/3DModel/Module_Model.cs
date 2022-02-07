using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	/// <summary>
	/// 모델 처리 모듈 
	/// Import, Export
	/// </summary>
	public partial class Module_Model : MonoBehaviour, IModule
	{
		protected int id = (int)Definition.ModuleID.AModel;
		[SerializeField] private GameObject model;

		public int ID { get => id; set => id = value; }
		protected GameObject Model { get => model; set => model=value; }

		void Start()
		{
			Debug.Log($"{this.name} started");
		}

		/// <summary>
		/// 모델 받아오기
		/// </summary>
		public void OnImport()
		{
			InImport();
		}

		/// <summary>
		/// 모델 출력하기
		/// </summary>
		public void OnExport()
		{
			InExport();
		}
	}
}
