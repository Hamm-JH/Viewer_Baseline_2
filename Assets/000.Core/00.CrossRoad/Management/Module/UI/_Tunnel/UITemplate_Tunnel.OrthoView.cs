using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Management;

	public partial class UITemplate_Tunnel : AUI
	{
		private void Event_ToggleOrthoView(bool _isOrthogonal)
		{
			ContentManager.Instance.Function_ToggleOrthoView(_isOrthogonal);

		}
	}
}
