using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Exceptions
{
    public class PlatformNotDefinedException : Exceptions
    {
        public PlatformNotDefinedException()
        {
            m_Message = $"Platform code not defined";
        }

        public PlatformNotDefinedException(PlatformCode _pCode)
        {
            m_Message = $"Platform code [{_pCode.ToString()}] is not defined";
        }

        public PlatformNotDefinedException(string message) : base(message)
        {
            m_Message = message;
        }

        public override string Message => m_Message;
        public override string ToString()
        {
            return m_Message;
        }
    }
}
