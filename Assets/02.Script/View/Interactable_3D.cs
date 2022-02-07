using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	public abstract class Interactable_3D : MonoBehaviour, IInteractable
	{
		/// <summary>
		/// 인스턴스 선택시 실행
		/// </summary>
		public abstract void OnSelect();
		//{
		//	Debug.Log($"[{this.GetType().BaseType.Name}] [{this.name}] Selected");
		//}

		/// <summary>
		/// 인스턴스 선택 해제시 실행
		/// </summary>
		public abstract void OnDeselect();
		//{
		//	Debug.Log($"[{this.GetType().BaseType.Name}] [{this.name}] DeSelected");
		//}
	}
}
