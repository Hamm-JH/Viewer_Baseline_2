﻿using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    using SmartInspect;

    public partial class UITemplate_SmartInspect : AUI
    {
		/// <summary>
		/// 프로젝트 이벤트 베이스
		/// </summary>
		[Header("프로젝트 이벤트 베이스")]
		public EventBase m_eventBase;

		[Header("UI 리소스 모음")]
		public UIResources m_uiResources;

		/// <summary>
		/// 모듈 단위 요소들 모음
		/// </summary>
		[Header("프로젝트 모듈 요소")]
        public ModuleElement m_moduleElements;

		/// <summary>
		/// 프로젝트 베이스 UI 모음
		/// </summary>
		[Header("프로젝트 베이스 UI")]
        public Base m_basePanel;

		/// <summary>
		/// 프로젝트 하단 바 모음
		/// </summary>
		[Header("프로젝트 하단 바")]
        public BottomBar m_bottomBar;

		/// <summary>
		/// 프로젝트 범용 패널 모음
		/// </summary>
		[Header("프로젝트 범용 패널")]
		public GeneralElement m_general;

		public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
		{
			
		}

        public override void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter)
        {
            switch(_uType)
            {
				case Inspect_EventType.Setting_01_IconSize:
					Setting_SetIconSize(_value, _setter);
					break;

				case Inspect_EventType.Setting_02_ModelTransparency:
					Setting_SetModelTransparency(_value, _setter);
					break;

				case Inspect_EventType.Setting_03_ZoomSensitivity:
					Setting_SetZoomSensitivity(_value, _setter);
					break;

				case Inspect_EventType.Setting_04_FontSize:
					Setting_SetFontSize(_value, _setter);
					break;
            }
        }

        public override void GetUIEvent(UIEventType _uType, Interactable _setter)
		{
			switch (_uType)
			{
                #region Base Events

                #region Base - TopMenu

                case UIEventType.Ins_Top_Profile:
					TopMenu_ToggleProfile();
					break;

                #endregion

                #region Base - LeftMenu

                case UIEventType.Ins_Left_ModuleDmg:
					LeftMenu_SetDmg();
					break;

				case UIEventType.Ins_Left_ModuleRcv:
					LeftMenu_SetRcv();
					break;

				case UIEventType.Ins_Left_ModuleSet:
					LeftMenu_SetSetting();
					break;

				#endregion

				#endregion

				#region Module Dmg

				case UIEventType.Ins_MDmg_LBar_1DamagedInfo:
					MDmg_LBar_1ToggleDInfo();
					break;

				case UIEventType.Ins_MDmg_LBar_2DamagedList:
					MDmg_LBar_2ToggleDList();
					break;

				case UIEventType.Ins_MDmg_LBar_3StatusInfo:
					MDmg_LBar_3ToggleSInfo();
					break;

				#endregion

				#region Module Rcv

				case UIEventType.Ins_MRcv_LBar_1RepairedList:
					MRcv_LBar_1RepairList();
					break;

				case UIEventType.Ins_MRcv_LBar_2ReinforcedList:
					MRcv_LBar_2ReinforcedList();
					break;

				case UIEventType.Ins_MRcv_LBar_3StatusInfo:
					MRcv_LBar_3StatusInfo();
					break;

				case UIEventType.Ins_MRcv_LBar_4Dimension:
					MRcv_LBar_4Dimension();
					break;

				case UIEventType.Ins_MRcv_LBar_5DrawingPrint:
					MRcv_LBar_5DrawingPrint();
					break;

				#endregion

				#region Module Adm

				case UIEventType.Ins_MAdm_LBar_1TotalMaintenance:
					MAdm_LBar_1ToggleTotalMaintenance();
					break;

				case UIEventType.Ins_MAdm_LBar_2MaintenanceTimeline:
					MAdm_LBar_2ToggleMaintenanceTimeline();
					break;

				case UIEventType.Ins_MAdm_LBar_3StatusInfo:
					MAdm_LBar_3ToggleInfo();
					break;

				case UIEventType.Ins_MAdm_LBar_4Report:
					MAdm_LBar_4PrintReport();
					break;

				//-------------------------------------

				case UIEventType.Ins_MAdm_TL_SelectYear1:
					Ins_MAdm_TL_SelectYear1(_setter);
					break;

				case UIEventType.Ins_MAdm_TL_SelectYear5:
					Ins_MAdm_TL_SelectYear5(_setter);
					break;

				case UIEventType.Ins_MAdm_TL_SelectYear10:
					Ins_MAdm_TL_SelectYear10(_setter);
					break;

				case UIEventType.Ins_MAdm_TL_SelectYear50:
					Ins_MAdm_TL_SelectYear50(_setter);
					break;

				//-------------------------------------

				case UIEventType.Ins_MAdm_TL_SelectALL:
					Ins_MAdm_TL_SelectALL(_setter);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Ins_MAdm_TL_SelectCrack:
					Ins_MAdm_TL_SelectCrack(_setter);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Ins_MAdm_TL_SelectSpalling:
					Ins_MAdm_TL_SelectSpalling(_setter);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Ins_MAdm_TL_SelectEfflorescence:
					Ins_MAdm_TL_SelectEfflorescence(_setter);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Ins_MAdm_TL_SelectBreakage:
					Ins_MAdm_TL_SelectBreakage(_setter);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				//-------------------------------------

				case UIEventType.Ins_MAdm_TL_PrevYear:
					Ins_MAdm_TL_PrevYear(_setter);
					break;

				case UIEventType.Ins_MAdm_TL_NextYear:
					Ins_MAdm_TL_NextYear(_setter);
					break;

				#endregion

				#region ButtonBar

				// btnBar 01
				case UIEventType.View_Home:
					// 초기 화면으로 복귀
					Event_View_Home();
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					ButtonBar_OnSelect(_setter);
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

				#region Ins_Panel

				case UIEventType.Ins_Panel_OnImage:
					Panel_SetImage(_setter);
					break;

				case UIEventType.Ins_Panel_OnDetail:
					Panel_SetDetail(_setter);
					break;

					// issue count 변경
				case UIEventType.Ins_Panel_OnSelectCount:
					Ins_Panel_OnSelectCount(_setter);
					break;

                #endregion

            }
        }

        public override void GetUIEvent(Inspect_EventType _uType, Interactable _setter)
        {
            switch(_uType)
            {
				//--------------------------------------------------

				case Inspect_EventType.BtnBar_01_Home:
                    {
						Event_View_Home();
						Event_Toggle_ChildPanel(1, _setter.ChildPanel);
						ButtonBar_OnSelect(_setter);	// ButtonBar 토글
                    }
					break;

				case Inspect_EventType.BtnBar_02_ViewPort:
                    {
						Event_Toggle_ChildPanel(1, _setter.ChildPanel);
						Event_Toggle_ViewMode(_setter.ChildPanel);
						ButtonBar_OnSelect(_setter);
                    }
					break;

				case Inspect_EventType.BtnBar_03_OrthoView:
                    {
						Event_Toggle_ChildPanel(1, _setter.ChildPanel);
						Event_Toggle_ViewMode(_setter.ChildPanel);
						ButtonBar_OnSelect(_setter);
					}
					break;

				case Inspect_EventType.BtnBar_04_ShowAll:
                    {
						Event_Mode_ShowAll();
						Event_Toggle_ChildPanel(1, _setter.ChildPanel);
						ButtonBar_OnSelect(_setter);
					}
					break;

				case Inspect_EventType.BtnBar_05_Hide:
                    {
						Event_Mode_HideIsolate(_uType);
						Event_Toggle_ChildPanel(1, _setter.ChildPanel);
						ButtonBar_OnSelect(_setter);
					}
					break;

				case Inspect_EventType.BtnBar_06_Isolate:
                    {
						Event_Mode_HideIsolate(_uType);
						Event_Toggle_ChildPanel(1, _setter.ChildPanel);
						ButtonBar_OnSelect(_setter);
					}
					break;

				case Inspect_EventType.BtnBar_07_Setting:
                    {
						Event_Toggle_ChildPanel(1, _setter.ChildPanel);
						Event_Toggle_ViewMode(_setter.ChildPanel);
						ButtonBar_OnSelect(_setter);
					}
					break;

				case Inspect_EventType.BtnBar_08_SwitchBar:
                    {
						ButtonBar_MoveUI(_setter);

					}
					break;

				//--------------------------------------------------

				case Inspect_EventType.ViewPort_01_ISO:
                    {
						Event_Toggle_ViewMode(UIEventType.Viewport_ViewMode_ISO);
						Viewport_OnSelect(_setter);
						Event_Toggle_ViewMode(_setter.ChildPanel);
					}
					break;

				case Inspect_EventType.ViewPort_02_TOP:
                    {
						Event_Toggle_ViewMode(UIEventType.Viewport_ViewMode_TOP);
						Viewport_OnSelect(_setter);
						Event_Toggle_ViewMode(_setter.ChildPanel);
					}
					break;

				case Inspect_EventType.ViewPort_03_SIDE:
                    {
						Event_Toggle_ViewMode(UIEventType.Viewport_ViewMode_SIDE);
						Viewport_OnSelect(_setter);
						Event_Toggle_ViewMode(_setter.ChildPanel);
					}
					break;

				case Inspect_EventType.ViewPort_04_BOTTOM:
                    {
						Event_Toggle_ViewMode(UIEventType.Viewport_ViewMode_BOTTOM);
						Viewport_OnSelect(_setter);
						Event_Toggle_ViewMode(_setter.ChildPanel);
					}
					break;

				//--------------------------------------------------

				case Inspect_EventType.OrthoMode_01_OrthoView:
                    {
						Event_ToggleOrthoView(true);
						OrthoMode_OnSelect(_setter);
						Event_Toggle_ViewMode(_setter.ChildPanel);
					}
					break;

				case Inspect_EventType.OrthoMode_02_PerspectiveView:
                    {
						Event_ToggleOrthoView(false);
						OrthoMode_OnSelect(_setter);
						Event_Toggle_ViewMode(_setter.ChildPanel);
					}
					break;

				//--------------------------------------------------
			}
		}
    }
}
