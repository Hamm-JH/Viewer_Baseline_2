using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tunnel
{
	public static class TunnelConverter
	{
		public static string GetName(string name)
		{
			string result = "";

			string[] args = name.Split(',');

			if(args.Length > 3)
			{
				result = $"{args[0]},{args[1]},{args[3]}";
			}

			return result;
		}
	}
}
