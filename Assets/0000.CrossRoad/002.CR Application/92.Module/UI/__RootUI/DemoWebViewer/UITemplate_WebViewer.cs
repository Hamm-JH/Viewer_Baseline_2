using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using Definition.Data;
    using UnityEngine;
    using View;

    public partial class UITemplate_WebViewer : RootUI
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

        public override void GetUIEventPacket(Hover_EventType _type, APacket _value)
        {
            Elements.ForEach(x => x.GetUIEventPacket<Hover_EventType>(_type, _value));
        }
    }
}
