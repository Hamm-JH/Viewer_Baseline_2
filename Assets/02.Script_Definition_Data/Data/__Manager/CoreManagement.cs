using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Definition.Control;

namespace Definition.Data
{
	using Platform.Feature._Input;
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
		[SerializeField] CameraModes m_cameraMode;
		[SerializeField] Camera mainCam;

		[SerializeField] string modelURI;

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

		public Camera MainCam { get => mainCam; set => mainCam=value; }

		public string ModelURI { get => modelURI; set => modelURI=value; }

		#endregion

		#region Managed GameObjects

		/// <summary>
		/// 입력 인스턴스를 모아두는 최상위 입력값
		/// </summary>
		[SerializeField] GameObject m_rootInput;

		[SerializeField] GameObject m_cameraPoint;

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

		/// <summary>
		/// 카메라모드 세팅
		/// </summary>
		/// <param name="_eventType"></param>
		public void SetCameraMode(UIEventType _eventType)
		{
			// 현재 카메라 모드
			CameraModes _modes = m_cameraMode;

			int _modeIndex = (int)_modes / 0x10;

			int _typeIndex = 0;

			switch(_eventType)
			{
				case UIEventType.Toggle_ViewMode_ISO:	_typeIndex = 0x00;	break;
				case UIEventType.Toggle_ViewMode_TOP:	_typeIndex = 0x01;	break;
				case UIEventType.Toggle_ViewMode_SIDE:	_typeIndex = 0x02;	break;
				case UIEventType.Toggle_ViewMode_BOTTOM:_typeIndex = 0x03;	break;
			}

			_modes = (CameraModes)(_modeIndex * 0x10 + _typeIndex);

			CameraMode = _modes;
		}

		public void SetCameraPosition(Bounds centerBounds, Canvas rootCanvas, UIEventType eventType)
		{
			Vector3 center = centerBounds.center;
			Vector3 size = centerBounds.size;

			// 카메라 위치 정중앙에 두는 경우
			{
				m_cameraPoint.transform.position = center;

				mainCam.transform.localPosition = default(Vector3);
				mainCam.transform.rotation = Quaternion.Euler(Angle.Set(eventType));

				// 스크린 비율계산
				float ratio = 0f;
				RectTransform _rectT;
				if(rootCanvas.TryGetComponent<RectTransform>(out _rectT))
				{
					float width = _rectT.rect.width;
					float height = _rectT.rect.height;

					ratio = width > height ? width / height : height / width;
				}
				
				// size에 따라 거리 이동 공식 적용
				//Debug.LogError("Size에 따라 거리 이동 공식 적용"); // O

				// size의 반절, * 루트2 만큼 멀어져야 함
				float distance = Vector3.Distance(center, centerBounds.min);

				mainCam.transform.Translate(Vector3.back * distance * 1.4f * (ratio * 0.8f));
			}
		}
	}
}
