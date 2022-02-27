using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using Definition;
	using Definition._Issue;
	using View;

	public partial class Module_Model : AModule
	{
		[Header("Issue Datas")]
		[SerializeField] private List<Issue> m_Dmg;
		[SerializeField] private List<Issue> m_Rcv;

		[SerializeField] private List<GameObject> m_dmgObjs;
		[SerializeField] private List<GameObject> m_rcvObjs;

		public List<Issue> DmgData { get => m_Dmg; set => m_Dmg=value; }
		public List<Issue> RcvData { get => m_Rcv; set => m_Rcv=value; }

		public List<GameObject> DmgObjs { get => m_dmgObjs; set => m_dmgObjs=value; }
		public List<GameObject> RcvObjs { get => m_rcvObjs; set => m_rcvObjs=value; }

		public void GetIssue(WebType _webT, List<Issue> _issues)
		{
			switch(_webT)
			{
				case WebType.Issue_Dmg:
					m_Dmg = _issues;
					break;

				case WebType.Issue_Rcv:
					m_Rcv = _issues;
					break;
			}

			m_dmgObjs = new List<GameObject>();
			m_rcvObjs = new List<GameObject>();

			InitIssues(_webT, _issues);
		}

		private void InitIssues(WebType _webT, List<Issue> _issues)
		{
			_issues.ForEach(x =>
			{
				GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				obj.name = x.IssueOrderCode;
				obj.transform.position = x.PositionVector;
				obj.transform.rotation = Quaternion.identity;
				obj.AddComponent<Issue_Selectable>().Issue = x;

				// todo 0228 색상값도 고민해야함.

				if(_webT == WebType.Issue_Dmg)
				{
					m_dmgObjs.Add(obj);
				}
				else if(_webT == WebType.Issue_Rcv)
				{
					m_rcvObjs.Add(obj);
				}
			});
		}
	}
}
