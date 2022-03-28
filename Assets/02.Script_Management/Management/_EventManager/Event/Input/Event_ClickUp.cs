using Definition;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Items;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;
	using View;

	public class Event_ClickUp : EventData_Input
	{
		//public Camera m_camera;
		//public GraphicRaycaster m_grRaycaster;
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;

		public Event_ClickUp(InputEventType _eventType,
			int _btn, Vector3 _mousePos,
			Camera _camera, GraphicRaycaster _graphicRaycaster,
			UnityEvent<GameObject> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_grRaycaster = _graphicRaycaster;
			m_clickPosition = _mousePos;
			m_clickEvent = _event;
		}

		public override void OnProcess(List<ModuleCode> _mList)
		{
			Status _success = Status.Pass;
			//Status _fail = Status.Drop;

			m_selected3D = null;
			m_hit = default(RaycastHit);
			m_results = new List<RaycastResult>();

			Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

			if (Selected3D != null)
			{
				Func_Input_clickSuccessUp(_success);
			}
			// 빈 공간을 누른 경우
			else if (m_results.Count == 0)
			{
				//Element = null;
				Elements = null;
				StatusCode = Status.Pass;
				//StatusCode = _fail;
			}
			// UI 객체를 누른 경우 m_results.Count != 0
			else
			{
				//Element = null;
				Elements = null;
				StatusCode = Status.Pass;
			}
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

					ContentManager.Instance.OnSelect_3D(Elements.Last().Target);
				}
				else if (obj.TryGetComponent<Issue_Selectable>(out iObj))
				{
					m_clickEvent.Invoke(Elements.Last().Target);
					ContentManager.Instance.OnSelect_Issue(Elements.Last().Target);
				}
			}
			// 빈 공간을 선택한 경우
			else
			{
				// UI 선택의 결과가 0 이상인 경우
				if (Results.Count != 0)
				{
					if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
					{
						List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

						if (IsClickOnKeymap(hits))
						{
							ContentManager.Instance.Input_KeymapClick(m_clickPosition);
						}
					}
				}
				else
				{
					m_clickEvent.Invoke(null);
					ContentManager.Instance.OnSelect_3D(null);
				}
			}
		}

		protected void Func_Input_clickSuccessUp(Status _success)
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
