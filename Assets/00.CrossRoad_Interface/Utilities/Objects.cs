using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Objects
    {
        public static bool TryGetValue<T>(GameObject _obj, out T _value) where T : class
        {
            bool result = false;

            _value = null;

            // 만약 객체가 해당 T에 해당하는 컴포넌트를 가진 경우 _value에 할당
            if(_obj.TryGetComponent<T>(out _value))
            {
                // 결과 bool도 true로 변경
                result = true;
            }

            return result;
        }
    }
}
