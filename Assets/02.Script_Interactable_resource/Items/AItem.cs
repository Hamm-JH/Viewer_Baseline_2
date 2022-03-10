using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
	[System.Serializable]
	public abstract class AItem : MonoBehaviour, IItem
	{
		public abstract void UpdateState(List<ModuleCode> _mList);

		public virtual void SetGuide(GameObject _target, Vector3 _baseAngle, UIEventType _uType) { }
	}
}
