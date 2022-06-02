using System.Collections;
using System.Collections.Generic;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using UnityEngine;
    using View;

    public partial class RootUI : AUI
    {
        public override void GetUIEvent<T>(float _value, T _type, Interactable _setter)
        {
            if (typeof(T) == typeof(UIEventType))
            {
                GetUIEvent(_value, (UIEventType)(object)_type, _setter);
            }
            else if (typeof(T) == typeof(Inspect_EventType))
            {
                GetUIEvent(_value, (Inspect_EventType)(object)_type, _setter);
            }
        }

        public override void GetUIEvent<T>(T _type, Interactable _setter)
        {
            if (typeof(T) == typeof(UIEventType))
            {
                GetUIEvent((UIEventType)(object)_type, _setter);
            }
            else if (typeof(T) == typeof(Inspect_EventType))
            {
                GetUIEvent((Inspect_EventType)(object)_type, _setter);
            }
        }
    }
}
