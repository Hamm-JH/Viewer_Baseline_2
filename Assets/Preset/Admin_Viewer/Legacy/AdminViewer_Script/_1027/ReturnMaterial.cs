using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	//public static class ReturnMaterial
	//{
	//	public static void SetMaterials(MeshRenderer source, Datas.ObjectType objType)
	//	{
	//		Material[] setter = SetMaterialByType(objType);

	//		source.materials = setter;
	//	}

	//	private static Material[] SetMaterialByType(Datas.ObjectType objType)
	//	{
	//		List<Material> result = new List<Material>();

	//		string basePath = "Material";

	//		switch (objType)
	//		{
	//			#region ETC
	//			case Datas.ObjectType.ETC_EmergencyCall:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyCall"));
	//					result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
	//				}
	//				break;

	//			case Datas.ObjectType.ETC_EmergencyExit:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/ETC_EmergencyExit"));
	//					result.Add(Resources.Load<Material>($"{basePath}/Base/Concrete"));
	//				}
	//				break;

	//			case Datas.ObjectType.ETC_fireplug:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/ETC_fireplug"));
	//					result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
	//				}
	//				break;

	//			case Datas.ObjectType.ETC_IndicatorLight1:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight1"));
	//					result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight1"));
	//					result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
	//				}
	//				break;

	//			case Datas.ObjectType.ETC_IndicatorLight2:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight2"));
	//					result.Add(Resources.Load<Material>($"{basePath}/ETC_IndicatorLight2"));
	//					result.Add(Resources.Load<Material>($"{basePath}/Base/Steel"));
	//				}
	//				break;
	//			#endregion

	//			#region MAIN
	//			case Datas.ObjectType.MAIN_Ceiling:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/MAIN_Ceiling"));
	//				}
	//				break;

	//			case Datas.ObjectType.MAIN_Cover:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/MAIN_Cover"));
	//				}
	//				break;

	//			case Datas.ObjectType.MAIN_Drain:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/MAIN_Drain"));
	//				}
	//				break;

	//			case Datas.ObjectType.MAIN_Paving:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/MAIN_Paving"));
	//				}
	//				break;

	//			case Datas.ObjectType.MAIN_SideWall:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/MAIN_SideWall"));
	//				}
	//				break;

	//			case Datas.ObjectType.MAIN_CorrugatedSteel:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/MAIN_CorrugatedSteel"));
	//				}
	//				break;
	//			#endregion

	//			#region WALL
	//			case Datas.ObjectType.WALL_CentralReservation:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/WALL_CentralReservation"));
	//				}
	//				break;

	//			case Datas.ObjectType.WALL_Gate:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/WALL_Gate"));
	//				}
	//				break;

	//			case Datas.ObjectType.WALL_Slope:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/WALL_Slope"));
	//				}
	//				break;
	//			#endregion

	//			#region JET
	//			case Datas.ObjectType.JET:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/JET"));
	//				}
	//				break;
	//			#endregion

	//			#region LIGHT
	//			case Datas.ObjectType.LIGHT:
	//				{
	//					result.Add(Resources.Load<Material>($"{basePath}/LIGHT"));
	//				}
	//				break;
	//			#endregion
	//		}

	//		return result.ToArray();
	//	}
	//}
}
