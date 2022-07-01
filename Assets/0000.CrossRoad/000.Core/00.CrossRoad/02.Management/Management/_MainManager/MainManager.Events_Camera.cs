using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
    using Definition.Control;
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

		public void UpdateCameraMode(CameraModes _mode)
        {
			_core.SetCameraMode(_mode);

			ContentManager.Instance.Update_CameraMode(_mode);
        }

		public void SetCameraPosition(Bounds centerBounds, Canvas rootCanvas, 
			UIEventType eventType = UIEventType.Viewport_ViewMode_ISO)
		{
			UpdateCameraMode(UIEventType.Viewport_ViewMode_ISO);

			_core.SetCameraPosition(centerBounds, rootCanvas, eventType);
		}

		public void SetCameraPosition(Bounds _center, Canvas _canvas, UIEventType _eType = UIEventType.Viewport_ViewMode_ISO,
			Vector3 _baseAngle = default(Vector3))
		{
			_core.SetCameraPosition(_center, _canvas, _eType, _baseAngle);
		}

		/// <summary>
		/// 카메라 오프셋값 초기화
		/// </summary>
		public void ResetCamdata_targetOffset()
		{
			_core.Cameras.ForEach(x => x.ResetData_targetOffset());
		}

		/// <summary>
		/// 카메라 패닝 제한거리 설정
		/// </summary>
		/// <param name="_value"></param>
		public void UpdateCamData_maxOffset(float _value)
		{
			_value = _value * 0.8f;
			_core.Cameras.ForEach(x => x.SetData_MaxOffset(_value));
		}

		
	}
}
