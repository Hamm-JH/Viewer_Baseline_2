using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class Event_Drag : EventData_Input
	{
		private Vector2 m_delta;
		UnityEvent<int, Vector2> m_dragEvent;

		/// <summary>
		/// 드래그 이벤트 생성자
		/// </summary>
		/// <param name="_eventType">이벤트 분류</param>
		/// <param name="_btn">마우스 버튼</param>
		/// <param name="_delta">드래그 정도</param>
		/// <param name="_camera">카메라</param>
		/// <param name="_grRaycaster">그래픽 레이캐스터</param>
		/// <param name="_event">드래그 이벤트 리스트</param>
		public Event_Drag(InputEventType _eventType,
			int _btn, Vector2 _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<int, Vector2> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_btn = _btn;
			m_delta = _delta;
			m_dragEvent = _event;
		}

		/// <summary>
		/// 이벤트 후처리
		/// </summary>
		/// <param name="_sEvents"></param>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			if (this.BtnIndex == 0)
			{
				if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
				{
					// 클릭다운 개체가 UI를 안눌렀는가?
					if (_sEvents[InputEventType.Input_clickDown].Results.Count == 0)
					{
						OnDrag(_sEvents);
					}
					else
					{
						if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
						{
							List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

							if(IsClickOnKeymap(hits))
							{
								ContentManager.Instance.Input_KeymapDrag(m_btn, m_delta);
							}
						}
					}
				}
			}
			else if (this.BtnIndex == 1)
			{
				if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
				{
					// 클릭다운 개체가 UI를 안 눌렀는가?
					if (_sEvents[InputEventType.Input_clickDown].Results.Count == 0)
					{
						OnDrag(_sEvents);
					}
					else
					{
						if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
						{
							List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

							if (IsClickOnKeymap(hits))
							{
								ContentManager.Instance.Input_KeymapDrag(m_btn, m_delta);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 드래그 실행 
		/// </summary>
		/// <param name="_sEvents">현재 이벤트</param>
		private void OnDrag(Dictionary<InputEventType, AEventData> _sEvents)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoWebViewer(pCode))
            {
				if (ModuleCodes.IsWorkQueueProcess()) return;

				m_dragEvent.Invoke(m_btn, m_delta);
			}
			else
            {
				m_dragEvent.Invoke(m_btn, m_delta);
			}

		}

		/// <summary>
		/// 이벤트 전처리
		/// </summary>
		/// <param name="_mList"></param>
		public override void OnProcess(List<ModuleCode> _mList)
		{
			StatusCode = Status.Update;
		}
	}
}
