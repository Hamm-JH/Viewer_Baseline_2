using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Definition.Data;

    [System.Serializable]
    public class UIColor : IData
    {
        public enum ColorTag
        {
            color_default,
            color_hover,
            color_select,
            color_deselect,
        }

        public Color color_default = new Color(0x2b/255f, 0x70/255f, 0xc6/255f, 1);
        public Color color_hover = new Color(0x1a/255f, 0x5f/255f, 0xac/255f, 0.8f);
        public Color color_select = new Color(0x1a / 255f, 0x5f / 255f, 0xac / 255f, 1);
        public Color color_deSelect = new Color(0xba / 255f, 0xba / 255f, 0xba / 255f, 1);

        public Color SetColor(ColorTag _tag)
        {
            if(_tag == ColorTag.color_default)
            {
                return color_default;
            }
            else if(_tag == ColorTag.color_hover)
            {
                return color_hover;
            }
            else if(_tag == ColorTag.color_select)
            {
                return color_select;
            }
            else if(_tag == ColorTag.color_deselect)
            {
                return color_deSelect;
            }
            else
            {
                throw new System.Exception($"Unknown color tag {_tag.ToString()}");
            }
        }
    }
}
