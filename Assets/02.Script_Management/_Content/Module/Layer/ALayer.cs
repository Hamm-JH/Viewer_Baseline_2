using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Layer
{
	public abstract class ALayer : AModule
	{

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

		public override void OnCreate(ModuleID _id, FunctionCode _code)
		{
			throw new System.NotImplementedException();
		}
	}
}
