using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Tunnel
{
	public static class Tunnels
	{
		public static string GetName(string _value)
		{
			string result = "";

			string[] args = _value.Split(',');

			result = string.Format("{0}{1}{2}", $"{args[0]}구간,", $"{args[1]}련,", $"{args[3]}");
			//Debug.Log($"partName : {result}");

			return result;
		}
	}
}
