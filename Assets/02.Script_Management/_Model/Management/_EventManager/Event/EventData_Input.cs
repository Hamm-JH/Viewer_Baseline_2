using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;
	using UnityEngine.Events;

	public class EventData_Input : EventData
	{
		public Camera m_camera;
		public GraphicRaycaster m_graphicRaycaster;

		[Header("OnClick")]
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;


		[Header("OnDrag")]
		public int m_btn;
		public Vector2 m_delta;
		UnityEvent<int, Vector2> m_dragEvent;

		[Header("OnFocus")]
		public Vector3 m_focus;
		public float m_focusDelta;
		UnityEvent<Vector3, float> m_focusEvent;

		[Header("OnKey")]
		public List<KeyCode> m_keys;
		UnityEvent<List<KeyCode>> m_keyEvent;

		/// <summary>
		/// 클릭 이벤트 생성자
		/// </summary>
		/// <param name="_eventType"></param>
		public EventData_Input(InputEventType _eventType, 
			int _btn, Vector3 _mousePos,
			Camera _camera, GraphicRaycaster _graphicRaycaster,
			UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _graphicRaycaster;
			m_clickPosition = _mousePos;
			m_clickEvent = _event;
		}

		/// <summary>
		/// 드래그 이벤트 생성자
		/// </summary>
		/// <param name="_eventType"></param>
		/// <param name="_btn"></param>
		/// <param name="_delta"></param>
		/// <param name="_camera"></param>
		/// <param name="_grRaycaster"></param>
		/// <param name="_event"></param>
		public EventData_Input(InputEventType _eventType,
			int _btn, Vector2 _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<int, Vector2> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_btn = _btn;
			m_delta = _delta;
			m_dragEvent = _event;
		}

		/// <summary>
		/// 포커스 이벤트 생성자
		/// </summary>
		/// <param name="_eventType"></param>
		/// <param name="_focus"></param>
		/// <param name="_delta"></param>
		/// <param name="_camera"></param>
		/// <param name="_grRaycaster"></param>
		/// <param name="_event"></param>
		public EventData_Input(InputEventType _eventType,
			Vector3 _focus, float _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<Vector3, float> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_focus = _focus;
			m_focusDelta = _delta;
			m_focusEvent = _event;
		}

		/// <summary>
		/// 키 이벤트 생성자
		/// </summary>
		/// <param name="_eventType"></param>
		/// <param name="_kCode"></param>
		/// <param name="_camera"></param>
		/// <param name="_grRaycaster"></param>
		/// <param name="_event"></param>
		public EventData_Input(InputEventType _eventType,
			List<KeyCode> _kCode,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<List<KeyCode>> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_keys = _kCode;
			m_keyEvent = _event;
		}

		public override void OnProcess(GameObject _cObj)
		{
			switch(EventType)
			{
				case InputEventType.Input_clickDown:
					// 객체 선택 단계에선 code :: skip
					// 데칼 배치 단계에선 code :: skip

					return;

				case InputEventType.Input_clickSuccessUp:
					// 객체 선택 단계에선 code :: pass, drop
					{
						Debug.Log(EventType.ToString());
						GameObject selected3D = null;
						RaycastHit hit = default(RaycastHit);

						Get_Collect3DObject(m_clickPosition, out selected3D, out hit);

						if(selected3D != null)
						{
							IInteractable interactable;
							if(selected3D.TryGetComponent<IInteractable>(out interactable))
							{
								Element = interactable;
								StatusCode = Status.Pass;
								return;
							}
						}
						Element = null;
						StatusCode = Status.Drop;
					}
					// 데칼 배치 단계에선 code :: pass, drop
					break;

				case InputEventType.Input_drag:
					// 객체 선택 단계에선 code :: Update
					// 데칼 배치 단계에선 code :: Update
					StatusCode = Status.Update;
					break;

				case InputEventType.Input_focus:
					// 객체 선택 단계에선 code :: Update
					// 데칼 배치 단계에선 code :: Update
					StatusCode = Status.Update;
					break;

				case InputEventType.Input_key:
					// 객체 선택 단계에선 code :: Update
					// 데칼 배치 단계에선 code :: Update
					StatusCode = Status.Update;
					break;
			}
		}

		/// <summary>
		/// 이벤트 실행 승인시 실행
		/// </summary>
		public override void DoEvent()
		{
			switch(EventType)
			{
				case InputEventType.Input_clickDown:
				case InputEventType.Input_clickFailureUp:
					break;

				case InputEventType.Input_clickSuccessUp:
					m_clickEvent.Invoke(Element.Target);
					break;

				case InputEventType.Input_drag:
					m_dragEvent.Invoke(m_btn, m_delta);
					break;

				case InputEventType.Input_focus:
					m_focusEvent.Invoke(m_focus, m_focusDelta);
					break;

				case InputEventType.Input_key:
					m_keyEvent.Invoke(m_keys);
					break;
			}
		}

		#region Click - 객체 선택

		/// <summary>
		/// UI를 건드렸을 경우를 제외한, 3D 객체 선택상태인지 확인한다.
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="obj"></param>
		/// <param name="_hit"></param>
		private void Get_Collect3DObject(Vector3 _mousePos, out GameObject obj, out RaycastHit _hit)
		{
			obj = null;

			//RaycastHit _hit = default(RaycastHit);
			GameObject _selected3D = Get_GameObject3D(_mousePos, out _hit);
			List<RaycastResult> results = Get_GameObjectUI(_mousePos);

			if (results.Count != 0)
			{

			}
			else
			{
				if (_selected3D != null)
				{
					obj = _selected3D;
				}
			}
		}

		/// <summary>
		/// 3D 객체를 마우스 위치에서 가져옴
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="_hitPoint"></param>
		/// <returns></returns>
		private GameObject Get_GameObject3D(Vector3 _mousePos, out RaycastHit _hitPoint)
		{
			GameObject obj = null;

			// 3D 선택
			RaycastHit _hit;
			Ray _ray = m_camera.ScreenPointToRay(_mousePos);
			if (Physics.Raycast(_ray, out _hit))
			{
				obj = _hit.collider.gameObject;
				_hitPoint = _hit;
				return obj;
			}
			else
			{
				_hitPoint = default(RaycastHit);
				return obj;
			}
		}

		/// <summary>
		/// UI 객체를 마우스 위치에서 가져옴
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <returns></returns>
		private List<RaycastResult> Get_GameObjectUI(Vector3 _mousePos)
		{
			List<RaycastResult> results = new List<RaycastResult>();

			// UI 선택
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = _mousePos;

			m_graphicRaycaster.Raycast(pointerEventData, results);

			return results;
		}

		#endregion
	}
}
