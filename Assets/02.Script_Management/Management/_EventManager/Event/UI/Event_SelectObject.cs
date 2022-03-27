using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.UIs
{
	using Definition;
	using UnityEngine.Events;
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
