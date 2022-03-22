using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.UI
{
	using Definition;
	using Definition._Issue;
	using System.Linq;

	public class Ad_Panel_Detail : MonoBehaviour
	{
		[SerializeField] Ad_Panel_ElementType m_peType;

		[SerializeField] S3_B2_Panel_Detail s3b2_panelDetail;

		private Ad_PanelType m_pType;
		private UIEventType m_uType;

		public void SetPanel(Ad_PanelType _pType, UIEventType _uType)
		{
			m_pType = _pType;
			m_uType = _uType;

			if(m_uType == UIEventType.Ad_nav_state3)
			{
				if(m_pType == Ad_PanelType.b2)
				{
					GetPanelData_s3_b2();
				}
			}
			//else if(m_uType == UIEventType.Ad_nav_state4)
			//{
			//	if (m_pType == Ad_PanelType.b2)
			//	{
			//		ToggleObject(false);
			//	}
			//}
		}

		private void ToggleObject(bool isOn)
		{
			gameObject.SetActive(isOn);
		}

		private void GetPanelData_s3_b2()
		{
			GameObject selected = ContentManager.Instance._SelectedObj;
			List<Issue> dmgList = ContentManager.Instance._Model.DmgData;

			List<Issue> correctIssues = new List<Issue>();

			foreach (Issue issue in dmgList)
			{
				if (selected.name == issue.CdBridgeParts)
				{
					correctIssues.Add(issue);
				}
			}

			SetPanelDatas(correctIssues);
		}

		private void SetPanelDatas(List<Issue> _issues)
		{
			if(_issues.Count == 0)
			{
				// 비활성
				SetPanelData(null);
			}
			else
			{
				// 활성
				Issue issue = _issues.First();

				SetPanelData(issue);
			}
		}

		public void SetPanelData(Issue _issue)
		{
			if(_issue == null)
			{
				s3b2_panelDetail.m_user.text = "";
				s3b2_panelDetail.m_partName.text = "선택된 정보 없음";
				s3b2_panelDetail.m_width.text = "0000";
				s3b2_panelDetail.m_vertical.text = "0000";
				s3b2_panelDetail.m_depth.text = "0000";
				s3b2_panelDetail.m_description.text = "---";
			}
			else
			{
				s3b2_panelDetail.m_user.text = _issue.NmUser;
				s3b2_panelDetail.m_partName.text = _issue.__PartName;
				s3b2_panelDetail.m_width.text = _issue.Width;
				s3b2_panelDetail.m_vertical.text = _issue.Height;
				s3b2_panelDetail.m_depth.text = _issue.Depth;
				s3b2_panelDetail.m_description.text = _issue.DmgDescription;
			}
		}
	}
}
