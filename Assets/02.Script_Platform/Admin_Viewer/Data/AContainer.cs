using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.Data
{
	using API;
	using Management;
	using Platform.Feature.Camera;

	/// <summary>
	/// TODO ������Ʈ���� �Ҵ�Ǿ�� �ϴ� �����͸� ��Ƶδ� �����̳��� ������ �����Ѵ�.
	/// ���� AdminViewer ������� ������ �Ҵ�Ǿ� ������, �̴� ���� ScriptableObject�� ����Ǿ�
	/// ���� �ν��Ͻ��� �����ϵ��� ������ �����̴�.
	/// </summary>
	[System.Serializable]
	public class AContainer
	{
		public Ad_Keymap m_keymap;

		public Ad_Capture m_capture;

		public void InitContainer()
		{
			MainManager.Instance.InitSubCameraResource(m_keymap.m_keymapCamera);
			//m_keymap.m_keymapCamera.gameObject.AddComponent<BIMCamera>();
		}
	}
}
