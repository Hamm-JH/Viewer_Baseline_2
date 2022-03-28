using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Management;

	public partial class UITemplate_Tunnel : AUI
	{
		public void Event_Legacy_ChangeCameraDirection(string t_surfaceCode)
		{
			string surface = t_surfaceCode;

			ContentManager.Instance._API.SendRequest(Definition.SendRequestCode.SelectObject6Shape, t_surfaceCode);
		}
	}
}
