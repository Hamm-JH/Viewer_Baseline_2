using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Exceptions
{
    using System;

    /// <summary>
    /// ���� ����
    /// </summary>
    public class Exceptions : Exception
    {
        protected string m_Message;

        public Exceptions()
        {
        }

        public Exceptions(string message) : base(message)
        {
            m_Message = message;
        }
    }
}
