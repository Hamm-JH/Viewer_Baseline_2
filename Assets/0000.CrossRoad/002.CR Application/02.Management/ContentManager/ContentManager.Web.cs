using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Management
{
	using Definition;
	using Definition._Issue;
	using Management.Events;
	using System;
	using Utilities;
	using View;

	public partial class ContentManager : IManager<ContentManager>
	{
		//[SerializeField] WebManager legacyWeb;

		/// <summary>
		/// 3D 객체 선택시 이벤트 발생
		/// </summary>
		/// <param name="_obj"></param>
		public void OnSelect_3D(GameObject _obj)
		{
			//Debug.Log(_obj?.name);
			_API.SendRequest(SendRequestCode.SelectObject, (object)_obj);
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
				_API.SendRequest(SendRequestCode.SelectIssue,
					iObj.Issue.IssueOrderCode,
					iObj.Issue.CdBridgeParts,
					iObj.Issue.YnRecover);
			}
		}

		public UseCase AppUseCase { get; internal set; }
		//public SceneStatus ViewSceneStatus { get; internal set; }
		public Transform SelectedObject { get; internal set; }
		//public Issue_Selectable CacheIssueEntity { get; internal set; }
		public string DcMemberSurface { get; internal set; }

		public Material PinModeSkyboxMaterial { get; internal set; }
		public Material DefaultSkyboxMaterial { get; internal set; }

		/// <summary>
		/// 이슈 초기화
		/// </summary>
		public void Reset_IssueObject()
		{
			// 기존 Issue Object 리스트 접근, 기존 요소 삭제
			_Model.DeleteIssues();

			// 다시 리스트 초기화
			_API.InitializeModelIssue();
		}

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

		/// <summary>
		/// 웹에서 3D 객체 선택
		/// </summary>
		/// <param name="transform"></param>
		internal void Select3DObject(GameObject _obj)
		{
			EventManager.Instance.OnEvent(new EventData_API(
				InputEventType.API_SelectObject,
				_obj,
				MainManager.Instance.cameraExecuteEvents.selectEvent
				));
		}

		/// <summary>
		/// 웹에서 점검정보 선택
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="v"> ? </param>
		internal void SelectIssue(GameObject _obj, bool _v)
		{
			EventManager.Instance.OnEvent(new EventData_API(
				InputEventType.API_SelectIssue,
				_obj,
				MainManager.Instance.cameraExecuteEvents.selectEvent
				));
		}
		
		// 카메라 각도 변경

		// int 코드 필요
		// string 코드 필요
		// vector3 베이스 각도 필요

		internal void SetCameraMode(GameObject _obj, ViewRotations _vCode, Vector3 _baseAngle)
		{
			UIEventType uType = Parsers.OnParse(_vCode);

			// 카메라에 현재 뷰 모드 업데이트
			MainManager.Instance.UpdateCameraMode(uType);

			// 카메라에 각도변경 지시
			SetCameraCenter(_obj, _baseAngle, uType);
		}

		//internal void DirectionAngle(int v, Vector3 angle)
		//{
		//	throw new NotImplementedException();
		//}

		// 우측 UI창 크기변경
		internal void GetRightWebUIWidth(float width)
		{
			Debug.Log($"정보창 UI폭 변경 :: 값 : {width}");
		}

		// 작업정보 세팅
		internal void SetIssueCode(string issueCode, string issueIndex)
		{
			throw new NotImplementedException();
		}

		// 작업정보 Material 세팅
		internal void SetIssueMaterial(MeshRenderer _render, IssueType _issueType, IssueCodes _issueCode)
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

		// 점검정보 On/Off
		internal void Toggle_Issues(IssueVisualizeOption _option)
		{

		}

		// 카메라 모드 변경
		internal void SetCameraMode(int v)
		{
			
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

			// TODO :: CHECK :: ★★ 선택 객체, 선택 면의 존재하는 손상정보 반환하기

			return result;
		}
	}
}
