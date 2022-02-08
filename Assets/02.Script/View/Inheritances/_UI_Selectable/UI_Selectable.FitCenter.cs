using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Management;

	public partial class UI_Selectable : Interactable
	{
		public void FitCenter()
		{
			ContentManager.Instance.SetCameraCenterPosition();
		}
	}
}
