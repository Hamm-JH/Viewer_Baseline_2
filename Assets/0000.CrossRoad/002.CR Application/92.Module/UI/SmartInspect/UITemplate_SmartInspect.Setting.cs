using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
    using Management;
    using Management.Content;
    using View;

    public partial class UITemplate_SmartInspect : AUI
	{
		public void Setting_SetIconSize(float _value, Interactable _setter)
        {
            ContentManager.Instance.Set_Issue_Scale(_value);
            ((SmartInspectManager)SmartInspectManager.Instance).setting.IconScale = _value;
        }

		public void Setting_SetModelTransparency(float _value, Interactable _setter)
        {
            ContentManager.Instance.Set_Model_Transparency(_value);
            ((SmartInspectManager)SmartInspectManager.Instance).setting.ModelAlpha = _value;
        }

		public void Setting_SetZoomSensitivity(float _value, Interactable _setter)
        {
            ((SmartInspectManager)SmartInspectManager.Instance).setting.ZoomSensitivity = _value;
        }

		public void Setting_SetFontSize(float _value, Interactable _setter)
        {
            ((SmartInspectManager)SmartInspectManager.Instance).setting.FontSize = _value;
        }
    }
}
