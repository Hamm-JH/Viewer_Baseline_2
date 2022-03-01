using Kino;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

public partial class WebManager : MonoBehaviour
{
    #region Instance

    private static WebManager instance;

    public static WebManager Instance
	{
        get
		{
            if(instance == null)
			{
                instance = FindObjectOfType<WebManager>() as WebManager;
			}
            return instance;
		}
	}

    #endregion

    #region 변수 선언부

    //[SerializeField] private MainManager mainManager;
    //[SerializeField] private VirtualWebManager webPage;
    //[SerializeField] private DimViewManager dimViewManager;

    [SerializeField] private bool isRegisterMode;

    int doubleClickRemover = 0;

    // FinishRegisterMode 웹코드 변경 이전의 버전 실행용 코드
    // True : 변경 이전 코드 실행
    // False : 변경 이후 코드 실행 (이 경우 변수 적용 코드 재배치 하고 이 변수 삭제하면 된다.)
    public bool isUnderUpdateVersion;

    #endregion

    #region 속성 선언부

    //public MainManager __Manager
    //{
    //    get => mainManager;
    //    set => mainManager = value;
    //}

    //public DimViewManager __viewmanager
    //{
    //    get => dimViewManager;
    //    set => dimViewManager = value;
    //}

    public bool IsRegisterMode
    {
        get => isRegisterMode;
        set => isRegisterMode = value;
    }

    #endregion

    private void Start()
    {
        IsRegisterMode = false;
    }

    [DllImport("__Internal")]
    private static extern void UnityRequest(string argument);


    private void CheckReceiveInput()
	{
		// O 리셋이슈
		if (Input.GetKeyDown(KeyCode.Q))
		{
			//Debug.Log("Q");
			ReceiveRequest("ResetIssue");
		}
		// O 객체선택
		else if (Input.GetKeyDown(KeyCode.W))
		{
			//Debug.Log("W");
			ReceiveRequest("SelectObject/1,1,M_Ce,천장");
		}
		// X 특정 손상정보 선택
		else if (Input.GetKeyDown(KeyCode.E))
		{
			//Debug.Log("E");
			// TODO 1101 손상 / 보수 / 보강코드 생성시 테스트 가능
			Debug.LogError("1101 손상 / 보수 / 보강코드 생성시 테스트 가능");
			//ReceiveRequest("SelectIssue/00000000_00000001");   // 뒤에건 손상 / 보수 / 보강코드
		}
		// O 6면 선택
		else if (Input.GetKeyDown(KeyCode.R))
		{
			//Debug.Log("R");
			//ReceiveRequest("SelectObject6Shape/Top");
			//ReceiveRequest("SelectObject6Shape/Bottom");
			//ReceiveRequest("SelectObject6Shape/Front");
			//ReceiveRequest("SelectObject6Shape/Back");
			//ReceiveRequest("SelectObject6Shape/Left");
			ReceiveRequest("SelectObject6Shape/Right");
		}
		// O 화면 폭 조정
		else if (Input.GetKeyDown(KeyCode.T))
		{
			//Debug.Log("T");
			ReceiveRequest("InformationWidthChange/500");
		}
		// ? 손상 / 보강 상태 변경
		else if (Input.GetKeyDown(KeyCode.Y))
		{
			//Debug.Log("Y");
			ReceiveRequest("SetIssueStatus/00000000-00000000/0");
		}
		// X 등록 단계에서 핀 설정 모드로 진입
		else if (Input.GetKeyDown(KeyCode.I))
		{
			//Debug.Log("I");
			ReceiveRequest("ChangePinMode");
		}
		// O 등록 모드 시작
		else if (Input.GetKeyDown(KeyCode.O))
		{
			ReceiveRequest("InitializeRegisterMode");
		}
		// X 등록 모드 끝내기
		else if (Input.GetKeyDown(KeyCode.P))
		{
			ReceiveRequest("FinishRegisterMode");
		}
	}

    private void Update()
    {
        CheckReceiveInput();
    }
}
