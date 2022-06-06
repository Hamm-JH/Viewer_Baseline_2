using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 컨트롤 파트의 Enum 변수 정의
/// </summary>
namespace Definition.Control
{


	/// <summary>
	/// (모델? 컨트롤? 뷰?) 카메라 회전 모드 정의
	/// </summary>
	public enum CameraModes
	{
		NotDefine = -1,			// 정의되지 않음
		ORBIT_ISO = 0x10,		// 궤도 정사영 모드
		ORBIT_Top = 0x11,		// 궤도 상부 뷰 모드
		ORBIT_Side = 0x12,		// 궤도 측면 뷰 모드
		ORBIT_Bottom = 0x13,	// 궤도 하부 뷰 모드

		FREE_ISO = 0x20,		// 자유시점 모드
		FREE_Top = 0x21,		// 자유시점 상부 뷰 모드
		FREE_Side = 0x22,		// 자유시점 측면 뷰 모드
		FREE_Bottom = 0x23,		// 자유시점 하부 뷰 모드

		BIM_ISO = 0x30,			// BIM 모드
		BIM_Top = 0x31,			// BIM 상부 뷰 모드
		BIM_Side = 0x32,		// BIM 측면 뷰 모드
		BIM_Bottom = 0x33,		// BIM 하부 뷰 모드
		BIM_FRONT = 0x34,		// BIM 측전면 뷰 모드
		BIM_BACK = 0x35,		// BIM 측후면 뷰 모드
		BIM_LEFT = 0x36,		// BIM 측좌면 뷰 모드
		BIM_RIGHT = 0x37,		// BIM 측우면 뷰 모드
		BIM_Panning = 0x38,		// BIM 패닝

		OnlyRotate = 0x40,		// 제자리에서 회전만 수행
	}
}
