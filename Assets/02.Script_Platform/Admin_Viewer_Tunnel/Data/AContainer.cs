using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.Data
{
	using API;
	using Definition;
	using Management;
	using Platform.Feature.Camera;

	/// <summary>
	/// TODO 프로젝트별로 할당되어야 하는 데이터를 모아두는 컨테이너의 원형을 정의한다.
	/// 현재 AdminViewer 기반으로 데이터 할당되어 있으며, 이는 추후 ScriptableObject로 변경되어
	/// 단일 인스턴스로 동작하도록 변경할 예정이다.
	/// </summary>
	[System.Serializable]
	public class AContainer
	{
		public Ad_Keymap m_keymap;

		public Ad_Capture m_capture;

		public void InitContainer()
		{
			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsDemoAdminViewer(pCode))
			{
				MainManager.Instance.InitSubCameraResource(m_keymap.m_keymapCamera);
			}
			//m_keymap.m_keymapCamera.gameObject.AddComponent<BIMCamera>();
		}
	}
}
