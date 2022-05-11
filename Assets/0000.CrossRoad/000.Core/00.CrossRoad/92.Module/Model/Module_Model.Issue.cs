using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using Definition;
	using Definition._Issue;
	using Management;
	using View;

	public partial class Module_Model : AModule
	{
		private GameObject m_rootIssue;
		[Header("Issue Datas")]
		[SerializeField] private List<Issue> m_Dmg;
		[SerializeField] private List<Issue> m_Rcv;

		[SerializeField] private List<GameObject> m_dmgObjs;
		[SerializeField] private List<GameObject> m_rcvObjs;
		[SerializeField] private List<GameObject> m_issueObjs;

		public GameObject RootIssue 
		{ 
			get
			{
				if(m_rootIssue == null)
				{
					InitRootIssue(out m_rootIssue);
				}

				return m_rootIssue;
			}
		}
		public List<Issue> DmgData { get => m_Dmg; set => m_Dmg=value; }
		public List<Issue> RcvData { get => m_Rcv; set => m_Rcv=value; }

		public List<GameObject> DmgObjs { get => m_dmgObjs; set => m_dmgObjs=value; }
		public List<GameObject> RcvObjs { get => m_rcvObjs; set => m_rcvObjs=value; }
		public List<GameObject> IssueObjs 
		{ 
			get
			{
				if(m_issueObjs == null)
				{
					m_issueObjs = new List<GameObject>();
				}
				return m_issueObjs;
			}
		}

		#region Delete Issue

		public void DeleteIssues()
		{
			DmgData.Clear();
			RcvData.Clear();
			DmgObjs.Clear();
			RcvObjs.Clear();
			IssueObjs.ForEach(x => Destroy(x));
			IssueObjs.Clear();
		}

		#endregion

		#region Get Issue

		public void GetIssue(WebType _webT, List<Issue> _issues)
		{
			List<GameObject> _iObjs = new List<GameObject>();

			bool isDmg = false;
			switch (_webT)
			{
				case WebType.Issue_Dmg:
					m_Dmg = _issues;
					m_dmgObjs = new List<GameObject>();
					_iObjs = m_dmgObjs;
					isDmg = true;
					break;

				case WebType.Issue_Rcv:
					m_Rcv = _issues;
					m_rcvObjs = new List<GameObject>();
					_iObjs = m_rcvObjs;
					isDmg = false;
					break;
			}

			InitIssues(_webT, _issues);

			SetIssuesInRoot(_iObjs);

			CollectAllIssues(IssueObjs, _iObjs);

			// TODO 야매 compcheck 1, 2
			if(isDmg)
			{
				ContentManager.Instance.CompCheck(1);
			}
			else
			{
				ContentManager.Instance.CompCheck(2);
			}
		}

		private void InitRootIssue(out GameObject _root)
		{
			GameObject obj = new GameObject("root issue");
			obj.transform.position = default(Vector3);
			obj.transform.rotation = Quaternion.identity;

			_root = obj;
		}

		private void InitIssues(WebType _webT, List<Issue> _issues)
		{
			_issues.ForEach(x =>
			{
				GameObject obj = Issues.CreateIssue(1, _webT, x);
				//GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
				//obj.name = x.IssueOrderCode;
				//obj.transform.position = x.PositionVector;
				//obj.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 0));
				//obj.AddComponent<Issue_Selectable>().Issue = x;

				// todo 0228 색상값도 고민해야함.

				MaterialType mType = MaterialType.Issue;
				if(_webT == WebType.Issue_Dmg)
				{
					m_dmgObjs.Add(obj);
					mType = MaterialType.Issue_dmg;
				}
				else if(_webT == WebType.Issue_Rcv)
				{
					m_rcvObjs.Add(obj);
					mType = MaterialType.Issue_rcv;
				}

				//MeshRenderer render;
				//if(obj.TryGetComponent<MeshRenderer>(out render))
				//{
				//	render.material = Materials.Set(mType);
				//	// TODO 0228 :: 손상 변환테이블 만들고 추후 적용
				//	Textures.Set(render, TextureType.crack);
				//}
			});
		}

		private void SetIssuesInRoot(List<GameObject> _issues)
		{
			_issues.ForEach(x => x.transform.SetParent(RootIssue.transform));
		}

		private void CollectAllIssues(List<GameObject> _target, List<GameObject> _iObjs)
		{
			_iObjs.ForEach(x => _target.Add(x));
		}

		#endregion

	}
}
