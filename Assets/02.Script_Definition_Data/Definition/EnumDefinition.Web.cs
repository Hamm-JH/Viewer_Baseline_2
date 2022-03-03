using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{

	public enum WebType
	{
		Issue_Dmg = 0x11,
		Issue_Rcv = 0x12,

	}
    public enum SendRequestCode
    {
        Null,
        SelectObject,   // 3D 객체 선택
        SelectIssue,    // 손상/보강 선택
        SelectObject6Shape, // 6면 객체 선택
        SelectSurfaceLocation,  // 9면 선택

        InitializeRegisterMode, // 손상 등록 모드
        SetPinVector,           // 위치벡터 할당

    }

    public enum ReceiveRequestCode
    {
        Null,

        ResetIssue,         // 손상/보강 리셋

        SelectObject,       // 3D 객체 선택
        SelectIssue,        // 손상/보강 선택
        SelectObject6Shape, // 6면 객체 선택
        SelectSurfaceLocation,  // 9면 선택
        InformationWidthChange, // 정보창 폭 변경 수신

        //SetIssueStatus,         // 손상 / 보강 상태 변경
        ChangePinMode,          // PinMode 설정 변경
        InitializeRegisterMode, // 등록 모드 시작
        FinishRegisterMode,     // 등록 단계 종료

    }
}
