using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    using TMPro;
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

	[System.Serializable]
	public class RText
    {
		public Color color_normal;
		public Color color_selected;

		public void SetText(Text _source, bool isSelected)
        {
			if(isSelected)
            {
				_source.color = color_selected;
            }
			else
            {
				_source.color = color_normal;
			}
        }

		public void SetText(TextMeshProUGUI _source, bool isSelected)
        {
			if (isSelected)
			{
				_source.color = color_selected;
			}
			else
			{
				_source.color = color_normal;
			}
		}
    }

    #region --------- main -----------

    /// <summary>
    /// SmartInspect UI Resources
    /// </summary>
    [System.Serializable]
	public class UIResources
	{
		public Resource_ProcessMenu m_processMenu;
		public Resource_LeftBar_Dmg m_dmg_leftbar;
		public Resource_LeftBar_Rcv m_rcv_leftbar;
		public Resource_LeftBar_Adm m_adm_leftbar;
	}

	#endregion --------- main -----------

	[System.Serializable]
	public class Resource_ProcessMenu
	{
		[Header("시설물 손상 Left bar")]
		public RImage m1_bgImage;
		public RImage m1_mainImage;
		public RImage m1_barImage;
		public RText m1_descText;

		[Header("시설물 보수 Left bar")]
		public RImage m2_bgImage;
		public RImage m2_mainImage;
		public RImage m2_barImage;
		public RText m2_descText;

		[Header("유지관리 이력 Left bar")]
		public RImage m3_bgImage;
		public RImage m3_mainImage;
		public RImage m3_barImage;
		public RText m3_descText;

		/// <summary>
		/// 색 변경 :: 다른 개체들과 연동되어 색상이 바뀌는 집합
		/// </summary>
		public void SetImage(int index,
			Image m1_bg, Image m1_main, Image m1_side, TextMeshProUGUI m1_text,
			Image m2_bg, Image m2_main, Image m2_side, TextMeshProUGUI m2_text,
			Image m3_bg, Image m3_main, Image m3_side, TextMeshProUGUI m3_text)
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
			m1_descText.SetText(m1_text, select1);

			m2_bgImage.SetImage(m2_bg, select2);
			m2_mainImage.SetImage(m2_main, select2);
			m2_barImage.SetImage(m2_side, select2);
			m2_descText.SetText(m2_text, select2);

			m3_bgImage.SetImage(m3_bg, select3);
			m3_mainImage.SetImage(m3_main, select3);
			m3_barImage.SetImage(m3_side, select3);
			m3_descText.SetText(m3_text, select3);
		}
	}

	[System.Serializable]
	public class Resource_LeftBar_Dmg
    {
		[Header("Damaged info")]
		public RImage l1_bgImage;
		public RImage l1_mainImage;

		[Header("Damaged List")]
		public RImage l2_bgImage;
		public RImage l2_mainImage;

		[Header("Status Info")]
		public RImage l3_bgImage;
		public RImage l3_mainImage;

		/// <summary>
		/// 각각 독립적으로 색상이 바뀌어야 하는 개체
		/// </summary>
		/// <param name="index"></param>
		/// <param name="img_bg"></param>
		/// <param name="img_main"></param>
		public void SetImage(Image img_bg, Image img_main, int index, bool isOn)
        {
			RImage targetImg_bg = null;
			RImage targetImg_main = null;

			switch(index)
            {
				case 0:
					targetImg_bg = l1_bgImage;
					targetImg_main = l1_mainImage;
					break;

				case 1:
					targetImg_bg = l2_bgImage;
					targetImg_main = l2_mainImage;
					break;

				case 2:
					targetImg_bg = l3_bgImage;
					targetImg_main = l3_mainImage;
					break;
			}

			if (targetImg_bg == null || targetImg_main == null)
            {
				throw new Definition.Exceptions.ImagesNotAssigned();
			}

			targetImg_bg.SetImage(img_bg, isOn);
			targetImg_main.SetImage(img_main, isOn);

		}
	}

	[System.Serializable]
	public class Resource_LeftBar_Rcv
    {
		[Header("Repaired List")]
		public RImage l1_bgImage;
		public RImage l1_mainImage;

		[Header("Reinforced List")]
		public RImage l2_bgImage;
		public RImage l2_mainImage;

		[Header("Status Info")]
		public RImage l3_bgImage;
		public RImage l3_mainImage;

		[Header("Dimension")]
		public RImage l4_bgImage;
		public RImage l4_mainImage;

		[Header("Drawing Print")]
		public RImage l5_bgImage;
		public RImage l5_mainImage;

		public void SetImage(Image img_bg, Image img_main, int index, bool isOn)
        {
			RImage targetImg_bg = null;
			RImage targetImg_main = null;

			switch(index)
            {
				case 0:
					targetImg_bg = l1_bgImage;
					targetImg_main = l1_mainImage;
					break;

				case 1:
					targetImg_bg = l2_bgImage;
					targetImg_main = l2_mainImage;
					break;

				case 2:
					targetImg_bg = l3_bgImage;
					targetImg_main = l3_mainImage;
					break;

				case 3:
					targetImg_bg = l4_bgImage;
					targetImg_main = l4_mainImage;
					break;

				case 4:
					targetImg_bg = l5_bgImage;
					targetImg_main = l5_mainImage;
					break;
			}

			if(targetImg_bg == null || targetImg_main == null)
            {
				throw new Definition.Exceptions.ImagesNotAssigned();
            }

			targetImg_bg.SetImage(img_bg, isOn);
			targetImg_main.SetImage(img_main, isOn);
        }
	}

	[System.Serializable]
	public class Resource_LeftBar_Adm
	{
		[Header("Total Maintenance")]
		public RImage l1_bgImage;
		public RImage l1_mainImage;

		[Header("Maintenance Timeline")]
		public RImage l2_bgImage;
		public RImage l2_mainImage;

		[Header("Status Info")]
		public RImage l3_bgImage;
		public RImage l3_mainImage;

		[Header("Report")]
		public RImage l4_bgImage;
		public RImage l4_mainImage;

		public void SetImage(Image img_bg, Image img_main, int index, bool isOn)
		{
			RImage targetImg_bg = null;
			RImage targetImg_main = null;

			switch (index)
			{
				case 0:
					targetImg_bg = l1_bgImage;
					targetImg_main = l1_mainImage;
					break;

				case 1:
					targetImg_bg = l2_bgImage;
					targetImg_main = l2_mainImage;
					break;

				case 2:
					targetImg_bg = l3_bgImage;
					targetImg_main = l3_mainImage;
					break;

				case 3:
					targetImg_bg = l4_bgImage;
					targetImg_main = l4_mainImage;
					break;
			}

			if (targetImg_bg == null || targetImg_main == null)
			{
				throw new Definition.Exceptions.ImagesNotAssigned();
			}

			targetImg_bg.SetImage(img_bg, isOn);
			targetImg_main.SetImage(img_main, isOn);
		}
	}
}
