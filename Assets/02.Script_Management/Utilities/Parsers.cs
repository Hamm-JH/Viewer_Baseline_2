using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	using Definition;

	public static class Parsers
	{
		public static T1 Parse<T1>(object _v1)
		{
			return (T1)_v1;
		}

		public static UIEventType OnParse(ViewRotations _vCode)
		{
			UIEventType uType = UIEventType.Null;

			switch (_vCode)
			{
				case ViewRotations.Top: uType = UIEventType.Viewport_ViewMode_TOP; break;
				case ViewRotations.Bottom: uType = UIEventType.Viewport_ViewMode_BOTTOM; break;
				case ViewRotations.Front: uType = UIEventType.Viewport_ViewMode_SIDE_FRONT; break;
				case ViewRotations.Back: uType = UIEventType.Viewport_ViewMode_SIDE_BACK; break;
				case ViewRotations.Left: uType = UIEventType.Viewport_ViewMode_SIDE_LEFT; break;
				case ViewRotations.Right: uType = UIEventType.Viewport_ViewMode_SIDE_RIGHT; break;
			}

			return uType;
		}
	}
}
