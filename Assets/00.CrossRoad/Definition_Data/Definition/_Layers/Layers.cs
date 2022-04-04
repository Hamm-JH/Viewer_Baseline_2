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
					result = LayerMask.NameToLayer("BA");
					break;

				case 4:
					result = LayerMask.NameToLayer("TO");
					break;

				case 5:
					result = LayerMask.NameToLayer("BO");
					break;

				case 6:
					result = LayerMask.NameToLayer("LE");
					break;

				case 7:
					result = LayerMask.NameToLayer("RE");
					break;
			}

			return result;
		}

		/// <summary>
		/// 0 ALL
		/// 1 34상태 main
		/// 2 34상태 sub
		/// 11 FR
		/// 12 BA
		/// 13 TO
		/// 14 BO
		/// 15 LE
		/// 16 RE
		/// 21 FR only dim
		/// 22 BA only dim
		/// 23 TO only dim
		/// 24 BO only dim
		/// 25 LE only dim
		/// 26 RE only dim
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

				case 11:
					layers.Add("Cache");
					layers.Add("FR");
					mask = Set(layers);
					break;

				case 12:
					layers.Add("Cache");
					layers.Add("BA");
					mask = Set(layers);
					break;

				case 13:
					layers.Add("Cache");
					layers.Add("TO");
					mask = Set(layers);
					break;

				case 14:
					layers.Add("Cache");
					layers.Add("BO");
					mask = Set(layers);
					break;

				case 15:
					layers.Add("Cache");
					layers.Add("LE");
					mask = Set(layers);
					break;

				case 16:
					layers.Add("Cache");
					layers.Add("RE");
					mask = Set(layers);
					break;

				case 21:
					layers.Add("FR");
					mask = Set(layers);
					break;

				case 22:
					layers.Add("BA");
					mask = Set(layers);
					break;

				case 23:
					layers.Add("TO");
					mask = Set(layers);
					break;

				case 24:
					layers.Add("BO");
					mask = Set(layers);
					break;

				case 25:
					layers.Add("LE");
					mask = Set(layers);
					break;

				case 26:
					layers.Add("RE");
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
