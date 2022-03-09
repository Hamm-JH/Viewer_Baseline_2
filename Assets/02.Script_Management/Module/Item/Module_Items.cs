using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Item
{
	public partial class Module_Items : AModule
	{
		Items.Controller_LocationGuide m_guide;

		public override void OnStart()
		{
			if(m_currentFunction == Definition.FunctionCode.Item_LocationGuide)
			{
				GameObject obj = Instantiate<GameObject>(ItemList.Load(m_currentFunction));
				m_guide = obj.GetComponent<Items.Controller_LocationGuide>();
			}
			
			// TODO 0309 内靛 府叼泛记 沥府
		}

		// Start is called before the first frame update
		//void Start()
		//{
		//	OnStart();
		//}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
