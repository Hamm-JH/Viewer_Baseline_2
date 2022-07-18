using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events
{
	using Definition;
	using View;

	public class EventData_UI : AEventData
	{
		private List<GameObject> m_modelObj;

		/// <summary>
		/// UI 이벤트 생성자
		/// </summary>
		/// <param name="_eventType">이벤트 분류</param>
		/// <param name="_uiEvent">UI 이벤트 분류</param>
		/// <param name="_toggle">UI 토글여부 분류</param>
		/// <param name="_modelObj">모델 객체 리스트</param>
		public EventData_UI(InputEventType _eventType,
			UIEventType _uiEvent, ToggleType _toggle, List<GameObject> _modelObj)
		{
			EventType = _eventType;
			UiEventType = _uiEvent;
			ToggleType = _toggle;
			m_modelObj = _modelObj;
		}

		/// <summary>
		/// 이벤트 전처리
		/// </summary>
		/// <param name="_mList">모듈 리스트</param>
		public override void OnProcess(List<ModuleCode> _mList)
		{
			if (EventType != InputEventType.UI_Invoke)
			{
				StatusCode = Status.Skip;
				return;
			}

			switch (UiEventType)
			{
				case UIEventType.Mode_Hide:
				case UIEventType.Mode_Hide_Off:
				case UIEventType.Mode_Isolate:
				case UIEventType.Mode_Isolate_Off:
					StatusCode = Status.Update;
					break;
			}
		}

		/// <summary>
		/// 이벤트 후처리
		/// </summary>
		/// <param name="_sEvents">현재 이벤트 상태</param>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			switch(UiEventType)
			{
				case UIEventType.Mode_Hide:
				case UIEventType.Mode_Hide_Off:
				case UIEventType.Mode_Isolate:
				case UIEventType.Mode_Isolate_Off:
					{
						bool isHide = false;
						if (UiEventType == UIEventType.Mode_Hide
							|| UiEventType == UIEventType.Mode_Hide_Off) isHide = true;

						// _selected :: 현재 선택되어있는 객체들
						List<GameObject> _selected = new List<GameObject>();
						_sEvents[InputEventType.Input_clickSuccessUp].Elements.ForEach(x => _selected.Add(x.Target));

						// m_modelObj :: 모든 모델객체들
						foreach(GameObject obj in m_modelObj)
						{
							// 반복의 개별객체가 현재 선택된 객체중의 하나인가?
							bool isSelectedModel = _selected.Find(x => x == obj) == null ? false : true;

							float alpha = 0.1f;
							bool thisHide = false;
							// 이 객체가 맞음, 숨겨야됨			true true -> alpha = 0.1
							// 이 객체가 맞음, 제외 숨겨야됨	true false -> alpha = 1
							// 이 객체 아님, 숨겨야됨			false true -> alpha = 1
							// 이 객체 아님, 제외 숨겨야됨		false false -> alpha = 0.1

							if (isSelectedModel)
							{
								alpha = isHide ? 0.1f : 1f;
								thisHide = isHide ? true : false;
							}
							else
							{
								alpha = isHide ? 1f : 0.1f;
								thisHide = isHide ? false : true;
							}

							Obj_Selectable selectable;
							if (obj.TryGetComponent<Obj_Selectable>(out selectable))
							{
								selectable.OnDeselect<UIEventType, bool>(UiEventType, thisHide);
							}
						}
					}
					break;
			}
		}
	}
}
