using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Exceptions
{
    public class ModuleNotInstantiated : Exceptions
    {
        public ModuleNotInstantiated()
        {
            m_Message = $"module code not instantiated";
        }

        public ModuleNotInstantiated(ModuleID _id)
        {
            m_Message = $"module [{_id.ToString()}] is not instantiated";
        }

        public ModuleNotInstantiated(string message) : base(message)
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
