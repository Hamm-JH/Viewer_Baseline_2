using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using AdminViewer.API;
	using Definition;
	using Management.Events;
	using Platform.Feature.Camera;
	using System.Data;

	public partial class ContentManager : IManager<ContentManager>
	{
		public void Ad_InitUI(RectTransform _kmPanel)
		{
			m_container.m_keymap.m_keymapPanel = _kmPanel;
		}

		public void Functin_PrintExcel()
		{
			Ad_Capture capture = m_container.m_capture;

			_API.DownloadReport(capture);
		}

		public void Function_S5b1_SetSubPanel()
		{
			m_container.m_capture.s5b1_panel.SetSubPanel_s5b1();
		}

		public void Function_S5b2_SetSubHistoryTable(DataTable _dataTable)
		{
			m_container.m_capture.s5b2_panel.SetSubHistoryTable(_dataTable);
		}

		public void Function_SetCameraCenterPosition()
		{
			//Cameras.SetCameraCenter(m_container.m_keymap.m_keymapCamera,
			//	m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>());
			Cameras.SetCameraCenter(m_container.m_keymap.m_keymapCamera,
				m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>());
		}

        public void Function_SetCameraCenterPosition(Camera _cam)
        {
            //Cameras.SetCameraCenter(m_container.m_keymap.m_keymapCamera,
            //	m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>());
            Cameras.SetCameraCenter(_cam,
                _cam.gameObject.GetComponent<BIMCamera>());
        }

        public void Input_SelectObject(GameObject _obj)
		{
			m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>().OnSelect(_obj);
		}

		public void Input_KeymapClick(Vector3 _pos)
		{
			Camera cam = m_container.m_keymap.m_keymapCamera;
			Vector2 mPosition = cam.WorldToViewportPoint(_pos);
			//Vector2 mPosition = cam.WorldToViewportPoint(_pos);
			//Vector2 mPosition = cam.WorldToScreenPoint(_pos);
			RectTransform keymapPanel = m_container.m_keymap.m_keymapPanel;

			float screenWidth = Screen.width;
			float screenHeight = Screen.height;

			int defScreenWidth = Screen.width;// 1920;
			int defScreenHeight = Screen.height;// 1080;

			Vector2 panelPivot = keymapPanel.position;         // 패널 중심위치
			Vector2 panelOffsetMin = new Vector2(              // 패널 최소위치
				keymapPanel.offsetMin.x / defScreenWidth * screenWidth,
				keymapPanel.offsetMin.y / defScreenHeight * screenHeight
				);
			Vector2 panelOffsetMax = new Vector2(              // 패널 최대위치
				keymapPanel.offsetMax.x / defScreenWidth * screenWidth,
				keymapPanel.offsetMax.y / defScreenHeight * screenHeight
				);

			Vector2 panelMinPoint = panelPivot + panelOffsetMin;    // 패널 최소점
			Vector2 panelMaxPoint = panelPivot + panelOffsetMax;    // 패널 최대점

			Vector2 _panelClickPosition = (Vector2)_pos - panelMinPoint;   // 패널 클릭위치
			//Vector2 _panelClickPosition = mPosition - panelMinPoint;   // 패널 클릭위치

			Vector2 panelSize = new Vector2(                        // 패널 크기
				keymapPanel.sizeDelta.x / defScreenWidth * screenWidth,
				keymapPanel.sizeDelta.y / defScreenHeight * screenHeight
				);

			Vector2 _normalizeClickPosition = _panelClickPosition / panelSize;  // 노말 변환된 패널 클릭위치

			Vector2 cameraViewRect = new Vector2(cam.pixelWidth, cam.pixelHeight);
			Vector2 translatedClickCamPosition = _normalizeClickPosition * cameraViewRect;

			//EventManager.Instance.OnEvent(new Events.EventData_Input(
			//		_eventType: InputEventType.Input_clickSuccessUp,
			//		_btn: 0,
			//		_mousePos: translatedClickCamPosition,
			//		_camera: cam,
			//		_graphicRaycaster: _GrRaycaster,
			//		_event: MainManager.Instance.cameraExecuteEvents.selectEvent
			//		));

			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(translatedClickCamPosition);
			//Physics.Raycast(ray, out hit);

			if (Physics.Raycast(ray, out hit))
			{
				//Debug.Log("Hello");
				//Debug.Log(hit.collider.name);

				//Debug.Log(hit.collider.transform);

				EventManager.Instance.OnEvent(new EventData_API(
					InputEventType.API_SelectObject,
					hit.collider.gameObject,
					MainManager.Instance.cameraExecuteEvents.selectEvent
				));

				//clickedTransform = hit.collider.transform;

				//Manager.EventClassifier.Instance.OnEvent<KeymapElement>(Control.Status.Click, gameObject.GetComponent<KeymapElement>(), eventData);

				//Manager.EventClassifier.Instance.OnEvent<MP2_DmgListElement>(Control.Status.Click, gameObject.GetComponent<MP2_DmgListElement>(), eventData);
			}
			else
			{
				//clickedTransform = null;

			}
		}

		public void Input_KeymapDrag(int _btn, Vector2 _delta)
		{
			m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>().OnDrag(_btn, _delta);
		}

		public void Input_KeymapFocus(Vector3 _point, float _delta)
		{
			m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>().OnFocus(_point, _delta);
		}
	}
}
