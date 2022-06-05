using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using Definition;
	using Management;
	using UnityEngine.UI;

	public partial class UITemplate_Tunnel : AUI
	{
		///// <summary>
		///// 모두 보이기
		///// </summary>
		//public void Event_View_Home()
		//{
		//	ContentManager.Instance.SetCameraCenterPosition();
		//	m_sliders.ResetSlider_transparency();
		//	m_sliders.ResetSlider_scale();
		//	MainManager.Instance.UpdateCameraMode(Definition.Control.CameraModes.BIM_ISO);
		//}

		//public void Event_Toggle_ChildPanel(int index, GameObject ChildPanel)
		//{
		//	// 자식 객체 토글
		//	// 이 인스턴스가 가진 childPanel을 보내 토글 끄기 대상에서 제외한다.

		//	TogglePanelList(index, ChildPanel);
		//	//m_rootUI.TogglePanelList(index, ChildPanel);
		//}

		//private void Event_Toggle_ViewMode(GameObject ChildPanel)
		//{
		//	bool toggle = false;

		//	if (ChildPanel != null)
		//	{
		//		toggle = !(bool)ChildPanel.activeSelf;

		//		ChildPanel.SetActive(toggle);

		//		//foreach (GameObject obj in m_uiFXs)
		//		//{
		//		//	obj.SetActive(toggle);
		//		//}
		//	}
		//}

		//private void Event_Toggle_ViewMode(UIEventType _eventType)
		//{
		//	MainManager.Instance.UpdateCameraMode(_eventType);
		//	ContentManager.Instance.SetCameraCenterPosition(_eventType);
		//}
	}
}
