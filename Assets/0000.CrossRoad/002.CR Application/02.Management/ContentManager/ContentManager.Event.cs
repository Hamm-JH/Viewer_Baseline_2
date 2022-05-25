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
		public void CreateNewModule(ModuleCode mCode)
		{
			EventManager.Instance._Statement.CreateNewModule(mCode);
		}

		public ModuleStatus GetModuleStatus(ModuleCode mCode)
		{
			AModuleStatus mstatus = EventManager.Instance._Statement.GetModuleStatus(mCode);

			return mstatus.m_moduleStatus;
		}

		public void SetModuleStatus(ModuleCode _mCode, ModuleStatus _mStatus)
		{
			EventManager.Instance._Statement.SetModuleStatus(_mCode, _mStatus);
		}

		/// <summary>
		/// 카메라 중심 위치 변경
		/// </summary>
		/// <param name="eventType"></param>
		public void SetCameraCenterPosition(UIEventType eventType = UIEventType.Viewport_ViewMode_ISO)
		{
			Bounds _b = _CenterBounds;
			Canvas _canvas = _Canvas;

			MainManager.Instance.ResetCamdata_targetOffset();
			MainManager.Instance.SetCameraPosition(_b, _canvas, eventType);
		}

		/// <summary>
		/// 특정 객체를 중심으로 카메라 각도 변경
		/// </summary>
		/// <param name="_obj"></param>
		/// <param name="_baseAngle"></param>
		/// <param name="_eType"></param>
		public void SetCameraCenter(GameObject _obj, Vector3 _baseAngle, UIEventType _eType)
		{
			MeshRenderer render;
			if(_obj.TryGetComponent<MeshRenderer>(out render))
			{
				Bounds _b = render.bounds;
				Canvas _canvas = _Canvas;

				MainManager.Instance.SetCameraPosition(_b, _canvas, _eType, _baseAngle);
			}
		}

		/// <summary>
		/// UI에 객체 선택시의 정보 업데이트
		/// </summary>
		/// <param name="selected"></param>
		public void Get_SelectedData_UpdateUI(GameObject selected)
		{
			// 터널 정보 업데이트
			_UIInstances.ForEach(x => x.SetObjectData_Tunnel(selected));
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
		public void RunModule_UIInstantiate(UnityAction<List<GameObject>> action)
		{
			PlatformCode platform = MainManager.Instance.Platform;

			List<GameObject> templates = Template.GetUITemplates(platform);

			if(templates != null)
			{
				action.Invoke(templates);
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

			bool IsCanProcessDecal = false;
			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsDemoWebViewer(pCode))
            {
				IsCanProcessDecal = true;
            }
			else
            {
				throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }

			foreach(GameObject obj in _IssueObjects)
			{
				Issue_Selectable selectable;
				if(obj.TryGetComponent<Issue_Selectable>(out selectable))
				{
					selectable.OnDeselect<UIEventType, float>(UIEventType.Slider_Icon_Scale, _value);

					if(IsCanProcessDecal)
					{
						selectable.Issue.Waypoint.IssueWayPoint.SetScale(_value);
					}
				}

			}
		}

		/// <summary>
		/// 모델 객체의 재질 상태를 리셋
		/// </summary>
		public void Reset_ModelObject()
		{
			PlatformCode pCode = MainManager.Instance.Platform;
			GraphicCode gCode = MainManager.Instance.Graphic;

			foreach(GameObject obj in _ModelObjects)
			{
				obj.SetActive(true);

				MeshRenderer render;
				if (obj.TryGetComponent<MeshRenderer>(out render))
				{
					//Material mat = render.material;
					//Color colr = mat.color;

					if(Platforms.IsBridgePlatform(pCode))
                    {
						Platform.Bridge.Bridge_Materials.Set(render, gCode, obj.name);
                    }
					else if(Platforms.IsTunnelPlatform(pCode))
                    {
						Platform.Tunnel.TunnelCode tCode = Platform.Tunnel.Tunnels.GetPartCode(obj.name);

						Platform.Tunnel.Tunnel_Materials.Set(render, gCode, tCode);
                    }
					else
                    {
						throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
                    }
					//Materials.Set(render, ColorType.Default1, 1);

					Materials.ToOpaqueMode(render);
				}

				Obj_Selectable selectable;
				if(obj.TryGetComponent<Obj_Selectable>(out selectable))
				{
					selectable.IsInteractable = true;


				}
			}
		}

		/// <summary>
		/// 등록단계에서 캐시 등록
		/// </summary>
		public void Cache_SelectedObject()
		{
			GameObject obj = ContentManager.Instance._SelectedObj;

			EventManager.Instance._CacheObject = obj;
		}

		/// <summary>
		/// 캐시 삭제
		/// </summary>
		public void Remove_Cache()
		{
			//EventManager.Instance._CacheObject = null;

			EventManager.Instance._Statement.Destroy_CacheObject();
			EventManager.Instance._Statement.Destroy_CachePin();
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
			_Interaction.UiInstances.ForEach(x => x.TogglePanelList(index, null));
		}

		public void Function_ToggleOrthoView(bool _isOrthogonal)
		{
			MainManager.Instance.MainCamera.orthographic = _isOrthogonal;
		}

		
	}
}
