using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.UI
{
	using Definition;
	using Definition._Issue;
	using Management;
	using Management.Events;
	using Module.UI;
	using System.Data;
	using System.Linq;
	using TMPro;
	using UnityEngine.UI;

	public partial class Ad_Panel : MonoBehaviour
	{
		[SerializeField] UITemplate_AdminViewer m_uiRoot;

		public Ad_PanelType m_pid;
		public List<Ad_Panel_Table> r_TableContents;

		[Header("Title")]
		public Image m_icon;
		public TextMeshProUGUI m_title;
		public TextMeshProUGUI m_titleCount;
		public TextMeshProUGUI m_endText;

		public TextMeshProUGUI m_user;

		

		[Header("B2 element")]
		[SerializeField] B2_element b2_element;

		[Header("State5 b1")]
		[SerializeField] S5b1_element s5b1_element;

		[Header("State5 b2")]
		[SerializeField] S5b2_element __el;

		[Header("State5 m1")]
		[SerializeField] S5m1_element s5m1_element;

		public void SetPanel(UIEventType _uType)
		{
			if (m_pid == Ad_PanelType.Null) return;
			//if (r_TableContents == null || r_TableContents.Count == 0) return;

			switch(_uType)
			{
				case UIEventType.Ad_nav_state3:
				case UIEventType.Ad_nav_state4:
					m_uiRoot.SetObject_IfNotSet();
					break;
			}

			switch (_uType)
			{
				case UIEventType.Ad_nav_state1:
					SetPanel_State1(m_pid);
					break;
				case UIEventType.Ad_nav_state2:
					SetPanel_State2(m_pid);
					break;
				case UIEventType.Ad_nav_state3:
					SetPanel_State3(m_pid, _uType);
					break;
				case UIEventType.Ad_nav_state4:
					SetPanel_State4(m_pid, _uType);
					break;
				case UIEventType.Ad_nav_state5:
					SetPanel_State5(m_pid);
					break;
			}

			// 테이블의 조건이 맞으면 테이블 세팅 시행
			r_TableContents.ForEach(x => x.SetTable(m_pid, _uType));
			//r_TableContents.First().SetTable(m_pid, _uType);

		}

		private void SetPanel_State1(Ad_PanelType _pType)
		{
			// 동작없음
		}

		private void SetPanel_State2(Ad_PanelType _pType)
		{
			if(_pType == Ad_PanelType.bm)
			{
				int dmgCount = ContentManager.Instance._Model.DmgData.Count;

				m_icon.sprite = m_uiRoot.Icons.m_icon_dmg;
				m_icon.color = Colors.Set(ColorType.UI_dmg, 1);
				m_title.text = "손상정보";
				m_titleCount.text = dmgCount.ToString();
				m_titleCount.color = Colors.Set(ColorType.UI_dmg, 1);
				m_endText.text = "건";
			}
		}

		private void SetPanel_State3(Ad_PanelType _pType, UIEventType _uType)
		{
			if(_pType == Ad_PanelType.b1)
			{
				GameObject obj = ContentManager.Instance._SelectedObj;

				string oName = "";
				if (obj != null) oName = obj.name;

				oName = GetPartName(oName);

				m_icon.sprite = m_uiRoot.Icons.m_icon_dmg;
				m_icon.color = Colors.Set(ColorType.UI_dmg, 1);
				m_title.text = $"{oName} 손상목록";
			}
			else if(_pType == Ad_PanelType.b2)
			{
				GameObject obj = ContentManager.Instance._SelectedObj;

				string oName = "";
				if (obj != null) oName = obj.name;

				oName = GetPartName(oName);

				m_titleCount.enabled = false;
				m_user.enabled = true;

				m_icon.sprite = m_uiRoot.Icons.m_icon_detail;
				m_icon.color = Colors.Set(ColorType.White, 1);
				m_title.text = $"{oName}";


				r_TableContents.ForEach(x => x.gameObject.SetActive(false));
				b2_element.m_detailPanel.gameObject.SetActive(true);

				b2_element.m_detailPanel.SetPanel(_pType, _uType);
			}
		}

		private void SetPanel_State4(Ad_PanelType _pType, UIEventType _uType)
		{
			if (_pType == Ad_PanelType.b1)
			{
				GameObject obj = ContentManager.Instance._SelectedObj;

				string oName = "";
				if (obj != null) oName = obj.name;

				oName = GetPartName(oName);

				m_icon.sprite = m_uiRoot.Icons.m_icon_rcv;
				m_icon.color = Colors.Set(ColorType.UI_rcv, 1);
				m_title.text = $"{oName} 보수목록";
			}
			else if (_pType == Ad_PanelType.b2)
			{
				GameObject obj = ContentManager.Instance._SelectedObj;

				string oName = "";
				if (obj != null) oName = obj.name;

				oName = GetPartName(oName);

				m_icon.sprite = m_uiRoot.Icons.m_icon_rein;
				m_icon.color = Colors.Set(ColorType.UI_rein, 1);
				m_title.text = $"{oName} 보강목록";

				m_titleCount.enabled = false;
				m_user.enabled = false;

				r_TableContents.ForEach(x => x.gameObject.SetActive(true));
				b2_element.m_detailPanel.gameObject.SetActive(false);
			}
		}

		private void SetPanel_State5(Ad_PanelType _pType)
		{
			if(_pType == Ad_PanelType.s5b1)
			{
				//Debug.LogError("마지막 3구간 b1 처리");

				SetPanel_s5b1();

				ContentManager.Instance.Function_S5b1_SetSubPanel();
			}
			else if (_pType == Ad_PanelType.s5b2)
			{
				//Debug.LogError("마지막 3구간 b2 처리");

				ContentManager.Instance._API.RequestHistoryData(GetHistoryTable);
			}
			else if (_pType == Ad_PanelType.s5m1)
			{
				// 타이틀 구간에 코드 작성할것 없음
				int dmgIndex = ContentManager.Instance._Model.DmgData.Count;
				int rcvIndex = ContentManager.Instance._Model.RcvData.Count;
				int reinIndex = 0;

				s5m1_element.m_s5m1_count_dmg.text = dmgIndex.ToString();
				s5m1_element.m_s5m1_count_rcv.text = rcvIndex.ToString();
				s5m1_element.m_s5m1_count_rein.text = reinIndex.ToString();
			}
		}

		public void SetSubPanel_s5b1()
		{
			SetPanel_s5b1();
		}

		private void SetPanel_s5b1()
		{
			List<Issue> dmgs = ContentManager.Instance._Model.DmgData;
			List<Issue> rcvs = ContentManager.Instance._Model.RcvData;

			int allDmg = dmgs.Count;
			int allRcv = rcvs.Count;

			int dmg1 = dmgs.FindAll(x => x.IssueCode == IssueCodes.Crack).Count;
			int rcv1 = rcvs.FindAll(x => x.IssueCode == IssueCodes.Crack).Count;

			int dmg2 = dmgs.FindAll(x => x.IssueCode == IssueCodes.Spalling).Count;
			int rcv2 = rcvs.FindAll(x => x.IssueCode == IssueCodes.Spalling).Count;

			int dmg3 = dmgs.FindAll(x => x.IssueCode == IssueCodes.Efflorescence).Count;
			int rcv3 = rcvs.FindAll(x => x.IssueCode == IssueCodes.Efflorescence).Count;

			int dmg4 = dmgs.FindAll(x => x.IssueCode == IssueCodes.breakage).Count;
			int rcv4 = rcvs.FindAll(x => x.IssueCode == IssueCodes.breakage).Count;

			s5b1_element.m_s5b1_allDmg.text = allDmg.ToString();
			s5b1_element.m_s5b1_allRcv.text = allRcv.ToString();

			s5b1_element.m_s5b1_1crack_dmg.text = dmg1.ToString();
			s5b1_element.m_s5b1_1crack_rcv.text = rcv1.ToString();

			s5b1_element.m_s5b1_2spalling_dmg.text = dmg2.ToString();
			s5b1_element.m_s5b1_2spalling_rcv.text = rcv2.ToString();

			s5b1_element.m_s5b1_3Efflorence_dmg.text = dmg3.ToString();
			s5b1_element.m_s5b1_3Efflorence_rcv.text = rcv3.ToString();

			s5b1_element.m_s5b1_4Breakage_dmg.text = dmg4.ToString();
			s5b1_element.m_s5b1_4Breakage_rcv.text = rcv4.ToString();
		}

		private string GetPartName(string _value)
		{
			string result = _value;

			PlatformCode pCode = MainManager.Instance.Platform;
			if (Platforms.IsTunnelPlatform(pCode))
			{
				result = Platform.Tunnel.Tunnels.GetName(_value);
			}
			else if (Platforms.IsBridgePlatform(pCode))
			{
				result = Platform.Bridge.Bridges.GetName(_value);
			}

			return result;
		}

		/// <summary>
		/// Root UI에서 사용자의 이름을 할당한다.
		/// </summary>
		/// <param name="_name"></param>
		public void SetIssueUserName(string _name)
		{
			if(m_pid == Ad_PanelType.b2)
			{
				m_user.text = _name;
			}
		}

	}
}
