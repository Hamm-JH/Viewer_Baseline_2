using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Tunnel
{
    using AdminViewer.Tunnel;
    using Definition;

    public static class Tunnel_Materials
    {
		public static void Set(MeshRenderer source, GraphicCode _gCode, TunnelCode _tCode )
        {
			Material[] mats = Set(_gCode, _tCode);

			source.materials = mats;
        }

        private static Material[] Set(GraphicCode _gCode, TunnelCode _tCode)
        {
			Material[] result = null;

			switch(_gCode)
            {
				case GraphicCode.Single_Color:
					result = Set_SingleColor(_tCode);
					break;

				case GraphicCode.Platform_Texturing:
					result = Set_PlatformTexturing(_tCode);
					break;

				default:
					throw new Definition.Exceptions.GraphicCodeNotDefined(_gCode);
            }

			return result;
        }

		private static Material[] Set_SingleColor(TunnelCode _tCode)
        {
			List<Material> result = new List<Material>();

			result.Add(Resources.Load<Material>($"Projects/Tunnel/Materials/Default"));

			return result.ToArray();
        }

		private static Material[] Set_PlatformTexturing(TunnelCode _tCode)
        {
			List<Material> result = new List<Material>();

			string basePath = "Projects/Tunnel/Materials";

			switch (_tCode)
			{
				#region ETC

				case TunnelCode.ETC_EmergencyCall:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyCall"));
						//result.Add(Resources.Load<Material>($"{basePath}/Steel"));
					}
					break;

				case TunnelCode.ETC_EmergencyExit:
					{
                        //result.Add(Resources.Load<Material>($"{basePath}/Concrete"));
                        result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyExit"));
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
