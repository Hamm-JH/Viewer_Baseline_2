using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Prefabs
	{
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
