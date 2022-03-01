﻿using System.Collections;
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
		//[SerializeField] EventData m_selectedEvent;
		[SerializeField] Dictionary<InputEventType, EventData> m_selectedEvents;
		[SerializeField] GameObject cacheDownObj;

		//public EventData SelectedEvent { get => m_selectedEvent; set => m_selectedEvent=value; }
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
			try
			{
			}
			catch (System.Exception e)
			{
				Debug.LogError($"Event Error :: {e.HelpLink.ToString()} {e.Message.ToString()}");
			}
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

						// 클릭다운 개체가 있을 경우
						if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
						{
							cacheDownObj = null;
							_sEvents.Remove(InputEventType.Input_clickDown);
						}

						// 다중 선택 조건에 적용되는가?
						if(isMultiCondition(_sEvents))
						{
							// 정상적으로 객체가 선택이 된 경우
							if(_currEvent.Elements != null)
							{

								// 클릭 성공 개체가 이미 있을 경우
								if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
								{
									_currEvent.Elements.ForEach(x => _sEvents[InputEventType.Input_clickSuccessUp].Elements.Add(x));
									//_sEvents[InputEventType.Input_clickSuccessUp].Elements.Add
								}
								// 클릭 성공 개체가 없을 경우
								else
								{
									_sEvents.Add(InputEventType.Input_clickSuccessUp, _currEvent);
								}

								// 다중 객체의 OnSelect 시행
								// TODO 0228 :: 일단 단일 객체 이벤트로 대체
								_sEvents[InputEventType.Input_clickSuccessUp].Elements.ForEach(x => x.OnSelect());
								_sEvents[InputEventType.Input_clickSuccessUp].DoEvent();
								//_currEvent.DoEvent();
							}
							// 빈 공간을 누른 경우 (UI를 누른 경우의 수는 Status.Drop으로 차단함)
							else
							{

							}
						}
						// 단일 선택 상태
						else
						{
							// 정상적으로 객체가 선택이 된 경우
							if (_currEvent.Elements != null)
							{
								// 단일 객체의 OnSelect 시행
								_currEvent.Elements.ForEach(x => x.OnSelect());
								// 단일 객체의 cameraEvent 시행
								_currEvent.DoEvent();

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
							// 빈 공간을 누른 경우 (UI를 누른 경우의 수는 Status.Drop으로 차단함)
							else
							{
								_currEvent.DoEvent();
							}
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
						EventData_Input _ev = (EventData_Input)_currEvent;

						// 현재 입력된 키가 1개 이상일 경우
						if(_ev.m_keys != null && _ev.m_keys.Count != 0)
						{
							// 키 데이터 업데이트
							if(SelectedEvents.ContainsKey(InputEventType.Input_key))
							{
								SelectedEvents[InputEventType.Input_key] = _ev;
							}
							else
							{
								SelectedEvents.Add(InputEventType.Input_key, _ev);
							}
						}
						else
						{
							// 기존 저장된 키 지우기
							if(SelectedEvents.ContainsKey(InputEventType.Input_key))
							{
								SelectedEvents.Remove(InputEventType.Input_key);
							}
						}

						// 즉발 (Not caching)
						_currEvent.DoEvent();
					}
					break;

				case InputEventType.UI_Invoke:
					{
						if (!SelectedEvents.ContainsKey(InputEventType.Input_clickSuccessUp)) return;
						if (SelectedEvents[InputEventType.Input_clickSuccessUp].Elements == null) return;

						_currEvent.DoEvent(_sEvents);

						SelectedEvents[InputEventType.Input_clickSuccessUp].Elements.ForEach(x => x.OnDeselect());
						SelectedEvents.Remove(InputEventType.Input_clickSuccessUp);
					}
					break;
			}
		}


		/// <summary>
		/// Click + 다중 선택 조건 확인
		/// </summary>
		/// <param name="_sEvents"></param>
		/// <returns></returns>
		private bool isMultiCondition(Dictionary<InputEventType, EventData> _sEvents)
		{
			bool result = false;
			List<KeyData> kd = null;

			// 키 입력이 있는 상태인가?
			if(_sEvents.ContainsKey(InputEventType.Input_key))
			{
				EventData_Input _ev = (EventData_Input)_sEvents[InputEventType.Input_key];
				kd = _ev.m_keys;
			}

			// 입력 키 존재하는 경우
			if(kd != null)
			{
				KeyCode targetCode = MainManager.Instance.Data.KeyboardData.keyCtrl;
				
				// 키 정보중에 LeftCtrl이 존재하는가?
				if(kd.Find(x => x.m_keyCode == targetCode) != null)
				{
					result = true;
				}
			}

			return result;
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
							_sEvents[InputEventType.Input_clickSuccessUp].Elements.ForEach(x => x.OnDeselect());
						}
					}
					break;

				case InputEventType.Input_drag:
				case InputEventType.Input_focus:
				case InputEventType.Input_key:
					break;

				case InputEventType.UI_Invoke:
					break;
			}
		}
	}
}
