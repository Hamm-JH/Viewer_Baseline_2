using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Item
{
	using Definition;

	public partial class Module_Items : AModule
	{
		public void CreateItem(FunctionCode _fCode)
		{
			GameObject obj = null;
			
			// guide 코드 
			if (_fCode == Definition.FunctionCode.Item_LocationGuide)
			{
				obj = Instantiate<GameObject>(ItemList.Load(_fCode));
				m_guide = obj.GetComponent<Items.Controller_LocationGuide>();

				// 관리를 위해 아이템리스트 업데이트
				m_itemList.Add(m_guide);
			}

			// Item들을 이 모듈 아래로 모음
			obj.transform.SetParent(transform);
		}
	}
}
