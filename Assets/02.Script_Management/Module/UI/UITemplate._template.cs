using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
	public class UITemplate : AUI
	{
		public override void OnStart()
		{
			Debug.LogWarning("UITemplate OnStart");
		}

		public override void OnModuleComplete()
		{
			Debug.LogError("load complete");
		}

		public override void ReInvokeEvent()
		{
			throw new System.NotImplementedException();
		}

		public override void GetUIEvent(UIEventType _uType, UI_Selectable _setter)
		{
			throw new System.NotImplementedException();
		}

		public override void GetUIEvent(float _value, UIEventType _uType, UI_Selectable _setter)
		{
			throw new System.NotImplementedException();
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
