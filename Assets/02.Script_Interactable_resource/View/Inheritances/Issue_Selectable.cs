using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
	using Definition;
	using Definition._Issue;
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

		//public override bool IsInteractable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

		public override void OnChangeValue(float _value)
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect()
		{
			Debug.Log("Issue OnDeselect");

		}

		public override void OnDeselect<T>(T t)
		{
			Debug.Log("Issue OnDeselect");
		}

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

		// 점검정보의 크기를 변경
		private void OnDeselect_IssueScale(PlatformCode _platform, UIEventType _uiType, float _value)
		{
			if (!IsInteractable) return;

			// 최종 확인
			if (_uiType == UIEventType.Slider_Icon_Scale)
			{
				transform.localScale = Vector3.one * _value;
			}
		}

		public override void OnSelect()
		{
			// 이슈정보 선택 처리
			Debug.LogError("Issue Selectable Onselect");
		}




		public void SwitchRender(bool isOn)
		{
			this.GetComponent<MeshRenderer>().enabled = isOn;
			this.GetComponent<Collider>().enabled = isOn;
		}

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

		public void SetMaterial(Material mat)
		{
			transform.GetComponent<MeshRenderer>().material = mat;
		}
	}
}
