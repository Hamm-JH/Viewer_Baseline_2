using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Events;

	/// <summary>
	/// 이벤트 관리
	/// </summary>
	public partial class EventManager : IManager<EventManager>
	{
		[SerializeField] EventData selectedEvent;
		[SerializeField] GameObject cacheDownObj;

		public void OnEvent(EventData currEvent)
		{
			// 선택된 이벤트 상태가 없는 경우, 아무 동작을 수행하지 않는 더미 인스턴스를 생성한다.
			if(selectedEvent == null)
			{
				selectedEvent = new EventData_Empty();
			}

			// 현재 발생한 이벤트가 동작을 필요로 하지 않는 이벤트일 경우가 있다.
			// 이 경우를 필터링한다.
			if (!IsEventCanDoit(currEvent)) return;

			// 받아온 이벤트의 내부처리 메서드를 시행
			// 내부에 연산 결과로 내부의 interactable 인터페이스 상속 인스턴스 업데이트됨.
			// 추후 전달 데이터가 늘어나면 새로 내부 클래스 작성하기
			currEvent.OnProcess(cacheDownObj);

			// 이벤트 실행
			DoEvent(selectedEvent, currEvent);
		}

		/// <summary>
		/// 받은 EventData 인스턴스의 이벤트 타입이 실행 가능한 이벤트 타입인지 확인한다.
		/// </summary>
		/// <param name="_curr"> 실행 가능성 파악해야할 EventData </param>
		/// <returns> 실행 가능할시 true, 실행 불가능시 false</returns>
		private bool IsEventCanDoit(EventData _curr)
		{
			bool result = true;

			switch(_curr.EventType)
			{
				case Definition.InputEventType.NotDef:
				//case Definition.InputEventType.Input_clickFailureUp:
					result = false;
					break;
			}

			return result;
		}

		private void DoEvent(EventData _selected, EventData _curr)
		{
			// 이전 이벤트 데이터와 현재 이벤트 데이터의 상태를 확인한다.
			// 이전 이벤트의 Element가 없다면
			// -> 현재 이벤트만 확인
			if (_selected.Element == null)
			{
				// 현재 이벤트의 Element가 null이 아닐경우
				// 현재 이벤트를 이전 이벤트로 이전하고 업데이트
				if (_curr.Element != null)
				{
					UpdateNewEvent(_curr);
				}
				return;
			}

			// _selected.Element == null인 상태는 모두 필터링됨
			
			// 이벤트 코드 :: Drop의 경우 deselect event만 수행
			// 이벤트 코드 :: Skip의 경우 (clickDown) update new event만 수행
			// 이벤트 코드 :: Pass의 경우 update new event, deselect event 둘 다 수행
			switch(_curr.StatusCode)
			{
				case Definition.Status.Drop:
					DeselectEvent(_selected);
					break;

				case Definition.Status.Pass:
					DeselectEvent(_selected);
					UpdateNewEvent(_curr);
					break;

				case Definition.Status.Update:
					UpdateNewEvent(_curr);
					break;

				case Definition.Status.Skip:
					break;
			}

			// 내부처리 결과에도 _curr.Element가 null인 경우 이전 이벤트 데이터 처리를 해제한다.
			//if(_curr.Element == null)
			//{
			//	//Debug.Log(_curr.Element.Equals(null));
			//	DeselectEvent(_selected);
			//	return;
			//}

			//if(_curr.EventType != Definition.InputEventType.Input_clickDown)
			//{
			//	// 남은 경우의 수 : _selected, _curr 둘다 Element가 존재함
			//	Debug.Log($"***** {_curr.EventType.ToString()}");
			//	DeselectEvent(_selected);
			//	UpdateNewEvent(_curr);
			//}
		}

		/// <summary>
		/// 선택 이벤트 실행구간
		/// 특정 이벤트는 이전 이벤트 영역으로 이전되지 않는다. (즉발성)
		/// </summary>
		/// <param name="currEvent"></param>
		private void UpdateNewEvent(EventData currEvent)
		{
			// 마우스 클릭 다운 단계
			if(currEvent.EventType == Definition.InputEventType.Input_clickDown)
			{
				cacheDownObj = currEvent.Element.Target;
			}
			// 마우스 클릭 끝 (가능함)
			else if(currEvent.EventType == Definition.InputEventType.Input_clickSuccessUp)
			{
				currEvent.Element.OnSelect();
				currEvent.DoEvent();
				//MainManager.Instance.cameraExecuteEvents.selectEvent.Invoke(currEvent.Element.Target);	// 마우스 클릭 성공시 실행 (객체 선택 모드)
				cacheDownObj = null;
				selectedEvent = currEvent;
			}
			// 마우스 클릭 끝 (불가능함)
			else if(currEvent.EventType == Definition.InputEventType.Input_clickFailureUp)
			{
				cacheDownObj = null;
			}
			else if(currEvent.EventType == Definition.InputEventType.Input_drag
				|| currEvent.EventType == Definition.InputEventType.Input_focus
				|| currEvent.EventType == Definition.InputEventType.Input_key)
			{
				currEvent.DoEvent();
			}

		}

		/// <summary>
		/// 선택 해제 이벤트 실행구간
		/// </summary>
		/// <param name="selectedEvent"></param>
		private void DeselectEvent(EventData selectedEvent)
		{

			// 마우스 클릭 끝 (가능함)
			if (selectedEvent.EventType == Definition.InputEventType.Input_clickSuccessUp)
			{
				//selectedEvent.Element.OnSelect();
				selectedEvent.Element.OnDeselect();
				selectedEvent = null;
			}


		}
	}
}
