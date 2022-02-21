using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Function
{
	public class Module_FuncSelection : AModule
	{
		// 스카이 박스 그래픽 컨트롤
		// 전체 그래픽 톤 관리

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
