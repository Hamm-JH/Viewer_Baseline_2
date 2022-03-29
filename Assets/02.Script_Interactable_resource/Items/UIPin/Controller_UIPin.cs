using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.UIPin
{
	public class Controller_UIPin : AItem
	{
		public List<GameObject> m_objectList;

		public override void UpdateState(List<ModuleCode> _mList)
		{
			// 이 아이템은 조건 제한이 없음.
			gameObject.SetActive(true);
		}
	}
}
