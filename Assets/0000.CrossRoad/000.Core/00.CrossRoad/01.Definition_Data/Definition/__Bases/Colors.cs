using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Colors
	{
		/// <summary>
		/// 기존의 색을 유지하고, 투명도만 조정한다.
		/// </summary>
		/// <param name="type">색상 타입</param>
		/// <param name="_tgColor">전처리된 색상</param>
		/// <param name="_alpha">반투명도 값</param>
		/// <returns></returns>
		public static Color Set(ColorType type, Color _tgColor, float _alpha)
        {
			Color color = _tgColor;

			color = new Color(color.r, color.g, color.b, _alpha);

			return color;
        }

		/// <summary>
		/// 설정된 색을 사용한다.
		/// </summary>
		/// <param name="type">색상 타입</param>
		/// <param name="_alpha">반투명도 값</param>
		/// <returns></returns>
		public static Color Set(ColorType type, float _alpha)
		{
			Color color = default(Color);

			switch(type)
			{
				case ColorType.White:
					color = new Color(1, 1, 1, _alpha);
					break;

				case ColorType.Default1:
					color = new Color(0x33/255f, 0x7e/255f, 0xf2/255f, _alpha);
					//color = new Color(0xFF/255f, 0xC5/255f, 0x00/255f, _alpha);
					break;

				case ColorType.Selected1:
					color = new Color(0xff/255f, 0x7c/255f, 0x2d/255f, _alpha);
					//color = new Color(0x00/255f, 0xff/255f, 0x00/255f, _alpha);
					break;

				case ColorType.UI_Default:
					color = new Color(0x55/255f, 0x55/255f, 0x55/255f, _alpha);
					break;

				case ColorType.UI_Highlight:
					color = new Color(0xff/255f, 0xff/255f, 0x00/255f, _alpha);
					break;

				case ColorType.UI_Ad_Img_Default:
					color = new Color(0x30/255f, 0x3b/255f, 0x4e/255f, _alpha);
					break;

				case ColorType.UI_Ad_Img_Highlight:
					color = new Color(0x2b/255f, 0x70/255f, 0xc6/255f, _alpha);
					break;

				case ColorType.UI_Ad_Txt_Default:
					color = new Color(0xe8/255f, 0xea/255f, 0xee/255f, _alpha);
					break;

				case ColorType.UI_Ad_Txt_Highlight:
					color = new Color(0xc4/255f, 0xd8/255f, 0xf1/255f, _alpha);
					break;

				case ColorType.UI_dmg:
					color = new Color(0x255/255f, 0x00/255f, 0x00/255f, _alpha);
					break;

				case ColorType.UI_rcv:
					color = new Color(0x00/255f, 0x00/255f, 0x255/255f, _alpha);
					break;

				case ColorType.UI_rein:
					color = new Color(0x00/255f, 0x255/255f, 0x00/255f, _alpha);
					break;
			}

			return color;
		}
	}
}