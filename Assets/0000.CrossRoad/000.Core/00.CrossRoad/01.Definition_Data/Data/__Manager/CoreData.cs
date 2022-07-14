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
	public partial class CoreData : IData
	{
		[Header("System Value")]
		[SerializeField] PlatformCode m_platform;
		[SerializeField] GraphicCode m_graphic;

		/// <summary>
		/// �÷��� �ڵ�
		/// </summary>
		public PlatformCode Platform { get => m_platform; set => m_platform=value; }

		/// <summary>
		/// �׷��� �ڵ�
		/// </summary>
		public GraphicCode Graphic { get => m_graphic; set => m_graphic=value; }

		//----------------------------------------------------------------------------------

		[Header("Input Value")]
		[SerializeField] Mouse.Data mouseData;
		[SerializeField] Keyboard.Data keyboardData;
		[SerializeField] Touchpad.Data touchpadData;

		/// <summary>
		/// ���콺 �Է°��� ����
		/// </summary>
		public Mouse.Data MouseData { get => mouseData; set => mouseData=value; }
		
		/// <summary>
		/// Ű���� �Է°��� ����
		/// </summary>
		public Keyboard.Data KeyboardData { get => keyboardData; set => keyboardData=value; }

		/// <summary>
		/// ��ġ�е� �Է°��� ����
		/// </summary>
		public Touchpad.Data TouchpadData { get => touchpadData; set => touchpadData = value; }

		[SerializeField] ICamera.Data cameraData;

		/// <summary>
		/// ī�޶� ����
		/// </summary>
		public ICamera.Data CameraData { get => cameraData; set => cameraData=value; }
		
		//----------------------------------------------------------------------------------

		[Header("Module Value")]
		[SerializeField] List<ModuleID> m_moduleList;
		[SerializeField] List<FunctionCode> m_functionCode;

		/// <summary>
		/// ��� ����Ʈ
		/// </summary>
		public List<ModuleID> ModuleLists { get => m_moduleList; set => m_moduleList=value; }

		/// <summary>
		/// ����� Ư�� ����ڵ� ����Ʈ
		/// </summary>
		public List<FunctionCode> FunctionCodes { get => m_functionCode; set => m_functionCode=value; }

		[Header("Web Parameter")]
		[SerializeField] string m_url;
		[SerializeField] string m_baseURL;
		[SerializeField] string m_modelURI;
		[SerializeField] string m_addressURI;
		[SerializeField] string m_issueDmgURI;
		[SerializeField] string m_issueRcvURI;
		[SerializeField] string m_imageURL;
		[SerializeField] string m_historyURL;
		[SerializeField] string m_keyCode;

		/// <summary>
		/// �⺻ URL
		/// </summary>
		public string BaseURL { get => m_baseURL; set => m_baseURL=value; }

		/// <summary>
		/// �� ��û URI
		/// </summary>
		public string ModelURI { get => m_modelURI; set => m_modelURI=value; }

		/// <summary>
		/// �ּ� ��û URI
		/// </summary>
		public string AddressURI { get => m_addressURI; set => m_addressURI=value; }

		/// <summary>
		/// �ջ����� ��û URI
		/// </summary>
		public string IssueDmgURI { get => m_issueDmgURI; set => m_issueDmgURI=value; }

		/// <summary>
		/// �������� ��û URI
		/// </summary>
		public string IssueRcvURI { get => m_issueRcvURI; set => m_issueRcvURI=value; }

		/// <summary>
		/// �̹��� ��û URL
		/// </summary>
		public string ImageURL { get => m_imageURL; set => m_imageURL=value; }

		/// <summary>
		/// �ջ� �̷����� ��û URL
		/// </summary>
		public string HistoryURL { get => m_historyURL; set => m_historyURL=value; }

		/// <summary>
		/// ��Ÿ�� �߿� �������� �ִ� ���� Ű �ڵ�
		/// </summary>
		public string KeyCode { get => m_keyCode; set => m_keyCode=value; }
    }
}
