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
	public partial class CoreData : IData
	{
		[Header("System Value")]
		[SerializeField] PlatformCode m_platform;
		[SerializeField] GraphicCode m_graphic;

		/// <summary>
		/// 플랫폼 코드
		/// </summary>
		public PlatformCode Platform { get => m_platform; set => m_platform=value; }

		/// <summary>
		/// 그래픽 코드
		/// </summary>
		public GraphicCode Graphic { get => m_graphic; set => m_graphic=value; }

		//----------------------------------------------------------------------------------

		[Header("Input Value")]
		[SerializeField] Mouse.Data mouseData;
		[SerializeField] Keyboard.Data keyboardData;
		[SerializeField] Touchpad.Data touchpadData;

		/// <summary>
		/// 마우스 입력관리 정보
		/// </summary>
		public Mouse.Data MouseData { get => mouseData; set => mouseData=value; }
		
		/// <summary>
		/// 키보드 입력관리 정보
		/// </summary>
		public Keyboard.Data KeyboardData { get => keyboardData; set => keyboardData=value; }

		/// <summary>
		/// 터치패드 입력관리 정보
		/// </summary>
		public Touchpad.Data TouchpadData { get => touchpadData; set => touchpadData = value; }

		[SerializeField] ICamera.Data cameraData;

		/// <summary>
		/// 카메라 정보
		/// </summary>
		public ICamera.Data CameraData { get => cameraData; set => cameraData=value; }
		
		//----------------------------------------------------------------------------------

		[Header("Module Value")]
		[SerializeField] List<ModuleID> m_moduleList;
		[SerializeField] List<FunctionCode> m_functionCode;

		/// <summary>
		/// 모듈 리스트
		/// </summary>
		public List<ModuleID> ModuleLists { get => m_moduleList; set => m_moduleList=value; }

		/// <summary>
		/// 모듈의 특정 기능코드 리스트
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
		/// 기본 URL
		/// </summary>
		public string BaseURL { get => m_baseURL; set => m_baseURL=value; }

		/// <summary>
		/// 모델 요청 URI
		/// </summary>
		public string ModelURI { get => m_modelURI; set => m_modelURI=value; }

		/// <summary>
		/// 주소 요청 URI
		/// </summary>
		public string AddressURI { get => m_addressURI; set => m_addressURI=value; }

		/// <summary>
		/// 손상정보 요청 URI
		/// </summary>
		public string IssueDmgURI { get => m_issueDmgURI; set => m_issueDmgURI=value; }

		/// <summary>
		/// 보수정보 요청 URI
		/// </summary>
		public string IssueRcvURI { get => m_issueRcvURI; set => m_issueRcvURI=value; }

		/// <summary>
		/// 이미지 요청 URL
		/// </summary>
		public string ImageURL { get => m_imageURL; set => m_imageURL=value; }

		/// <summary>
		/// 손상 이력정보 요청 URL
		/// </summary>
		public string HistoryURL { get => m_historyURL; set => m_historyURL=value; }

		/// <summary>
		/// 런타임 중에 보여지고 있는 모델의 키 코드
		/// </summary>
		public string KeyCode { get => m_keyCode; set => m_keyCode=value; }
    }
}
