using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using Definition.Control;
	using Platform.Feature._Input;
	using global::Platform.Feature.Camera;
	using Management;
	
	/// <summary>
	/// 주 관리자의 메인 관리변수
	/// </summary>
	[System.Serializable]
	public partial class CoreManagement : IData
	{
		#region [Private] Managed GameObjects

		[Header("Private")]
		/// <summary>
		/// 입력 인스턴스를 모아두는 최상위 입력값
		/// </summary>
		[SerializeField] GameObject m_rootInput;

		[SerializeField] GameObject m_cameraPoint;

		
		public GameObject CameraPoint { get => m_cameraPoint; }

		#endregion
		
		#region Pre-defined 사전 정의 영역

		[Header("Pre-define data set")]
		/// <summary>
		/// 플랫폼에 대한 정의값을 관리
		/// </summary>
		[SerializeField] PlatformCode m_platform;
		[SerializeField] GraphicCode m_graphicMode;
		[SerializeField] CameraModes m_cameraMode;

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

		public CameraModes CameraMode 
		{ 
			get => m_cameraMode;
			set
			{
				m_cameraMode = value;

				// 카메라 모드 변경에 따른 변경 작업 (일원화)
				Cameras.ForEach(x => x.CamMode = value);
			}
		}

		#endregion

		[Header("Dimension")]
		public Material m_dimLineMat;
		public Material m_outlineMat;

		public Material OutlineMat { get => m_outlineMat; set => m_outlineMat=value; }

	}
}
