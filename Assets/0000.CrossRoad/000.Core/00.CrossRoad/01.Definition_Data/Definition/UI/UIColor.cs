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

        /// <summary>
        /// 기본 색상
        /// </summary>
        public Color color_default = new Color(0x2b/255f, 0x70/255f, 0xc6/255f, 1);

        /// <summary>
        /// 호버링 색상
        /// </summary>
        public Color color_hover = new Color(0x1a/255f, 0x5f/255f, 0xac/255f, 0.8f);

        /// <summary>
        /// 선택 색상
        /// </summary>
        public Color color_select = new Color(0x1a / 255f, 0x5f / 255f, 0xac / 255f, 1);

        /// <summary>
        /// 선택 해제 색상
        /// </summary>
        public Color color_deSelect = new Color(0xba / 255f, 0xba / 255f, 0xba / 255f, 1);

        /// <summary>
        /// 색상 할당
        /// </summary>
        /// <param name="_tag">변환 해야될 색상 상태</param>
        /// <returns>색</returns>
        /// <exception cref="System.Exception">의도하지 않은 색상 상태가 들어올 경우 오류발생</exception>
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
