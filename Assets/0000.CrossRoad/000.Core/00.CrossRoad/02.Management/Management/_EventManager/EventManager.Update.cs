﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Management.Events;
	using Management.Events.Inputs;
    using Module.Model;
    using System.Linq;
    using View;

    public partial class EventManager : IManager<EventManager>
	{
		/// <summary>
		/// 선택 이벤트 실행구간
		/// 특정 이벤트는 이전 이벤트 영역으로 이전되지 않는다. (즉발성)
		/// </summary>
		/// <param name="_currEvent"></param>
		private void UpdateNewEvent(Dictionary<InputEventType, AEventData> _sEvents, AEventData _currEvent)
		{
			if(isMultiCondition(_sEvents))
            {
				// 현재 입력 상태값 멀티로 할당
				_currEvent.inputStatus = InputStatus.Multi;
			}
			else
            {
				// 현재 입력 상태값 싱글로 할당
				_currEvent.inputStatus = InputStatus.Single;
			}


			switch (_currEvent.EventType)
			{
				case InputEventType.Input_clickDown:
					{
						_currEvent.DoEvent(_sEvents);
					}
					break;

				case InputEventType.Input_clickFailureUp:
					{
						_currEvent.DoEvent(_sEvents);
					}
					break;

				case InputEventType.Input_clickSuccessUp:
					{
						// TODO :: 1ST :: ★ 이벤트 선택의 분기구조 개선
						// 1. 모든 이벤트에 대해 MultiCondition 확인
						// 2. MultiContition일 경우, 아닐 경우 이벤트 실행 분기 설정

						// Caching
						// 허공 선택시
						// UI 선택시
						// 3D 선택시
						// TODO 0519 잠시 꺼둠. 문제 생기면 다시 켬
						//_currEvent.DoEvent(_sEvents);


						// 다중 선택 조건에 적용되는가?
						if (isMultiCondition(_sEvents))
						{
							// 정상적으로 객체가 선택이 된 경우
							if (_currEvent.Elements != null)
							{
								AddEvent<InputEventType, AEventData>(_currEvent.EventType, _currEvent, _sEvents, true);

								// 다중 객체의 OnSelect 시행
								// TODO :: 1ST :: ★ 일단 단일 객체 이벤트로 대체
								_sEvents[InputEventType.Input_clickSuccessUp].Elements.ForEach(x => x.OnSelect());
								_sEvents[InputEventType.Input_clickSuccessUp].DoEvent(_sEvents);
							}
							// 빈 공간을 누른 경우
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
								_currEvent.DoEvent(_sEvents);

								// 이벤트 리스트 업데이트
								AddEvent<InputEventType, AEventData>(_currEvent.EventType, _currEvent, _sEvents);
							}
							// 빈 공간을 누른 경우
							else
							{
								_currEvent.DoEvent(_sEvents);
							}
						}

						// 클릭다운 개체가 있을 경우
						if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
						{
							//cacheDownObj = null;
							_sEvents.Remove(InputEventType.Input_clickDown);
						}

					}
					break;

				case InputEventType.Input_drag:
					{
						_currEvent.DoEvent(_sEvents);
					}
					break;

				case InputEventType.Input_focus:
					{
						_currEvent.DoEvent(_sEvents);
					}
					break;

				case InputEventType.Input_hover:
                    {
						_currEvent.DoEvent(_sEvents);
                    }
					break;

				case InputEventType.Input_key:
					{
						_currEvent.DoEvent(_sEvents);
					}
					break;

				case InputEventType.UI_Invoke:
					{
						//_currEvent.DoEvent(_sEvents);
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
					{
						_currEvent.DoEvent(_sEvents);
					}
					break;
			}
		}

		/// <summary>
		/// Click + 다중 선택 조건 확인
		/// </summary>
		/// <param name="_sEvents">현재 이벤트 상태</param>
		/// <returns>true : 참</returns>
		private bool isMultiCondition(Dictionary<InputEventType, AEventData> _sEvents)
		{
			bool result = false;
			List<KeyData> kd = null;

			// 키 입력이 있는 상태인가?
			if (_sEvents.ContainsKey(InputEventType.Input_key))
			{
				Event_Key _ev = (Event_Key)_sEvents[InputEventType.Input_key];
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
		private void DeselectEvent(Dictionary<InputEventType, AEventData> _sEvents, AEventData _currEvent)
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
							List<View.IInteractable> list = _sEvents[InputEventType.Input_clickSuccessUp].Elements;
							int index = list.Count;
                            for (int i = 0; i < index; i++)
                            {
								Obj_Selectable oObj;
								Issue_Selectable iObj;

								if(list[i].Target.TryGetComponent<Obj_Selectable>(out oObj))
                                {
									oObj.OnDeselect();
                                }
								else if(list[i].Target.TryGetComponent<Issue_Selectable>(out iObj))
                                {
									iObj.OnDeselect();

									Module_Model model = ContentManager.Instance.Module<Module_Model>();
									GameObject target = model.ModelObjects.Find(x => x.gameObject.name.Contains(iObj.Issue.CdBridgeParts)); //.TryGetComponent<Obj_Selectable>(out oObj);
									if(target != null)
                                    {
										if(target.TryGetComponent<Obj_Selectable>(out oObj))
                                        {
											oObj.OnDeselect();	
                                        }
                                    }

                                }
                            }

							//_sEvents[InputEventType.Input_clickSuccessUp].Elements.ForEach(x => x.OnDeselect());
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
