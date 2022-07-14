using Definition.Control;
using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Angle
	{
		/// <summary>
		/// UIEventType에 따라 각도를 변경한다.
		/// </summary>
		/// <param name="eventType">UIEventType</param>
		/// <returns>Euler 각도값 반환</returns>
		public static Vector3 Set(UIEventType eventType)
		{
			Vector3 result = default(Vector3);

			PlatformCode pCode = MainManager.Instance.Platform;

			CameraModes camMode = MainManager.Instance.CurrentCameraMode;

			switch (eventType)
			{
				case UIEventType.Viewport_ViewMode_ISO:
					result = new Vector3(45, 45, 0);
					break;

				case UIEventType.Viewport_ViewMode_TOP:
					if(Platforms.IsTunnelPlatform(pCode))
					{
						if (camMode == CameraModes.TunnelInside_Rotate)
                        {
							result = new Vector3(-90, 0, 0);
                        }
						else
                        {
							result = new Vector3(90, 0, -90);
						}
					}
					else if(Platforms.IsBridgePlatform(pCode))
					{
						result = new Vector3(90, 0, -90);
					}
					break;

				case UIEventType.Viewport_ViewMode_BOTTOM:
					if(Platforms.IsTunnelPlatform(pCode))
					{
						if (camMode == CameraModes.TunnelInside_Rotate)
                        {
							result = new Vector3(90, 0, 0);
                        }
						else
                        {
							result = new Vector3(-90, 0, 90);
						}
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
