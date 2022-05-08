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
		/// <summary>
		/// 위치 요소 데이터 (저장용)
		/// </summary>
		[System.Serializable]
		public class LocationElementData
        {
			/// <summary>
			/// 위치 객체가 선택되었는가?
			/// </summary>
			public bool m_isLocationSelected = false;

			/// <summary>
			/// 위치 객체 임시 collider onoff
			/// </summary>
			public GameObject m_locationObject;

			/// <summary>
			/// 위치 인덱스를 가진다.
			/// </summary>
			public int m_locationIndex;

			public LocationElementData()
            {
				m_isLocationSelected = false;	// 
				m_locationObject = null;
            }
        }

		//public Camera m_camera;
		//public GraphicRaycaster m_grRaycaster;
		public Vector3 m_clickPosition;
		UnityEvent<GameObject> m_clickEvent;

		/// <summary>
		/// 위치요소 데이터가 잡혔을 경우 생성되는 객체
		/// </summary>
		LocationElementData locData = null;

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

			PlatformCode pCode = MainManager.Instance.Platform;
			if (!Platforms.IsDemoWebViewer(pCode)) return;  // 아래 코드는 데모 웹 뷰어에서 동작함.


			if (Selected3D == null) return;

			// 선택한 객체가 아이템 객체인가?
			IItem item;
			if(Selected3D.TryGetComponent<IItem>(out item))
            {
				// 이 객체는 locationElement를 포함하는가?
				if(_Items.IsLocationElement(Selected3D))
                {
					// locationElement에서 위치값을 검출한다.
					int locIndex = ((LocationElement)item).Index;

					// 신규 locationElementData를 생성하고, 검출한 정보를 저장한다.
					locData = new LocationElementData();

					locData.m_locationIndex = locIndex;
					locData.m_locationObject = Selected3D;

					// 임시로 collider끔
					locData.m_locationObject.GetComponent<Collider>().enabled = false;

					// 다시 collider 수행
					Get_Collect3DObject(m_clickPosition, out m_selected3D, out m_hit, out m_results);

					// 다시 Selected3D 연산 수행
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

					// collider 다시 활성화
					locData.m_locationObject.GetComponent<Collider>().enabled = true;
				}
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
					StartEvent_SelectObject(Elements.Last().Target, _sEvents);
					//m_clickEvent.Invoke(Elements.Last().Target);
					//ContentManager.Instance.OnSelect_3D(Elements.Last().Target);
				}
				else if (obj.TryGetComponent<Issue_Selectable>(out iObj))
				{
					StartEvent_SelectIssue(Elements.Last().Target, _sEvents);
					//m_clickEvent.Invoke(Elements.Last().Target);
					//ContentManager.Instance.OnSelect_Issue(Elements.Last().Target);
				}
			}
			// UI 또는 빈 공간을 선택한 경우
			else
			{
				// UI 선택의 결과가 0 이상인 경우
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

		public void StartEvent_SelectObject(GameObject _obj, Dictionary<InputEventType, AEventData> _sEvents)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				// 객체는 34단계에서 눌리면 안됨.
				AModuleStatus aStat = EventManager.Instance._Statement.GetModuleStatus(ModuleCode.Issue_Administration);
				if(aStat != null)
				{
					// 현재 상태가 기본 관리모드(1 2 5)인가?
					if (aStat.IsDefaultAdministrationMode())
					{
						m_clickEvent.Invoke(_obj);
					}
					// 지정모드 (3 4)
					else { }
				}
				
			}
			else if(Platforms.IsDemoWebViewer(pCode))
            {
				// 위치 데이터가 null일 경우
				if(locData == null)
                {
					m_clickEvent.Invoke(_obj);
					ContentManager.Instance.OnSelect_3D(_obj);

					string curr = Elements.First().Target.name;
					string selected = "";
					if(_sEvents.ContainsKey(InputEventType.Input_clickSuccessUp))
					{
						selected = _sEvents[InputEventType.Input_clickSuccessUp].Elements.First().Target.name;
					}

					if (curr == selected)
					{
						Debug.Log("같은 객체를 선택함");
					}
                }
				// 위치 데이터가 null이 아닌 경우
				else if(locData != null)
                {
					//locData.m_locationIndex	// 인덱스
					//m_hit.point; // 클릭위치

					// 캐시 객체가 있는지 확인
					if(EventManager.Instance._CachePin == null)
                    {
						EventManager.Instance._CachePin = _Items.CreateCachePin(m_hit);
                    }
					else
                    {
						_Items.MoveCachePin(EventManager.Instance._CachePin, m_hit);
                    }

					ContentManager.Instance.Module<Module_WebAPI>().SendRequest(SendRequestCode.SelectSurfaceLocation, locData.m_locationIndex, m_hit.point);

					locData = null;
                }
			}
			//else if(Platforms.IsViewerPlatform(pCode))
			//{
			//	m_clickEvent.Invoke(_obj);
			//	ContentManager.Instance.OnSelect_3D(_obj);
			//}
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
					// 각 UI 개체 중에서 UITemplate_SmartInspect에 해당하는 개체가 있는지 확인한다. 존재하는 경우
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

		public void StartEvent_SelectIssue(GameObject _obj, Dictionary<InputEventType, AEventData> _sEvents)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			if(Platforms.IsDemoAdminViewer(pCode))
			{
				m_clickEvent.Invoke(_obj);
			}
			else if(Platforms.IsDemoWebViewer(pCode))
            {
				m_clickEvent.Invoke(_obj);
				ContentManager.Instance.OnSelect_Issue(_obj);
			}
			//else if(Platforms.IsViewerPlatform(pCode))
			//{
			//	m_clickEvent.Invoke(_obj);
			//	ContentManager.Instance.OnSelect_Issue(_obj);
			//}
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
				// ui 선택리스트 0 아니고 clickDown이 있는가?
				if (Results.Count != 0 && _sEvents.ContainsKey(InputEventType.Input_clickDown)) 
				{
					AModuleStatus aStat = EventManager.Instance._Statement.GetModuleStatus(ModuleCode.Issue_Administration);

					// null 아니고 지정모드(3 4)인가?
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
			else if(Platforms.IsDemoWebViewer(pCode))
            {
				m_clickEvent.Invoke(null);
				ContentManager.Instance.OnSelect_3D(null);
			}
			//else if(Platforms.IsViewerPlatform(pCode))
			//{
			//	m_clickEvent.Invoke(null);
			//	ContentManager.Instance.OnSelect_3D(null);
			//}
			else if(Platforms.IsSmartInspectPlatform(pCode))
            {
				m_clickEvent.Invoke(null);

				Module_Interaction interaction = Content.SmartInspectManager.Instance.Module<Module_Interaction>(ModuleID.Interaction);

				UITemplate_SmartInspect ui;
				foreach (var value in interaction.UiInstances)
				{
					// 각 UI 개체 중에서 UITemplate_SmartInspect에 해당하는 개체가 있는지 확인한다. 존재하는 경우
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
