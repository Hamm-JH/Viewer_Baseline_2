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

	public partial class MainManager : IManager<MainManager>
	{
		#region 0-1 데이터 입력 단계

		/// <summary>
		/// 앱 시작시 데이터 요청
		/// </summary>
		/// <returns></returns>
		private IEnumerator RequestDataset(UnityAction callback)
		{
			yield return new WaitForEndOfFrame();

			//-------------------------------------------- 데이터 요청

			// 시스템 데이터
			Debug.LogError("서버에서 데이터를 받으면, 데이터를 분배한다.");
			//PlatformCode platformCode = PlatformCode.PC_Maker1;
			//GraphicCode graphicCode = GraphicCode.Template00;
			//CameraMode input_camMode = CameraMode.BIM_ISO;

			// 마우스 데이터 (기본값)
			//float dragBoundary = 1.5f;
			string _urlQuery = "http://wesmart.synology.me:45001/unity/admin_viewer/index.html?cdTunnel=20211202-00000283"; // 아크터널 ?

			string[] _cdBridge = _urlQuery.Split('=');
			string __modelURL = "/3dmodel/";
			_cdBridge = _cdBridge[1].Split('&');
			string BridgeCode = _cdBridge[0];

			string uri = string.Format($"http://wesmart.synology.me:45001{__modelURL}{BridgeCode}.gltf");
			//Debug.Log(uri);

			string _modelURI = uri;
			//string _modelURI = "http://wesmart.synology.me:45001/3dmodel/20211202-00000283.gltf";

			_data = _templateDatas[m_templateIndex];
			_data.ModelURI = _modelURI;

			//-------------------------------------------- 초기화

			// 데이터 할당
			//_core._Platform = platformCode;
			//_core.GraphicMode = graphicCode;
			//_core.CameraMode = input_camMode;

			//-------------------------------------------- 다음 단계

			yield return new WaitForEndOfFrame();

			callback.Invoke();
			yield break;
		}


		#endregion

		#region 0-2 데이터 입력 완료시 실행

		private void SetSystemInstance()
		{
			InitCoreData(_data);

			// 입력 인스턴스 초기화
			InitInputResource(_core._Platform);

			// 카메라 리소스 초기화
			InitCameraResource(_core.CameraMode, cameraExecuteEvents);

			Load_Scene(_core._Platform);
		}

		#endregion

		#region 1 입력 초기화

		private void InitCoreData(CoreData _data)
		{
			_core._Platform = _data.Platform;
			_core.GraphicMode = _data.Graphic;
		}

		/// <summary>
		/// 입력 인스턴스 할당코드 모음
		/// </summary>
		public void InitInputResource(PlatformCode _pCode)
		{
			// TODO : ★ 빌드의 입력장치가 정해졌을때 장치입력을 받는 인스턴스를 생성하도록 개편

			int platformIndex = (int)_pCode / 0x10;

			// platformIndex table
			// 1 : WebGL
			// 2 : Mobile
			// 3 : PC

			if (platformIndex == 1 || platformIndex == 3) // webgl
			{
				InitSingleInputResource<Platform.Feature._Input.Mouse>("mouse");
				InitSingleInputResource<Platform.Feature._Input.Keyboard>("keyboard");
			}
			else if (platformIndex == 2)        // mobile
			{
				// TODO Input Mobile
				Debug.LogError("아직 Mobile 코드가 작성되지 않았습니다. 작성 후 업데이트 필요함.");
			}
		}

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
			SetCamera<FreeCamera>(main, _cameraExecuteEvents);
			SetCamera<OrbitCamera>(main, _cameraExecuteEvents);
			SetCamera<BIMCamera>(main, _cameraExecuteEvents);

			// 코어의 카메라모드 변경으로 모든 카메라의 모드 일괄변경
			_core.CameraMode = _mode;
		}

		private void SetCamera<T>(Camera _main, Events.CameraEvents _camEvents) where T : ICamera
		{
			T component = null;
			// 카메라에 해당 카레마가 등록되지 않았다면
			if (!_main.gameObject.TryGetComponent<T>(out component))
			{
				component = _main.gameObject.AddComponent<T>();
			}

			// 코어 카메라 컴포넌트 등록
			_core.Cameras.Add(component);

			// 카메라에서 연결 대상 메서드 등록
			component.SetAction(_camEvents);
		}

		#endregion
	}
}
