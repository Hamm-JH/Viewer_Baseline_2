using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using UnityEngine.Events;
	using Platform.Feature.Camera;
	using Platform.Feature._Input;
	using Definition;
	using Definition.Control;
	using Definition.Data;
    using Bearroll.UltimateDecals;

    public partial class MainManager : IManager<MainManager>
	{
		#region 0-1 데이터 입력 단계

		/// <summary>
		/// 데이터셋 요청
		/// </summary>
		/// <param name="action">데이터셋 요청 처리시 콜백 액션</param>
		private void RequestDataset(UnityAction<CoreData> action)
		{
			StartCoroutine(_templateDatas[m_templateIndex].Initialize(action));
		}


		#endregion

		#region 0-2 데이터 입력 완료시 실행

		/// <summary>
		/// 시스템 인스턴스 데이터 할당
		/// </summary>
		/// <param name="_finishedData">설정 완료된 데이터</param>
		private void SetSystemInstance(CoreData _finishedData)
		{
			//UD_Manager.instance.DoDestroy();

			_data = _finishedData;

			// 초기 시작시 이벤트 초기화
			Event_Initialize();

			// 입력 발생시 실행되는 액션 초기화
			SetAction(ManagerActionIndex.InputAction);

			InitCoreData(_data);

			// 입력 인스턴스 초기화
			InitInputResource(_core._Platforms);

			// 카메라 리소스 초기화
			InitCameraResource(_core.CameraMode, cameraExecuteEvents);

			Load_Scene(_core._Platforms);
		}

		#endregion

		#region 1 입력 초기화

		/// <summary>
		/// 코어 데이터 초기화
		/// </summary>
		/// <param name="_data">미리 세팅되어있는 코어 데이터 프리셋</param>
		private void InitCoreData(CoreData _data)
		{
			_core._Platforms = _data.Platform;
			_core.GraphicMode = _data.Graphic;
		}

		/// <summary>
		/// 입력 인스턴스 할당코드 모음
		/// </summary>
		/// <param name="_pCode">플랫폼 코드</param>
		public void InitInputResource(PlatformCode _pCode)
		{
			// TODO :: 2ND :: ★ 빌드의 입력장치가 정해졌을때 장치입력을 받는 인스턴스를 생성하도록 개편

			// 같은 WebGL이지만 구동되는 기기가 다르면 다른 플랫폼으로 쳐야한다.
			// :: 플랫폼 코드만으로 해결이 안됨.

			if(Platforms.IsMobilePlatform(_pCode))
            {
				InitSingleInputResource<Platform.Feature._Input.Touchpad>("touchpad");
				//Debug.LogError("아직 Mobile 코드가 작성되지 않았습니다. 작성 후 업데이트 필요함.");
			}
			else if(Platforms.IsPCPlatform(_pCode))
            {
				InitSingleInputResource<Platform.Feature._Input.Mouse>("mouse");
				InitSingleInputResource<Platform.Feature._Input.Keyboard>("keyboard");
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(_pCode);
            }
		}

		/// <summary>
		/// 단일 입력 리소스 생성
		/// </summary>
		/// <typeparam name="T"> 입력 클래스 </typeparam>
		/// <param name="name"> 입력 클래스 이름 </param>
		private void InitSingleInputResource<T>(string name) where T : IInput
		{
			GameObject obj = new GameObject(name);  // 입력 신규객체 생성
			T component = obj.AddComponent<T>();    // 입력 인스턴스 생성

			// 코어 리소스에 입력 인스턴스 등록
			_core.SetInputResource(
				_source: obj,
				_code: component,
				_events: inputEvents);
		}

		#endregion

		#region 1 카메라 초기화

		/// <summary>
		/// 카메라 인스턴스 초기화코드 모음
		/// </summary>
		/// <param name="_mode"></param>
		/// <param name="_cameraExecuteEvents"></param>
		public void InitCameraResource(CameraModes _mode, Events.CameraEvents _cameraExecuteEvents)
		{
			Camera main = _core.MainCam;
			//SetCamera<FreeCamera>(main, _cameraExecuteEvents);
			//SetCamera<OrbitCamera>(main, _cameraExecuteEvents);
			SetCamera<BIMCamera>(_core, _data, _cameraExecuteEvents);

			// 코어의 카메라모드 변경으로 모든 카메라의 모드 일괄변경
			_core.CameraMode = _mode;
		}

		/// <summary>
		/// 서브 카메라 자원 초기화 실행
		/// </summary>
		/// <param name="subCamera">서브 카메라 객체</param>
		public void InitSubCameraResource(Camera subCamera)
		{
			//cameraExecuteEvents;

			SetCamera<BIMCamera>(subCamera, _core, _data);
		}

		/// <summary>
		/// SubCam 등록
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_cam"></param>
		/// <param name="_core"></param>
		/// <param name="_data"></param>
		private void SetCamera<T>(Camera _cam, CoreManagement _core, CoreData _data) where T : ICamera
		{
			T component = null;

			// 카메라에 해당 카메라가 등록되지 않았다면
			if (!_cam.gameObject.TryGetComponent<T>(out component))
			{
				component = _cam.gameObject.AddComponent<T>();
			}

			// 코어 카메라 컴포넌트 등록
			_core.Cameras.Add(component);

			// 카메라에서 연결 대상 메서드 등록
			component.Default = _core.CameraPoint;
			component.SetData(_data.CameraData);
		}

		/// <summary>
		/// 카메라 설정 
		/// </summary>
		/// <typeparam name="T">카메라 클래스</typeparam>
		/// <param name="_core">코어 관리 클래스</param>
		/// <param name="_data">코어 데이터</param>
		/// <param name="_camEvents">카메라 입력 이벤트</param>
		private void SetCamera<T>(CoreManagement _core, CoreData _data, Events.CameraEvents _camEvents) where T : ICamera
		{
			T component = null;
			Camera _main = _core.MainCam;
			
			// 카메라에 해당 카메라가 등록되지 않았다면
			if (!_main.gameObject.TryGetComponent<T>(out component))
			{
				component = _main.gameObject.AddComponent<T>();
			}

			// 코어 카메라 컴포넌트 등록
			_core.Cameras.Add(component);

			// 카메라에서 연결 대상 메서드 등록
			component.SetAction(_camEvents);
			component.Default = _core.CameraPoint;
			component.SetData(_data.CameraData);
		}

		#endregion
	}
}
