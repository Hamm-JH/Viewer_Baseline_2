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
					iObj.Issue.IsDmg
					/*iObj.Issue.YnRecover*/);
			}

			if (iObj == null) return;

			// TODO 손상정보가 존재할시 아래 선택지 필요
			// 선택 객체가 손상인지, 보수인지 알아야 함(변경 완료)
			// 선택 객체가 어떤 객체인지 이름을 알아야 함(이건 이미 있음)
		}

		//public UseCase AppUseCase { get; internal set; }
		//public SceneStatus ViewSceneStatus { get; internal set; }
		public Transform SelectedObject { get; internal set; }
		
		/// <summary>
		/// 6면 정보
		/// </summary>
		public string DcMemberSurface { get; internal set; }

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

		/// <summary>
		/// 카메라 위치 초기화
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		internal void InitCamPosition()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 웹에서 3D 객체 선택
		/// </summary>
		/// <param name="transform">선택된 3D 객체</param>
		internal void Select3DObject(GameObject _obj)
		{
			EventManager.Instance.OnEvent(new EventData_API(
				InputEventType.API_SelectObject,
				_obj,
				MainManager.Instance.cameraExecuteEvents.selectEvent
				));
		}

		/// <summary>
		/// 웹에서 손상정보 선택
		/// </summary>
		/// <param name="transform">선택된 손상정보</param>
		internal void SelectIssue(GameObject _obj)
		{
			EventManager.Instance.OnEvent(new EventData_API(
				InputEventType.API_SelectIssue,
				_obj,
				MainManager.Instance.cameraExecuteEvents.selectEvent
				));
		}
		
		/// <summary>
		/// 카메라 각도 변경
		/// </summary>
		/// <param name="_obj">선택된 객체</param>
		/// <param name="_vCode">주시 각도</param>
		/// <param name="_baseAngle">기본 각도</param>
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
		
		/// <summary>
		/// 작업정보 Material 설정
		/// </summary>
		/// <param name="_render"></param>
		/// <param name="_issueType"></param>
		/// <param name="_issueCode"></param>
		internal void SetIssueMaterial(MeshRenderer _render, IssueType _issueType, IssueCodes _issueCode)
		{

		}

		/// <summary>
		/// 손상정보 On/Off
		/// </summary>
		/// <param name="_option">손상정보 표시 옵션</param>
		internal void Toggle_Issues(IssueVisualizeOption _option)
		{

		}

		/// <summary>
		/// 선택 면에 존재하는 손상정보 반환
		/// </summary>
		/// <param name="shapeCode">면 정보</param>
		/// <returns>손상 정보 리스트 반환</returns>
		internal List<string> GetTargetIssues(string shapeCode)
		{
			List<string> result = new List<string> { "Crack,1", "Spalling,3" };

			// TODO :: CHECK :: ★★ 선택 객체, 선택 면의 존재하는 손상정보 반환하기

			return result;
		}
	}
}
