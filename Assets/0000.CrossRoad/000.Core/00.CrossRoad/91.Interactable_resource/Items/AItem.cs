using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
	[System.Serializable]
	public abstract class AItem : MonoBehaviour, IItem
	{
		/// <summary>
		/// ���� ������Ʈ
		/// </summary>
		/// <param name="_mList"></param>
		public abstract void UpdateState(List<ModuleCode> _mList);

		/// <summary>
		/// ���̵� ����
		/// </summary>
		/// <param name="_target"></param>
		/// <param name="_baseAngle"></param>
		/// <param name="_uType"></param>
		public virtual void SetGuide(GameObject _target, Vector3 _baseAngle, UIEventType _uType) { }
	}
}
