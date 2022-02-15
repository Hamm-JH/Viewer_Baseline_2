using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using UnityEngine.Events;


	public partial class MainManager : IManager<MainManager>
	{
		// MainManager.Events_Input에 존재하는 이벤트에 관련된 이벤트 연산 메서드

		/// <summary>
		/// 주관리자 시작시 이벤트 초기화
		/// </summary>
		public void Event_Initialize()
		{
			inputEvents = new Events.InputEvents(
				_mouse: _data.MouseData,
				_keyboard: _data.KeyboardData );

			cameraExecuteEvents = new Events.CameraEvents(
				_camera: _data.CameraData );
		}

		/// <summary>
		/// 이벤트에 액션 추가
		/// </summary>
		/// <param name="_type"></param>
		/// <param name="_action"></param>
		//public void Event_Add(InputEventType _type, object _action)
		//{
		//	bool isAssignFailed = false;

		//	if (_type == InputEventType.Input_click)
		//	{
		//		UnityAction<InputEventType, int, Vector3> unityAction = _action as UnityAction<InputEventType, int, Vector3>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.clickEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_drag)
		//	{
		//		UnityAction<InputEventType, int, Vector2> unityAction = _action as UnityAction<InputEventType, int, Vector2>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.dragEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_focus)
		//	{
		//		UnityAction<InputEventType, Vector3, float> unityAction = _action as UnityAction<InputEventType, Vector3, float>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.focusEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_key)
		//	{
		//		UnityAction<InputEventType, List<KeyCode>> unityAction = _action as UnityAction<InputEventType, List<KeyCode>>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.keyEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}

		//	if (isAssignFailed)
		//	{
		//		Debug.LogError("UnityAction to UnityEvent is failed");
		//	}
		//}

		/// <summary>
		/// 이벤트에 액션 업데이트
		/// </summary>
		/// <param name="_type"></param>
		/// <param name="_action"></param>
		//public void Event_Update(InputEventType _type, object _action)
		//{
		//	bool isAssignFailed = false;

		//	if (_type == InputEventType.Input_click)
		//	{
		//		UnityAction<InputEventType, int, Vector3> unityAction = _action as UnityAction<InputEventType, int, Vector3>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.clickEvent.RemoveListener(unityAction);
		//			inputEvents.clickEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_drag)
		//	{
		//		UnityAction<InputEventType, int, Vector2> unityAction = _action as UnityAction<InputEventType, int, Vector2>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.dragEvent.RemoveListener(unityAction);
		//			inputEvents.dragEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_focus)
		//	{
		//		UnityAction<InputEventType, Vector3, float> unityAction = _action as UnityAction<InputEventType, Vector3, float>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.focusEvent.RemoveListener(unityAction);
		//			inputEvents.focusEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_key)
		//	{
		//		UnityAction<InputEventType, List<KeyCode>> unityAction = _action as UnityAction<InputEventType, List<KeyCode>>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.keyEvent.RemoveListener(unityAction);
		//			inputEvents.keyEvent.AddListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}

		//	if (isAssignFailed)
		//	{
		//		Debug.LogError("UnityAction to UnityEvent is failed");
		//	}
		//}

		/// <summary>
		/// 이벤트에 특정 액션 삭제
		/// </summary>
		/// <param name="_type"></param>
		/// <param name="_action"></param>
		//public void Event_Delete(InputEventType _type, object _action)
		//{
		//	bool isAssignFailed = false;

		//	if (_type == InputEventType.Input_click)
		//	{
		//		UnityAction<InputEventType, int, Vector3> unityAction = _action as UnityAction<InputEventType, int, Vector3>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.clickEvent.RemoveListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_drag)
		//	{
		//		UnityAction<InputEventType, int, Vector2> unityAction = _action as UnityAction<InputEventType, int, Vector2>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.dragEvent.RemoveListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_focus)
		//	{
		//		UnityAction<InputEventType, Vector3, float> unityAction = _action as UnityAction<InputEventType, Vector3, float>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.focusEvent.RemoveListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}
		//	else if (_type == InputEventType.Input_key)
		//	{
		//		UnityAction<InputEventType, List<KeyCode>> unityAction = _action as UnityAction<InputEventType, List<KeyCode>>;

		//		if (unityAction != null)
		//		{
		//			inputEvents.keyEvent.RemoveListener(unityAction);
		//		}
		//		else isAssignFailed = true;
		//	}

		//	if (isAssignFailed)
		//	{
		//		Debug.LogError("UnityAction to UnityEvent is failed");
		//	}
		//}

		/// <summary>
		/// 이벤트의 모든 액션 삭제
		/// </summary>
		/// <param name="_type"></param>
		//public void Event_DeleteAll(InputEventType _type)
		//{
		//	if (_type == InputEventType.Input_click)
		//	{
		//		inputEvents.clickEvent.RemoveAllListeners();
		//	}
		//	else if (_type == InputEventType.Input_drag)
		//	{
		//		inputEvents.dragEvent.RemoveAllListeners();
		//	}
		//	else if (_type == InputEventType.Input_focus)
		//	{
		//		inputEvents.focusEvent.RemoveAllListeners();
		//	}
		//	else if (_type == InputEventType.Input_key)
		//	{
		//		inputEvents.keyEvent.RemoveAllListeners();
		//	}
		//}
	}
}
