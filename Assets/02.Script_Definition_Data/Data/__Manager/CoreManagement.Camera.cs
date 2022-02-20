using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using Definition.Control;
	using global::Platform.Feature.Camera;

	public partial class CoreManagement : IData
	{
		[Header("Resource :: Camera")]
		[SerializeField] Camera mainCam;

		public Camera MainCam { get => mainCam; set => mainCam=value; }

		/// <summary>
		/// 생성된 카메라 인스턴스 리스트
		/// </summary>
		[SerializeField] List<ICamera> m_camera;

		public List<ICamera> Cameras { get => m_camera; set => m_camera=value; }

		/// <summary>
		/// 카메라모드 세팅
		/// </summary>
		/// <param name="_eventType"></param>
		public void SetCameraMode(UIEventType _eventType)
		{
			// 현재 카메라 모드
			CameraModes _modes = m_cameraMode;

			int _modeIndex = (int)_modes / 0x10;

			int _typeIndex = 0;

			switch (_eventType)
			{
				case UIEventType.Toggle_ViewMode_ISO: _typeIndex = 0x00; break;
				case UIEventType.Toggle_ViewMode_TOP: _typeIndex = 0x01; break;
				case UIEventType.Toggle_ViewMode_SIDE: _typeIndex = 0x02; break;
				case UIEventType.Toggle_ViewMode_BOTTOM: _typeIndex = 0x03; break;
			}

			_modes = (CameraModes)(_modeIndex * 0x10 + _typeIndex);

			CameraMode = _modes;
		}

		public void SetCameraPosition(Bounds centerBounds, Canvas rootCanvas, UIEventType eventType)
		{
			Vector3 center = centerBounds.center;
			Vector3 size = centerBounds.size;

			// 카메라 위치 정중앙에 두는 경우
			{
				m_cameraPoint.transform.position = center;

				mainCam.transform.localPosition = default(Vector3);
				mainCam.transform.rotation = Quaternion.Euler(Angle.Set(eventType));

				// 스크린 비율계산
				float ratio = 0f;
				RectTransform _rectT;
				if (rootCanvas.TryGetComponent<RectTransform>(out _rectT))
				{
					float width = _rectT.rect.width;
					float height = _rectT.rect.height;

					ratio = width > height ? width / height : height / width;
				}

				// size에 따라 거리 이동 공식 적용
				//Debug.LogError("Size에 따라 거리 이동 공식 적용"); // O

				// size의 반절, * 루트2 만큼 멀어져야 함
				float distance = Vector3.Distance(center, centerBounds.min);

				mainCam.transform.Translate(Vector3.back * distance * 1.4f * (ratio * 0.8f));
			}
		}
	}
}
