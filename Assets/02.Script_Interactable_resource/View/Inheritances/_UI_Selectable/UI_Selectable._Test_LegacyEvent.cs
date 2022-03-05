using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Management;

	public partial class UI_Selectable : Interactable
	{
		public void Event_Legacy_ChangeCameraDirection()
		{
			string surface = t_surfaceCode;

			ContentManager.Instance._API.SendRequest(Definition.SendRequestCode.SelectObject6Shape, t_surfaceCode);
		}
	}
}
