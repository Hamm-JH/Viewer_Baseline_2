using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Definition.Data;
    using System;
    using View;

    public partial class Element_Hover : AElement
    {
        public override void GetUIEvent(UIEventType _uType, Interactable _setter)
        {

        }

        public override void GetUIEvent(Inspect_EventType _uType, Interactable _setter)
        {

        }

        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
        {

        }

        public override void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter)
        {

        }

        //public override void GetUIEvent(Hover_EventType _type, Interactable _setter) { }

        //public override void GetUIEvent(float _value, Hover_EventType _type, Interactable _setter) { }

        public override void GetUIEventPacket(Hover_EventType _type, APacket _value)
        {
            switch (_type)
            {
                case Hover_EventType.OnHover:
                    HoverPanel_OnHover(_value);
                    break;

                case Hover_EventType.OffHover:
                    HoverPanel_OffHover();
                    break;
            }
        }

    }
}
