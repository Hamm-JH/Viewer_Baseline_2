using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Bridge
{
	public enum BridgeCode
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

	public enum BridgeCodeDetail
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
}
