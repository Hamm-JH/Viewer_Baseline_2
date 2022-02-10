using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Prefabs
	{
		public static bool Set(PrefabType type, out GameObject result)
		{
			result = null;

			switch(type)
			{
				case PrefabType.Decal:
					result = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/DecalPoint"));
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
