using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using UnityEngine.Events;

	public partial class ContentManager : IManager<ContentManager>
	{
		/// <summary>
		/// 카메라 중심 위치 변경
		/// </summary>
		/// <param name="eventType"></param>
		public void SetCameraCenterPosition(UIEventType eventType = UIEventType.Viewport_ViewMode_ISO)
		{
			Bounds _b = _CenterBounds;
			Canvas _canvas = _Canvas;

			MainManager.Instance.SetCameraPosition(_b, _canvas, eventType);
		}

		/// <summary>
		/// UI에 객체 선택시의 정보 업데이트
		/// </summary>
		/// <param name="selected"></param>
		public void Get_SelectedData_UpdateUI(GameObject selected)
		{
			// 터널 정보 업데이트
			_UIInstance.SetObjectData_Tunnel(selected);
		}

		/// <summary>
		/// Module_Model에서 모델 Import
		/// </summary>
		/// <param name="action"></param>
		public void RunModule_ModelImport(UnityAction<string> action)
		{
			action.Invoke(MainManager.Instance.ModelURI);
		}

		/// <summary>
		/// Module_Interaction UI Instantiate
		/// </summary>
		/// <param name="action"></param>
		public void RunModule_UIInstantiate(UnityAction<GameObject> action)
		{
			PlatformCode platform = MainManager.Instance.Platform;

			GameObject template = Template.GetUITemplate(platform);

			if(template != null)
			{
				action.Invoke(template);
			}
			else
			{
				throw new System.Exception("RunModule_UIInstantiate :: 생성하려는 UI 템플릿을 찾을 수 없습니다.");
			}
		}

		public void Set_Model_Transparency(float _value)
		{
			foreach(GameObject obj in _ModelObjects)
			{
				MeshRenderer render;
				if(obj.TryGetComponent<MeshRenderer>(out render))
				{
					Material mat = render.material;
					Color colr = mat.color;
					render.material.SetColor("_Color", new Color(colr.r, colr.g, colr.b, _value));
				}
			}
		}

		public void Toggle_ModelObject(ToggleType type)
		{
			// TODO 0223 1순위
			Events.EventData eData = EventManager.Instance.SelectedEvent;

			if(eData.Element == null)
			{
				Debug.LogError("EventData is null");
				return;
			}
			if(eData.Element.Target == null)
			{
				Debug.LogError("EventData Object is null");
				return;
			}

			GameObject selected = EventManager.Instance.SelectedEvent.Element.Target;

			bool isHide = false;
			switch(type)
			{
				case ToggleType.Hide:	isHide = true;	break;
				case ToggleType.Isolate:isHide = false;	break;
			}

			foreach(GameObject obj in _ModelObjects)
			{
				bool isSelctedObject = selected == obj;

				float alpha = 0.1f;
				// 이 객체가 맞음, 숨겨야됨			true true -> alpha = 0.1
				// 이 객체가 맞음, 제외 숨겨야됨	true false -> alpha = 1
				// 이 객체 아님, 숨겨야됨			false true -> alpha = 1
				// 이 객체 아님, 제외 숨겨야됨		false false -> alpha = 0.1

				if(isSelctedObject)
				{
					alpha = isHide ? 0.1f : 1f;
				}
				else
				{
					alpha = isHide ? 1f : 0.1f;
				}

				MeshRenderer render;
				if(obj.TryGetComponent<MeshRenderer>(out render))
				{
					Material mat = render.material;
					Color colr = mat.color;
					render.material.SetColor("_Color", new Color(colr.r, colr.g, colr.b, alpha));
				}
			}
		}

		public void Function_ToggleOrthoView(bool _isOrthogonal)
		{
			MainManager.Instance.MainCamera.orthographic = _isOrthogonal;
		}
	}
}
