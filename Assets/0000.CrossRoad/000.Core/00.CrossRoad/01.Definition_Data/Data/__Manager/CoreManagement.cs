using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using Definition.Control;
	using Platform.Feature._Input;
	using global::Platform.Feature.Camera;
	using Management;
    using UnityEngine.Rendering.Universal;

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

		/// <summary>
		/// 카메라 주시 지점 표시객체
		/// </summary>
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

		/// <summary>
		/// 런타임 중의 플랫폼 코드
		/// </summary>
		public PlatformCode _Platforms
		{ 
			get => m_platform;
			set => m_platform=value;
		}

		/// <summary>
		/// 런타임 중의 그래픽 코드
		/// </summary>
		public GraphicCode GraphicMode 
		{ 
			get => m_graphicMode; 
			set => m_graphicMode=value; 
		}

		/// <summary>
		/// 런타임 중의 카메라 모드
		/// </summary>
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
		/// <summary>
		/// 치수선 Material
		/// </summary>
		public Material m_dimLineMat;

		/// <summary>
		/// 외곽선 Material
		/// </summary>
		public Material m_outlineMat;

		/// <summary>
		/// 포워드 렌더링 설정 데이터
		/// </summary>
		public ForwardRendererData m_renderSetting;

		/// <summary>
		/// 외곽선 Material
		/// </summary>
		public Material OutlineMat { get => m_outlineMat; set => m_outlineMat=value; }

	}
}
