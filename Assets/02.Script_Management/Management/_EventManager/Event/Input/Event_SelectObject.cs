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
			// �� ������ ������ ���
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

				// API�� ������ ��ü�� �����ϴ� �̺�Ʈ
				// Ű�� ī�޶��� Ÿ�� ��ġ ����
				ContentManager.Instance.Input_SelectObject(_obj);

				GameObject selected;
				// �� ������ ���� ���� ��ü�� �����ϴ� ���
				if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
				{
					// ���� ���� ��ü�� �Ҵ�
					selected = _sEvents[InputEventType.Input_clickSuccessUp].Elements.Last().Target;

					// ���� ��ü�� ���� ��ü�� ���� ���(���� Ŭ������ ħ)
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
