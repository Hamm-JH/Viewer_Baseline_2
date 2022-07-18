using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management.Events.Inputs
{
	using Definition;
    using Management.Content;
    using Module.Interaction;
    using Module.UI;
    using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class Event_Focus : EventData_Input
	{
		private Vector3 m_focus;
		private float m_focusDelta;
		UnityEvent<Vector3, float> m_focusEvent;

		/// <summary>
		/// 포커스 이벤트 
		/// </summary>
		/// <param name="_eventType">이벤트 분류</param>
		/// <param name="_focus">포커스 위치</param>
		/// <param name="_delta">포커스 정도</param>
		/// <param name="_camera">카메라</param>
		/// <param name="_grRaycaster">그래픽 레이캐스터</param>
		/// <param name="_event">포커스 이벤트</param>
		public Event_Focus(InputEventType _eventType,
			Vector3 _focus, float _delta,
			Camera _camera, GraphicRaycaster _grRaycaster,
			UnityEvent<Vector3, float> _event)
		{
			StatusCode = Status.Ready;

			EventType = _eventType;
			m_camera = _camera;
			m_grRaycaster = _grRaycaster;
			m_focus = _focus;
			m_focusDelta = _delta;
			m_focusEvent = _event;
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
		/// <param name="_sEvents">현재 이벤트 리스트</param>
		/// <exception cref="Definition.Exceptions.PlatformNotDefinedException">정의되지 않은 플랫폼 코드 접근</exception>
		public override void DoEvent(Dictionary<InputEventType, AEventData> _sEvents)
		{
			m_selected3D = null;
			m_hit = default(RaycastHit);
			m_results = new List<RaycastResult>();

			Get_Collect3DObject(Input.mousePosition, out m_selected3D, out m_hit, out m_results);

			float _delta = m_focusDelta;
			PlatformCode pCode = MainManager.Instance.Platform;
			if (Platforms.IsDemoWebViewer(pCode)) 
			{
				_delta = m_focusDelta;
			}
			else if(Platforms.IsSmartInspectPlatform(pCode))
            {
				float zoomSensitivity = ((SmartInspectManager)(SmartInspectManager.Instance)).setting.ZoomSensitivity;

				//Module_Interaction interaction = ContentManager.Instance.Module<Module_Interaction>();

				//UITemplate_SmartInspect inspect = (UITemplate_SmartInspect)interaction.UiInstances[0];

				_delta = m_focusDelta * zoomSensitivity;
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }

			if (Selected3D != null)
			{
				m_focusEvent.Invoke(m_focus, _delta);
			}
			// 빈 공간을 누른 경우
			else if (m_results.Count == 0)
			{
				m_focusEvent.Invoke(m_focus, _delta);
			}
			// UI 객체를 누른 경우 m_results.Count != 0
			else
			{
				pCode = MainManager.Instance.Platform;

				if(Platforms.IsDemoAdminViewer(pCode))
                {
					Debug.LogError("포커스시 키맵 UI를 포커싱하는지 확인 필요");
					ContentManager.Instance.Input_KeymapFocus(m_focus, _delta);
                }
			}
		}
	}
}
