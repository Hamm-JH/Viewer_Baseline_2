#define DEBUG_INPUT
#undef DEBUG_INPUT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Control._Input;
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
		public UnityAction<Vector3> onClickAction;
		
		public UnityAction<int, Vector2> onDragAction;
		
		public UnityAction<Vector3, float> onFocusAction;
		
		public UnityAction<List<KeyCode>> onKeyAction;
#endregion

#region OnClick actions

		/// <summary>
		/// 클릭
		/// </summary>
		/// <param name="_mousePos"></param>
		public void OnClick(Vector3 _mousePos)
		{
			//Debug.Log($"OnClick method");

			GameObject _selected3D = null;
			//GameObject _selectedUI = null;

			_selected3D = Get_GameObject3D(_mousePos);

			List<RaycastResult> results = Get_GameObjectUI(_mousePos);

			// 마우스에 걸린 UI가 하나 이상일 경우
			if (results.Count != 0)
			{
				Debug.LogWarning("UI 이벤트 분류 미지정 상태");
			}
			//else if (_selected3D != null)
			else
			{
				if(_selected3D != null)
				{
					// 상호작용 가능 객체가 있는지 판단
					View.IInteractable interactable;
					if(_selected3D.TryGetComponent<View.IInteractable>(out interactable))
					{
						//Debug.Log(interactable.Target.name);
						EventManager.Instance.OnEvent(new Events.EventData(interactable));
					}
				}
				else
				{
					EventManager.Instance.OnEvent(null);
				}

				//Debug.Log(_selected3D.name);
				main.cameraExecuteEvents.selectEvent.Invoke(_selected3D);
			}

			
			// 3D, UI객체 선택 확인
			// 1개일 경우 - 선택된 객체의 클릭 이벤트 전달
			// 2개일 경우 - 3D 또는 UI중에 한 객체의 이벤트 전달
			

			// 카메라에 이벤트 전달
			//main.cameraExecuteEvents.clickEvent.Invoke(_mousePos);
		}

		public void Method_ClickDebug(Vector3 _mousePosition)
		{
			Debug.Log($"Method click debug : {_mousePosition}");
		}

		private GameObject Get_GameObject3D(Vector3 _mousePos)
		{
			GameObject obj = null;

			// 3D 선택
			RaycastHit _hit;
			Ray _ray = main.MainCamera.ScreenPointToRay(_mousePos);
			if (Physics.Raycast(_ray, out _hit, 100))
			{
				obj = _hit.collider.gameObject;
				return obj;
			}
			else
			{
				return obj;
			}
		}

		private List<RaycastResult> Get_GameObjectUI(Vector3 _mousePos)
		{
			List<RaycastResult> results = new List<RaycastResult>();

			// UI 선택
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = _mousePos;

			main.Content._GrRaycaster.Raycast(pointerEventData, results);

			return results;
		}

#endregion

#region OnDrag actions

		/// <summary>
		/// 드래그
		/// </summary>
		/// <param name="_delta"></param>
		public void OnDrag(int btn, Vector2 _delta)
		{
			//Debug.Log($"OnDrag method");

			// 카메라의 드래그 이벤트 실행
			main.cameraExecuteEvents.dragEvent.Invoke(btn, _delta);
		}

		public void Method_DragDebug(int btn, Vector2 delta)
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
		public void OnFocus(Vector3 _focus, float _delta)
		{
			// 카메라의 포커스 이벤트 실행
			main.cameraExecuteEvents.focusEvent.Invoke(_focus, _delta);
		}

		public void Method_FocusDebug(Vector3 focus, float delta)
		{
			Debug.Log($"Method focus debug : {focus}, {delta}");
		}

#endregion

#region OnKey actions

		/// <summary>
		/// 키 입력
		/// </summary>
		/// <param name="_kCode"></param>
		public void OnKey(List<KeyCode> _kCode)
		{
			// 키 입력 이벤트 실행
			main.cameraExecuteEvents.keyEvent.Invoke(_kCode);
		}

		public void Method_KeyDebug(List<KeyCode> key)
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
