using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using View;

    public abstract partial class AElement : AUI
    {
        [SerializeField] AUI m_rootUI;

        public AUI RootUI { get => m_rootUI; set => m_rootUI = value; }

        public override void GetUIEvent<T>(float _value, T _type, Interactable _setter)
        {
            if (typeof(T) == typeof(UIEventType))
            {
                GetUIEvent(_value, (UIEventType)(object)_type, _setter);
            }
            else if(typeof(T) == typeof(Inspect_EventType))
            {
                GetUIEvent(_value, (Inspect_EventType)(object)_type, _setter);
            }
            else if(typeof(T) == typeof(BottomBar_EventType))
            {
                GetUIEvent(_value, (BottomBar_EventType)(object)_type, _setter);
            }
        }

        public override void GetUIEvent<T>(T _type, Interactable _setter)
        {
            if (typeof(T) == typeof(UIEventType))
            {
                GetUIEvent((UIEventType)(object)_type, _setter);
            }
            else if(typeof(T) == typeof(Inspect_EventType))
            {
                GetUIEvent((Inspect_EventType)(object)_type, _setter);
            }
            else if(typeof(T) == typeof(BottomBar_EventType))
            {
                GetUIEvent((BottomBar_EventType)(object)_type, _setter);
            }
        }

        public virtual void GetUIEvent(float _value, BottomBar_EventType _type, Interactable _setter) { }

        public virtual void GetUIEvent(BottomBar_EventType _type, Interactable _setter) { }

    }
}
