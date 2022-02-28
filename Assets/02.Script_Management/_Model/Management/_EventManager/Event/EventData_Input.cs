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
	using System.Linq;

	[System.Serializable]
	public class EventData_Input : EventData
	{
		public Camera m_camera;
		public GraphicRaycaster m_graphicRaycaster;

		public GameObject m_cache;

		[Header("OnClick")]
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;


		[Header("OnDrag")]
		//public int m_btn;
		public Vector2 m_delta;
		UnityEvent<int, Vector2> m_dragEvent;

		[Header("OnFocus")]
		public Vector3 m_focus;
		public float m_focusDelta;
		UnityEvent<Vector3, float> m_focusEvent;

		[Header("OnKey")]
		public List<KeyData> m_keys;
		UnityEvent<List<KeyData>> m_keyEvent;

		

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
			List<KeyData> _kData,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<List<KeyData>> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_graphicRaycaster = _grRaycaster;
			m_keys = _kData;
			m_keyEvent = _event;
		}

		public override void OnProcess(GameObject _cObj)
		{
			switch(EventType)
			{
				case InputEventType.Input_clickDown:
					// 객체 선택 단계에선 code :: pass, skip
					{
						Status _success = Status.Update;
						Status _fail = Status.Update;

						Debug.Log(EventType.ToString());
						m_selected3D = null;
						m_hit = default(RaycastHit);
						m_results = new List<RaycastResult>();

						Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

						if (Selected3D != null)
						{
							IInteractable interactable;
							if (Selected3D.TryGetComponent<IInteractable>(out interactable))
							{
								Element = interactable;
								StatusCode = _success;
								return;
							}
						}
						Element = null;
						StatusCode = _fail;
					}
					// 데칼 배치 단계에선 code :: skip

					return;

				case InputEventType.Input_clickSuccessUp:
					// 객체 선택 단계에선 code :: pass, drop
					{
						Status _success = Status.Pass;
						Status _fail = Status.Drop;

						Debug.Log(EventType.ToString());
						m_selected3D = null;
						m_hit = default(RaycastHit);
						m_results = new List<RaycastResult>();

						Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

						if(Selected3D != null)
						{
							IInteractable interactable;
							if(Selected3D.TryGetComponent<IInteractable>(out interactable))
							{
								Elements = new List<IInteractable>();
								Elements.Add(interactable);

								Element = interactable;

								m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
								m_clickEvent.AddListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
								StatusCode = _success;
								return;
							}
						}
						// 빈 공간을 누른 경우
						else if(m_results.Count == 0)
						{
							Element = null;
							Elements = null;
							StatusCode = Status.Pass;
							//StatusCode = _fail;
						}
						// UI 객체를 누른 경우 m_results.Count != 0
						else
						{
							Element = null;
							Elements = null;
							StatusCode = Status.Skip;
						}
					}
					// 데칼 배치 단계에선 code :: pass, drop
					break;

				case InputEventType.Input_clickFailureUp:
					{
						// 객체 선택 단계에선 code :: Update
						// 데칼 배치 단계에선 code :: Update
						StatusCode = Status.Update;
					}
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

					// KeyData는 동작 구조 파악을 위해 조건을 명시함. 실제 처리는 EventManager에서 처리
					// KeyData 리스트가 null 또는 0이 아닌 경우
					if(m_keys != null && m_keys.Count != 0)
					{
						StatusCode = Status.Update;
					}
					// KeyData 리스트가 null인 경우
					else
					{
						StatusCode = Status.Update;
					}
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
					// 객체를 올바르게 선택한 경우
					if(Elements != null)
					{
						m_clickEvent.Invoke(Elements.Last().Target);
						ContentManager.Instance.Toggle_ChildTabs(1);
					}
					// 빈 공간을 선택한 경우
					else
					{
						m_clickEvent.Invoke(null);
						ContentManager.Instance.Toggle_ChildTabs(1);
					}
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

		public override void DoEvent(List<GameObject> _objs)
		{
			
		}

		public override void DoEvent(Dictionary<InputEventType, EventData> _sEvents)
		{
			
		}

		//#region Click + Key - 다중 객체 선택

		//private bool isMultiCondition(Dictionary<InputEventType, EventData> _sEvents)
		//{
		//	List<KeyData> kd = null;
		//	bool result = false;

		//	// Control 누르고 있는 상태인가?
		//	if (_sEvents.ContainsKey(InputEventType.Input_key))
		//	{
		//		EventData_Input _ev = (EventData_Input)_sEvents[InputEventType.Input_key];
		//		kd = _ev.m_keys;
		//	}

		//	// 입력 키 존재하는 경우
		//	if (kd != null)
		//	{
		//		KeyCode targetCode = MainManager.Instance.Data.KeyboardData.keyCtrl;

		//		// TODO 0228 MNum
		//		// 키 정보중에 LeftControl이 존재하는가?
		//		if (kd.Find(x => x.m_keyCode == targetCode) != null)
		//		{
		//			result = true;
		//		}
		//	}

		//	return result;
		//}

		//#endregion

		#region Click - 객체 선택

		/// <summary>
		/// UI를 건드렸을 경우를 제외한, 3D 객체 선택상태인지 확인한다.
		/// </summary>
		/// <param name="_mousePos"></param>
		/// <param name="obj"></param>
		/// <param name="_hit"></param>
		private void Get_Collect3DObject(Vector3 _mousePos, out GameObject obj, out RaycastHit _hit, out List<RaycastResult> _results)
		{
			obj = null;

			//RaycastHit _hit = default(RaycastHit);
			GameObject _selected3D = Get_GameObject3D(_mousePos, out _hit);
			_results = Get_GameObjectUI(_mousePos);

			if (_results.Count != 0)
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

			Debug.Log($"***** raycast count : {results.Count}");

			return results;
		}

		





		#endregion
	}
}
