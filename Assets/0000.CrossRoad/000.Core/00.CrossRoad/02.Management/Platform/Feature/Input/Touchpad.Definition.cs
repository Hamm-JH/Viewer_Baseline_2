using Management.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform.Feature._Input
{
    public partial class Touchpad : IInput
    {
        /// <summary>
        /// 터치패트 초기화
        /// </summary>
        /// <param name="inputEvents">입력 이벤트</param>
        /// <returns></returns>
        public override bool OnStart(ref InputEvents inputEvents)
        {
            onClick = false;

            m_InputEvents = inputEvents;

            SetDefinition<Data>(inputEvents.TouchpadData);

            return true;
        }

        /// <summary>
        /// 터치패드 입력데이터 할당
        /// </summary>
        /// <typeparam name="TValue">입력 데이터 정의</typeparam>
        /// <param name="_data"></param>
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
