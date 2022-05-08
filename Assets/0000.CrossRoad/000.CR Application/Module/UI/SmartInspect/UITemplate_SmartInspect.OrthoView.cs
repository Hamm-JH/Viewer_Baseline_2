using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Definition;
    using Management;
    using UnityEngine;
    using View;

    public partial class UITemplate_SmartInspect : AUI
    {
        private void Event_ToggleOrthoView(bool _isOrthogonal)
        {
            ContentManager.Instance.Function_ToggleOrthoView(_isOrthogonal);
        }
    }
}
