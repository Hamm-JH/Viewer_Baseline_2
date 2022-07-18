using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Objects
    {
        /// <summary>
        /// 타입에 맞는 객체 검출 시도
        /// </summary>
        /// <typeparam name="T">타입 T</typeparam>
        /// <param name="_obj">목표 객체</param>
        /// <param name="_value">검출된 타입 T</param>
        /// <returns>true : 검출 완료</returns>
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
