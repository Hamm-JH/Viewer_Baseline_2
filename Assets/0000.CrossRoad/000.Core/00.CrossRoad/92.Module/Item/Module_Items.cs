using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Item
{
	using Definition;
	using Items;

	public partial class Module_Items : AModule
	{
		[SerializeField] List<AItem> m_itemList;


		Items.Controller_LocationGuide m_guide;
		Items.Controller_Compass m_compass;
		
	}
}
