using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
	using Definition._Issue;
	using Management;

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

		public override bool IsInteractable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

		public override void OnChangeValue(float _value)
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect()
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect<T>(T t)
		{
			throw new System.NotImplementedException();
		}

		public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
		{
			throw new System.NotImplementedException();
		}

		public override void OnSelect()
		{
			// 이슈정보 선택 처리
			Debug.LogError("Issue Selectable Onselect");
		}
	}
}
