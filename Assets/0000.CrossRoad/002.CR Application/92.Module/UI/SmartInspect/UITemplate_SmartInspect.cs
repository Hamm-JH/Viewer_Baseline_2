using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Definition.Data;
    using SmartInspect;
    using View;

    public partial class UITemplate_SmartInspect : RootUI
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
		/// 프로젝트 범용 패널 모음
		/// </summary>
		[Header("프로젝트 범용 패널")]
		public GeneralElement m_general;

        //public override void GetUIEvent

        public override void GetUIEventPacket(Hover_EventType _type, APacket _value)
        {
            Elements.ForEach(x => x.GetUIEventPacket<Hover_EventType>(_type, _value));
        }

        public override void GetUIEventPacket(BottomBar_EventType _type, APacket _value)
        {
            Elements.ForEach(x => x.GetUIEventPacket<BottomBar_EventType>(_type, _value));
        }

        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
        {

        }

        public override void GetUIEvent(float _value, Inspect_EventType _uType, Interactable _setter)
        {

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
            switch (_uType)
            {
                case Inspect_EventType.Image_Enlarge_Center:
                    {
                        Panel_EnlargeImage(_setter);
                    }
                    break;

                case Inspect_EventType.Toggle:
                    {
                        Event_Toggle_ViewMode(_setter.ChildPanel);
                    }
                    break;

                case Inspect_EventType.Toggle_FacInfo:
                    {
                        Event_Toggle_FacInfo();
                    }
                    break;
            }
        }
    }
}
