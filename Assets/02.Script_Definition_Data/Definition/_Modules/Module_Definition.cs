using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{

	public enum ModuleCode
	{
		// 기본 상태는 베이스로 깔림
		//Default = 

		/// <summary>
		/// 점검정보 모듈 (진입시 사용)
		/// </summary>
		WorkQueue = 0x10,

		/// <summary>
		/// 점검정보 - 등록단계
		/// </summary>
		Work_Pinmode = 0x11,

		/// <summary>
		/// 점검정보 관리 (관리자뷰어 기능 통칭)
		/// </summary>
		Issue_Administration = 0x90,
	}

	/// <summary>
	/// 모듈 상태코드
	/// </summary>
	public enum ModuleStatus
	{
		/// <summary>
		/// 점검정보 관리 뷰 1 ~ 5단계
		/// </summary>
		Administration_view1 = 0x01,
		Administration_view2 = 0x02,
		Administration_view3 = 0x03,
		Administration_view4 = 0x04,
		Administration_view5 = 0x05,
	}
}

