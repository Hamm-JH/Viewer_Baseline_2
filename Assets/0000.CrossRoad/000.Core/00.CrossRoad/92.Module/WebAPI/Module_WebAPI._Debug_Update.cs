using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Module.WebAPI
{
	/// <summary>
	/// 템플릿
	/// </summary>
	public partial class Module_WebAPI : AModule
	{
		[DllImport("__Internal")]
		private static extern void UnityRequest(string argument);


		private void CheckReceiveInput()
		{
			#region ResetIssue
			// O 리셋이슈
			if (Input.GetKeyDown(KeyCode.Q))
			{
				//Debug.Log("Q");
				ReceiveRequest("ResetIssue");
			}
			#endregion
			#region SelectObject
			// O 객체선택
			else if (Input.GetKeyDown(KeyCode.W))
			{
				//Debug.Log("W");
				ReceiveRequest("SelectObject/1,1,M_Ce,천장");
			}
			#endregion
			#region SelectIssue
			// X 특정 손상정보 선택
			else if (Input.GetKeyDown(KeyCode.E))
			{
				ReceiveRequest("SelectIssue/20211206-00000039");
			}
			#endregion
			#region 6Shape
			else if (Input.GetKeyDown(KeyCode.Keypad1))
			{
				ReceiveRequest("SelectObject6Shape/Top");
			}
			else if (Input.GetKeyDown(KeyCode.Keypad2))
			{
				ReceiveRequest("SelectObject6Shape/Bottom");
			}
			else if (Input.GetKeyDown(KeyCode.Keypad3))
			{
				ReceiveRequest("SelectObject6Shape/Front");
			}
			else if (Input.GetKeyDown(KeyCode.Keypad4))
			{
				ReceiveRequest("SelectObject6Shape/Back");
			}
			else if (Input.GetKeyDown(KeyCode.Keypad5))
			{
				ReceiveRequest("SelectObject6Shape/Left");
			}
			else if (Input.GetKeyDown(KeyCode.Keypad6))
			{
				ReceiveRequest("SelectObject6Shape/Right");
			}
			#endregion
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
			else if(Input.GetKeyDown(KeyCode.A))
            {
				ReceiveRequest("ChangeTab/DMG");
            }
			else if (Input.GetKeyDown(KeyCode.S))
			{
				ReceiveRequest("ChangeTab/RCV");
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				ReceiveRequest("SelectIssue/20220520-00000001");
			}
		}

#if UNITY_EDITOR
		private void Update()
		{
			CheckReceiveInput();
		}
#endif

	}
}
