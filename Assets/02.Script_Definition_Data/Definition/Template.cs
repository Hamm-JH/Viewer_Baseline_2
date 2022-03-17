using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Template
	{
		public static GameObject GetUITemplate(PlatformCode _pCode)
		{
			GameObject obj = null;

			if (Platforms.IsDemoAdminViewer(_pCode))
			{
				obj = Resources.Load<GameObject>("UI/UI_AdminViewer");
			}
			else if(Platforms.IsSmartInspectPlatform(_pCode))
			{
				obj = Resources.Load<GameObject>("UI/UITemplate_0302");
			}

			return obj;
		}
	}
}
