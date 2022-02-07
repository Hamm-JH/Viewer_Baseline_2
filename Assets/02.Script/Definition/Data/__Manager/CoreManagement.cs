using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Control._Input;
using Definition.Control;

namespace Definition.Data
{
	using global::Platform.Feature.Camera;
	using Management;
	
	/// <summary>
	/// 주 관리자의 메인 관리변수
	/// </summary>
	[System.Serializable]
	public class CoreManagement : IData
	{
		#region Pre-defined 사전 정의 영역

		[Header("Pre-define data set")]
		/// <summary>
		/// 플랫폼에 대한 정의값을 관리
		/// </summary>
		[SerializeField] PlatformCode m_platform;
		[SerializeField] GraphicCode m_graphicMode;
		[SerializeField] CameraMode m_cameraMode;
		[SerializeField] Camera mainCam;

		public PlatformCode _Platform
		{ 
			get => m_platform;
			set => m_platform=value;
		}

		public GraphicCode GraphicMode 
		{ 
			get => m_graphicMode; 
			set => m_graphicMode=value; 
		}

		public CameraMode CameraMode 
		{ 
			get => m_cameraMode;
			set
			{
				m_cameraMode = value;

				// 카메라 모드 변경에 따른 변경 작업 (일원화)
				Cameras.ForEach(x => x.CamMode = value);
			}
		}

		public Camera MainCam { get => mainCam; set => mainCam=value; }

		#endregion

		#region Managed GameObjects

		/// <summary>
		/// 입력 인스턴스를 모아두는 최상위 입력값
		/// </summary>
		[SerializeField] GameObject m_rootInput;

		#endregion

		
		[Header("Input")]

		/// <summary>
		/// 생성된 입력 인스턴스 리스트
		/// </summary>
		[SerializeField] List<IInput> m_inputsource;

		/// <summary>
		/// 생성된 카메라 인스턴스 리스트
		/// </summary>
		[SerializeField] List<ICamera> m_camera;

		

		public List<IInput> Inputsource { get => m_inputsource; set => m_inputsource=value; }

		public List<ICamera> Cameras { get => m_camera; set => m_camera=value; }
		



		/// <summary>
		/// 입력 리소스를 입력 root 아래에 할당
		/// </summary>
		/// <param name="_source"> GameObject </param>
		/// <param name="_code"> Code::Input </param>
		public void SetInputResource(GameObject _source, IInput _code, Management.Events.InputEvents _events)
		{
			if(m_rootInput == null)
			{
				Debug.LogError("rootInput is null");
				return;
			}
			if(_source == null || _code == null)
			{
				Debug.LogError("_source or _code is null");
				return;
			}

			_source.transform.SetParent(m_rootInput.transform);

			m_inputsource.Add(_code);

			_code.OnStart(ref _events);
		}

		public void SetCameraResource(ICamera _component)
		{

		}
	}
}
