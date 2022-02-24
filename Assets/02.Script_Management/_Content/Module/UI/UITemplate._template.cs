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

		public override void TogglePanelList(int _index, GameObject _exclusive)
		{
			throw new System.NotImplementedException();
		}
	}
}
