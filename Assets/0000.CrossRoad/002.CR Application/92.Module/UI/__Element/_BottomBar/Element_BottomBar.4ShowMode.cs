using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Management;
    using View;

    public partial class Element_BottomBar : AElement
    {
        private void Show_All(Interactable _setter)
        {
            ContentManager.Instance.Reset_ModelObject();
            _Settings.ResetSlider_transparency();
            _Settings.ResetSlider_iconSize();

            _Children.Off();
        }

        private void Show_Hide(Interactable _setter)
        {
            ContentManager.Instance.Toggle_ModelObject(UIEventType.Mode_Hide, ToggleType.Hide);

            _Children.Off();
        }

        private void Show_Isolate(Interactable _setter)
        {
            ContentManager.Instance.Toggle_ModelObject(UIEventType.Mode_Isolate, ToggleType.Isolate);

            _Children.Off();
        }
    }
}
