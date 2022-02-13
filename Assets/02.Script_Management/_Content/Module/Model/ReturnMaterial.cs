using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public static class ReturnMaterial
	{
		public static void SetMaterials(MeshRenderer source, Definition.TunnelObjectType objType)
		{
			Material[] setter = SetMaterialByType(objType);

			source.materials = setter;
		}

		private static Material[] SetMaterialByType(Definition.TunnelObjectType objType)
		{
			List<Material> result = new List<Material>();

			string basePath = "Material";

			switch (objType)
			{
				#region ETC
				case Definition.TunnelObjectType.ETC_EmergencyCall:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyCall"));
						result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
					}
					break;

				case Definition.TunnelObjectType.ETC_EmergencyExit:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyExit"));
						result.Add(Resources.Load<Material>($"{basePath}/Base/Concrete"));
					}
					break;

				case Definition.TunnelObjectType.ETC_fireplug:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_fireplug"));
						//result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
					}
					break;

				case Definition.TunnelObjectType.ETC_IndicatorLight1:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight1"));
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight1"));
						result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
					}
					break;

				case Definition.TunnelObjectType.ETC_IndicatorLight2:
					{
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight2"));
						result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight2"));
						result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
					}
					break;
				#endregion

				#region MAIN
				case Definition.TunnelObjectType.MAIN_Ceiling:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Ceiling"));
					}
					break;

				case Definition.TunnelObjectType.MAIN_Cover:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Cover"));
					}
					break;

				case Definition.TunnelObjectType.MAIN_Drain:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Drain"));
					}
					break;

				case Definition.TunnelObjectType.MAIN_Paving:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_Paving"));
					}
					break;

				case Definition.TunnelObjectType.MAIN_SideWall:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_SideWall"));
					}
					break;

				case Definition.TunnelObjectType.MAIN_CorrugatedSteel:
					{
						result.Add(Resources.Load<Material>($"{basePath}/MAIN_CorrugatedSteel"));
					}
					break;
				#endregion

				#region WALL
				case Definition.TunnelObjectType.WALL_CentralReservation:
					{
						result.Add(Resources.Load<Material>($"{basePath}/WALL_CentralReservation"));
					}
					break;

				case Definition.TunnelObjectType.WALL_Gate:
					{
						result.Add(Resources.Load<Material>($"{basePath}/WALL_Gate"));
					}
					break;

				case Definition.TunnelObjectType.WALL_Slope:
					{
						result.Add(Resources.Load<Material>($"{basePath}/WALL_Slope"));
					}
					break;
				#endregion

				#region JET
				case Definition.TunnelObjectType.JET:
					{
						result.Add(Resources.Load<Material>($"{basePath}/JET"));
					}
					break;
				#endregion

				#region LIGHT
				case Definition.TunnelObjectType.LIGHT:
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
