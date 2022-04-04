using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Template
	{
		public static List<GameObject> GetUITemplates(PlatformCode _pCode)
		{
			List<GameObject> list = new List<GameObject>();

			if (Platforms.IsDemoAdminViewer(_pCode))
			{
				list.Add(Resources.Load<GameObject>("UI/UITemplate_0328 2"));
				list.Add(Resources.Load<GameObject>("UI/UI_AdminViewer"));
			}
			else if(Platforms.IsSmartInspectPlatform(_pCode))
			{
				list.Add(Resources.Load<GameObject>("UI/UITemplate_0302"));
			}

			return list;
		}
	}
}
