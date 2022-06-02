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
        private void Ortho_1Orthogonal(Interactable _setter)
        {
            ContentManager.Instance.Function_ToggleOrthoView(true);

            _Children.Off();
        }

        private void Ortho_2Perspective(Interactable _setter)
        {
            ContentManager.Instance.Function_ToggleOrthoView(false);

            _Children.Off();
        }
    }
}
