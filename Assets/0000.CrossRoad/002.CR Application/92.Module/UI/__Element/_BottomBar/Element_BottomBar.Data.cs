using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using UnityEngine.UI;
    using View;

    public partial class Element_BottomBar : AElement
    {
		[System.Serializable]
		public class Sliders
		{
			public Slider m_transparencySlider;
			public Slider m_scaleSlider;

			public void ResetSlider_transparency()
			{
				if (m_transparencySlider == null) return;

				m_transparencySlider.value = 1;
			}

			public void ResetSlider_scale()
			{
				if (m_scaleSlider == null) return;

				m_scaleSlider.value = 1;
			}
		}
	}
}
