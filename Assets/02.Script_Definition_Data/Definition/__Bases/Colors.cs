using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Colors
	{
		public static Color Set(ColorType type, float _alpha)
		{
			Color color = default(Color);

			switch(type)
			{
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

				case ColorType.UI_Ad_Default:
					color = new Color(0x55/255f, 0x55/255f, 0x55/255f, _alpha);
					break;

				case ColorType.UI_Ad_Highlight:
					color = new Color(0xff/255f, 0xff/255f, 0x00/255f, _alpha);
					break;
			}

			return color;
		}
	}
}
