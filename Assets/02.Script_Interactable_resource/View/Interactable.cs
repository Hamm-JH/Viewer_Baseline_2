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

		public abstract GameObject Target { get; }
		public RaycastHit Hit { get => hit; set => hit = value; }

		/// <summary>
		/// 인스턴스 선택시 실행
		/// </summary>
		public abstract void OnSelect();

		/// <summary>
		/// 인스턴스 선택 해제시 실행
		/// </summary>
		public abstract void OnDeselect();

		public abstract void OnChangeValue(float _value);
	}
}
