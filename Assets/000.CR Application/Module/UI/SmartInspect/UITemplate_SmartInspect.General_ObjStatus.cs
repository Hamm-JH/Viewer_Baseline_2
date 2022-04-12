using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        public void General_SetMainImage(Texture2D _tex)
		{
            m_general.m_objStatus.mainImage.texture = _tex;
		}
        // Initialize에서
            // 주소값 받아가기
            // 주 이미지 받아가기

        private void General_SetAddress(string value)
		{
            m_general.m_objStatus.addressText.text = value;
		}
    }
}
