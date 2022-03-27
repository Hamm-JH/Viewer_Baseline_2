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

			// �Ŵ�, ���κ� lv5
			PSCI25 = 1,
			PSCI30,
			RCT25,

			// ����� lv5
			PSCI,
			RCT,
			RA,
			RCS,

			// ���� lv5
			AP,
			CP,

			// ���� lv5
			GRC,
			GRS,
			GR,
			CU,

			// �������� lv5
			MO,
			NB,
			RI,
			TRP,

			// ���� lv5
			SGA,
			RTA,
			RBA,        // �߰�
						//RA

			// ���� lv5
			WLP,
			TP,
			RAP,
			RBP,
			//RTA
			//RA

			// ��ħ lv5
			RUP,
			POT,

			// ���� lv5
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
				result = "����";
				_arg = _arg.Replace("SP", "");
			}
			else if(_arg.Contains("AP"))
			{
				result = "����";
				_arg = _arg.Replace("AP", "");
			}

			result = $"{result} {_arg}";

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

		public static string ConvertLv4String(CodeLv4 code)
		{
			switch (code)
			{
				case CodeLv4.GD: return "�Ŵ�";
				case CodeLv4.CB: return "���κ�";
				case CodeLv4.SL: return "�����";
				case CodeLv4.DS: return "����";
				case CodeLv4.GR: return "����";
				case CodeLv4.GF: return "��ȣ�ü�";
				case CodeLv4.JI: return "��������";
				case CodeLv4.AB: return "����";
				case CodeLv4.PI: return "����";
				case CodeLv4.BE: return "��ħ";
				case CodeLv4.FT: return "����";
				default: return CodeLv4.Null.ToString();
			}
		}

		public static string ConvertLv5String(CodeLv5 code)
		{
			switch (code)
			{
				// �Ŵ�, ���κ�
				case CodeLv5.PSCI25: return string.Format($"PSCI 25m");
				case CodeLv5.PSCI30: return string.Format($"PSCI 30m");
				case CodeLv5.RCT25: return string.Format($"RCT 25m");

				// �����
				case CodeLv5.PSCI: return string.Format($"PSCI");
				case CodeLv5.RCT: return string.Format($"RCT");
				case CodeLv5.RA: return string.Format($"��౳");
				case CodeLv5.RCS: return string.Format($"RCS");

				// ����
				case CodeLv5.AP: return string.Format($"�ƽ���Ʈ");
				case CodeLv5.CP: return string.Format($"��ũ��Ʈ");

				// ����
				case CodeLv5.GRC: return string.Format($"��ũ��Ʈ");
				case CodeLv5.GRS: return string.Format($"������");

				// ��ȣ�ü� (����)
				case CodeLv5.GR: return string.Format($"��ȣ��");
				case CodeLv5.CU: return string.Format($"����");

				// ��������
				case CodeLv5.MO: return string.Format($"��뼿");
				case CodeLv5.NB: return string.Format($"��������Ʈ");
				case CodeLv5.RI: return string.Format($"��������Ʈ");
				case CodeLv5.TRP: return string.Format($"Ʈ�����÷���");

				// ����
				case CodeLv5.SGA: return string.Format($"���߷½�");
				case CodeLv5.RTA: return string.Format($"��T��");
				case CodeLv5.RBA: return string.Format($"��౳");

				// ����
				case CodeLv5.WLP: return string.Format($"��ü��");
				case CodeLv5.TP: return string.Format($"T��");
				case CodeLv5.RAP: return string.Format($"�����");
				case CodeLv5.RBP: return string.Format($"��౳");

				// ��ħ
				case CodeLv5.RUP: return string.Format($"ź��");
				case CodeLv5.POT: return string.Format($"��Ʈ");

				// ����
				case CodeLv5.MAT: return string.Format($"MAT");
				default: return CodeLv5.Null.ToString();
			}
		}
	}
}
