using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.UI
{
	using Definition;
	using Definition._Issue;
	using TMPro;
	using UnityEngine.UI;

	public class Ad_TableElement : MonoBehaviour
	{
		public GameObject r_bm_State2_dmg;
		public GameObject r_b1_State3_dmg;
		public GameObject r_b1_State4_rcv;
		public GameObject r_b2_State4_rein;
		public GameObject r_m1_State5_dmg;
		public GameObject r_m1_State5_rcv;
		public GameObject r_m1_State5_rein;

		[Header("state2 dmg")]
		public TextMeshProUGUI bm2d_1number;
		public TextMeshProUGUI bm2d_2issueName;
		public TextMeshProUGUI bm2d_3partName;
		public TextMeshProUGUI bm2d_4location;
		public TextMeshProUGUI bm2d_5date;

		[Header("state3 dmg")]
		public TextMeshProUGUI b13d_1issueName;
		public TextMeshProUGUI b13d_2location;
		public TextMeshProUGUI b13d_3date;
		public Button b13d_4image;

		[Header("state4 rcv")]
		public TextMeshProUGUI b14rc_1repMethod;
		public TextMeshProUGUI b14rc_2date;
		public Button b14rc_3image;

		[Header("state4 rein")]
		public TextMeshProUGUI b24re_1reinMethod;
		public TextMeshProUGUI b24re_2date;
		public Button b24re_3image;

		public void SetTableElement(Ad_PanelType _pType, UIEventType _uType /* TODO 데이터*/)
		{
			if (_uType == UIEventType.Ad_nav_state2)
			{
				if (_pType == Ad_PanelType.bm)
				{
					SetTableElement_s2_bm();
				}
			}
			else if (_uType == UIEventType.Ad_nav_state3)
			{
				if (_pType == Ad_PanelType.b1)
				{
					SetTableElement_s3_b1();
				}
			}
			else if (_uType == UIEventType.Ad_nav_state4)
			{
				if (_pType == Ad_PanelType.b1)
				{
					SetTableElement_s4_b1();
				}
				else if (_pType == Ad_PanelType.b2)
				{
					SetTableElement_s4_b2();
				}
			}
			else if (_uType == UIEventType.Ad_nav_state5)
			{
				if (_pType == Ad_PanelType.s5m1)
				{
					SetTableElement_s5_s5m1();
				}
			}
		}

		public void SetTableElement(Ad_PanelType _pType, UIEventType _uType, int _index, Issue _data)
		{
			if (_uType == UIEventType.Ad_nav_state2)
			{
				if (_pType == Ad_PanelType.bm)
				{
					SetTableElement_s2_bm(_index, _data);
				}
			}
			else if (_uType == UIEventType.Ad_nav_state3)
			{
				if (_pType == Ad_PanelType.b1)
				{
					SetTableElement_s3_b1(_data);
				}
			}
			else if (_uType == UIEventType.Ad_nav_state4)
			{
				if (_pType == Ad_PanelType.b1)
				{
					SetTableElement_s4_b1();
				}
				else if (_pType == Ad_PanelType.b2)
				{
					SetTableElement_s4_b2();
				}
			}
			else if (_uType == UIEventType.Ad_nav_state5)
			{
				if (_pType == Ad_PanelType.s5m1)
				{
					SetTableElement_s5_s5m1();
				}
			}
		}

		#region 패널별 데이터 할당

		private void SetTableElement_s2_bm(int _index, Issue _data)
		{
			SetTableElement_s2_bm();

			bm2d_1number.text = $"{_index+1}";
			bm2d_2issueName.text = $"{_data.IssueCode.ToString()}";
			bm2d_3partName.text = $"{_data.CdBridgeParts}";
			bm2d_4location.text = $"{_data.DcLocation}";
			bm2d_5date.text = $"{_data.DateDmg}";
		}

		/// <summary>
		/// 상태2번 bm패널
		/// </summary>
		private void SetTableElement_s2_bm(/* TODO 데이터*/)
		{
			//Debug.Log("SetTableElement_s2_bm");

			r_bm_State2_dmg.SetActive(true);
			r_b1_State3_dmg.SetActive(false);
			r_b1_State4_rcv.SetActive(false);
			r_b2_State4_rein.SetActive(false);
			r_m1_State5_dmg .SetActive(false);
			r_m1_State5_rcv .SetActive(false);
			r_m1_State5_rein.SetActive(false);
		}

		private void SetTableElement_s3_b1(Issue _data)
		{
			SetTableElement_s3_b1();

			b13d_1issueName.text = $"{_data.IssueCode.ToString()}";	// 손상정보
			b13d_2location.text = $"{_data.DcLocation} TODO";   // 손상위치
			b13d_3date.text = $"{_data.DateDmg}";		// 날짜
		}

		/// <summary>
		/// 상태3번 b1패널
		/// </summary>
		private void SetTableElement_s3_b1(/* TODO 데이터*/)
		{
			//Debug.Log("SetTableElement_s3_b1");

			r_bm_State2_dmg.SetActive(false);
			r_b1_State3_dmg.SetActive(true);
			r_b1_State4_rcv.SetActive(false);
			r_b2_State4_rein.SetActive(false);
			r_m1_State5_dmg.SetActive(false);
			r_m1_State5_rcv.SetActive(false);
			r_m1_State5_rein.SetActive(false);
		}

		/// <summary>
		/// 상태4번 b1패널
		/// </summary>
		private void SetTableElement_s4_b1(/* TODO 데이터*/)
		{
			//Debug.Log("SetTableElement_s4_b1");

			r_bm_State2_dmg.SetActive(false);
			r_b1_State3_dmg.SetActive(false);
			r_b1_State4_rcv.SetActive(true);
			r_b2_State4_rein.SetActive(false);
			r_m1_State5_dmg.SetActive(false);
			r_m1_State5_rcv.SetActive(false);
			r_m1_State5_rein.SetActive(false);
		}

		/// <summary>
		/// 상태4번 b2패널
		/// </summary>
		private void SetTableElement_s4_b2(/* TODO 데이터*/)
		{
			//Debug.Log("SetTableElement_s4_b2");

			r_bm_State2_dmg.SetActive(false);
			r_b1_State3_dmg.SetActive(false);
			r_b1_State4_rcv.SetActive(false);
			r_b2_State4_rein.SetActive(true);
			r_m1_State5_dmg.SetActive(false);
			r_m1_State5_rcv.SetActive(false);
			r_m1_State5_rein.SetActive(false);
		}

		/// <summary>
		/// 상태5번 s5m1패널
		/// </summary>
		private void SetTableElement_s5_s5m1(/* TODO 데이터*/)
		{
			//Debug.Log("SetTableElement_s5_s5m1");

			r_bm_State2_dmg.SetActive(false);
			r_b1_State3_dmg.SetActive(false);
			r_b1_State4_rcv.SetActive(false);
			r_b2_State4_rein.SetActive(false);
			r_m1_State5_dmg.SetActive(true);
			r_m1_State5_rcv.SetActive(false);
			r_m1_State5_rein.SetActive(false);
		}

		#endregion
	}
}
