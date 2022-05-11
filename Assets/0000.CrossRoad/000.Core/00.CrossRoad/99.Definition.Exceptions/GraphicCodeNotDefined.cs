using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Exceptions
{
    public class GraphicCodeNotDefined : Exceptions
    {
        public GraphicCodeNotDefined()
        {
            m_Message = $"Image is not assigned";
        }

        public GraphicCodeNotDefined(GraphicCode _gCode)
        {
            m_Message = $"Graphic code [{_gCode.ToString()}] is not defined";
        }

        public GraphicCodeNotDefined(string message) : base(message)
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
