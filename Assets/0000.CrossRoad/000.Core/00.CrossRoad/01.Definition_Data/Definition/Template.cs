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

			if(Platforms.IsDemoWebViewer(_pCode))
            {
				//list.Add(Resources.Load<GameObject>("UI/BottomBar - DemoWeb"));
				list.Add(Resources.Load<GameObject>("UI/UIElement/BottomBar"));
			}
			else if (Platforms.IsDemoAdminViewer(_pCode))
			{
				list.Add(Resources.Load<GameObject>("UI/UITemplate_0328 2"));
				list.Add(Resources.Load<GameObject>("UI/UI_AdminViewer"));
			}
			else if(Platforms.IsSmartInspectPlatform(_pCode))
			{
				//list.Add(Resources.Load<GameObject>("UI/UITemplate_0302"));
				list.Add(Resources.Load<GameObject>("UI/SmartInspect"));
			}
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(_pCode);
            }

			return list;
		}
	}
}
