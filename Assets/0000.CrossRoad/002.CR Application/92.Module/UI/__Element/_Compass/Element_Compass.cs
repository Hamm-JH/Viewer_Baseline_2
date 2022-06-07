using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Management;
    using Module.Interaction;
    using System.Linq;
    using View;

    public partial class Element_Compass : AElement
    {
        public override void GetUIEvent(UIEventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

        public override void GetUIEvent(Inspect_EventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

        public override void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

        public override void GetUIEvent(Compass_EventType _type, Interactable _setter)
        {
            switch(_type)
            {
                case Compass_EventType.Compass_Prev:
                    Compass_prev(_setter);
                    break;

                case Compass_EventType.Compass_Next:
                    Compass_next(_setter);
                    break;
            }
        }

        private void Start()
        {
            PlatformCode pCode = MainManager.Instance.Platform;
            Module_Interaction interaction = ContentManager.Instance.Module<Module_Interaction>();

            AUI aui;
            aui = interaction.UiInstances.First();
            RootUI = aui;
        }
    }
}
