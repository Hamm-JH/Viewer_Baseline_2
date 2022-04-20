using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Tunnel
{
	using AdminViewer.Tunnel;

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

		public static TunnelCode GetPartCode(string _value)
		{
			TunnelCode result = TunnelCode.Null;

			string param = _value.Split(',')[2];
			//Debug.Log(param);
			switch (param)
			{
				#region ETC
				case "Etc_L_Ec":
				case "Etc_R_Ec":
					{
						result = TunnelCode.ETC_EmergencyCall;
					}
					break;

				case "Etc_L_F":
				case "Etc_R_F":
					{
						result = TunnelCode.ETC_fireplug;
					}
					break;
				case "Etc_L_Ex":
				case "Etc_R_Ex":
					{
						result = TunnelCode.ETC_EmergencyExit;
					}
					break;
				case "Etc_L_T1":
				case "Etc_R_T1":
					{
						result = TunnelCode.ETC_IndicatorLight1;
					}
					break;
				case "Etc_L_T2":
				case "Etc_R_T2":
					{
						result = TunnelCode.ETC_IndicatorLight2;
					}
					break;
				#endregion

				#region LIGHT
				case "L_L_C":
				case "L_R_C":
				case "L_C":
				case "L_L_Si":
				case "L_R_Si":
					{
						result = TunnelCode.LIGHT;
					}
					break;
				#endregion

				#region WALL
				case "S_R_Div":
				case "E_L_Div":
					{
						result = TunnelCode.WALL_CentralReservation;
					}
					break;

				case "S_C_Ga":
				case "S_L_GaIn":
				case "S_L_rW0":
				case "S_L_rW45":
				case "S_L_rW90":
				case "S_R_Ga":
				case "S_R_GaIn":
				case "S_R_rW0":
				case "S_R_rW45":
				case "S_R_rW90":

				case "E_C_Ga":
				case "E_L_GaIn":
				case "E_R_GaIn":
				case "E_L_rW0":
				case "E_L_rW45":
				case "E_L_rW90":
				case "E_R_Ga":
				case "E_R_rW0":
				case "E_R_rW45":
				case "E_R_rW90":
					{
						result = TunnelCode.WALL_Gate;
					}
					break;

				case "S_C_Sl":
				case "S_L_SlIn":
				case "S_L_SlOut":
				case "S_L_Sl45":
				case "S_L_Sl90":
				case "S_R_SlIn":
				case "S_R_SlOut":
				case "S_R_Sl45":
				case "S_R_Sl90":

				case "E_C_Sl":
				case "E_L_SlIn":
				case "E_L_SlOut":
				case "E_L_Sl45":
				case "E_L_Sl90":
				case "E_R_SlIn":
				case "E_R_SlOut":
				case "E_R_Sl45":
				case "E_R_Sl90":
					{
						result = TunnelCode.WALL_Slope;
					}
					break;
				#endregion

				#region MAIN
				case "M_L_DIn":
				case "M_L_DOut":
				case "M_R_DIn":
				case "M_R_DOut":
					{
						result = TunnelCode.MAIN_Drain;
					}
					break;

				case "M_L_Co":
				case "M_R_Co":
					{
						result = TunnelCode.MAIN_Cover;
					}
					break;

				case "M_P":
					{
						result = TunnelCode.MAIN_Paving;
					}
					break;

				case "M_L_SwI":
				case "M_R_SwI":
				case "M_L_SwO":
				case "M_R_SwO":
					{
						result = TunnelCode.MAIN_SideWall;
					}
					break;

				case "M_Ce":
				case "M_L_Ce":
				case "M_R_Ce":
					{
						result = TunnelCode.MAIN_Ceiling;
						//string liningMaterial = Manager.MainManager.Instance.liningMaterial;
						//if (liningMaterial == "콘크리트")
						//{
						//	result = TunnelCode.MAIN_Ceiling;
						//}
						//else if (liningMaterial == "파형강판")
						//{
						//	result = TunnelCode.MAIN_CorrugatedSteel;
						//}
					}
					break;
				#endregion

				#region JET
				case "J_L_JL":
				case "J_C_JC":
				case "J_R_JR":
					{
						result = TunnelCode.JET;
					}
					break;

					#endregion

			}

			return result;
		}

		public static List<TunnelCode> GetCodeList()
        {
			List<TunnelCode> list = new List<TunnelCode>();

			list.Add(TunnelCode.ETC_EmergencyCall		);
			list.Add(TunnelCode.ETC_fireplug			);
			list.Add(TunnelCode.ETC_EmergencyExit		);
			list.Add(TunnelCode.LIGHT					);
			list.Add(TunnelCode.MAIN_Drain				);
			list.Add(TunnelCode.MAIN_Cover				);
			list.Add(TunnelCode.MAIN_Paving				);
			list.Add(TunnelCode.MAIN_SideWall			);
			list.Add(TunnelCode.MAIN_Ceiling			);
			list.Add(TunnelCode.JET						);
			list.Add(TunnelCode.WALL_CentralReservation );
			list.Add(TunnelCode.WALL_Gate				);
			list.Add(TunnelCode.WALL_Slope				);

			return list;
        }

		public static string GetCodeName(TunnelCode _code)
        {
			string result = "";

			switch(_code)
            {
				case TunnelCode.ETC_EmergencyCall:			result = "비상전화";	break;
				case TunnelCode.ETC_fireplug:				result = "소화전";	break;
				case TunnelCode.ETC_EmergencyExit:			result = "비상구";	break;
				case TunnelCode.LIGHT:						result = "조명";	break;
				case TunnelCode.MAIN_Drain:					result = "배수로";	break;
				case TunnelCode.MAIN_Cover:					result = "배수로 덮개";	break;
				case TunnelCode.MAIN_Paving:				result = "포장";		break;
				case TunnelCode.MAIN_SideWall:				result = "측벽";		break;
				case TunnelCode.MAIN_Ceiling:				result = "천장";		break;
				case TunnelCode.JET:						result = "팬";		break;
				case TunnelCode.WALL_CentralReservation:	result = "중앙분리대";	break;
				case TunnelCode.WALL_Gate:					result = "옹벽";		break;
				case TunnelCode.WALL_Slope:					result = "사면";		break;

            }

			return result;
        }
	}
}
