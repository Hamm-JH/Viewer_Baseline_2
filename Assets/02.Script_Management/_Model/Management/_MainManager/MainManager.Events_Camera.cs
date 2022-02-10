using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using UnityEngine.Events;

	public partial class MainManager : IManager<MainManager>
	{
		/// <summary>
		/// MainManager.InitCameraResource에서 생성된 카메라 인스턴스에 전달하는 이벤트
		/// 카메라 입력 진입시 실행되는 이벤트 인스턴스
		/// </summary>
		public Events.CameraEvents cameraExecuteEvents;

		public void UpdateCameraMode(UIEventType _eventType)
		{
			_core.SetCameraMode(_eventType);
		}

		public void SetCameraPosition(Bounds centerBounds, Canvas rootCanvas, UIEventType eventType = UIEventType.Toggle_ViewMode_ISO)
		{
			_core.SetCameraPosition(centerBounds, rootCanvas, eventType);
		}
	}
}
