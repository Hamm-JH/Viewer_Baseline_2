using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	/// <summary>
	/// 손상 정보의 분류
	/// </summary>
	public enum IssueType
	{
		Null,
		damage,	// 손상
		recover	// 보수
	}

	/// <summary>
	/// API 호출분기
	/// </summary>
	public enum WebType
	{
		Address = 0x10,			// 주소 API 호출

		Issue_Dmg = 0x11,		// 손상 API 호출
		Issue_Rcv = 0x12,		// 보수 API 호출

		Image_main = 0x21,		// 주 이미지 API 호출
		Image_single = 0x22,	// 단일 이미지 API 호출

		history = 0x31,			// 이력정보 API 호출
		imageHistory = 0x32,	// 이력 정보 단위의 API 호출
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
		ChangeTab,			// 탭 변경

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

	/// <summary>
	/// Scene 표시 상태코드
	/// </summary>
	public enum SceneStatus
	{
		EnvironmentVariablesSetting = 0,        // 환경변수 세팅
		GetGLTF = 1,                            // GLTF 파일 받아오기
		ObjectInitialize = 2,                   // GLTF 받은 객체 초기화
		Ready = 3,                              // Viewer Scene 초기화 완료 상태 / 기본 상태

		Register = 4,                           // 손상/보강 등록 상태
		Register_PinMode = 5,                   // 손상/보강 핀 등록 상태
		Default = 6,							// 기본 상태
		Modified = 7,							// 상태 변환중의 상태
		Delete = 8,								// 특정 요소 제거중인 상태
	}


	/// <summary>
	/// 카메라 보는 방향각도 코드
	/// </summary>
	public enum ViewRotations
	{
		Null = -1,
		Top = 0,		// 상
		Bottom = 1,		// 하
		Front = 2,		// 전
		Back = 3,		// 후
		Left = 4,		// 좌
		Right = 5,		// 우
	}

	/// <summary>
	/// 손상 표시정보 옵션
	/// </summary>
	public enum IssueVisualizeOption
	{
		All_ON,
		All_OFF,
		SelectedTarget
	}
}
