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
			// �� �������� ���� ������ ����.
			gameObject.SetActive(true);
		}
	}
}
