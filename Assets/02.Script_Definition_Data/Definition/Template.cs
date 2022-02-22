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
				case PlatformCode.PC_Viewer1:
					obj = Resources.Load<GameObject>("UI/TestView0221");
					break;
			}

			return obj;
		}
	}
}
