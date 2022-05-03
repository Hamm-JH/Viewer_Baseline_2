using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Management.Events;
    using System.Linq;
    using View;

    public static class _Events
    {
        public static bool IsSameObjectSelected(List<IInteractable> _currElement, Dictionary<InputEventType, AEventData> _sEvents,
            out GameObject _currObj, out GameObject _selectedObj)
        {
            bool result = false;

            _currObj = null;
            _selectedObj = null;

            if(_currElement.Count != 0)
            {
                _currObj = _currElement.Last().Target;
            }

            if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
            {
                _selectedObj = _sEvents[InputEventType.Input_clickSuccessUp].Elements.Last().Target;
            }

            // 두 값이 Null인 상태를 배제한다.
            // 두 객체중 하나는 null이 아닌가?
            if( _currObj != null || _selectedObj != null)
            {
                // 두 객체는 같은가?
                if(_currObj == _selectedObj)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
