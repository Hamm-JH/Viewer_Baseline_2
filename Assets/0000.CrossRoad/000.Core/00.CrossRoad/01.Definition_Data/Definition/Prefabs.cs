using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Prefabs
	{
		/// <summary>
		/// PrefabType ������� �ʿ��� ������ ��ü�� �����ϰ� ����� ���� bool ���� ��ȯ�Ѵ�.
		/// </summary>
		/// <param name="type">PrefabType</param>
		/// <param name="result">������ �ν��Ͻ�</param>
		/// <returns>���� �����ڵ�
		/// true ��ȯ : ���� ����
		/// false ��ȯ : ���� ����
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
