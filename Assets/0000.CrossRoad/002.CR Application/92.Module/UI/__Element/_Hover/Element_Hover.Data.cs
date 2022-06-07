using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using TMPro;
    using UnityEngine.UI;
    using View;

    public partial class Element_Hover : AElement
    {
        [System.Serializable]
        public class State_Image
        {
            public Color m_defaultColor;
            public Sprite m_defaultSprite;

            public void Set_Default(Image _tg)
            {
                _tg.sprite = m_defaultSprite;
                _tg.color = m_defaultColor;
            }
        }

        [System.Serializable]
        public class Data
        {
            public GameObject m_hoverPanel;
            public Image m_titleIcon;
            public TextMeshProUGUI m_titleText;
            public TextMeshProUGUI m_hoverText1;
            public TextMeshProUGUI m_hoverText2;
            public TextMeshProUGUI m_hoverText3;
            public TextMeshProUGUI m_hoverText4;
            public TextMeshProUGUI m_hoverText5;
        }

        [System.Serializable]
        public class Resource
        {
            [Header("State")]
            public State_Image m_damage;
            public State_Image m_recover;

            [Header("Data")]
            public Data m_data;


        }

        //[SerializeField] Data m_data;
        [SerializeField] Resource m_resource;
    }
}
