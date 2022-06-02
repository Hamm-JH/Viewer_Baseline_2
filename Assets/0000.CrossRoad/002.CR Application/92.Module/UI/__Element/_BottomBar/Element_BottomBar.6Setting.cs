using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using View;
    using Management;

    public partial class Element_BottomBar : AElement
    {
        // 세팅창 켜서 나오는 버튼, 슬라이더 이벤트
        private void Setting_IconSize(float _value, Interactable _setter)
        {
            settings.Update_iconSize(_value);

            ContentManager.Instance.Set_Issue_Scale(_value);
        }

        private void Setting_ModelTransparency(float _value, Interactable _setter)
        {
            settings.Update_transparency(_value);

            ContentManager.Instance.Set_Model_Transparency(_value);
        }

        private void Setting_ZoomSensitivity(float _value, Interactable _setter)
        {
            settings.Update_sensitivity(_value);
        }

        private void Setting_FontSize(float _value, Interactable _setter)
        {
            settings.Update_fontSize(_value);
        }
    }
}
