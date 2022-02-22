using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	using Management;

	public partial class UI_Selectable : Interactable
	{
		private void Event_ToggleOrthoView(bool _isOrthogonal)
		{
			ContentManager.Instance.Function_ToggleOrthoView(_isOrthogonal);
			
		}
	}
}
