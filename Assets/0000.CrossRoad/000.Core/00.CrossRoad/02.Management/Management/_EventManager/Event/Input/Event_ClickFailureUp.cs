using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
	using UnityEngine.Events;
	using UnityEngine.UI;

	public class Event_ClickFailureUp : EventData_Input
	{
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;

		/// <summary>
		/// 선택 실패 이벤트
		/// </summary>
		/// <param name="_eType">이벤트 분류</param>
		/// <param name="_btn">마우스 버튼</param>
		/// <param name="_mousePos">클릭 위치</param>
		/// <param name="_camera">카메라</param>
		/// <param name="_grRaycaster">그래픽 레이캐스터</param>
		/// <param name="_event">이벤트</param>
		public Event_ClickFailureUp(InputEventType _eType,
			int _btn, Vector3 _mousePos,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_clickPosition = _mousePos;
			m_clickEvent = _event;
		}

		/// <summary>
		/// 이벤트 전처리
		/// </summary>
		/// <param name="_mList">모듈 리스트</param>
		public override void OnProcess(List<ModuleCode> _mList)
		{
			StatusCode = Status.Update;
		}

		/// <summary>
		/// 이벤트 후처리
		/// </summary>
		/// <param name="_sEvents">현재 이벤트</param>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			if( _sEvents.ContainsKey(InputEventType.Input_clickDown))
			{
				_sEvents.Remove(InputEventType.Input_clickDown);
			}
		}
	}
}
