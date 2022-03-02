using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Management.Events;
	using UnityEngine.Events;
	using View;

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
			_value = 0.1f + _value * 0.9f;

			//float boundary = 0.8f;
			//bool isOpaque = _value > boundary ? true : false;

			// 모든 3D 모델을 순회한다.
			foreach(GameObject obj in _ModelObjects)
			{
				Obj_Selectable selectable;
				if (obj.TryGetComponent<Obj_Selectable>(out selectable))
				{
					selectable.OnDeselect<UIEventType, float>(UIEventType.Slider_Model_Transparency, _value);
				}
			}
		}

		public void Set_Issue_Scale(float _value)
		{
			_value = 0.2f + _value * 0.8f;

			foreach(GameObject obj in _IssueObjects)
			{
				Issue_Selectable selectable;
				if(obj.TryGetComponent<Issue_Selectable>(out selectable))
				{
					selectable.OnDeselect<UIEventType, float>(UIEventType.Slider_Icon_Scale, _value);
				}
			}
		}

		public void Reset_ModelObject()
		{
			foreach(GameObject obj in _ModelObjects)
			{
				MeshRenderer render;
				if (obj.TryGetComponent<MeshRenderer>(out render))
				{
					Material mat = render.material;
					Color colr = mat.color;

					Materials.Set(render, ColorType.Default1, 1);
					//render.material.SetColor("_Color", new Color(colr.r, colr.g, colr.b, 1));

					Materials.ToOpaqueMode(render);
				}

				Obj_Selectable selectable;
				if(obj.TryGetComponent<Obj_Selectable>(out selectable))
				{
					selectable.IsInteractable = true;
				}
			}
		}

		public void Toggle_ModelObject(UIEventType _eventType, ToggleType _toggleType)
		{
			EventManager.Instance.OnEvent(new Events.EventData_UI(
				_eventType: InputEventType.UI_Invoke,
				_uiEvent: _eventType,
				_toggle: _toggleType,
				_modelObj: _ModelObjects
				));
		}

		/// <summary>
		/// 가져온 UI 인스턴스의 특정 레벨의 child 패널을 모두 끈다.
		/// </summary>
		/// <param name="index"></param>
		public void Toggle_ChildTabs(int index)
		{
			_Interaction.UiInstance.TogglePanelList(index, null);
		}

		public void Function_ToggleOrthoView(bool _isOrthogonal)
		{
			MainManager.Instance.MainCamera.orthographic = _isOrthogonal;
		}
	}
}
