using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Template
	{
		public static GameObject GetUITemplate(PlatformCode _platform)
		{
			GameObject obj = null;

			switch(_platform)
			{
				case PlatformCode.WebGL_AdminViewer:
					obj = Resources.Load<GameObject>("UI/UI_AdminViewer");
					break;

				case PlatformCode.PC_Viewer_Tunnel:
				case PlatformCode.PC_Viewer_Bridge:
					//obj = Resources.Load<GameObject>("UI/TestView0221");
					obj = Resources.Load<GameObject>("UI/UITemplate_0302");
					break;
			}

			return obj;
		}
	}
}
