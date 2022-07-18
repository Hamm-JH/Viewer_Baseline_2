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
		/// 주관리자 시작시 이벤트 초기화
		/// </summary>
		public void Event_Initialize()
		{
			inputEvents = new Events.InputEvents(
				_mouse: _data.MouseData,
				_keyboard: _data.KeyboardData,
				_touchpad: _data.TouchpadData);

			cameraExecuteEvents = new Events.CameraEvents(
				_camera: _data.CameraData );
		}
	}
}
