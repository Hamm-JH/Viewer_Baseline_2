using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Exceptions
{
    public class SceneNotExisted : Exceptions
    {
        public SceneNotExisted()
        {
            m_Message = $"Scene not existed";
        }

        public SceneNotExisted(SceneName _sCode)
        {
            m_Message = $"Scene [{_sCode.ToString()}] is not existed";
        }

        public SceneNotExisted(string message) : base(message)
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
