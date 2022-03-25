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
	using Items;

	[System.Serializable]
	public class EventData_Input : AEventData
	{
		public Camera m_camera;
		public GraphicRaycaster m_graphicRaycaster;

		public GameObject m_cache;

		[Header("OnClick")]
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;

		// API 단에서 선택되었는지를 확인하는 변수
		bool m_isPassAPI;

		public bool IsPassedAPI
		{
			get => m_isPassAPI;
		}

		// API 단에서 선택된 객체
		GameObject m_inAPISelected;
		

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

			m_isPassAPI = false;
		}

		/// <summary>
		/// API에서 넘겨받은 객체정보로 선택 이벤트를 처리한다.
		/// </summary>
		/// <param name="_eventType"></param>
		/// <param name="_obj"></param>
		/// <param name="_event"></param>
		public EventData_Input(InputEventType _eventType,
			GameObject _obj, UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_inAPISelected = _obj;
			m_clickEvent = _event;

			m_isPassAPI = true;
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

		public override void OnProcess(List<ModuleCode> _mList)
		{
			switch(EventType)
			{
				case InputEventType.Input_clickDown:
					// 객체 선택 단계에선 code :: pass, skip
					{
						Status _success = Status.Update;
						Status _fail = Status.Update;

						//Debug.Log(EventType.ToString());
						Elements = new List<IInteractable>();
						m_selected3D = null;
						m_hit = default(RaycastHit);
						m_results = new List<RaycastResult>();

						Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

						if (Selected3D != null)
						{
							IInteractable interactable;
							if (Selected3D.TryGetComponent<IInteractable>(out interactable))
							{
								Elements.Add(interactable);
								//Element = interactable;
								StatusCode = _success;
								return;
							}
						}

						Elements = null;
						StatusCode = _fail;
					}
					// 데칼 배치 단계에선 code :: skip

					return;

				case InputEventType.Input_clickSuccessUp:
					// 객체 선택 단계에선 code :: pass, drop
					{
						Status _success = Status.Pass;
						//Status _fail = Status.Drop;

						// PinMode가 아닌 경우
						if(!IsInPinMode(_mList))
						{
							if(m_isPassAPI)
							{
								m_selected3D = m_inAPISelected;

								if(Selected3D != null)
								{
									Func_Input_clickSuccessUp(_success);
								}
							}
							else
							{
								m_selected3D = null;
								m_hit = default(RaycastHit);
								m_results = new List<RaycastResult>();

								Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

								if(Selected3D != null)
								{
									Func_Input_clickSuccessUp(_success);
								}
								// 빈 공간을 누른 경우
								else if(m_results.Count == 0)
								{
									//Element = null;
									Elements = null;
									StatusCode = Status.Pass;
									//StatusCode = _fail;
								}
								// UI 객체를 누른 경우 m_results.Count != 0
								else
								{
									//Element = null;
									Elements = null;
									StatusCode = Status.Pass;
								}
							}
						}
						// PinMode인 경우
						else
						{
							List<RaycastHit> hits;
							Ray ray = Camera.main.ScreenPointToRay(m_clickPosition);

							// 해당 포인터에서 일직선 상으로 레이캐스트
							hits = Physics.RaycastAll(ray).ToList();
							// 레이캐스트 결과가 1개 이상일 경우
							if (hits.Count != 0)
							{
								RaycastHit hit_element = default(RaycastHit);
								RaycastHit hit_selectable = default(RaycastHit);
								LocationElement element = null;
								Obj_Selectable selectable = null;

								hit_element = hits.Find(x => x.collider.gameObject.TryGetComponent<LocationElement>(out element));
								hit_selectable = hits.Find(x => x.collider.gameObject.TryGetComponent<Obj_Selectable>(out selectable));

								//element = hits.Find(x => x.collider.gameObject.TryGetComponent<LocationElement>(out element)).collider.GetComponent<LocationElement>();
								//selectable = hits.Find(x => x.collider.TryGetComponent<Obj_Selectable>(out selectable)).collider.GetComponent<Obj_Selectable>();

								// 위치값과 3D 객체가 모두 눌린 경우
								if (element && selectable)
								{
									EventStatement _state = EventManager.Instance._Statement;

									GameObject cache = _state.CachePin;

									Debug.Log($"element index : {element.Index + 1}, selectable : {selectable.name}");
									if(cache)
									{
										cache.transform.position = hit_selectable.point;
									}
									else
									{
										GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
										obj.name = "Issue element";
										obj.transform.position = hit_selectable.point;

										_state.CachePin = obj;
									}
									//Debug.Log($"element : {element.name}, selectable : {selectable.name}");

									// TODO 데이터 집어넣고, 캐싱
								}

							}
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
		/// 모듈코드에서 PinMode가 존재하는가
		/// </summary>
		/// <param name="_mList"></param>
		/// <returns></returns>
		private bool IsInPinMode(List<ModuleCode> _mList)
		{
			bool result = false;

			if(_mList.Contains(ModuleCode.Work_Pinmode))
			{
				result = true;
			}

			return result;
		}

		private void Func_Input_clickSuccessUp(Status _success)
		{
			Obj_Selectable sObj;
			Issue_Selectable iObj;

			if (Selected3D.TryGetComponent<Obj_Selectable>(out sObj))
			{
				Elements = new List<IInteractable>();
				Elements.Add(sObj);

				//Element = sObj;

				PlatformCode _pCode = MainManager.Instance.Platform;
				if(Platforms.IsSmartInspectPlatform(_pCode))
				{
					m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
					m_clickEvent.AddListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				}
				StatusCode = _success;
				return;
			}
			else if (Selected3D.TryGetComponent<Issue_Selectable>(out iObj))
			{
				Elements = new List<IInteractable>();
				Elements.Add(iObj);

				//Element = iObj;

				m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				StatusCode = _success;
				return;
			}
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			switch(EventType)
			{
				case InputEventType.Input_clickSuccessUp:
					{
						if (Elements != null)
						{
							GameObject obj = Elements.Last().Target;

							Obj_Selectable sObj;
							Issue_Selectable iObj;

							if (obj.TryGetComponent<Obj_Selectable>(out sObj))
							{
								m_clickEvent.Invoke(Elements.Last().Target);
								// TODO
							
								if(IsPassedAPI)
								{
									ContentManager.Instance.Input_SelectObject(Elements.Last().Target);
								}

								ContentManager.Instance.OnSelect_3D(Elements.Last().Target);
								//ContentManager.Instance.Toggle_ChildTabs(1);
							}
							else if (obj.TryGetComponent<Issue_Selectable>(out iObj))
							{
								if (IsPassedAPI)
								{
									ContentManager.Instance.Input_SelectObject(Elements.Last().Target);
								}

								m_clickEvent.Invoke(Elements.Last().Target);
								ContentManager.Instance.OnSelect_Issue(Elements.Last().Target);
								//ContentManager.Instance.Toggle_ChildTabs(1);
							}
						}
						// 빈 공간을 선택한 경우
						else
						{
							// UI 선택의 결과가 0 이상인 경우
							if (Results.Count != 0)
							{
								if(_sEvents.ContainsKey(InputEventType.Input_clickDown))
								{
									List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

									if(IsClickOnKeymap(hits))
									{
										ContentManager.Instance.Input_KeymapClick(m_clickPosition);
									}
								}
							}
							else
							{
								m_clickEvent.Invoke(null);
								ContentManager.Instance.OnSelect_3D(null);
							}
						}
					}
					break;

				case InputEventType.Input_drag:
					{
						PlatformCode pCode = MainManager.Instance.Platform;
						if(Platforms.IsDemoAdminViewer(pCode))
						{
							if(_sEvents.ContainsKey(InputEventType.Input_clickDown))
							{
								List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

								if(IsClickOnKeymap(hits))
								{
									ContentManager.Instance.Input_KeymapDrag(m_btn, m_delta);
								}
								else
								{
									m_dragEvent.Invoke(m_btn, m_delta);
								}
							}
							else
							{
								m_dragEvent.Invoke(m_btn, m_delta);
							}
						}
					}
					break;

				case InputEventType.Input_focus:
					{
						m_selected3D = null;
						m_hit = default(RaycastHit);
						m_results = new List<RaycastResult>();

						Get_Collect3DObject(Input.mousePosition, out m_selected3D, out m_hit, out m_results);

						if (Selected3D != null)
						{
							m_focusEvent.Invoke(m_focus, m_focusDelta);
						}
						// 빈 공간을 누른 경우
						else if (m_results.Count == 0)
						{
							m_focusEvent.Invoke(m_focus, m_focusDelta);
						}
						// UI 객체를 누른 경우 m_results.Count != 0
						else
						{
							ContentManager.Instance.Input_KeymapFocus(m_focus, m_focusDelta);
							//if (IsClickOnKeymap(_sEvents[InputEventType.Input_clickDown].Results))
							//{
							//}
						}
					}
					break;

				case InputEventType.Input_key:
					m_keyEvent.Invoke(m_keys);
					break;
			}
		}

		/// <summary>
		/// AdminViewer 키맵 선택 이벤트
		/// </summary>
		private bool IsClickOnKeymap(List<RaycastResult> _hits)
		{
			bool result = false;

			if(_hits.Count != 0)
			{
				_hits.ForEach(x =>
				{
					if(x.gameObject.name.Contains("Keymap"))
					{
						result = true;
					}
				});
			}

			return result;
		}

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

			//Debug.Log($"***** raycast count : {results.Count}");

			return results;
		}

		





		#endregion
	}
}
