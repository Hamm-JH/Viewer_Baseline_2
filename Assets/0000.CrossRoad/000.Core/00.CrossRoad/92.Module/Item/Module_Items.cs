﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Item
{
	using Definition;
	using Items;

	public partial class Module_Items : AModule
	{
		/// <summary>
		/// 아이템 리스트
		/// </summary>
		[SerializeField] List<AItem> m_itemList;


		Items.Controller_LocationGuide m_guide;
		Items.Controller_Compass m_compass;

		public Items.Controller_Compass Compass
        {
			get { return m_compass; }
        }
		
	}
}
