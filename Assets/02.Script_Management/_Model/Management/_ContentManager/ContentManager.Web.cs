using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneStatus
{
	EnvironmentVariablesSetting = 0,        // 환경변수 세팅
	GetGLTF = 1,                            // GLTF 파일 받아오기
	ObjectInitialize = 2,                   // GLTF 받은 객체 초기화
	Ready = 3,                              // Viewer Scene 초기화 완료 상태 / 기본 상태

	Register = 4,                           // 손상/보강 등록 상태
	Register_PinMode = 5,                   // 손상/보강 핀 등록 상태
	Default = 6,
	Modified = 7,
	Delete = 8,
}

public enum UseCase
{
	NotDefined = -1,
	Bridge = 0,
	Tunnel = 1
}

public enum ViewRotations
{
	//ALL,
	Top,
	Bottom,
	Front,
	Back,
	Left,
	Right
}

public enum BoolValue
{
	True,
	False
}

public enum IssueVisualizeOption
{
	All_ON,
	All_OFF,
	SelectedTarget
}

namespace Management
{
	using Definition._Issue;
	using Issues;
	using System;
	using View;

	public partial class ContentManager : IManager<ContentManager>
	{
		[SerializeField] WebManager legacyWeb;

		/// <summary>
		/// 3D 객체 선택시 이벤트 발생
		/// </summary>
		/// <param name="_obj"></param>
		public void OnSelect_3D(GameObject _obj)
		{
			legacyWeb.SendRequest(SendRequestCode.SelectObject, _obj);
		}

		/// <summary>
		/// Issue 객체 선택시 이벤트 발생
		/// </summary>
		/// <param name="_obj"></param>
		public void OnSelect_Issue(GameObject _obj)
		{
			Issue_Selectable iObj;
			if (_obj.TryGetComponent<Issue_Selectable>(out iObj))
			{
				legacyWeb.SendRequest(SendRequestCode.SelectIssue,
					iObj.Issue.IssueOrderCode,
					iObj.Issue.CdBridgeParts,
					iObj.Issue.YnRecover);
			}
		}

		public UseCase AppUseCase { get; internal set; }
		public SceneStatus ViewSceneStatus { get; internal set; }
		public Transform SelectedObject { get; internal set; }
		public Issue_Selectable CacheIssueEntity { get; internal set; }
		public string DcMemberSurface { get; internal set; }

		public Material PinModeSkyboxMaterial { get; internal set; }
		public Material DefaultSkyboxMaterial { get; internal set; }

		// 캠 위치 초기화
		internal void InitCamPosition()
		{
			throw new NotImplementedException();
		}

		// 손상정보 초기화?
		internal void SetIssueObject()
		{
			throw new NotImplementedException();
		}

		// 3D 객체 선택
		internal void Select3DObject(Transform transform)
		{
			throw new NotImplementedException();
		}

		// Issue 객체 선택
		internal void SelectIssue(Transform transform, bool v)
		{
			throw new NotImplementedException();
		}
		
		// 카메라 각도 변경
		internal void DirectionAngle(int v, Vector3 angle)
		{
			throw new NotImplementedException();
		}

		// 큐브 이미지 세팅
		internal void SetInspectionImage(int v)
		{
			throw new NotImplementedException();
		}

		// 우측 UI창 크기변경
		internal void GetRightWebUIWidth(float width)
		{
			throw new NotImplementedException();
		}

		// 작업정보 세팅
		internal void SetIssueCode(string issueCode, string issueIndex)
		{
			throw new NotImplementedException();
		}

		// 작업정보 Material 세팅
		internal void SetIssueMaterial(MeshRenderer _render, IssueType _issueType, Code _issueCode)
		{

		}

		// 9면큐브 스위치
		internal void SwitchGuideCubeOnOff(bool v)
		{
			throw new NotImplementedException();
		}

		// 특정 면 대상으로 카메라 위치변환
		internal void ChangeCameraDirection(string side, Bounds nB, Vector3 eulerAngles, bool isEtc)
		{
			throw new NotImplementedException();
		}

		// 치수선 On/Off
		internal void Toggle_Dimension(bool _isOn)
		{

		}

		// 점검정보 On/Off
		internal void Toggle_Issues(IssueVisualizeOption _option)
		{

		}

		// 카메라 모드 변경
		internal void SetCameraMode(int v)
		{
			throw new NotImplementedException();
		}

		// 카메라 줌
		internal void ZoomCamera(Vector3 centerVector, float maxValue, bool v1, bool v2)
		{
			throw new NotImplementedException();
		}

		internal List<string> RequestAvailableSurface()
		{
			List<string> result = new List<string> { "Top", "Bottom", "Left", "Right", "Front", "Back" };

			return result;
		}

		internal List<string> GetTargetIssues(string shapeCode)
		{
			List<string> result = new List<string> { "Crack,1", "Spalling,3" };

			Debug.LogError("GetTargetissues");

			return result;
		}
	}
}
