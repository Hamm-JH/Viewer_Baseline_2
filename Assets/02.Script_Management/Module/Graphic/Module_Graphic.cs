using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Graphic
{
	using Definition;

	public class Module_Graphic : AModule
	{
		// ��ī�� �ڽ� �׷��� ��Ʈ��
		// ��ü �׷��� �� ����

		// Start is called before the first frame update
		void Start()
		{
			OnCreate(ModuleID.Graphic, FunctionCode.Graphic);
		}

		public override void OnStart()
		{
			Debug.LogError($"{this.GetType().ToString()} Run");
			
			// TODO 0222 : �׷��� ���ø��� ��Ÿ�� ����
		}
	}
}
