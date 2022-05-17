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
}
