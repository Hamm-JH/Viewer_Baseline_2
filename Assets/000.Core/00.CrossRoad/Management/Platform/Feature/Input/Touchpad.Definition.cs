using Management.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature._Input
{
    public partial class Touchpad : IInput
    {

        public override bool OnStart(ref InputEvents inputEvents)
        {
            onClick = false;

            m_InputEvents = inputEvents;

            SetDefinition<Data>(inputEvents.TouchpadData);

            return true;
        }

        protected override void SetDefinition<TValue>(TValue _data)
        {
            Data _instance = _data as Data;

            if(_instance != null)
            {
                defData = _instance;
            }
        }
    }
}
