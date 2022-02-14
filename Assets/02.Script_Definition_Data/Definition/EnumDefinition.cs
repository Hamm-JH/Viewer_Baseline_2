using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{

	/// <summary>
	/// 플랫폼 정의
	/// </summary>
	public enum PlatformCode
	{
		NotDef = -1,
		WebGL_Template1 = 0x10,
		WebGL_Template2 = 0x11,
		Mobile_Template1 = 0x20,
		Mobile_Template2 = 0x21,
		PC_Maker1 = 0x30,
		PC_Viewer1 = 0x31,
	}

	public enum GraphicCode
	{
		NotDef = -1,
		Template00 = 0x00,
		Template10 = 0x10,
		Template20 = 0x20,
		Template30 = 0x30,
	}

	/// <summary>
	/// 씬 이름을 관리하는 코드
	/// </summary>
	public enum SceneName
	{
		NotDef = -1,
		Maker = 0x00,
		PC_Viewer = 0x01,

	}

	public enum ModuleID
	{
		NotDef = -1,
		AModel = 0x10,
		Model_Import = 0x11,
		Model_Export = 0x12,

		AInteraction = 0x20,
		Interaction_3D = 0x21,
		Interaction_UI = 0x22,

		AWebAPI = 0x30,
		API_Front = 0x31,
		API_Back = 0x32
	}

	/// <summary>
	/// Control.Input :: 입력 이벤트, clickEvent의 선택된 객체유형 정의
	/// </summary>
	public enum ObjectType
	{
		Object,
		UI
	}

	/// <summary>
	/// MainManager :: 주 관리자에서 이벤트 변수에 접근할 때 사용하는 코드
	/// </summary>
	public enum MainEventType
	{
		NotDef = -1,
		Input_click,
		Input_drag,
		Input_focus,
		Input_key
	}

	public enum ManagerActionIndex
	{
		NotDef = -1,
		InputAction = 0,
	}

	public enum UIEventType
	{
		Toggle_ViewMode = 0x10,
		Toggle_ViewMode_ISO = 0x11,
		Toggle_ViewMode_TOP = 0x12,
		Toggle_ViewMode_SIDE = 0x13,
		Toggle_ViewMode_BOTTOM = 0x14,


		Fit_Center = 0x50,
	}

	public enum ColorType
	{
		Default1 = 0x10,
		Selected1 = 0x20,
		
	}

	public enum MaterialType
	{
		Default1 = 0x10,
		ObjDefault1 = 0x99,
	}

	public enum PrefabType
	{
		Decal = 0x10,

	}

	public enum LayerNames
	{
		Default = 0,
		TransparentFX = 1,

		Water = 4,
		UI = 5,
		DecalPoint = 6
	}

	//----- Test

	/// <summary>
	/// 객체의 타입 (Material 할당용)
	/// </summary>
	public enum TunnelObjectType
	{
		Null = -1,
		// 기타
		ETC_EmergencyCall,
		ETC_fireplug,
		ETC_EmergencyExit,
		ETC_IndicatorLight1,
		ETC_IndicatorLight2,

		MAIN_Drain,
		MAIN_Cover,
		MAIN_Paving,
		MAIN_SideWall,
		MAIN_Ceiling,
		MAIN_CorrugatedSteel,

		LIGHT,

		JET,

		WALL_CentralReservation,
		WALL_Gate,
		WALL_Slope
	}
}
