using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Events;

	/// <summary>
	/// 이벤트 관리
	/// </summary>
	public partial class EventManager : IManager<EventManager>
	{
		[SerializeField] EventData m_selectedEvent;
		[SerializeField] Dictionary<InputEventType, EventData> m_selectedEvents;
		[SerializeField] GameObject cacheDownObj;

		public EventData SelectedEvent { get => m_selectedEvent; set => m_selectedEvent=value; }
		public Dictionary<InputEventType, EventData> SelectedEvents { get => m_selectedEvents; set => m_selectedEvents=value; }

		private void Start()
		{
			SelectedEvents = new Dictionary<InputEventType, EventData>();
		}

		public void OnEvent(EventData currEvent)
		{
			// 선택된 이벤트 상태가 없는 경우, 아무 동작을 수행하지 않는 더미 인스턴스를 생성한다.
			if (SelectedEvents.Count == 0)
			{
				EventData_Empty dummy = new EventData_Empty();
				SelectedEvents.Add(dummy.EventType, dummy);
			}

			// 현재 발생한 이벤트가 동작을 필요로 하지 않는 이벤트일 경우가 있다.
			// 이 경우를 필터링한다.
			if (!IsEventCanDoit(currEvent)) return;

			// 받아온 이벤트의 내부처리 메서드를 시행
			// 내부에 연산 결과로 내부의 interactable 인터페이스 상속 인스턴스 업데이트됨.
			// 추후 전달 데이터가 늘어나면 새로 내부 클래스 작성하기
			currEvent.OnProcess(cacheDownObj);

			DoEvent(SelectedEvents, currEvent);

			////-------------------------------------------------------------------------

			//// 선택된 이벤트 상태가 없는 경우, 아무 동작을 수행하지 않는 더미 인스턴스를 생성한다.
			//if (SelectedEvent == null)
			//{
			//	SelectedEvent = new EventData_Empty();
			//}

			//// 현재 발생한 이벤트가 동작을 필요로 하지 않는 이벤트일 경우가 있다.
			//// 이 경우를 필터링한다.
			//if (!IsEventCanDoit(currEvent)) return;

			//// 받아온 이벤트의 내부처리 메서드를 시행
			//// 내부에 연산 결과로 내부의 interactable 인터페이스 상속 인스턴스 업데이트됨.
			//// 추후 전달 데이터가 늘어나면 새로 내부 클래스 작성하기
			//currEvent.OnProcess(cacheDownObj);

			//// 이벤트 실행
			//DoEvent(SelectedEvent, currEvent);
		}

		/// <summary>
		/// 받은 EventData 인스턴스의 이벤트 타입이 실행 가능한 이벤트 타입인지 확인한다.
		/// </summary>
		/// <param name="_curr"> 실행 가능성 파악해야할 EventData </param>
		/// <returns> 실행 가능할시 true, 실행 불가능시 false</returns>
		private bool IsEventCanDoit(EventData _curr)
		{
			bool result = true;

			switch (_curr.EventType)
			{
				case Definition.InputEventType.NotDef:
					//case Definition.InputEventType.Input_clickFailureUp:
					result = false;
					break;
			}

			return result;
		}

		private void DoEvent(Dictionary<InputEventType, EventData> _sEvents, EventData _curr)
		{
			// 이전 이벤트 상태가 아무것도 없는 상태였다면?
			if (IsSelectedElementNull(_sEvents))
			{
				// 현재 이벤트 상태가 NotDef가 아니라면?
				if(!IsSelectedElementNull(_curr))
				{
					UpdateNewEvent(_sEvents, _curr);
				}
				return;
			}

			// 이벤트 코드 :: Drop의 경우 deselect event만 수행
			// 이벤트 코드 :: Skip의 경우 (clickDown) update new event만 수행
			// 이벤트 코드 :: Pass의 경우 update new event, deselect event 둘 다 수행
			switch (_curr.StatusCode)
			{
				case Definition.Status.Drop:
					DeselectEvent(_sEvents, _curr);
					break;

				case Definition.Status.Pass:
					DeselectEvent(_sEvents, _curr);
					UpdateNewEvent(_sEvents, _curr);
					break;

				case Definition.Status.Update:
					UpdateNewEvent(_sEvents, _curr);
					break;

				case Definition.Status.Skip:
					break;
			}

		}

		#region Null Event Check

		/// <summary>
		/// 직전에 의미있는 이벤트를 동작했는가 확인
		/// </summary>
		/// <param name="_sEvents"></param>
		/// <returns> true :: Null상태, false :: Null아님 </returns>
		private bool IsSelectedElementNull(Dictionary<InputEventType, EventData> _sEvents)
		{
			bool result = false;

			// type이 NotDef만 있으면 true
			// type이 NotDef외에 존재하면 false
			foreach (InputEventType type in _sEvents.Keys)
			{
				if(type == InputEventType.NotDef)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}

			return result;
		}

		/// <summary>
		/// 현재 이벤트의 상태가 NotDef인가?
		/// </summary>
		/// <param name="_curr"></param>
		/// <returns></returns>
		private bool IsSelectedElementNull(EventData _curr)
		{
			bool result = false;

			if(_curr.EventType == InputEventType.NotDef)
			{
				result = true;
			}
			else
			{
				result = false;
			}

			return result;
		}

		#endregion

		/// <summary>
		/// 선택 이벤트 실행구간
		/// 특정 이벤트는 이전 이벤트 영역으로 이전되지 않는다. (즉발성)
		/// </summary>
		/// <param name="_currEvent"></param>
		private void UpdateNewEvent(Dictionary<InputEventType, EventData> _sEvents, EventData _currEvent)
		{
			switch(_currEvent.EventType)
			{
				case InputEventType.Input_clickDown:
					{
						// Caching
						// clickDown 캐시 업데이트
						if(_currEvent.Element != null)
						{
							cacheDownObj = _currEvent.Element.Target;
						}

						if(SelectedEvents.ContainsKey(InputEventType.Input_clickDown))
						{
							SelectedEvents[InputEventType.Input_clickDown] = _currEvent;
						}
						else
						{
							SelectedEvents.Add(_currEvent.EventType, _currEvent);
						}
					}
					break;

				case InputEventType.Input_clickFailureUp:
					{
						// Caching
						// ClickDown 캐시 삭제
						if(SelectedEvents.ContainsKey(InputEventType.Input_clickDown))
						{
							cacheDownObj = null;
							SelectedEvents.Remove(InputEventType.Input_clickDown);
						}
					}
					break;

				case InputEventType.Input_clickSuccessUp:
					{
						// Caching
						// 허공 선택시
						// UI 선택시
						// 3D 선택시

						_currEvent.Element.OnSelect();
						_currEvent.DoEvent();

						// 클릭다운 개체가 있을 경우
						if(_sEvents.ContainsKey(InputEventType.Input_clickDown))
						{
							cacheDownObj = null;
							_sEvents.Remove(InputEventType.Input_clickDown);
						}

						// 클릭 성공 개체가 있을 경우
						if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
						{
							_sEvents[InputEventType.Input_clickSuccessUp] = _currEvent;
						}
						// 클릭 성공 개체가 없을 경우
						else
						{
							_sEvents.Add(InputEventType.Input_clickSuccessUp, _currEvent);
						}
					}
					break;

				case InputEventType.Input_drag:
					{
						// 즉발 (Not caching)
						// 클릭다운 개체가 있는가?
						//if(_currEvent)
						if(_currEvent.BtnIndex == 0)
						{
							if(_sEvents.ContainsKey(InputEventType.Input_clickDown))
							{
								// 클릭다운 개체가 UI를 안눌렀는가?
								if(_sEvents[InputEventType.Input_clickDown].Results.Count == 0)
								{
									_currEvent.DoEvent();
								}
							}
						}
						else if(_currEvent.BtnIndex == 1)
						{
							_currEvent.DoEvent();
						}
					}
					break;

				case InputEventType.Input_focus:
					{
						// 즉발 (Not caching)
						_currEvent.DoEvent();
					}
					break;

				case InputEventType.Input_key:
					{
						// 즉발 (Not caching)
						_currEvent.DoEvent();
					}
					break;
			}

			{
				//// 마우스 클릭 다운 단계
				//if(_currEvent.EventType == Definition.InputEventType.Input_clickDown)
				//{
				//	// 특정 객체를 클릭한 경우에 실행
				//	if(_currEvent.Element != null)
				//	{
				//		cacheDownObj = _currEvent.Element.Target;
				//	}
				//	SelectedEvent = _currEvent;
				//}
				//// 마우스 클릭 끝 (가능함)
				//else if(_currEvent.EventType == Definition.InputEventType.Input_clickSuccessUp)
				//{
				//	_currEvent.Element.OnSelect();
				//	_currEvent.DoEvent();
				//	//MainManager.Instance.cameraExecuteEvents.selectEvent.Invoke(currEvent.Element.Target);	// 마우스 클릭 성공시 실행 (객체 선택 모드)
				//	cacheDownObj = null;
				//	SelectedEvent = _currEvent;
				//}
				//// 마우스 클릭 끝 (불가능함)
				//else if(_currEvent.EventType == Definition.InputEventType.Input_clickFailureUp)
				//{
				//	cacheDownObj = null;
				//	if(SelectedEvent.EventType == Definition.InputEventType.Input_clickDown)
				//	{
				//		if(SelectedEvent != null)
				//		{
				//			//DeselectEvent(SelectedEvent);
				//			SelectedEvent = null;
				//		}
				//	}
				//}
				//else if(_currEvent.EventType == Definition.InputEventType.Input_focus
				//	|| _currEvent.EventType == Definition.InputEventType.Input_key)
				//{
				//	_currEvent.DoEvent();
				//}
				//else if(_currEvent.EventType == Definition.InputEventType.Input_drag)
				//{
				//	// 이전 이벤트가 clickDown이고, UI를 눌렀었는가?
				//	if (SelectedEvent.EventType == Definition.InputEventType.Input_clickDown &&
				//		SelectedEvent.Results.Count > 0) { }
				//	// 위의 조건이 아닐 경우
				//	else
				//	{
				//		_currEvent.DoEvent();
				//	}
				//}
			}

		}

		/// <summary>
		/// 선택 해제 이벤트 실행구간
		/// </summary>
		/// <param name="_event"></param>
		private void DeselectEvent(Dictionary<InputEventType, EventData> _sEvents, EventData _currEvent)
		{
			switch(_currEvent.EventType)
			{
				case InputEventType.Input_clickDown:
				case InputEventType.Input_clickFailureUp:
					break;

				case InputEventType.Input_clickSuccessUp:
					{
						// 이전에 선택했던 개체가 존재하는가?
						if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
						{
							_sEvents[InputEventType.Input_clickSuccessUp].Element.OnDeselect();
						}
					}
					break;

				case InputEventType.Input_drag:
				case InputEventType.Input_focus:
				case InputEventType.Input_key:
					break;
			}
		}
	}
}
