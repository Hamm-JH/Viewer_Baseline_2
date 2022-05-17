using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Bridge
{
	using System;

	public static class Bridges
	{
		#region Code 

		//public enum BridgeCode
		//{
		//	ALL = -1,
		//	Null = 0,
		//	GD = 1,
		//	CB = 2,
		//	SL = 3,
		//	DS = 4,
		//	GR = 5,
		//	GF = 6,
		//	JI = 7,
		//	AB = 8,
		//	PI = 9,
		//	BE = 10,
		//	FT = 11
		//}

		//public enum BridgeCodeDetail
		//{
		//	Null = 0,

		//	// 거더, 가로보 lv5
		//	PSCI25 = 1,
		//	PSCI30,
		//	RCT25,

		//	// 슬라브 lv5
		//	PSCI,
		//	RCT,
		//	RA,
		//	RCS,

		//	// 포장 lv5
		//	AP,
		//	CP,

		//	// 난간 lv5
		//	GRC,
		//	GRS,
		//	GR,
		//	CU,

		//	// 신축이음 lv5
		//	MO,
		//	NB,
		//	RI,
		//	TRP,

		//	// 교대 lv5
		//	SGA,
		//	RTA,
		//	RBA,        // 추가
		//				//RA

		//	// 교각 lv5
		//	WLP,
		//	TP,
		//	RAP,
		//	RBP,
		//	//RTA
		//	//RA

		//	// 받침 lv5
		//	RUP,
		//	POT,

		//	// 기초 lv5
		//	MAT
		//}

		#endregion

		public static List<string> GetPartSurfaces(GameObject _obj)
        {
			List<string> result = new List<string>();

			result.Add("Top");
			result.Add("Bottom");
			result.Add("Front");
			result.Add("Back");
			result.Add("Left");
			result.Add("Right");

			return result;
        }

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

		public static BridgeCode GetPartCode(string _name)
        {
			BridgeCode result = BridgeCode.Null;

			string arg1 = _name.Split('_')[1];
			result = ConvertLv4Code(arg1);

			return result;
        }

		public static BridgeCodeDetail GetPartCodeDetail(string _name)
        {
			BridgeCodeDetail result = BridgeCodeDetail.Null;

			string[] args = _name.Split('_');
			if(args.Length >= 3)
            {
				result = ConvertLv5Code(args[2]);
			}

			return result;
        }

		private static string GetArg1(string _arg)
		{
			string result = "";

			BridgeCode lv4 = ConvertLv4Code(_arg);
			result = ConvertLv4String(lv4);
			
			return result;
		}

		private static string GetArg2(string _value)
		{
			string result = "";

			BridgeCodeDetail lv5 = ConvertLv5Code(_value);
			result = ConvertLv5String(lv5);

			return result;
		}

		public static BridgeCode ConvertLv4Code(string arg)
		{
			BridgeCode lv4;

			if (Enum.TryParse<BridgeCode>(arg, out lv4))
			{
				return lv4;
			}
			else
			{
				return BridgeCode.Null;
			}
		}

		public static BridgeCodeDetail ConvertLv5Code(string arg)
		{
			BridgeCodeDetail lv5;

			if (Enum.TryParse<BridgeCodeDetail>(arg, out lv5))
			{
				return lv5;
			}
			else
			{
				return BridgeCodeDetail.Null;
			}
		}

		public static List<BridgeCode> GetCodeList()
        {
			List<BridgeCode> list = new List<BridgeCode>();

			list.Add(BridgeCode.GD);
			list.Add(BridgeCode.CB);
			list.Add(BridgeCode.SL);
			list.Add(BridgeCode.DS);
			list.Add(BridgeCode.GR);
			list.Add(BridgeCode.GF);
			list.Add(BridgeCode.JI);
			list.Add(BridgeCode.AB);
			list.Add(BridgeCode.PI);
			list.Add(BridgeCode.BE);
			list.Add(BridgeCode.FT);

			return list;
        }

		public static string ConvertLv4String(BridgeCode code)
		{
			switch (code)
			{
				case BridgeCode.GD: return "거더";
				case BridgeCode.CB: return "가로보";
				case BridgeCode.SL: return "슬라브";
				case BridgeCode.DS: return "포장";
				case BridgeCode.GR: return "난간";
				case BridgeCode.GF: return "방호시설";
				case BridgeCode.JI: return "신축이음";
				case BridgeCode.AB: return "교대";
				case BridgeCode.PI: return "교각";
				case BridgeCode.BE: return "받침";
				case BridgeCode.FT: return "기초";
				default: return BridgeCode.Null.ToString();
			}
		}

		public static string ConvertLv5String(BridgeCodeDetail code)
		{
			switch (code)
			{
				// 거더, 가로보
				case BridgeCodeDetail.PSCI25: return string.Format($"PSCI 25m");
				case BridgeCodeDetail.PSCI30: return string.Format($"PSCI 30m");
				case BridgeCodeDetail.RCT25: return string.Format($"RCT 25m");

				// 슬라브
				case BridgeCodeDetail.PSCI: return string.Format($"PSCI");
				case BridgeCodeDetail.RCT: return string.Format($"RCT");
				case BridgeCodeDetail.RA: return string.Format($"라멘교");
				case BridgeCodeDetail.RCS: return string.Format($"RCS");

				// 포장
				case BridgeCodeDetail.AP: return string.Format($"아스팔트");
				case BridgeCodeDetail.CP: return string.Format($"콘크리트");

				// 난간
				case BridgeCodeDetail.GRC: return string.Format($"콘크리트");
				case BridgeCodeDetail.GRS: return string.Format($"강재형");

				// 방호시설 (난간)
				case BridgeCodeDetail.GR: return string.Format($"방호벽");
				case BridgeCodeDetail.CU: return string.Format($"연석");

				// 신축이음
				case BridgeCodeDetail.MO: return string.Format($"모노셀");
				case BridgeCodeDetail.NB: return string.Format($"엔비조인트");
				case BridgeCodeDetail.RI: return string.Format($"레일조인트");
				case BridgeCodeDetail.TRP: return string.Format($"트렌스플렉스");

				// 교대
				case BridgeCodeDetail.SGA: return string.Format($"반중력식");
				case BridgeCodeDetail.RTA: return string.Format($"역T형");
				case BridgeCodeDetail.RBA: return string.Format($"라멘교");

				// 교각
				case BridgeCodeDetail.WLP: return string.Format($"벽체형");
				case BridgeCodeDetail.TP: return string.Format($"T형");
				case BridgeCodeDetail.RAP: return string.Format($"라멘형");
				case BridgeCodeDetail.RBP: return string.Format($"라멘교");

				// 받침
				case BridgeCodeDetail.RUP: return string.Format($"탄성");
				case BridgeCodeDetail.POT: return string.Format($"포트");

				// 기초
				case BridgeCodeDetail.MAT: return string.Format($"MAT");
				default: return BridgeCodeDetail.Null.ToString();
			}
		}
	}
}
