using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Tunnel
{
    using AdminViewer.Tunnel;

    public static class Tunnel_Materials
    {
        public static Material[] Set(TunnelCode _tCode)
        {
            List<Material> result = new List<Material>();

            string basePath = "Projects/Tunnel/Materials";

            switch(_tCode)
            {
                #region ETC

                case TunnelCode.ETC_EmergencyCall:
                    {
                        result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyCall"));
                        result.Add(Resources.Load<Material>($"{basePath}/Steel"));
                    }
                    break;

				case TunnelCode.ETC_EmergencyExit:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyExit"));
						result.Add(Resources.Load<Material>($"{basePath}/Concrete"));
					}
					break;

				case TunnelCode.ETC_fireplug:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_fireplug"));
						//result.Add(Resources.Load<Material>($"{basePath}/Steel"));
					}
					break;

				case TunnelCode.ETC_IndicatorLight1:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight1"));
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight1"));
						result.Add(Resources.Load<Material>($"{basePath}/Steel"));
					}
					break;

				case TunnelCode.ETC_IndicatorLight2:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight2"));
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight2"));
						result.Add(Resources.Load<Material>($"{basePath}/Steel"));
					}
					break;

				#endregion

				#region MAIN
				case TunnelCode.MAIN_Ceiling:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Ceiling"));
					}
					break;

				case TunnelCode.MAIN_Cover:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Cover"));
					}
					break;

				case TunnelCode.MAIN_Drain:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Drain"));
					}
					break;

				case TunnelCode.MAIN_Paving:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Paving"));
					}
					break;

				case TunnelCode.MAIN_SideWall:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_SideWall"));
					}
					break;

				case TunnelCode.MAIN_CorrugatedSteel:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_CorrugatedSteel"));
					}
					break;
				#endregion

				#region WALL
				case TunnelCode.WALL_CentralReservation:
					{
						result.Add(Resources.Load<Material>($"{basePath}/WALL_CentralReservation"));
					}
					break;

				case TunnelCode.WALL_Gate:
					{
						result.Add(Resources.Load<Material>($"{basePath}/WALL_Gate"));
					}
					break;

				case TunnelCode.WALL_Slope:
					{
						result.Add(Resources.Load<Material>($"{basePath}/WALL_Slope"));
					}
					break;
				#endregion

				#region JET
				case TunnelCode.JET:
					{
						result.Add(Resources.Load<Material>($"{basePath}/JET"));
					}
					break;
				#endregion

				#region LIGHT
				case TunnelCode.LIGHT:
					{
						result.Add(Resources.Load<Material>($"{basePath}/LIGHT"));
					}
					break;
					#endregion
			}

			return result.ToArray();
        }
    }
}
