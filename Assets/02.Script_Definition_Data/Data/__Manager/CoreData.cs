using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using Platform.Feature._Input;
	using Platform.Feature.Camera;

	/// <summary>
	/// 플랫폼 시작시 동적 변환을 위한 변수 모음
	/// </summary>
	[System.Serializable]
	public class CoreData : IData
	{
		[SerializeField] PlatformCode m_platform;
		[SerializeField] GraphicCode m_graphic;
		[SerializeField] string m_modelURI;

		[SerializeField] Mouse.Data mouseData;
		[SerializeField] Keyboard.Data keyboardData;

		public Mouse.Data MouseData { get => mouseData; set => mouseData=value; }
		public Keyboard.Data KeyboardData { get => keyboardData; set => keyboardData=value; }


		[SerializeField] ICamera.Data cameraData;
		public ICamera.Data CameraData { get => cameraData; set => cameraData=value; }

		[SerializeField] List<ModuleID> m_moduleList;
		[SerializeField] List<FunctionCode> m_functionCode;

		public List<ModuleID> ModuleLists { get => m_moduleList; set => m_moduleList=value; }
		public List<FunctionCode> FunctionCodes { get => m_functionCode; set => m_functionCode=value; }


		//20211202-00000283

		public PlatformCode Platform { get => m_platform; set => m_platform=value; }
		public GraphicCode Graphic { get => m_graphic; set => m_graphic=value; }
		public string ModelURI { get => m_modelURI; set => m_modelURI=value; }
	}
}
