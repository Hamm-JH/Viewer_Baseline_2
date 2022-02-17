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

#if DEBUG_INPUT
			onClickAction += Method_ClickDebug;
			onDragAction += Method_DragDebug;
			onFocusAction += Method_FocusDebug;
			onKeyAction += Method_KeyDebug;
#endif

		}

#region Actions
		public UnityAction<InputEventType, int, Vector3> onClickAction;
		
		public UnityAction<InputEventType, int, Vector2> onDragAction;
		
		public UnityAction<InputEventType, Vector3, float> onFocusAction;
		
		public UnityAction<InputEventType, List<KeyCode>> onKeyAction;
#endregion

#region OnClick actions

		/// <summary>
		/// 클릭
		/// </summary>
		/// <param name="_mousePos"></param>
		public void OnClick(InputEventType type, int btn, Vector3 _mousePos)
		{
			// 필터링 (__추후 변수 추출)
			if(btn == 0)
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

			{
				// 클릭 성공은 좌클릭만 받음
				// 

				//GameObject _selected3D = null;
				//RaycastHit _hit = default(RaycastHit);

				//Get_Collect3DObject(_mousePos, out _selected3D, out _hit);

				////_selected3D = Get_GameObject3D(_mousePos, out _hit);
				////List<RaycastResult> results = Get_GameObjectUI(_mousePos);

				//if (_selected3D != null)
				//{
				//	// 상호작용 가능 객체가 있는지 판단
				//	View.IInteractable interactable;
				//	if (_selected3D.TryGetComponent<View.IInteractable>(out interactable))
				//	{
				//		// 데이터 할당
				//		interactable.Hit = _hit;

				//		//Debug.Log(interactable.Target.name);
				//		EventManager.Instance.OnEvent(new Events.EventData_Input(
				//			_eventType: type,
				//			_btn: btn,
				//			_mousePos: _mousePos,
				//			_camera: main.MainCamera,
				//			_graphicRaycaster: main.Content._GrRaycaster
				//			));

				//		main.cameraExecuteEvents.selectEvent.Invoke(_selected3D);
				//	}
				//}
				//else
				//{
				//	//EventManager.Instance.OnEvent(new Events.EventData_Input(
				//	//	_target: null,
				//	//	_mainEventType: type
				//	//	));
				//}

				//--------------------------------

				//// 마우스에 걸린 UI가 하나 이상일 경우
				//if (results.Count != 0)
				//{
				//	Debug.LogWarning("UI 이벤트 분류 미지정 상태");
				//}
				////else if (_selected3D != null)
				//else
				//{
				//	if(_selected3D != null)
				//	{
				//		// 상호작용 가능 객체가 있는지 판단
				//		View.IInteractable interactable;
				//		if(_selected3D.TryGetComponent<View.IInteractable>(out interactable))
				//		{
				//			// 데이터 할당
				//			interactable.Hit = _hit;

				//			//Debug.Log(interactable.Target.name);
				//			EventManager.Instance.OnEvent(new Events.EventData(interactable));

				//			main.cameraExecuteEvents.selectEvent.Invoke(_selected3D);
				//		}
				//	}
				//	else
				//	{
				//		EventManager.Instance.OnEvent(null);
				//	}

				//	//Debug.Log(_selected3D.name);
				//}

				//-----

				//if (_selected3D != null)
				//{
				//	// 상호작용 가능 객체가 있는지 판단
				//	View.IInteractable interactable;
				//	if (_selected3D.TryGetComponent<View.IInteractable>(out interactable))
				//	{
				//		// 데이터 할당
				//		interactable.Hit = _hit;
				//		//interactable.InputEventType

				//		//Debug.Log(interactable.Target.name);
				//		EventManager.Instance.OnEvent(new Events.EventData(
				//			_target: interactable,
				//			_mainEventType: type
				//			));

				//		main.cameraExecuteEvents.selectEvent.Invoke(_selected3D);
				//	}
				//}
				//else
				//{
				//	EventManager.Instance.OnEvent(null);
				//}

				//if (type == MainEventType.Input_clickDown)
				//{
				//	if (_selected3D != null)
				//	{
				//		// 상호작용 가능 객체가 있는지 판단
				//		View.IInteractable interactable;
				//		if (_selected3D.TryGetComponent<View.IInteractable>(out interactable))
				//		{
				//			// 데이터 할당
				//			interactable.Hit = _hit;
				//			//interactable.InputEventType

				//			//Debug.Log(interactable.Target.name);
				//			EventManager.Instance.OnEvent(new Events.EventData(
				//				_target: interactable,
				//				_mainEventType: type
				//				));

				//			main.cameraExecuteEvents.selectEvent.Invoke(_selected3D);
				//		}
				//	}
				//	else
				//	{
					
				//	}
				//}
				//else if (type == MainEventType.Input_clickSuccessUp)
				//{

				//}
				//else if (type == MainEventType.Input_clickFailureUp)
				//{

				//}
			}
		}

		public void Method_ClickDebug(InputEventType type, int btn, Vector3 _mousePos)
		{
			Debug.Log($"Method click debug : {_mousePos}");
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

		public void Method_DragDebug(InputEventType type, int btn, Vector2 delta)
		{
			Debug.Log($"Method drag debug : {delta}");
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

		public void Method_FocusDebug(InputEventType type, Vector3 focus, float delta)
		{
			Debug.Log($"Method focus debug : {focus}, {delta}");
		}

#endregion

#region OnKey actions

		/// <summary>
		/// 키 입력
		/// </summary>
		/// <param name="_kCode"></param>
		public void OnKey(InputEventType type, List<KeyCode> _kCode)
		{
			// 키 입력 이벤트 실행
			EventManager.Instance.OnEvent(new Events.EventData_Input(
				_eventType: type,
				_kCode: _kCode,
				_camera: main.MainCamera,
				_grRaycaster: main.Content._GrRaycaster,
				_event: main.cameraExecuteEvents.keyEvent
				));
		}

		public void Method_KeyDebug(InputEventType type, List<KeyCode> key)
		{
			string pressedKeys = "";
			foreach(KeyCode _key in key)
			{
				pressedKeys += $"{_key.ToString()}, ";
			}

			Debug.Log($"Method key debug : {pressedKeys}");
		}

#endregion
	}
}
