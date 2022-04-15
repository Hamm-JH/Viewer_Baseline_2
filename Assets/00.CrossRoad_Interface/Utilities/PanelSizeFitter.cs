using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    using TMPro;

    public class PanelSizeFitter : MonoBehaviour
    {
        public RectTransform m_thisRect;
        //public float m_currHeight;
        public RectTransform m_childRect;

        private void LateUpdate()
        {
            Vector2 delta = m_thisRect.sizeDelta;
            m_thisRect.sizeDelta = new Vector2(delta.x, m_childRect.sizeDelta.y);
        }

        // Update is called once per frame
        //void Update()
        //{
        //    Vector2 delta = m_thisRect.sizeDelta;
        //    m_thisRect.sizeDelta = new Vector2(delta.x, m_childRect.sizeDelta.y);
        //    //Debug.Log(m_childRect.sizeDelta.y);
        //}
    }
}
