using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using UnityEngine.Events;


	public partial class MainManager : IManager<MainManager>
	{
		// MainManager.Events_Input에 존재하는 이벤트에 관련된 이벤트 연산 메서드

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
