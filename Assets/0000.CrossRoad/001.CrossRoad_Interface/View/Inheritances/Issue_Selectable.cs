using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
	using Definition;
	using Definition._Issue;
    using Items;
    using Management;
	using System;

	public class Issue_Selectable : Interactable
	{
		[SerializeField] Issue m_issue;

		public Issue Issue { get => m_issue; set => m_issue=value; }

		public override GameObject Target => gameObject;

		public override List<GameObject> Targets
		{
			get
			{
				string _name = gameObject.name;

				//List<GameObject> lst = ContentManager.Instance._ModelObjects;

				List<GameObject> result = new List<GameObject>();

				return result;
			}
		}

		/// <summary>
		/// 값 변경
		/// </summary>
		/// <param name="_value"></param>
		/// <exception cref="System.NotImplementedException"></exception>
        public override void OnChangeValue(float _value)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 선택 해제
		/// </summary>
		public override void OnDeselect()
		{
			Debug.Log("Issue OnDeselect");

		}

		/// <summary>
		/// 선택 해제
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="t1"></param>
		/// <param name="t2"></param>
		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{
			if (!IsInteractable) return;
			Debug.Log("Issue OnDeselect");

			UIEventType type;

			if(t1 is UIEventType && t2 is float)
			{
				type = (UIEventType)Enum.ToObject(typeof(UIEventType), t1);
				float value = (float)(object)t2;
				PlatformCode pCode = MainManager.Instance.Platform;

				switch(type)
				{
					case UIEventType.Slider_Icon_Scale:
						OnDeselect_IssueScale(pCode, type, value);
						break;
				}
			}
		}

		/// <summary>
		/// 점검정보의 크기 변경
		/// </summary>
		/// <param name="_platform"></param>
		/// <param name="_uiType"></param>
		/// <param name="_value"></param>
		private void OnDeselect_IssueScale(PlatformCode _platform, UIEventType _uiType, float _value)
		{
			if (!IsInteractable) return;

			// 최종 확인
			if (_uiType == UIEventType.Slider_Icon_Scale)
			{
				transform.localScale = Vector3.one * _value;
			}
		}

		/// <summary>
		/// 객체 선택
		/// </summary>
		public override void OnSelect()
		{
			// 이슈정보 선택 처리
			//Debug.LogError("Issue Selectable Onselect");
			Debug.Log("Issue Selectable Onselect");
		}



		/// <summary>
		/// 렌더 스위칭
		/// </summary>
		/// <param name="isOn"></param>
		public void SwitchRender(bool isOn)
		{
			this.GetComponent<MeshRenderer>().enabled = isOn;
			this.GetComponent<Collider>().enabled = isOn;
		}

		/// <summary>
		/// 손상코드 할당
		/// </summary>
		/// <param name="issueCode"></param>
		/// <param name="issueIndex"></param>
		public void SetIssueCode(string issueCode, string issueIndex)
		{
			Issue.IssueCode = (IssueCodes)Enum.Parse(typeof(IssueCodes), issueCode);
		}

		/// <summary>
		/// 손상/보강을 등록할때 등록된 IssueStatus를 확인하고 등록
		/// </summary>
		public void SetMaterial(IssueType issueType = IssueType.Null)
		{
			if (Issue.IssueCode == IssueCodes.Null)
			{
				//transform.GetComponent<MeshRenderer>().material = defaultMaterial;
				return;
			}

			if (issueType == IssueType.damage)
			{
				ContentManager.Instance.SetIssueMaterial(transform.GetComponent<MeshRenderer>(), issueType, Issue.IssueCode);
				//transform.GetComponent<MeshRenderer>().material = MainManager.Instance.DamagedIssueMaterialList[(int)issueData.IssueCode];
			}
			else if (issueType == IssueType.recover)
			{
				ContentManager.Instance.SetIssueMaterial(transform.GetComponent<MeshRenderer>(), issueType, Issue.IssueCode);
				//transform.GetComponent<MeshRenderer>().material = MainManager.Instance.RecoverIssueMaterialList[(int)issueData.IssueCode];
			}
			else
			{
				Debug.LogError("Issues.Entity def mat deprecated");
				//transform.GetComponent<MeshRenderer>().material = defaultMaterial;
			}
		}

		/// <summary>
		/// 재질 할당
		/// </summary>
		/// <param name="mat"></param>
		public void SetMaterial(Material mat)
		{
			transform.GetComponent<MeshRenderer>().material = mat;
		}
	}
}
