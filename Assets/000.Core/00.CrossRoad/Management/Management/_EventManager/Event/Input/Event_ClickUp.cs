using Definition;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Items;
    using Module.Interaction;
    using Module.Model;
    using Module.UI;
    using Module.WebAPI;
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
			// �� ������ ���� ���
			else if (m_results.Count == 0)
			{
				//Element = null;
				Elements = null;
				StatusCode = Status.Pass;
				//StatusCode = _fail;
			}
			// UI ��ü�� ���� ��� m_results.Count != 0
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
					StartEvent_SelectObject(Elements.Last().Target);
					//m_clickEvent.Invoke(Elements.Last().Target);
					//ContentManager.Instance.OnSelect_3D(Elements.Last().Target);
				}
				else if (obj.TryGetComponent<Issue_Selectable>(out iObj))
				{
					StartEvent_SelectIssue(Elements.Last().Target);
					//m_clickEvent.Invoke(Elements.Last().Target);
					//ContentManager.Instance.OnSelect_Issue(Elements.Last().Target);
				}
			}
			// UI �Ǵ� �� ������ ������ ���
			else
			{
				// UI ������ ����� 0 �̻��� ���
				if (Results.Count != 0)
				{
					StartEvent_SelectUI(_sEvents, Results);
					//if (_sEvents.ContainsKey(InputEventType.Input_clickDown))
					//{
					//	List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

					//	if (IsClickOnKeymap(hits))
					//	{
					//		ContentManager.Instance.Input_KeymapClick(m_clickPosition);
					//	}
					//}
				}
				else
				{
					StartEvent_SelectNull();
					//m_clickEvent.Invoke(null);
					//ContentManager.Instance.OnSelect_3D(null);
				}
			}
		}

		public void StartEvent_SelectObject(GameObject _obj)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				// ��ü�� 34�ܰ迡�� ������ �ȵ�.
				AModuleStatus aStat = EventManager.Instance._Statement.GetModuleStatus(ModuleCode.Issue_Administration);
				if(aStat != null)
				{
					// ���� ���°� �⺻ �������(1 2 5)�ΰ�?
					if (aStat.IsDefaultAdministrationMode())
					{
						m_clickEvent.Invoke(_obj);
					}
					// ������� (3 4)
					else { }
				}
				
			}
			else if(Platforms.IsViewerPlatform(pCode))
			{
				m_clickEvent.Invoke(_obj);
				ContentManager.Instance.OnSelect_3D(_obj);
			}
			else if(Platforms.IsSmartInspectPlatform(pCode))
            {
				Debug.Log($"***** Hello inspect platform");
				m_clickEvent.Invoke(_obj);
				//Module_WebAPI api = Content.SmartInspectManager.Instance.Module<Module.WebAPI.Module_WebAPI>(ModuleID.WebAPI);
				//Module_Model model = Content.SmartInspectManager.Instance.Module<Module_Model>(ModuleID.Model);

				Module_Interaction interaction = Content.SmartInspectManager.Instance.Module<Module_Interaction>(ModuleID.Interaction);

				UITemplate_SmartInspect ui;
				foreach(var value in interaction.UiInstances)
                {
					// �� UI ��ü �߿��� UITemplate_SmartInspect�� �ش��ϴ� ��ü�� �ִ��� Ȯ���Ѵ�. �����ϴ� ���
					if(Utilities.Objects.TryGetValue<UITemplate_SmartInspect>(value.gameObject, out ui))
                    {
						ui.Input_Select3DObject(_obj);
                    }
                }
				//interaction.UiInstances
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
		}

		public void StartEvent_SelectIssue(GameObject _obj)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				m_clickEvent.Invoke(_obj);
			}
			else if(Platforms.IsViewerPlatform(pCode))
			{
				m_clickEvent.Invoke(_obj);
				ContentManager.Instance.OnSelect_Issue(_obj);
			}
			else if(Platforms.IsSmartInspectPlatform(pCode))
            {
				m_clickEvent.Invoke(_obj);
			}
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
		}

		public void StartEvent_SelectUI(Dictionary<InputEventType, AEventData> _sEvents, List<RaycastResult> _results)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				// ui ���ø���Ʈ 0 �ƴϰ� clickDown�� �ִ°�?
				if (Results.Count != 0 && _sEvents.ContainsKey(InputEventType.Input_clickDown)) 
				{
					AModuleStatus aStat = EventManager.Instance._Statement.GetModuleStatus(ModuleCode.Issue_Administration);

					// null �ƴϰ� �������(3 4)�ΰ�?
					if (aStat != null && !aStat.IsDefaultAdministrationMode())
					{
						List<RaycastResult> hits = _sEvents[InputEventType.Input_clickDown].Results;

						if (IsClickOnKeymap(hits))
						{
							ContentManager.Instance.Input_KeymapClick(m_clickPosition);
						}
					}
				}
			}
		}

		public void StartEvent_SelectNull()
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				m_clickEvent.Invoke(null);

				ContentManager.Instance.Toggle_ChildTabs(1);
			}
			else if(Platforms.IsViewerPlatform(pCode))
			{
				m_clickEvent.Invoke(null);
				ContentManager.Instance.OnSelect_3D(null);
			}
			else if(Platforms.IsSmartInspectPlatform(pCode))
            {
				m_clickEvent.Invoke(null);

				Module_Interaction interaction = Content.SmartInspectManager.Instance.Module<Module_Interaction>(ModuleID.Interaction);

				UITemplate_SmartInspect ui;
				foreach (var value in interaction.UiInstances)
				{
					// �� UI ��ü �߿��� UITemplate_SmartInspect�� �ش��ϴ� ��ü�� �ִ��� Ȯ���Ѵ�. �����ϴ� ���
					if (Utilities.Objects.TryGetValue<UITemplate_SmartInspect>(value.gameObject, out ui))
					{
						ui.Input_Select3DObject(null);
					}
				}
			}
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
		} 

	}
}
