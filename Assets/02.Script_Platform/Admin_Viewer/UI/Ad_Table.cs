using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.UI
{
	using Definition;
	using Definition._Issue;
	using Management;

	public class Ad_Table : MonoBehaviour
	{
		public GameObject sub_s2_bm;
		public GameObject sub_s3_b1;
		public GameObject sub_s4_b1;
		public GameObject sub_s4_b2;
		public GameObject sub_s5_m1_dmg;
		public GameObject sub_s5_m1_rcv;
		public GameObject sub_s5_m1_rein;

		public GameObject rContent;

		public List<Ad_TableElement> tElems;

		private Ad_PanelType m_pType;
		private UIEventType m_uType;

		//------------------------------------------------------------------------------------------------------------------------------------------------

		#region Set Table

		/// <summary>
		/// 타입코드에 따라 이벤트 분기
		/// </summary>
		/// <param name="_pType"></param>
		/// <param name="_uType"></param>
		public void SetTable(Ad_PanelType _pType, UIEventType _uType)
		{
			m_pType = _pType;
			m_uType = _uType;

			if(m_uType == UIEventType.Ad_nav_state2)
			{
				if(m_pType == Ad_PanelType.bm)
				{
					SetTable_s2_bm();
				}
				else
				{
					ToggleObject(false);
				}
			}
			else if(m_uType == UIEventType.Ad_nav_state3)
			{
				if(m_pType == Ad_PanelType.b1)
				{
					SetTable_s3_b1();
				}
				else
				{
					ToggleObject(false);
				}
			}
			else if(m_uType == UIEventType.Ad_nav_state4)
			{
				if(m_pType == Ad_PanelType.b1)
				{
					SetTable_s4_b1();
				}
				else if(m_pType == Ad_PanelType.b2)
				{
					SetTable_s4_b2();
				}
				else
				{
					ToggleObject(false);
				}
			}
			else if(m_uType == UIEventType.Ad_nav_state5)
			{
				if(m_pType == Ad_PanelType.s5m1)
				{
					SetTable_s5_s5m1();
				}
				else if(m_pType == Ad_PanelType.s5b1)
				{
					// 테이블 요소가 아니면 여기서 실행 안함
					Debug.LogError("테이블 요소가 아니면 여기서 실행 안함");
				}
				else if(m_pType == Ad_PanelType.s5b2)
				{
					// 테이블 요소가 아니면 여기서 실행 안함
					Debug.LogError("테이블 요소가 아니면 여기서 실행 안함");
				}
				else
				{
					ToggleObject(false);
				}
			}

			// 위의 모든 경우에 걸리지 않으면 테이블 비활성
			//gameObject.SetActive(false);
		}

		private void ToggleObject(bool isOn)
		{
			gameObject.SetActive(isOn);
		}

		/// <summary>
		/// 상태2번 bm패널
		/// </summary>
		public void SetTable_s2_bm()
		{
			Debug.Log("SetTable_s2_bm");
			ToggleObject(true);


			sub_s2_bm.SetActive(true);
			sub_s3_b1.SetActive(false);
			sub_s4_b1.SetActive(false);
			sub_s4_b2.SetActive(false);
			sub_s5_m1_dmg .SetActive(false);
			sub_s5_m1_rcv .SetActive(false);
			sub_s5_m1_rein.SetActive(false);

			GetTableData_s2_bm();
		}

		/// <summary>
		/// 상태3번 b1패널
		/// </summary>
		public void SetTable_s3_b1()
		{
			Debug.Log("SetTable_s3_b1");
			ToggleObject(true);

			sub_s2_bm.SetActive(false);
			sub_s3_b1.SetActive(true);
			sub_s4_b1.SetActive(false);
			sub_s4_b2.SetActive(false);
			sub_s5_m1_dmg.SetActive(false);
			sub_s5_m1_rcv.SetActive(false);
			sub_s5_m1_rein.SetActive(false);

			GetTableData_s3_b1();
		}

		/// <summary>
		/// 상태4번 b1패널
		/// </summary>
		public void SetTable_s4_b1()
		{
			Debug.Log("SetTable_s4_b1");
			ToggleObject(true);

			sub_s2_bm.SetActive(false);
			sub_s3_b1.SetActive(false);
			sub_s4_b1.SetActive(true);
			sub_s4_b2.SetActive(false);
			sub_s5_m1_dmg.SetActive(false);
			sub_s5_m1_rcv.SetActive(false);
			sub_s5_m1_rein.SetActive(false);

			GetTableData_s4_b1();
		}

		/// <summary>
		/// 상태4번 b2패널
		/// </summary>
		public void SetTable_s4_b2()
		{
			Debug.Log("SetTable_s4_b2");
			ToggleObject(true);

			sub_s2_bm.SetActive(false);
			sub_s3_b1.SetActive(false);
			sub_s4_b1.SetActive(false);
			sub_s4_b2.SetActive(true);
			sub_s5_m1_dmg.SetActive(false);
			sub_s5_m1_rcv.SetActive(false);
			sub_s5_m1_rein.SetActive(false);

			GetTableData_s4_b2();
		}

		/// <summary>
		/// 상태5번 s5m1패널
		/// </summary>
		public void SetTable_s5_s5m1()
		{
			Debug.Log("SetTable_s5_s5m1");
			ToggleObject(true);

			sub_s2_bm.SetActive(false);
			sub_s3_b1.SetActive(false);
			sub_s4_b1.SetActive(false);
			sub_s4_b2.SetActive(false);
			sub_s5_m1_dmg.SetActive(true);
			sub_s5_m1_rcv.SetActive(false);
			sub_s5_m1_rein.SetActive(false);

			Debug.LogError("St5 s5m1 테이블 할당");
			TestData();
		}

		#endregion

		#region Get Table Data

		private void GetTableData_s2_bm()
		{
			List<Definition._Issue.Issue> dmgList = ContentManager.Instance._Model.DmgData;

			SetTableData(dmgList);
		}

		private void GetTableData_s3_b1()
		{
			GameObject selected = ContentManager.Instance._SelectedObj;
			List<Issue> dmgList = ContentManager.Instance._Model.DmgData;

			List<Issue> correctIssues = new List<Issue>();

			foreach(Issue issue in dmgList)
			{
				if(selected.name == issue.CdBridgeParts)
				{
					correctIssues.Add(issue);
				}
			}

			SetTableData(correctIssues);
		}

		private void GetTableData_s4_b1()
		{
			GameObject selected = ContentManager.Instance._SelectedObj;
			List<Issue> rcvList = ContentManager.Instance._Model.RcvData;

			List<Issue> correctIssues = new List<Issue>();

			foreach (Issue issue in rcvList)
			{
				if (selected.name == issue.CdBridgeParts)
				{
					correctIssues.Add(issue);
				}
			}

			SetTableData(correctIssues);
		}

		private void GetTableData_s4_b2()
		{
			SetTableData(new List<Issue>());
		}

		#endregion

		#region Set Table Data

		private void SetTableData(List<Issue> _datas)
		{
			Ad_PanelType pType = m_pType;
			UIEventType uType = m_uType;

			ClearElement();

			int index = _datas.Count;
			for (int i = 0; i < index; i++)
			{
				AddElement(pType, uType, i, _datas[i]);
			}
		}

		#endregion

		private void TestData()
		{
			List<string> datas = new List<string>();
			datas.Add("1");
			datas.Add("2");
			datas.Add("3");
			datas.Add("4");
			datas.Add("5");
			datas.Add("6");
			datas.Add("7");

			ClearElement();

			foreach (var data in datas)
			{
				AddElement(data);
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------------------------

		private void ClearElement()
		{
			Transform rElem = rContent.transform;

			int index = rElem.childCount;
			for (int i = 0; i < index; i++)
			{
				Destroy(rElem.GetChild(i).gameObject);
			}

			if (tElems == null) tElems = new List<Ad_TableElement>();
			tElems.Clear();
		}

		private void AddElement(Ad_PanelType _pType, UIEventType _uType, int _index, Issue _data)
		{
			GameObject elem = Instantiate<GameObject>(Resources.Load<GameObject>("UI/UIElement/ContentElements"), rContent.transform);
			Ad_TableElement elemCode = elem.GetComponent<Ad_TableElement>();

			elemCode.SetTableElement(_pType, _uType, _index, _data);

			tElems.Add(elemCode);
		}

		public void AddElement(string _data)
		{
			GameObject elem = Instantiate<GameObject>(Resources.Load<GameObject>("UI/UIElement/ContentElements"), rContent.transform);
			Ad_TableElement elemCode = elem.GetComponent<Ad_TableElement>();

			// TODO :: 요소 코드 동작 수행
			elemCode.SetTableElement(m_pType, m_uType);

			tElems.Add(elemCode);
		}

		public void AddElement(/* 필요 데이터 */)
		{
			GameObject elem = Instantiate<GameObject>(Resources.Load<GameObject>("UI/UIElement/ContentElements"), rContent.transform);
			Ad_TableElement elemCode = elem.GetComponent<Ad_TableElement>();

			// TODO :: 요소 코드 동작 수행

			tElems.Add(elemCode);
		}

		public void ClearElements()
		{
			tElems.ForEach(x => Destroy(x));
			tElems.Clear();
		}
	}
}
