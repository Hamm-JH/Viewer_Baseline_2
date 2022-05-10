using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	public class WebViewerDebug : MonoBehaviour
	{
		public GameObject m_eventTG;

		[Header("TO BO LE RE FR BA")]
		public string debug_6surface;

		[Header("부재 코드")]
		public string debug_partCode;

		[Header("00000000-00000000")]
		public string debug_dmgCode;

		[Header("00000000-00000000")]
		public string debug_rcvCode;

#if UNITY_EDITOR
		// Update is called once per frame
		void Update()
		{
			if(Input.anyKeyDown)
			{
				OnDebug();
			}
		}
#endif

		private void OnDebug()
		{
			if(Input.GetKeyDown(KeyCode.Q))
			{
				Input_InformationWidthChange();
			}
			else if(Input.GetKeyDown(KeyCode.W))
			{
				Input_InitializeRegisterMode();
			}
			else if(Input.GetKeyDown(KeyCode.E))
			{
				Input_FinishRegisterMode__False();
			}
			else if(Input.GetKeyDown(KeyCode.D))
			{
				Input_FinishRegisterMode__True_recover_delete();
			}
			else if(Input.GetKeyDown(KeyCode.C))
			{
				Input_FinishRegisterMode__True_damage_delete();
			}
			else if(Input.GetKeyDown(KeyCode.R))
			{
				Input_SelectObject();
			}
			else if(Input.GetKeyDown(KeyCode.T))
			{
				Input_SelectObject6Shape();
			}
			else if(Input.GetKeyDown(KeyCode.Y))
			{
				Input_ChangePinMode();
			}
			else if(Input.GetKeyDown(KeyCode.U))
			{
				Input_SelectIssue();
			}
		}

		private void Input_InformationWidthChange()
		{
			m_eventTG.SendMessage("ReceiveRequest", "InformationWidthChange");
		}

		private void Input_InitializeRegisterMode()
		{

		}

		private void Input_FinishRegisterMode__False()
		{

		}

		private void Input_FinishRegisterMode__True_recover_delete()
		{

		}
		private void Input_FinishRegisterMode__True_damage_delete()
		{
			
		}

		private void Input_SelectObject()
		{

		}

		private void Input_SelectObject6Shape()
		{

		}

		private void Input_ChangePinMode()
		{

		}

		private void Input_SelectIssue()
		{

		}
	}
}
