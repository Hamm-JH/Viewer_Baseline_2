using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Colors
	{
		public static Color Set(ColorType type)
		{
			Color color = default(Color);

			switch(type)
			{
				case ColorType.Default1:
					color = new Color(0xFF/255f, 0xC5/255f, 0x00/255f, 1);
					break;

				case ColorType.Selected1:
					color = new Color(0x00/255f, 0xff/255f, 0x00/255f, 1);
					break;
			}

			return color;
		}
	}
}
