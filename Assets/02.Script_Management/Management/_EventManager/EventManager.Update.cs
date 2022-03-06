using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Management.Events;
	using System.Linq;

	public partial class EventManager : IManager<EventManager>
	{
		/// <summary>
		/// 선택 이벤트 실행구간
		/// 특정 이벤트는 이전 이벤트 영역으로 이전되지 않는다. (즉발성)
		/// </summary>
		/// <param name="_currEvent"></param>
		private void UpdateNewEvent(Dictionary<InputEventType, EventData> _sEvents, EventData _currEvent)
		{
			switch (_currEvent.EventType)
			{
				case InputEventType.Input_clickDown:
					{
						// Caching
						// clickDown 캐시 업데이트
						if (_currEvent.Elements != null)
						{
							cacheDownObj = _currEvent.Elements.Last().Target;
						}

						if (EventStates.ContainsKey(InputEventType.Input_clickDown))
						{
							EventStates[InputEventType.Input_clickDown] = _currEvent;
						}
						else
						{
							EventStates.Add(_currEvent.EventType, _currEvent);
						}
					}
					break;

				case InputEventType.Input_clickFailureUp:
					{
						// Caching
						// ClickDown 캐시 삭제
						if (EventStates.ContainsKey(InputEventType.Input_clickDown))
						{
							cacheDownObj = null;
							EventStates.Remove(InputEventType.Input_clickDown);
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
						if (isMultiCondition(_sEvents))
						{
							// 정상적으로 객체가 선택이 된 경우
							if (_currEvent.Elements != null)
							{
								AddEvent<InputEventType, EventData>(_currEvent.EventType, _currEvent, _sEvents, true);

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

								// 이벤트 리스트 업데이트
								AddEvent<InputEventType, EventData>(_currEvent.EventType, _currEvent, _sEvents);
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
						if (_currEvent.BtnIndex == 0)
						{
							if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
							{
								// 클릭다운 개체가 UI를 안눌렀는가?
								if (_sEvents[InputEventType.Input_clickDown].Results.Count == 0)
								{
									_currEvent.DoEvent();
								}
							}
						}
						else if (_currEvent.BtnIndex == 1)
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
						if (_ev.m_keys != null && _ev.m_keys.Count != 0)
						{
							// 키 데이터 업데이트
							if (EventStates.ContainsKey(InputEventType.Input_key))
							{
								EventStates[InputEventType.Input_key] = _ev;
							}
							else
							{
								EventStates.Add(InputEventType.Input_key, _ev);
							}
						}
						else
						{
							// 기존 저장된 키 지우기
							if (EventStates.ContainsKey(InputEventType.Input_key))
							{
								EventStates.Remove(InputEventType.Input_key);
							}
						}

						// 즉발 (Not caching)
						_currEvent.DoEvent();
					}
					break;

				case InputEventType.UI_Invoke:
					{
						if (!EventStates.ContainsKey(InputEventType.Input_clickSuccessUp)) return;
						if (EventStates[InputEventType.Input_clickSuccessUp].Elements == null) return;

						_currEvent.DoEvent(_sEvents);

						EventStates[InputEventType.Input_clickSuccessUp].Elements.ForEach(x => x.OnDeselect());
						EventStates.Remove(InputEventType.Input_clickSuccessUp);
					}
					break;

					// 스킵됨
				case InputEventType.API_SelectObject:
				case InputEventType.API_SelectIssue:
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
			if (_sEvents.ContainsKey(InputEventType.Input_key))
			{
				EventData_Input _ev = (EventData_Input)_sEvents[InputEventType.Input_key];
				kd = _ev.m_keys;
			}

			// 입력 키 존재하는 경우
			if (kd != null)
			{
				KeyCode targetCode = MainManager.Instance.Data.KeyboardData.keyCtrl;

				// 키 정보중에 LeftCtrl이 존재하는가?
				if (kd.Find(x => x.m_keyCode == targetCode) != null)
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
			switch (_currEvent.EventType)
			{
				case InputEventType.Input_clickDown:
				case InputEventType.Input_clickFailureUp:
					break;

				case InputEventType.Input_clickSuccessUp:
					{
						// 이전에 선택했던 개체가 존재하는가?
						if (_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
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

					// 스킵됨.
				case InputEventType.API_SelectObject:
					break;
			}
		}
	}
}
