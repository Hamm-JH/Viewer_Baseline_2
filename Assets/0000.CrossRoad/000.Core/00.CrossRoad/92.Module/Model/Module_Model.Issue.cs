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
		[SerializeField] private List<Issue> m_allIssues;

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
		public List<Issue> AllIssues { get => m_allIssues; set => m_allIssues = value; }

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

		/// <summary>
		/// 손상 정보 삭제
		/// </summary>
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

		/// <summary>
		/// 손상정보 리스트 가져오기
		/// </summary>
		/// <param name="_webT">웹 분류</param>
		/// <param name="_issues">손상정보 리스트</param>
		public void GetIssue(WebType _webT, List<Issue> _issues)
		{
			if (m_allIssues == null) m_allIssues = new List<Issue>();

			List<GameObject> _iObjs = new List<GameObject>();

			bool isDmg = false;
			// TODO :: ! :: 손상 한번, 보수 한번을 전제로 작성된 코드
			switch (_webT)
			{
				case WebType.Issue_Dmg:
					m_Dmg = _issues;
					m_dmgObjs = new List<GameObject>();
					_iObjs = m_dmgObjs;
					// 모든 점검정보에 추가
					m_allIssues.AddRange(m_Dmg);
					isDmg = true;
					break;

				case WebType.Issue_Rcv:
					m_Rcv = _issues;
					m_rcvObjs = new List<GameObject>();
					_iObjs = m_rcvObjs;
					// 모든 점검정보에 추가
					m_allIssues.AddRange(m_Rcv);
					isDmg = false;
					break;
			}

			InitIssues(_webT, _issues);

			SetIssuesInRoot(_iObjs);

			CollectAllIssues(IssueObjs, _iObjs);
		}

		/// <summary>
		/// 루트 손상정보 가져오기
		/// </summary>
		/// <param name="_root">루트 손상정보 객체</param>
		private void InitRootIssue(out GameObject _root)
		{
			GameObject obj = new GameObject("root issue");
			obj.transform.position = default(Vector3);
			obj.transform.rotation = Quaternion.identity;

			_root = obj;
		}

		/// <summary>
		/// 손상정보 생성
		/// </summary>
		/// <param name="_webT">웹 분류</param>
		/// <param name="_issues">손상정보 리스트</param>
		private void InitIssues(WebType _webT, List<Issue> _issues)
		{
			_issues.ForEach(x =>
			{
				int _iIndex = MainManager.Instance.Test_IsIssueDecal ? 1 : 0;
				GameObject obj = Issues.CreateIssue(_iIndex, _webT, x);

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
			});
		}

		/// <summary>
		/// 루트 객체 안에서 손상정보 리스트 생성
		/// </summary>
		/// <param name="_issues"></param>
		private void SetIssuesInRoot(List<GameObject> _issues)
		{
			_issues.ForEach(x => x.transform.SetParent(RootIssue.transform));
		}

		/// <summary>
		/// 모든 손상정보 리스트를 수집
		/// </summary>
		/// <param name="_target">손상정보 리스트</param>
		/// <param name="_iObjs"></param>
		private void CollectAllIssues(List<GameObject> _target, List<GameObject> _iObjs)
		{
			_iObjs.ForEach(x => _target.Add(x));
		}

		#endregion

	}
}
