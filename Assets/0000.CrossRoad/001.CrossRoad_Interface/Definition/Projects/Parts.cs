using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Projects
{
	using Management;
	
	/// <summary>
	/// 부재명에 대한 컨트롤
	/// </summary>
    public static class Parts
    {
		/// <summary>
		/// 부재명 받아오기
		/// </summary>
		/// <param name="_value">객체명</param>
		/// <returns>가공된 부재명</returns>
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
