using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Bridge
{
	public static class Bridges
	{
		#region Code 

		public enum CodeLv4
		{
			ALL = -1,
			Null = 0,
			GD = 1,
			CB = 2,
			SL = 3,
			DS = 4,
			GR = 5,
			GF = 6,
			JI = 7,
			AB = 8,
			PI = 9,
			BE = 10,
			FT = 11
		}

		public enum CodeLv5
		{
			Null = 0,

			// 거더, 가로보 lv5
			PSCI25 = 1,
			PSCI30,
			RCT25,

			// 슬라브 lv5
			PSCI,
			RCT,
			RA,
			RCS,

			// 포장 lv5
			AP,
			CP,

			// 난간 lv5
			GRC,
			GRS,
			GR,
			CU,

			// 신축이음 lv5
			MO,
			NB,
			RI,
			TRP,

			// 교대 lv5
			SGA,
			RTA,
			RBA,        // 추가
						//RA

			// 교각 lv5
			WLP,
			TP,
			RAP,
			RBP,
			//RTA
			//RA

			// 받침 lv5
			RUP,
			POT,

			// 기초 lv5
			MAT
		}

		#endregion

		public static string GetName(string _value)
		{
			string result = "";

			string[] splits = _value.Split('_');

			result = GetNameArg(splits);
			//Debug.LogError(_value);
			//Debug.Log(result);

			return result;
		}

		private static string GetNameArg(string[] _splits)
		{
			string result = "";

			int index = _splits.Length;
			if(index == 2)
			{
				string arg0 = GetArg0(_splits[0]);
				string arg1 = GetArg1(_splits[1]);

				result = string.Format("{0} {1}", arg0, arg1);
			}
			else if(index >= 3)
			{
				string arg0 = GetArg0(_splits[0]);
				string arg1 = GetArg1(_splits[1]);
				string arg2 = GetArg2(_splits[2]);

				result = string.Format("{0} {1} {2}", arg0, arg1, arg2);
			}

			return result;
		}

		private static string GetArg0(string _arg)
		{
			string result = "";

			if(_arg.Contains("SP"))
			{
				result = "지간";
				_arg = _arg.Replace("SP", "");
			}
			else if(_arg.Contains("AP"))
			{
				result = "지점";
				_arg = _arg.Replace("AP", "");
			}

			result = $"{result} {_arg}";

			return result;
		}

		public static CodeLv4 GetPartCode(string _name)
        {
			CodeLv4 result = CodeLv4.Null;

			string arg1 = _name.Split('_')[1];
			result = ConvertLv4Code(arg1);

			return result;
        }

		private static string GetArg1(string _arg)
		{
			string result = "";

			CodeLv4 lv4 = ConvertLv4Code(_arg);
			result = ConvertLv4String(lv4);
			
			return result;
		}

		private static string GetArg2(string _value)
		{
			string result = "";

			CodeLv5 lv5 = ConvertLv5Code(_value);
			result = ConvertLv5String(lv5);

			return result;
		}

		public static CodeLv4 ConvertLv4Code(string arg)
		{
			CodeLv4 lv4;

			if (Enum.TryParse<CodeLv4>(arg, out lv4))
			{
				return lv4;
			}
			else
			{
				return CodeLv4.Null;
			}
		}

		public static CodeLv5 ConvertLv5Code(string arg)
		{
			CodeLv5 lv5;

			if (Enum.TryParse<CodeLv5>(arg, out lv5))
			{
				return lv5;
			}
			else
			{
				return CodeLv5.Null;
			}
		}

		public static List<CodeLv4> GetCodeList()
        {
			List<CodeLv4> list = new List<CodeLv4>();

			list.Add(CodeLv4.GD);
			list.Add(CodeLv4.CB);
			list.Add(CodeLv4.SL);
			list.Add(CodeLv4.DS);
			list.Add(CodeLv4.GR);
			list.Add(CodeLv4.GF);
			list.Add(CodeLv4.JI);
			list.Add(CodeLv4.AB);
			list.Add(CodeLv4.PI);
			list.Add(CodeLv4.BE);
			list.Add(CodeLv4.FT);

			return list;
        }

		public static string ConvertLv4String(CodeLv4 code)
		{
			switch (code)
			{
				case CodeLv4.GD: return "거더";
				case CodeLv4.CB: return "가로보";
				case CodeLv4.SL: return "슬라브";
				case CodeLv4.DS: return "포장";
				case CodeLv4.GR: return "난간";
				case CodeLv4.GF: return "방호시설";
				case CodeLv4.JI: return "신축이음";
				case CodeLv4.AB: return "교대";
				case CodeLv4.PI: return "교각";
				case CodeLv4.BE: return "받침";
				case CodeLv4.FT: return "기초";
				default: return CodeLv4.Null.ToString();
			}
		}

		public static string ConvertLv5String(CodeLv5 code)
		{
			switch (code)
			{
				// 거더, 가로보
				case CodeLv5.PSCI25: return string.Format($"PSCI 25m");
				case CodeLv5.PSCI30: return string.Format($"PSCI 30m");
				case CodeLv5.RCT25: return string.Format($"RCT 25m");

				// 슬라브
				case CodeLv5.PSCI: return string.Format($"PSCI");
				case CodeLv5.RCT: return string.Format($"RCT");
				case CodeLv5.RA: return string.Format($"라멘교");
				case CodeLv5.RCS: return string.Format($"RCS");

				// 포장
				case CodeLv5.AP: return string.Format($"아스팔트");
				case CodeLv5.CP: return string.Format($"콘크리트");

				// 난간
				case CodeLv5.GRC: return string.Format($"콘크리트");
				case CodeLv5.GRS: return string.Format($"강재형");

				// 방호시설 (난간)
				case CodeLv5.GR: return string.Format($"방호벽");
				case CodeLv5.CU: return string.Format($"연석");

				// 신축이음
				case CodeLv5.MO: return string.Format($"모노셀");
				case CodeLv5.NB: return string.Format($"엔비조인트");
				case CodeLv5.RI: return string.Format($"레일조인트");
				case CodeLv5.TRP: return string.Format($"트렌스플렉스");

				// 교대
				case CodeLv5.SGA: return string.Format($"반중력식");
				case CodeLv5.RTA: return string.Format($"역T형");
				case CodeLv5.RBA: return string.Format($"라멘교");

				// 교각
				case CodeLv5.WLP: return string.Format($"벽체형");
				case CodeLv5.TP: return string.Format($"T형");
				case CodeLv5.RAP: return string.Format($"라멘형");
				case CodeLv5.RBP: return string.Format($"라멘교");

				// 받침
				case CodeLv5.RUP: return string.Format($"탄성");
				case CodeLv5.POT: return string.Format($"포트");

				// 기초
				case CodeLv5.MAT: return string.Format($"MAT");
				default: return CodeLv5.Null.ToString();
			}
		}
	}
}
