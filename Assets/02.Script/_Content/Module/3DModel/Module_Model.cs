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
		public int ID { get => id; set => id = value; }


		[SerializeField] private GameObject model;
		[SerializeField] private Bounds centerBounds;

		protected GameObject Model { get => model; set => model=value; }
		public Bounds CenterBounds { get => centerBounds; }

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
