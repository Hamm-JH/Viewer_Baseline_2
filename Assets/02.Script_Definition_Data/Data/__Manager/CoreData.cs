using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using Platform.Feature._Input;
	using Platform.Feature.Camera;

	/// <summary>
	/// �÷��� ���۽� ���� ��ȯ�� ���� ���� ����
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

		[SerializeField] List<ModuleID> m_moduleList;
		[SerializeField] List<FunctionCode> m_functionCode;

		public List<ModuleID> ModuleLists { get => m_moduleList; set => m_moduleList=value; }
		public List<FunctionCode> FunctionCodes { get => m_functionCode; set => m_functionCode=value; }
	}
}
