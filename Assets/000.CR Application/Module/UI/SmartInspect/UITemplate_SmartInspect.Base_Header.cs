using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// 시설물 이름 할당
        /// </summary>
        /// <param name="value"></param>
        public void Header_SetObjectName(string value)
        {
            m_basePanel.m_header.objectName.text = value;
        }
    }
}
