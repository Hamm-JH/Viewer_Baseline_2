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

	public class Event_SelectObject : AEventData
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
					m_clickEvent.Invoke(Elements.Last().Target);
					// TODO

					// API로 접근한 개체가 실행하는 이벤트
					// 키맵 카메라의 타겟 위치 변경
					ContentManager.Instance.Input_SelectObject(Elements.Last().Target);

					ContentManager.Instance.OnSelect_3D(Elements.Last().Target);
					//ContentManager.Instance.Toggle_ChildTabs(1);
				}
				else if (obj.TryGetComponent<Issue_Selectable>(out iObj))
				{
					ContentManager.Instance.Input_SelectObject(Elements.Last().Target);

					m_clickEvent.Invoke(Elements.Last().Target);
					ContentManager.Instance.OnSelect_Issue(Elements.Last().Target);
					//ContentManager.Instance.Toggle_ChildTabs(1);
				}
			}
			// 빈 공간을 선택한 경우
			else
			{
				m_clickEvent.Invoke(null);
				ContentManager.Instance.OnSelect_3D(null);
				//// UI 선택의 결과가 0 이상인 경우
				//if (Results.Count != 0)
				//{
				//	if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
				//	{
				//		List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

				//		if (IsClickOnKeymap(hits))
				//		{
				//			ContentManager.Instance.Input_KeymapClick(m_clickPosition);
				//		}
				//	}
				//}
				//else
				//{
				//}
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

		private void Func_Input_clickSuccessUp(Status _success)
		{
			Obj_Selectable sObj;
			Issue_Selectable iObj;

			if (Selected3D.TryGetComponent<Obj_Selectable>(out sObj))
			{
				Elements = new List<IInteractable>();
				Elements.Add(sObj);

				//Element = sObj;

				PlatformCode _pCode = MainManager.Instance.Platform;
				if (Platforms.IsSmartInspectPlatform(_pCode))
				{
					m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
					m_clickEvent.AddListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				}
				StatusCode = _success;
				return;
			}
			else if (Selected3D.TryGetComponent<Issue_Selectable>(out iObj))
			{
				Elements = new List<IInteractable>();
				Elements.Add(iObj);

				//Element = iObj;

				m_clickEvent.RemoveListener(ContentManager.Instance.Get_SelectedData_UpdateUI);
				StatusCode = _success;
				return;
			}
		}
	}
}
