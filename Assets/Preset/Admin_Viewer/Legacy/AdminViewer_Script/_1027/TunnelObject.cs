using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public static class TunnelObject
	{
		/// <summary>
		/// 선택 객체가 시작 세그먼트인지 아닌지 판단
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static int IsStartSegment(string name)
		{
			int result = 0;

			string[] indexes = name.Split(',');

			if (indexes[0] == "1") result = 1;
			else result = -1;

			return result;
		}

		/// <summary>
		/// 측벽인 경우 카메라 이동값 결정
		/// </summary>
		/// <param name="_type"></param>
		/// <returns></returns>
		public static float SideWallRange(Datas.ObjectType _type)
		{
			float result = 0f;

			switch (_type)
			{
				case Datas.ObjectType.WALL_CentralReservation:
				case Datas.ObjectType.WALL_Gate:
				case Datas.ObjectType.WALL_Slope:
					result = 25;
					break;

				default:
					result = 0;
					break;
			}

			return result;
		}
	}
}