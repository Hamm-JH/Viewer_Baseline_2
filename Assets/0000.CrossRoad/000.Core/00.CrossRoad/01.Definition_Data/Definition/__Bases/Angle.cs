using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Angle
	{
		public static Vector3 Set(UIEventType eventType)
		{
			Vector3 result = default(Vector3);

			PlatformCode pCode = MainManager.Instance.Platform;


			switch(eventType)
			{
				case UIEventType.Viewport_ViewMode_ISO:
					result = new Vector3(45, 45, 0);
					break;

				case UIEventType.Viewport_ViewMode_TOP:
					if(Platforms.IsTunnelPlatform(pCode))
					{
						result = new Vector3(-90, 0, 0);
					}
					else if(Platforms.IsBridgePlatform(pCode))
					{
						result = new Vector3(90, 0, -90);
					}
					break;

				case UIEventType.Viewport_ViewMode_BOTTOM:
					if(Platforms.IsTunnelPlatform(pCode))
					{
						result = new Vector3(90, 0, 0);
					}
					else if(Platforms.IsBridgePlatform(pCode))
					{
						result = new Vector3(-90, 0, 90);
					}
					break;

				case UIEventType.Viewport_ViewMode_SIDE:
					result = new Vector3(0, 0, 0);
					break;

				case UIEventType.Viewport_ViewMode_SIDE_FRONT:
					if(Platforms.IsTunnelPlatform(pCode))
					{
						result = new Vector3(0, 180, 0);
					}
					else if(Platforms.IsBridgePlatform(pCode))
					{
						result = new Vector3(0, 180, 0);
					}
					break;

				case UIEventType.Viewport_ViewMode_SIDE_BACK:
					if(Platforms.IsTunnelPlatform(pCode))
					{
						result = new Vector3(0, 0, 0);
					}
					else if(Platforms.IsBridgePlatform(pCode))
					{
						result = new Vector3(0, 0, 0);
					}
					break;

				case UIEventType.Viewport_ViewMode_SIDE_LEFT:
					result = new Vector3(0, -90, 0);
					break;

				case UIEventType.Viewport_ViewMode_SIDE_RIGHT:
					result = new Vector3(0, 90, 0);
					break;
			}

			return result;
		}
	}
}
