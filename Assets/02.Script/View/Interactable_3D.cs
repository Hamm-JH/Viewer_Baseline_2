using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
	public abstract class Interactable_3D : MonoBehaviour, IInteractable
	{
		/// <summary>
		/// �ν��Ͻ� ���ý� ����
		/// </summary>
		public abstract void OnSelect();
		//{
		//	Debug.Log($"[{this.GetType().BaseType.Name}] [{this.name}] Selected");
		//}

		/// <summary>
		/// �ν��Ͻ� ���� ������ ����
		/// </summary>
		public abstract void OnDeselect();
		//{
		//	Debug.Log($"[{this.GetType().BaseType.Name}] [{this.name}] DeSelected");
		//}
	}
}
