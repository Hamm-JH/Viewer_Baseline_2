using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	/// <summary>
	/// 教臂畔 贸府等 MonoInstance
	/// </summary>
	/// <typeparam name="T">鸥涝 T</typeparam>
	public class MonoInstance<T> : MonoBehaviour where T : class
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if(instance == null)
				{
					var t = FindObjectOfType<MonoInstance<T>>() as T;
					instance = t;
				}
				return instance;
			}
		}

		private void OnDestroy()
		{
			instance = null;
		}
	}
}
