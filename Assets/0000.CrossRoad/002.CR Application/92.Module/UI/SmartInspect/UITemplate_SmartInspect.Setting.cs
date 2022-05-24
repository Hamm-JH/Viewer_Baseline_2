using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
    using Management;
    using View;

    public partial class UITemplate_SmartInspect : AUI
	{
		public void Setting_SetIconSize(float _value, Interactable _setter)
        {
            ContentManager.Instance.Set_Issue_Scale(_value);
        }

		public void Setting_SetModelTransparency(float _value, Interactable _setter)
        {
            ContentManager.Instance.Set_Model_Transparency(_value);
        }

		public void Setting_SetZoomSensitivity(float _value, Interactable _setter)
        {

        }

		public void Setting_SetFontSize(float _value, Interactable _setter)
        {
			
        }
    }
}
