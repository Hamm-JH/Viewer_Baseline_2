using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class SliderUpdater : MonoBehaviour
    {
        public TextMeshProUGUI m_text;

        public void OnSliderValueChanged(Slider _slider)
        {
            m_text.text = $"{(int)(_slider.value * 100)}";
        }
    }
}
