using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Layer
{
	public abstract class ALayer : MonoBehaviour, IModule
	{
		protected int id = (int)Definition.ModuleID.ALayer;
		public int ID { get => id; set => id = value; }

		/// <summary>
		/// ���̾� ù ������ ����
		/// </summary>
		public abstract void OnCreate();


		/// <summary>
		/// ���̾� ������ �ش� ���̾� �Ҵ�� ����
		/// </summary>
		public abstract void LayerIn();

		/// <summary>
		/// ���̾� ������Ʈ�� ����
		/// </summary>
		public abstract void LayerUpdate();

		/// <summary>
		/// ���̾� ���Ž� ����
		/// </summary>
		public abstract void LayerOut();
	}
}
