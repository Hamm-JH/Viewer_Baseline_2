using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using TMPro;
    using UnityEngine.UI;
    using View;

    public partial class Element_BottomBar : AElement
    {
		[System.Serializable]
		public class Children
        {
			[SerializeField] List<GameObject> list;

            public List<GameObject> _List { get => list; set => list = value; }

			public void On()
            {
				list.ForEach(x => x.SetActive(true));
            }

			public void Off()
            {
				list.ForEach(x => x.SetActive(false));
            }

			public void Toggle(GameObject _target)
            {
				list.ForEach(x => x.SetActive(x == _target && !x.activeSelf));
            }
        }

		[System.Serializable]
		public class Settings
		{
			public Slider m_iconSizeSlider;
			public Slider m_transparencySlider;
			public Slider m_sensitivitySlider;
			public Slider m_fontSizeSlider;

			[SerializeField] TextMeshProUGUI m_iconSizeText;
			[SerializeField] TextMeshProUGUI m_transparencyText;
			[SerializeField] TextMeshProUGUI m_sensitivityText;
			[SerializeField] TextMeshProUGUI m_fontSizeText;

			public void ResetSlider_iconSize()
			{
				if (m_iconSizeSlider == null) return;
				m_iconSizeSlider.value = 1;
				m_iconSizeText.text = "100";
			}

			public void ResetSlider_transparency()
			{
				if (m_transparencySlider == null) return;
				m_transparencySlider.value = 1;
				m_transparencyText.text = "100";
			}

			public void ResetSlider_Sensitivity()
            {
				if (m_sensitivitySlider == null) return;
				m_sensitivitySlider.value = 1;
				m_sensitivityText.text = "100";
            }

			public void ResetSlider_fontSize()
            {
				if (m_fontSizeSlider == null) return;
				m_fontSizeSlider.value = 1;
				m_fontSizeText.text = "100";
            }

			public void Update_iconSize(float _value)
            {
				if (m_iconSizeText == null) return;
				m_iconSizeText.text = ((int)(_value * 100)).ToString();
            }

			public void Update_transparency(float _value)
            {
				if (m_transparencyText == null) return;
				m_transparencyText.text = ((int)(_value * 100)).ToString();
			}

			public void Update_sensitivity(float _value)
            {
				if (m_sensitivityText == null) return;
				m_sensitivityText.text = ((int)(_value * 100)).ToString();
			}

			public void Update_fontSize(float _value)
            {
				if (m_fontSizeText == null) return;
				m_fontSizeText.text = ((int)(_value * 100)).ToString();
			}
		}

		[SerializeField] Children children;
		[SerializeField] Settings settings;

        public Children _Children { get => children; set => children = value; }
        public Settings _Settings { get => settings; set => settings = value; }
    }
}
