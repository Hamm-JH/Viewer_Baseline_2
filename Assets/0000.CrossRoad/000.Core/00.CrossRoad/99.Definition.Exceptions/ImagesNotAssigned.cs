using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Exceptions
{
    /// <summary>
    /// �Ҵ���� ���� �̹���
    /// </summary>
    public class ImagesNotAssigned : Exceptions
    {
        public ImagesNotAssigned()
        {
            m_Message = $"Image is not assigned";
        }

        public ImagesNotAssigned(string message) : base(message)
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
