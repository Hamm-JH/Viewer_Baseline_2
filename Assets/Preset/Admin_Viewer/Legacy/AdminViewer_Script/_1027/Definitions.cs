using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
	public enum Side
	{
		Left = 0,
		Right = 1
	}

	public enum WallDefinition
	{
		ERROR,
		ALLOFF,
		NOTSET,
		Deg0,
		Deg45,
		Deg90
	}

	public enum WallOption
	{
		ERROR,
		OnlyWall,
		OnlySlope,
		Both
	}

	/// <summary>
	/// 터널의 형태
	/// </summary>
	public enum TunnelType
	{
		NULL = -1,
		BOX,
		ROUND
	}

	/// <summary>
	/// 객체의 타입 (Material 할당용)
	/// </summary>
	public enum ObjectType
	{
		Null = -1,
		// 기타
		ETC_EmergencyCall,
		ETC_fireplug,
		ETC_EmergencyExit,
		ETC_IndicatorLight1,
		ETC_IndicatorLight2,

		MAIN_Drain,
		MAIN_Cover,
		MAIN_Paving,
		MAIN_SideWall,
		MAIN_Ceiling,
		MAIN_CorrugatedSteel,

		LIGHT,

		JET,

		WALL_CentralReservation,
		WALL_Gate,
		WALL_Slope
	}
}
