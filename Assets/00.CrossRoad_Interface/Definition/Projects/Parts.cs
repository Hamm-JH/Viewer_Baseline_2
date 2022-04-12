using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Projects
{
	using Management;
	
    public static class Parts
    {
		public static string GetPartName(string _value)
		{
			string result = _value;

			PlatformCode pCode = MainManager.Instance.Platform;
			if (Platforms.IsTunnelPlatform(pCode))
			{
				result = Platform.Tunnel.Tunnels.GetName(_value);
			}
			else if (Platforms.IsBridgePlatform(pCode))
			{
				result = Platform.Bridge.Bridges.GetName(_value);
			}
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }

			return result;
		}
    }
}
