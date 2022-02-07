using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using UnityEngine.Events;

	public partial class MainManager : IManager<MainManager>
	{
		/// <summary>
		/// MainManager.InitCameraResource에서 생성된 카메라 인스턴스에 전달하는 이벤트
		/// 카메라 입력 진입시 실행되는 이벤트 인스턴스
		/// </summary>
		public Events.CameraEvents cameraExecuteEvents;
	}
}
