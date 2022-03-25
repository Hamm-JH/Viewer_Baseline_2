using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Events;
	using View;

	/// <summary>
	/// 이벤트 관리
	/// </summary>
	public partial class EventManager : IManager<EventManager>
	{
		/// <summary>
		/// 이벤트 상태관리코드 인스턴스
		/// </summary>
		[SerializeField] EventStatement m_eStatus;

		/// <summary>
		/// 이벤트 단위로 현재 이벤트의 상태를 코드별로 저장한 변수
		/// </summary>
		[SerializeField] Dictionary<InputEventType, AEventData> m_EventStates;

		/// <summary>
		/// clickDown에서 clickUp 타이밍 동안 드래깅용 객체
		/// </summary>
		//[SerializeField] GameObject cacheDownObj;

		public Dictionary<InputEventType, AEventData> EventStates { get => m_EventStates; set => m_EventStates=value; }

		public EventStatement _Statement => m_eStatus;

		public List<ModuleCode> _ModuleList
		{
			get
			{
				return m_eStatus.ModuleList;
			}
		}

		public GameObject _CacheObject
		{
			get => m_eStatus.CacheObject;
			set => m_eStatus.CacheObject = value;
		}

		public GameObject _CachePin
		{
			get => m_eStatus.CachePin;
			set => m_eStatus.CachePin = value;
		}

		private void Awake()
		{
			//cacheDownObj = null;

			EventStates = new Dictionary<InputEventType, AEventData>();
			m_eStatus = new EventStatement();
			EventStates.Add(InputEventType.Statement, m_eStatus);
		}

		public void OnEvent(AEventData currEvent)
		{
			// 선택된 이벤트 상태가 없는 경우, 아무 동작을 수행하지 않는 더미 인스턴스를 생성한다.
			if (EventStates.Count == 0)
			{
				EventData_Empty dummy = new EventData_Empty();
				EventStates.Add(dummy.EventType, dummy);
			}

			// 현재 발생한 이벤트가 동작을 필요로 하지 않는 이벤트일 경우가 있다.
			// 이 경우를 필터링한다.
			if (!IsEventCanDoit(currEvent)) return;

			// 받아온 이벤트의 내부처리 메서드를 시행
			// 내부에 연산 결과로 내부의 interactable 인터페이스 상속 인스턴스 업데이트됨.
			// 추후 전달 데이터가 늘어나면 새로 내부 클래스 작성하기
			currEvent.OnProcess(_ModuleList);

			DoEvent(EventStates, currEvent);
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
		private bool IsEventCanDoit(AEventData _curr)
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


		private void DoEvent(Dictionary<InputEventType, AEventData> _sEvents, AEventData _curr)
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
		private bool IsSelectedElementNull(Dictionary<InputEventType, AEventData> _sEvents)
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
		private bool IsSelectedElementNull(AEventData _curr)
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

		

		
	}
}
