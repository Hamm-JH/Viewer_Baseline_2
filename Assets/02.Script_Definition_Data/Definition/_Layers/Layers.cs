using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Layers
	{
		/// <summary>
		/// 0 Default
		/// 1 Cache
		/// 2 FR
		/// 3 BA
		/// 4 TO
		/// 5 BO
		/// 6 LE
		/// 7 RE
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public static int GetLayerIndex(int index)
		{
			int result = 0;

			switch(index)
			{
				case 0:
					result = LayerMask.NameToLayer("Default");
					break;

				case 1:
					result = LayerMask.NameToLayer("Cache");
					break;

				case 2:
					result = LayerMask.NameToLayer("FR");
					break;

				case 3:
					result = LayerMask.NameToLayer("FR");
					break;

				case 4:
					result = LayerMask.NameToLayer("FR");
					break;

				case 5:
					result = LayerMask.NameToLayer("FR");
					break;

				case 6:
					result = LayerMask.NameToLayer("FR");
					break;

				case 7:
					result = LayerMask.NameToLayer("FR");
					break;
			}

			return result;
		}

		/// <summary>
		/// 0 ALL
		/// 1 34상태 main
		/// 2 34상태 sub
		/// 2 FR
		/// 3 BA
		/// 4 TO
		/// 5 BO
		/// 6 LE
		/// 7 RE
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public static int SetMask(int index)
		{
			List<string> layers = new List<string>();

			int mask = 0;
			switch(index)
			{
				case 0:
					mask = SetAll();
					break;

				case 1:
					layers.Add("Cache");
					layers.Add("UI");
					layers.Add("FR");
					layers.Add("BA");
					layers.Add("TO");
					layers.Add("BO");
					layers.Add("LE");
					layers.Add("RE");
					mask = Set(layers);
					break;

				case 2:
					layers.Add("Default");
					layers.Add("Cache");
					layers.Add("UI");
					mask = Set(layers);
					break;
			}

			return mask;
		}

		/// <summary>
		/// 레이어 리스트로 레이어마스크 할당
		/// </summary>
		/// <param name="_layers"></param>
		/// <returns></returns>
		public static int Set(List<string> _layers)
		{
			int mask = 0;

			_layers.ForEach(x =>
			{
				mask |= 1 << LayerMask.NameToLayer(x);
			});

			return mask;
		}

		public static int SetAll()
		{
			return -1;
		}
	}
}
