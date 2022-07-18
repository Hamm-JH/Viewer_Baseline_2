using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Platform.Feature._Input;
	using Platform.Feature.Camera;

	public abstract class IEvents 
	{
		[SerializeField] Mouse.Data mouseData;
		[SerializeField] Keyboard.Data keyboardData;
		[SerializeField] Touchpad.Data touchpadData;

		/// <summary>
		/// 마우스 데이터
		/// </summary>
		public Mouse.Data MouseData { get => mouseData; set => mouseData=value; }

		/// <summary>
		/// 키보드 데이터
		/// </summary>
		public Keyboard.Data KeyboardData { get => keyboardData; set => keyboardData=value; }

		/// <summary>
		/// 터치패드 데이터
		/// </summary>
        public Touchpad.Data TouchpadData { get => touchpadData; set => touchpadData = value; }

		
		[SerializeField] ICamera.Data cameraData;

		/// <summary>
		/// 카메라 데이터
		/// </summary>
		public ICamera.Data CameraData { get => cameraData; set => cameraData=value; }
    }
}
