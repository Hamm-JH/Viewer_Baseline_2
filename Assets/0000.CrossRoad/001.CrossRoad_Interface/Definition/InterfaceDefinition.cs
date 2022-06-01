using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	/// <summary>
	/// UI에서 이벤트에 반응할때의 분기를 결정
	/// </summary>
	public enum UIEventType
	{
		Null = 0,
		NotWork,

		/// <summary>
		/// UI 토글
		/// </summary>
		Toggle = 0x10,
		Toggle_ChildPanel1 = 0x11,

		Viewport_ViewMode = 0x20,
		Viewport_ViewMode_ISO = 0x21,
		Viewport_ViewMode_TOP = 0x22,
		Viewport_ViewMode_BOTTOM = 0x23,
		Viewport_ViewMode_SIDE = 0x24,
		Viewport_ViewMode_SIDE_FRONT = 0x25,
		Viewport_ViewMode_SIDE_BACK = 0x26,
		Viewport_ViewMode_SIDE_LEFT = 0x27,
		Viewport_ViewMode_SIDE_RIGHT = 0x28,

		/// <summary>
		/// 보기 :: 홈 
		/// </summary>
		View_Home = 0x29,

		OrthoView = 0x30,
		OrthoView_Orthogonal = 0x31,
		OrthoView_Perspective = 0x32,

		Mode_ShowAll = 0x40,
		/// <summary>
		/// 객체 선택시 Hide, Isolate
		/// Off가 붙으면 아예 객체를 끈다.
		/// </summary>
		Mode_Hide = 0x41,
		Mode_Hide_Off = 0x42,
		Mode_Isolate = 0x46,
		Mode_Isolate_Off = 0x47,


		Slider_Model_Transparency = 0x100,
		Slider_Icon_Scale = 0x101,

		Test_Surface = 0x110,

		Ad_nav_state1 = 0x200,
		Ad_nav_state2 = 0x201,
		Ad_nav_state3 = 0x202,
		Ad_nav_state4 = 0x203,
		Ad_nav_state5 = 0x204,

		Ad_Prev = 0x210,
		Ad_Next = 0x211,

		Ad_SetKeymapCenter = 0x212,
		Ad_SetImageWindow = 0x213,

		Ad_St3_Toggle = 0x216,
		Ad_St4_Toggle = 0x217,
		Ad_St5_Toggle = 0x218,
		Ad_St5_Toggle_m1_dmg = 0x219,
		Ad_St5_Toggle_m1_rcv = 0x21a,
		Ad_St5_Toggle_m1_rein = 0x21b,

		Ad_St5_PrintExcel = 0x21c,

		Ad_St_DimToggle = 0x21d,
		Ad_St_PrintDim = 0x21e,

		/// <summary>
		/// 모듈 왼쪽 dmg, rcv, setting
		/// </summary>
		Ins_Left_ModuleDmg = 0x300,
		Ins_Left_ModuleRcv = 0x301,
		Ins_Left_ModuleSet = 0x302,

		Ins_Top_Profile = 0x310,

		Ins_Panel_PerIssueCount = 0x311,

		/// <summary>
		/// Module Dmg LBar 1
		/// </summary>
		Ins_MDmg_LBar_1DamagedInfo = 0x320,
		Ins_MDmg_LBar_2DamagedList = 0x321,
		Ins_MDmg_LBar_3StatusInfo = 0x322,
		
		Ins_MRcv_LBar_1RepairedList = 0x340,
		Ins_MRcv_LBar_2ReinforcedList = 0x341,
		Ins_MRcv_LBar_3StatusInfo = 0x342,
		Ins_MRcv_LBar_4Dimension = 0x343,
		Ins_MRcv_LBar_5DrawingPrint = 0x344,

		Ins_MAdm_LBar_1TotalMaintenance = 0x346,
		Ins_MAdm_LBar_2MaintenanceTimeline = 0x347,
		Ins_MAdm_LBar_3StatusInfo = 0x348,
		Ins_MAdm_LBar_4Report = 0x349,

		Ins_Panel_OnSelectCount = 0x34f,
		Ins_Panel_OnImage = 0x350,
		Ins_Panel_OnDetail = 0x351,

		Ins_MAdm_TL_SelectYear1 = 0x352,
		Ins_MAdm_TL_SelectYear5 = 0x353,
		Ins_MAdm_TL_SelectYear10 = 0x354,
		Ins_MAdm_TL_SelectYear50 = 0x355,
		Ins_MAdm_TL_SelectALL = 0x356,
		Ins_MAdm_TL_SelectCrack = 0x357,
		Ins_MAdm_TL_SelectSpalling = 0x358,
		Ins_MAdm_TL_SelectEfflorescence = 0x359,
		Ins_MAdm_TL_SelectBreakage = 0x35a,
		
		Ins_MAdm_TL_PrevYear = 0x35b,
		Ins_MAdm_TL_NextYear = 0x35c,
	}

	public enum Inspect_EventType
    {
		NotWork,

		BtnBar_01_Home,
		BtnBar_02_ViewPort,
		BtnBar_03_OrthoView,
		BtnBar_04_ShowAll,
		BtnBar_05_Hide,
		BtnBar_06_Isolate,
		BtnBar_07_Setting,
		BtnBar_08_SwitchBar,

		BtnBar_11_ZoomIn,
		BtnBar_12_ZoomOut,

		ViewPort_01_ISO,
		ViewPort_02_TOP,
		ViewPort_03_SIDE,
		ViewPort_04_BOTTOM,

		OrthoMode_01_OrthoView,
		OrthoMode_02_PerspectiveView,

		Setting_01_IconSize,
		Setting_02_ModelTransparency,
		Setting_03_ZoomSensitivity,
		Setting_04_FontSize,

		Image_Enlarge_Center,
	}

	public enum BottomBar_EventType
    {
		NotWork = 0,

		Btn1_CameraMode = 1,
		Btn2_ViewPosition = 2,
		Btn3_OrthoMode = 3,
		Btn4_ShowMode = 4,
		Btn5_Pan = 5,
		Btn6_Setting = 6,
		Btn7_Info = 7,

		ZoomIn,
		ZoomOut,
    }

	public enum Compass_EventType
    {
		NotWork,

		Compass_FirstPosition,
		Compass_LastPosition,
    }

	public enum ModuleID
	{
		NotDef = -1,
		/// <summary>
		/// Code :: 모델
		/// </summary>
		Model = 0x10,

		/// <summary>
		/// Code :: 상호작용 ???
		/// </summary>
		Interaction = 0x20,

		/// <summary>
		/// Code :: 웹 상호작용
		/// </summary>
		WebAPI = 0x30,

		/// <summary>
		/// Code :: 그래픽
		/// </summary>
		Graphic = 0x50,

		/// <summary>
		/// Code :: UI 속성
		/// </summary>
		Prop_UI = 0x50,

		/// <summary>
		/// Item :: 한정목적용 Prefab 관리
		/// </summary>
		Item = 0x60,
	}

	public enum FunctionCode
	{
		Null = -1,

		Model_Import = 0x11,
		Model_Export = 0x12,

		Interaction_3D = 0x21,
		Interaction_UI = 0x22,

		API = 0x30,
		API_Front = 0x31,
		API_Back = 0x32,

		Select_DefaultMove = 0x41,
		Select_SelectObject = 0x42,
		Select_DrawPoint = 0x43,

		Graphic = 0x51,

		Item_LocationGuide = 0x61,
		Item_Compass = 0x62,
	}
}
