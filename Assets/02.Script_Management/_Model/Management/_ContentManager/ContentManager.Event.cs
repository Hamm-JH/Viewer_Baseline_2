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

		public void Function_ToggleOrthoView(bool _isOrthogonal)
		{
			MainManager.Instance.MainCamera.orthographic = _isOrthogonal;
		}
	}
}
