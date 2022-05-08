using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public static class NameParameter
	{
		public static Definition.TunnelObjectType GetMatParameter(string name, int index)
		{
			Definition.TunnelObjectType result = Definition.TunnelObjectType.Null;

			string[] indexes = name.Split(',');
			string param = "";

			if(indexes.Length > 1)
			{
				param = indexes[0];
			}

			
			if (index == 0)
			{
				param = indexes[0];/* name.Split(',')[0];*/
			}
			// 인덱스 뷰어 단계
			else if (index == 1 && indexes.Length > 1)
			{
				param = name.Split(',')[2];
				//Debug.Log(param);
			}

			//Debug.Log(param);

			switch (param)
			{
				#region ETC
				case "Etc_L_Ec":
				case "Etc_R_Ec":
					{
						result = Definition.TunnelObjectType.ETC_EmergencyCall;
					}
					break;

				case "Etc_L_F":
				case "Etc_R_F":
					{
						result = Definition.TunnelObjectType.ETC_fireplug;
					}
					break;
				case "Etc_L_Ex":
				case "Etc_R_Ex":
					{
						result = Definition.TunnelObjectType.ETC_EmergencyExit;
					}
					break;
				case "Etc_L_T1":
				case "Etc_R_T1":
					{
						result = Definition.TunnelObjectType.ETC_IndicatorLight1;
					}
					break;
				case "Etc_L_T2":
				case "Etc_R_T2":
					{
						result = Definition.TunnelObjectType.ETC_IndicatorLight2;
					}
					break;
				#endregion

				#region LIGHT
				case "L_L_C":
				case "L_R_C":
				case "L_C_C":
				case "L_L_Si":
				case "L_R_Si":
					{
						result = Definition.TunnelObjectType.LIGHT;
					}
					break;
				#endregion

				#region WALL
				case "S_R_Div":
				case "E_L_Div":
					{
						result = Definition.TunnelObjectType.WALL_CentralReservation;
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
						result = Definition.TunnelObjectType.WALL_Gate;
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
						result = Definition.TunnelObjectType.WALL_Slope;
					}
					break;
				#endregion

				#region MAIN
				case "M_L_DIn":
				case "M_L_DOut":
				case "M_R_DIn":
				case "M_R_DOut":
					{
						result = Definition.TunnelObjectType.MAIN_Drain;
					}
					break;

				case "M_L_Co":
				case "M_R_Co":
					{
						result = Definition.TunnelObjectType.MAIN_Cover;
					}
					break;

				case "M_P":
					{
						result = Definition.TunnelObjectType.MAIN_Paving;
					}
					break;

				case "M_L_SwI":
				case "M_L_SwO":
				case "M_R_SwI":
				case "M_R_SwO":
					{
						result = Definition.TunnelObjectType.MAIN_SideWall;
					}
					break;

				case "M_Ce":
				case "M_L_Ce":
				case "M_R_Ce":
					{
						result = Definition.TunnelObjectType.MAIN_Ceiling;
						//string liningMaterial = MainManager.Instance.liningMaterial;
						//if (liningMaterial == "콘크리트")
						//{
						//}
						//else if(liningMaterial == "파형강판")
						//{
						//	result = Definition.TunnelObjectType.MAIN_CorrugatedSteel;
						//}
					}
					break;
				#endregion

				#region JET
				case "J_L_JL":
				case "J_C_JC":
				case "J_R_JR":
					{
						result = Definition.TunnelObjectType.JET;
					}
					break;

					#endregion
			}

			return result;
		}
	}
}
