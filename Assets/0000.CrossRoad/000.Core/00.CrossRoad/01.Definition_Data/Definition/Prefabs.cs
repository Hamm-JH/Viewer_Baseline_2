using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Prefabs
	{
		/// <summary>
		/// PrefabType 기반으로 필요한 프리팹 객체를 생성하고 결과에 따라 bool 값을 반환한다.
		/// </summary>
		/// <param name="type">PrefabType</param>
		/// <param name="result">생성된 인스턴스</param>
		/// <returns>생성 상태코드
		/// true 반환 : 정상 생성
		/// false 반환 : 생성 오류
		/// </returns>
		public static bool TrySet(PrefabType type, out GameObject result)
		{
			result = null;

			switch(type)
			{
				case PrefabType.UltimateDecal:
					result = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/DecalPoint"));
					break;

				case PrefabType.EasyDecal:
					result = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/EasyDecal"));
					break;
			}


			if(result == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
