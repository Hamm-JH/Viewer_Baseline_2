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
		/// <summary>
		/// 키맵 UI 초기화
		/// </summary>
		/// <param name="_kmPanel">키맵 패널</param>
		public void Ad_InitUI(RectTransform _kmPanel)
		{
			m_container.m_keymap.m_keymapPanel = _kmPanel;
		}

		/// <summary>
		/// 엑셀 출력
		/// </summary>
		public void Functin_PrintExcel()
		{
			Ad_Capture capture = m_container.m_capture;

			_API.DownloadReport(capture);
		}

		/// <summary>
		/// 서브패널 생성
		/// </summary>
		public void Function_S5b1_SetSubPanel()
		{
			m_container.m_capture.s5b1_panel.SetSubPanel_s5b1();
		}

		/// <summary>
		/// 서브 이력 테이블 패널 생성
		/// </summary>
		/// <param name="_dataTable">배치할 데이터테이블</param>
		public void Function_S5b2_SetSubHistoryTable(DataTable _dataTable)
		{
			m_container.m_capture.s5b2_panel.SetSubHistoryTable(_dataTable);
		}

		/// <summary>
		/// 카메라 중심 위치 할당
		/// </summary>
		public void Function_SetCameraCenterPosition()
		{
			//Cameras.SetCameraCenter(m_container.m_keymap.m_keymapCamera,
			//	m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>());
			Cameras.SetCameraCenter(m_container.m_keymap.m_keymapCamera,
				m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>());
		}

		/// <summary>
		/// 카메라 중심 위치 할당
		/// </summary>
		/// <param name="_cam">카메라</param>
        public void Function_SetCameraCenterPosition(Camera _cam)
        {
            //Cameras.SetCameraCenter(m_container.m_keymap.m_keymapCamera,
            //	m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>());
            Cameras.SetCameraCenter(_cam,
                _cam.gameObject.GetComponent<BIMCamera>());
        }

		/// <summary>
		/// 키맵에서 객체 선택
		/// </summary>
		/// <param name="_obj">선택 객체</param>
        public void Input_SelectObjectOnKeymap(GameObject _obj)
		{
			m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>().OnSelect(_obj);
		}

		/// <summary>
		/// 키맵 클릭
		/// </summary>
		/// <param name="_pos">마우스 선택 위치</param>
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

			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(translatedClickCamPosition);
			//Physics.Raycast(ray, out hit);

			if (Physics.Raycast(ray, out hit))
			{
				EventManager.Instance.OnEvent(new EventData_API(
					InputEventType.API_SelectObject,
					hit.collider.gameObject,
					MainManager.Instance.cameraExecuteEvents.selectEvent
				));
			}
			else { }
		}

		/// <summary>
		/// 키맵 내부에서 드래깅
		/// </summary>
		/// <param name="_btn">마우스 버튼</param>
		/// <param name="_delta">드래그 정도</param>
		public void Input_KeymapDrag(int _btn, Vector2 _delta)
		{
			m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>().OnDrag(_btn, _delta);
		}

		/// <summary>
		/// 키맵 포커스 시작
		/// </summary>
		/// <param name="_point">포커스 위치</param>
		/// <param name="_delta">포커스 거리</param>
		public void Input_KeymapFocus(Vector3 _point, float _delta)
		{
			m_container.m_keymap.m_keymapCamera.gameObject.GetComponent<BIMCamera>().OnFocus(_point, _delta);
		}
	}
}
