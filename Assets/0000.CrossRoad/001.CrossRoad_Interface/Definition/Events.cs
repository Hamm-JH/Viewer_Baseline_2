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

            // �� ���� Null�� ���¸� �����Ѵ�.
            // �� ��ü�� �ϳ��� null�� �ƴѰ�?
            if( _currObj != null || _selectedObj != null)
            {
                // �� ��ü�� ������?
                if(_currObj == _selectedObj)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Event_ClickUp SelectUI ���� ���� ���� ��ü�� ���� ��ü���� Ȯ���ϴ� �޼���
        /// </summary>
        /// <param name="_currSelected"></param>
        /// <param name="_sEvents"></param>
        /// <param name="_currObj"></param>
        /// <param name="_selectedObj"></param>
        /// <returns></returns>
        public static bool IsSameObjectSelected(GameObject _currSelected, Dictionary<InputEventType, AEventData> _sEvents,
            out GameObject _currObj, out GameObject _selectedObj)
        {
            bool result = false;

            _currObj = null;
            _selectedObj = null;

            if(_currSelected == null)
            {
                return false;
            }
            else
            {
                _currObj = _currSelected;
            }

            if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
            {
                _selectedObj = _sEvents[InputEventType.Input_clickSuccessUp].Elements.Last().Target;
            }

            if(_currObj != null || _selectedObj != null)
            {
                if(_currObj == _selectedObj)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
