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
        private void ToggleElement(Interactable _setter)
        {
            _setter.ChildPanel.SetActive(!_setter.ChildPanel.activeSelf);
        }

        private void OnElement(Interactable _setter)
        {
            _setter.ChildPanel.SetActive(true);
        }

        private void OffElement(Interactable _setter)
        {
            _setter.ChildPanel.SetActive(false);
        }
    }
}
