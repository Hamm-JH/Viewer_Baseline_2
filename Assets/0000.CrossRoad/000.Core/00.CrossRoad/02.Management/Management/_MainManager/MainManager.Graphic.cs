using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using UnityEngine.Rendering.Universal;

	/// <summary>
	/// 코어의 그래픽 관리 코드
	/// </summary>
	public partial class MainManager : IManager<MainManager>
	{
		/// <summary>
		/// 외곽선 On/Off
		/// </summary>
		/// <param name="isOn">true : On</param>
		public void Graphic_ToggleOutline(bool isOn)
		{
			ForwardRendererData rSetting = RenderSetting;

			rSetting.rendererFeatures[0].SetActive(isOn);
		}

	}
}
