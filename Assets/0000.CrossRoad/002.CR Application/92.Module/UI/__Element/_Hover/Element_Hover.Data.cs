using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using TMPro;
    using View;

    public partial class Element_Hover : AElement
    {
        [System.Serializable]
        public class Data
        {
            public GameObject m_hoverPanel;
            public TextMeshProUGUI m_title;
            public TextMeshProUGUI m_hoverText1;
            public TextMeshProUGUI m_hoverText2;
            public TextMeshProUGUI m_hoverText3;
            public TextMeshProUGUI m_hoverText4;
            public TextMeshProUGUI m_hoverText5;
        }

        [SerializeField] Data m_data;
    }
}
