using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	public abstract class Interactable : MonoBehaviour, IInteractable
	{
		private RaycastHit hit;
		[SerializeField] protected bool m_isInteractable;
		public RaycastHit Hit { get => hit; set => hit = value; }

		public abstract GameObject Target { get; }
		public abstract List<GameObject> Targets { get; }
		public abstract bool IsInteractable { get; set; }

		/// <summary>
		/// 인스턴스 선택시 실행
		/// </summary>
		public abstract void OnSelect();

		/// <summary>
		/// 인스턴스 선택 해제시 실행
		/// </summary>
		public abstract void OnDeselect();

		public abstract void OnDeselect<T>(T t);

		public abstract void OnDeselect<T1, T2>(T1 t1, T2 t2);

		public abstract void OnChangeValue(float _value);
	}
}
