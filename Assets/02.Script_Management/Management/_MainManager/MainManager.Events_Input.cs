#define DEBUG_INPUT
#undef DEBUG_INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Platform.Feature;
	using Definition;
	using Definition.Data;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	/// <summary>
	/// 컨텐츠의 핵심 데이터를 관리하는 관리자 클래스
	/// </summary>
	public partial class MainManager : IManager<MainManager>
	{
		[Header("Input Events")]
		/// <summary>
		/// 입력 발생시 실행되는 이벤트 인스턴스
		/// </summary>
		public Events.InputEvents inputEvents;

		/// <summary>
		/// 입력 발생시 실행되는 메서드 집합
		/// </summary>
		public InputCallbackActions inputCallback;

		/// <summary>
		/// 입력 이벤트 초기화, 할당
		/// </summary>
		/// <param name="index"></param>
		public void SetAction(ManagerActionIndex index)
		{
			// 입력 액션 초기화
			if(index == ManagerActionIndex.InputAction)
			{
				// 콜백 액션 초기화
				inputCallback = new InputCallbackActions(this);

				// 콜백 액션을 이벤트와 연결
				inputEvents.clickEvent.AddListener	(inputCallback.onClickAction);
				inputEvents.dragEvent.AddListener	(inputCallback.onDragAction);
				inputEvents.focusEvent.AddListener	(inputCallback.onFocusAction);
				inputEvents.keyEvent.AddListener	(inputCallback.onKeyAction);
			}
		}

	}

	[System.Serializable]
	/// <summary>
	/// 입력 이벤트 발생시 발생하는 이벤트를 받아올 액션 모음 처리
	/// </summary>
	public class InputCallbackActions
	{
		/// <summary>
		/// 주관리자에 접속하기 위한 인스턴스
		/// </summary>
		[SerializeField] MainManager main;

		[SerializeField] GameObject cache;

		public InputCallbackActions(MainManager _main)
		{
			main = _main;

			SetCallbackAction();
		}

		/// <summary>
		/// 콜백 액션 초기화
		/// </summary>
		public void SetCallbackAction()
		{
			onClickAction += OnClick;
			onDragAction += OnDrag;
			onFocusAction += OnFocus;
			onKeyAction += OnKey;

		}

#region Actions
		public UnityAction<InputEventType, int, Vector3> onClickAction;
		
		public UnityAction<InputEventType, int, Vector2> onDragAction;
		
		public UnityAction<InputEventType, Vector3, float> onFocusAction;
		
		public UnityAction<InputEventType, List<KeyData>> onKeyAction;
#endregion

#region OnClick actions

		/// <summary>
		/// 클릭
		/// </summary>
		/// <param name="_mousePos"></param>
		public void OnClick(InputEventType type, int btn, Vector3 _mousePos)
		{
			//Debug.Log("OnClick");
			if(type == InputEventType.Input_clickDown)
			{
				EventManager.Instance.OnEvent(new Events.Inputs.Event_ClickDown(
					_eType: type,
					_btn: btn,
					_mousePos: _mousePos,
					_camera: main.MainCamera,
					_grRaycaster: main.Content._GrRaycaster,
					_event: main.cameraExecuteEvents.selectEvent
					));
			}
			else
			{
				EventManager.Instance.OnEvent(new Events.EventData_Input(
						_eventType: type,
						_btn: btn,
						_mousePos: _mousePos,
						_camera: main.MainCamera,
						_graphicRaycaster: main.Content._GrRaycaster,
						_event: main.cameraExecuteEvents.selectEvent
						));
			}
			// 필터링 (__추후 변수 추출)
			//if(btn == 0)
			//{
			//}
		}

#endregion

#region OnDrag actions

		/// <summary>
		/// 드래그
		/// </summary>
		/// <param name="_delta"></param>
		public void OnDrag(InputEventType type, int btn, Vector2 _delta)
		{
			// 카메라의 드래그 이벤트 실행
			EventManager.Instance.OnEvent(new Events.EventData_Input(
				_eventType: type,
				_btn: btn,
				_delta: _delta,
				_camera: main.MainCamera, 
				_grRaycaster: main.Content._GrRaycaster,
				_event: main.cameraExecuteEvents.dragEvent
				));
		}

#endregion

#region OnFocus actions

		/// <summary>
		/// 포커스
		/// </summary>
		/// <param name="_fucus"></param>
		/// <param name="_delta"></param>
		public void OnFocus(InputEventType type, Vector3 _focus, float _delta)
		{
			// 카메라의 포커스 이벤트 실행
			EventManager.Instance.OnEvent(new Events.EventData_Input(
				_eventType: type,
				_focus: _focus,
				_delta: _delta,
				_camera: main.MainCamera,
				_grRaycaster: main.Content._GrRaycaster,
				_event: main.cameraExecuteEvents.focusEvent
				));
			//main.cameraExecuteEvents.focusEvent.Invoke(_focus, _delta);
		}

#endregion

#region OnKey actions

		/// <summary>
		/// 키 입력
		/// </summary>
		/// <param name="_kCode"></param>
		public void OnKey(InputEventType type, List<KeyData> _kData)
		{
			//_kData.ForEach(x => Debug.Log($"state : {x.m_keyState.ToString()}, key : {x.m_keyCode.ToString()}"));

			// 키 입력 이벤트 실행
			EventManager.Instance.OnEvent(new Events.EventData_Input(
				_eventType: type,
				_kData: _kData,
				_camera: main.MainCamera,
				_grRaycaster: main.Content._GrRaycaster,
				_event: main.cameraExecuteEvents.keyEvent
				));
		}

#endregion
	}
}
