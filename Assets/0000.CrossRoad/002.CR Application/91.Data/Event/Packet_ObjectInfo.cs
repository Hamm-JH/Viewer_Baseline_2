using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
    public class Packet_ObjectInfo : APacket
    {
        [SerializeField] GameObject m_object;
        public GameObject _Object { get => m_object; set => m_object = value; }

        public Packet_ObjectInfo(GameObject _obj)
        {
            _Object = _obj;
        }

    }
}
