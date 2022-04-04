using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
	using System.Linq;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using View;

	public class Event_SelectObject : EventData_Input
	{
		GameObject m_inAPISelected;
		UnityEvent<GameObject> m_clickEvent;
		bool m_isPassAPI;

		public Event_SelectObject(InputEventType _eventType,
			GameObject _obj, UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_inAPISelected = _obj;
			m_clickEvent = _event;

			m_isPassAPI = true;
		}

		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			if (Elements != null)
			{
				GameObject obj = Elements.Last().Target;

				Obj_Selectable sObj;
				Issue_Selectable iObj;

				if (obj.TryGetComponent<Obj_Selectable>(out sObj))
				{
					StartEvent_KeymapSelectObject(Elements.Last().Target, _sEvents);
				}
				else if (obj.TryGetComponent<Issue_Selectable>(out iObj))
				{
					StartEvent_KeymapSelectIssue(Elements.Last().Target, _sEvents);
				}
			}
			// 빈 공간을 선택한 경우
			else
			{
				StartEvent_KeymapSelectNull();
			}
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			Status _success = Status.Pass;

			m_selected3D = m_inAPISelected;

			if (Selected3D != null)
			{
				Func_Input_clickSuccessUp(_success);
			}
		}

		private void StartEvent_KeymapSelectObject(GameObject _obj, Dictionary<InputEventType, AEventData> _sEvents)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				m_clickEvent.Invoke(_obj);

				// API로 접근한 개체가 실행하는 이벤트
				// 키맵 카메라의 타겟 위치 변경
				ContentManager.Instance.Input_SelectObject(_obj);

				GameObject selected;
				// 이 시점에 이전 선택 개체가 존재하는 경우
				if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
				{
					// 이전 선택 객체를 할당
					selected = _sEvents[InputEventType.Input_clickSuccessUp].Elements.Last().Target;

					// 현재 객체와 이전 객체가 같은 경우(더블 클릭으로 침)
					if(_obj == selected)
					{
						ContentManager.Instance._Interaction.ReInvokeStatusEvent();
					}
				}
			}
			else if(Platforms.IsViewerPlatform(pCode))
			{
				ContentManager.Instance.OnSelect_3D(_obj);
			}
		}

		private void StartEvent_KeymapSelectIssue(GameObject _obj, Dictionary<InputEventType, AEventData> _sEvents)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				ContentManager.Instance.Input_SelectObject(_obj);

				m_clickEvent.Invoke(_obj);
			}
			else if(Platforms.IsViewerPlatform(pCode))
			{
				ContentManager.Instance.OnSelect_Issue(_obj);
			}
		}

		private void StartEvent_KeymapSelectNull()
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				m_clickEvent.Invoke(null);
			}
			else if(Platforms.IsViewerPlatform(pCode))
			{
				m_clickEvent.Invoke(null);
				ContentManager.Instance.OnSelect_3D(null);
			}

		}
	}
}
