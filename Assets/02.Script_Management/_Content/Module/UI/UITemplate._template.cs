using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	public class UITemplate : AUI
	{
		public override void OnStart()
		{
			Debug.LogWarning("UITemplate OnStart");
		}

		public override void SetObjectData_Tunnel(GameObject selected)
		{
			
		}
	}
}
