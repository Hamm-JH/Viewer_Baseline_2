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
		public void Graphic_ToggleOutline(bool isOn)
		{
			ForwardRendererData rSetting = RenderSetting;

			rSetting.rendererFeatures[0].SetActive(isOn);
		}

		public void Graphic_ToggleOutline()
		{
			ForwardRendererData rSetting = RenderSetting;
			
			rSetting.rendererFeatures[0].SetActive(!rSetting.rendererFeatures[0].isActive);
			//Debug.Log($"{rSetting.rendererFeatures[0].name}");	// NewOutlineFeature
		}
	}
}
