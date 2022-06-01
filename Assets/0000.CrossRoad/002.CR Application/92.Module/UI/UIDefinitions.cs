using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using View;

    [System.Serializable]
    public class EventCode
    {
        [SerializeField] private UIEventType ui_eventType;
        [SerializeField] private Inspect_EventType inspect;
        [SerializeField] private BottomBar_EventType bottom;

        public UIEventType Ui_eventType { get => ui_eventType; set => ui_eventType = value; }
        public Inspect_EventType Inspect { get => inspect; set => inspect = value; }
        public BottomBar_EventType Bottom { get => bottom; set => bottom = value; }

        public void OnSelect(AUI _ui, Interactable _setter)
        {
            if(ui_eventType != UIEventType.Null || ui_eventType != UIEventType.NotWork)
            {
                _ui.GetUIEvent<UIEventType>(ui_eventType, _setter);
            }
            if(inspect != Inspect_EventType.NotWork)
            {
                _ui.GetUIEvent<Inspect_EventType>(inspect, _setter);
            }
            if(bottom != BottomBar_EventType.NotWork)
            {
                _ui.GetUIEvent<BottomBar_EventType>(bottom, _setter);
            }
        }

        public void OnChangeValue(float _value, AUI _ui, Interactable _setter)
        {
            if (ui_eventType != UIEventType.Null || ui_eventType != UIEventType.NotWork)
            {
                _ui.GetUIEvent<UIEventType>(_value, ui_eventType, _setter);
            }
            if (inspect != Inspect_EventType.NotWork)
            {
                _ui.GetUIEvent<Inspect_EventType>(_value, inspect, _setter);
            }
            if (bottom != BottomBar_EventType.NotWork)
            {
                _ui.GetUIEvent<BottomBar_EventType>(_value, bottom, _setter);
            }
        }
    }

}
