using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public enum IssueType
	{
		Null,
		damage,
		recover
	}

	/// <summary>
	/// API 호출분기
	/// </summary>
	public enum WebType
	{
		Address = 0x10,

		Issue_Dmg = 0x11,
		Issue_Rcv = 0x12,

		Image_main = 0x21,
		Image_single = 0x22,

		history = 0x31,
	}

	/// <summary>
	/// API :: Send 이벤트 코드
	/// </summary>
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

	/// <summary>
	/// API :: Receive 이벤트 코드
	/// </summary>
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

	public enum SceneStatus
	{
		EnvironmentVariablesSetting = 0,        // 환경변수 세팅
		GetGLTF = 1,                            // GLTF 파일 받아오기
		ObjectInitialize = 2,                   // GLTF 받은 객체 초기화
		Ready = 3,                              // Viewer Scene 초기화 완료 상태 / 기본 상태

		Register = 4,                           // 손상/보강 등록 상태
		Register_PinMode = 5,                   // 손상/보강 핀 등록 상태
		Default = 6,
		Modified = 7,
		Delete = 8,
	}

	

	public enum UseCase
	{
		NotDefined = -1,
		Bridge = 0,
		Tunnel = 1
	}

	public enum ViewRotations
	{
		Null = -1,
		Top = 0,
		Bottom = 1,
		Front = 2,
		Back = 3,
		Left = 4,
		Right = 5,
	}

	public enum BoolValue
	{
		True,
		False
	}

	public enum IssueVisualizeOption
	{
		All_ON,
		All_OFF,
		SelectedTarget
	}
}
