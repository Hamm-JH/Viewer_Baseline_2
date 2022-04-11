using Definition;
using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : AUI
    {
        /// <summary>
        /// UI 템플릿 초기화시 실행
        /// </summary>
        public override void OnStart()
        {
            Debug.Log("UITemplate_SmartInspect Start method");
        }

        public override void GetUIEvent(UIEventType _uType, Interactable _setter)
        {
			switch (_uType)
			{
				#region ButtonBar

				case UIEventType.View_Home:
					// 초기 화면으로 복귀
					Event_View_Home();
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					break;

				case UIEventType.Toggle:
				case UIEventType.Viewport_ViewMode:
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Toggle_ChildPanel1:
					// ChildPanel 1번 토글
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Viewport_ViewMode_ISO:
				case UIEventType.Viewport_ViewMode_TOP:
				case UIEventType.Viewport_ViewMode_SIDE:
				case UIEventType.Viewport_ViewMode_BOTTOM:
					Event_Toggle_ViewMode(_uType);
					break;

				case UIEventType.OrthoView_Orthogonal:
					Event_ToggleOrthoView(true);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.OrthoView_Perspective:
					Event_ToggleOrthoView(false);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				// 객체 모두 다시 켜기
				case UIEventType.Mode_ShowAll:
					Event_Mode_ShowAll();
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					break;

				case UIEventType.Mode_Hide:
				case UIEventType.Mode_Isolate:
					Event_Mode_HideIsolate(_uType);
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					break;

					#endregion
			}
		}

        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
        {
            throw new System.NotImplementedException();
        }

        public override void OnModuleComplete()
        {
            throw new System.NotImplementedException();
        }


        public override void ReInvokeEvent()
        {
            throw new System.NotImplementedException();
        }

        public override void SetObjectData_Tunnel(GameObject selected)
        {
            throw new System.NotImplementedException();
        }

        public override void TogglePanelList(int _index, GameObject _exclusive)
        {
			m_bottomBar.m_subPanels.ForEach(x => x.SetActive((x != _exclusive) ? false : x.activeSelf));
        }
    }
}
