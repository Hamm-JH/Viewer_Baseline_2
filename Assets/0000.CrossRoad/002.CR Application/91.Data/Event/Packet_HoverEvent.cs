using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
    public class Packet_HoverEvent : APacket
    {
        [SerializeField] Canvas m_rootCanvas;
        [SerializeField] Vector3 m_screenMousePos;
        [SerializeField] Definition._Issue.Issue m_issue;

        public Canvas _RootCanvas { get => m_rootCanvas; set => m_rootCanvas = value; }
        public Vector3 _ScreenMousePos { get => m_screenMousePos; set => m_screenMousePos = value; }
        public _Issue.Issue _Issue { get => m_issue; set => m_issue = value; }

        public Packet_HoverEvent(Canvas _rootCanvas, Vector3 _screenMousePos, Definition._Issue.Issue _issue)
        {
            m_rootCanvas = _rootCanvas;
            m_screenMousePos = _screenMousePos;
            m_issue = _issue;
        }
    }
}
