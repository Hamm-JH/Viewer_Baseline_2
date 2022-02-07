using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control._Input;
using Platform.Feature.Camera;

namespace Definition.Data
{

	/// <summary>
	/// 플랫폼 시작시 동적 변환을 위한 변수 모음
	/// </summary>
	[System.Serializable]
	public class CoreData : IData
	{
		[SerializeField] Mouse.Data mouseData;
		[SerializeField] Keyboard.Data keyboardData;

		public Mouse.Data MouseData { get => mouseData; set => mouseData=value; }
		public Keyboard.Data KeyboardData { get => keyboardData; set => keyboardData=value; }


		[SerializeField] ICamera.Data cameraData;
		public ICamera.Data CameraData { get => cameraData; set => cameraData=value; }
	}
}
