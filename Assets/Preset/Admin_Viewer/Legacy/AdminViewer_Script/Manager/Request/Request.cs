using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Request
    {
        public Type managerType;
        public RequestCode requestCode;

        public Request(Type _managerType, RequestCode _requestCode)
        {
            managerType = _managerType;
            requestCode = _requestCode;
        }
    }
}
