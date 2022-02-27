using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
	using Definition._Issue;

	public class Issue_Selectable : Interactable
	{
		[SerializeField] Issue m_issue;

		public Issue Issue { get => m_issue; set => m_issue=value; }

		public override GameObject Target => throw new System.NotImplementedException();

		public override List<GameObject> Targets => throw new System.NotImplementedException();

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
			throw new System.NotImplementedException();
		}
	}
}
