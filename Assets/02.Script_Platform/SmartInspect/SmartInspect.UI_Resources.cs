using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
	using UnityEngine.UI;

	[System.Serializable]
	public class RImage
	{
		public Sprite sprite_normal;
		public Sprite sprite_selected;

		public Color color_normal;
		public Color color_selected;

		public void SetImage(Image _source, bool isSelected)
		{
			if(isSelected)
			{
				_source.sprite = sprite_selected;
				_source.color = color_selected;
			}
			else
			{
				_source.sprite = sprite_normal;
				_source.color = color_normal;
			}
		}
	}

	/// <summary>
	/// SmartInspect UI Resources
	/// </summary>
	[System.Serializable]
	public class UIResources
	{
		public Resource_ProcessMenu m_processMenu;
	}

	[System.Serializable]
	public class Resource_ProcessMenu
	{
		[Header("�ü��� �ջ� Left bar")]
		public RImage m1_bgImage;
		public RImage m1_mainImage;
		public RImage m1_barImage;

		[Header("�ü��� ���� Left bar")]
		public RImage m2_bgImage;
		public RImage m2_mainImage;
		public RImage m2_barImage;

		[Header("�������� �̷� Left bar")]
		public RImage m3_bgImage;
		public RImage m3_mainImage;
		public RImage m3_barImage;

		public void SetImage(int index,
			Image m1_bg, Image m1_main, Image m1_side,
			Image m2_bg, Image m2_main, Image m2_side,
			Image m3_bg, Image m3_main, Image m3_side)
		{
			// -1 all false
			// 0 :: 1 true
			// 1 :: 2 true
			// 2 :: 3 true
			bool select1 = index == 0 ? true : false;
			bool select2 = index == 1 ? true : false;
			bool select3 = index == 2 ? true : false;

			m1_bgImage.SetImage(m1_bg, select1);
			m1_mainImage.SetImage(m1_main, select1);
			m1_barImage.SetImage(m1_side, select1);

			m2_bgImage.SetImage(m2_bg, select2);
			m2_mainImage.SetImage(m2_main, select2);
			m2_barImage.SetImage(m2_side, select2);

			m3_bgImage.SetImage(m3_bg, select3);
			m3_mainImage.SetImage(m3_main, select3);
			m3_barImage.SetImage(m3_side, select3);
		}
	}
}
