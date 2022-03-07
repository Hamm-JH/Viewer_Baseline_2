using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Positions
	{
		public static Vector3 SetLocal(UIEventType _eType)
		{
			Vector3 result = default(Vector3);

			switch(_eType)
			{
				case UIEventType.Viewport_ViewMode_TOP:
					result = new Vector3(-5, 0, 0);
					break;

				case UIEventType.Viewport_ViewMode_BOTTOM:

					break;

				case UIEventType.Viewport_ViewMode_SIDE_FRONT:

					break;

				case UIEventType.Viewport_ViewMode_SIDE_BACK:

					break;

				case UIEventType.Viewport_ViewMode_SIDE_LEFT:

					break;

				case UIEventType.Viewport_ViewMode_SIDE_RIGHT:

					break;
			}

			return result;
		}
	}
}
