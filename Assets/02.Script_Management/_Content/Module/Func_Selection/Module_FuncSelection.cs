using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Function
{
	public class Module_FuncSelection : AModule
	{
		// ��ī�� �ڽ� �׷��� ��Ʈ��
		// ��ü �׷��� �� ����

		// Start is called before the first frame update
		void Start()
		{

		}

		public override void OnStart()
		{
			Debug.LogError($"{this.GetType().ToString()} Run");
		}
	}
}
