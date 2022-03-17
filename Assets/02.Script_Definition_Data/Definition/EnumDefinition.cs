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
		WebGL_AdminViewer = 0x12,

		Mobile_Template1 = 0x20,
		Mobile_Template2 = 0x21,

		PC_Maker1 = 0x30,
		PC_Viewer_Tunnel = 0x31,
		PC_Viewer_Bridge = 0x32,
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
		Viewer = 0x01,

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
	}

	/// <summary>
	/// MainManager :: 주 관리자에서 이벤트 변수에 접근할 때 사용하는 코드
	/// </summary>
	public enum InputEventType
	{
		NotDef = -1,
		
		/// <summary>
		/// 뷰어 상태코드 정의
		/// </summary>
		Statement,

		/// <summary>
		/// 입력 관리코드에서 버튼 눌렀을때
		/// </summary>
		Input_clickDown,

		/// <summary>
		/// 입력 관리코드에서 드래그함 & 버튼 손뗐을때
		/// </summary>
		Input_clickFailureUp,

		/// <summary>
		/// 입력 관리코드에서 드래그X & 버튼 손뗐을때
		/// </summary>
		Input_clickSuccessUp,

		/// <summary>
		/// 드래그 입력
		/// </summary>
		Input_drag,

		/// <summary>
		/// 포커스 입력
		/// </summary>
		Input_focus,

		/// <summary>
		/// 키 입력
		/// </summary>
		Input_key,

		/// <summary>
		/// UI에서 발생함
		/// </summary>
		UI_Invoke,

		/// <summary>
		/// 객체 선택 API 이벤트
		/// </summary>
		API_SelectObject,

		/// <summary>
		/// 점검정보 선택 API 이벤트
		/// </summary>
		API_SelectIssue,

	}

	public enum ManagerActionIndex
	{
		NotDef = -1,
		InputAction = 0,
	}

	/// <summary>
	/// 이벤트 상태코드 결과
	/// </summary>
	public enum Status
	{
		/// <summary>
		/// 이벤트 처리 이전 단계
		/// </summary>
		Ready = 0,

		/// <summary>
		/// 선택해제 진행
		/// </summary>
		Drop = 0x10,

		/// <summary>
		/// 업데이트, 선택해제 진행
		/// </summary>
		Pass = 0x11,

		/// <summary>
		/// 업데이트만 진행
		/// </summary>
		Update = 0x12,

		/// <summary>
		/// 다음 단계를 진행하지 않는 단계
		/// </summary>
		Skip = 0x13,

		/// <summary>
		/// 이벤트 진행에 오류가 발생한 상태
		/// </summary>
		Error = 0x90,
	}

	/// <summary>
	/// UI에서 이벤트에 반응할때의 분기를 결정
	/// </summary>
	public enum UIEventType
	{
		Null = -1,

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
		Viewport_ViewMode_SIDE_BACK =  0x26,
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

		Ad_St3_Toggle = 0x216,
		Ad_St4_Toggle = 0x217,
		Ad_St5_Toggle = 0x218,
	}

	public enum UniqueUIEventType
	{
		NotDef = 0x00,

		SetChild_Highlight
	}

	/// <summary>
	/// 객체의 토글 방식
	/// </summary>
	public enum ToggleType
	{
		ALL = 0x00,
		Hide = 0x01,
		//Hide_Off = 0x02,

		Isolate = 0x11,
		//Isolate_Off = 0x12,
	}

	public enum ColorType
	{
		Default1 = 0x10,
		Selected1 = 0x20,
		
		UI_Default = 0x30,
		UI_Highlight = 0x31,

		UI_Ad_Default = 0x200,
		UI_Ad_Highlight = 0x201,
	}
	
	// Definition types

	public enum MaterialType
	{
		Default = 0x01,

		Default1 = 0x10,

		Issue = 0x20,
		Issue_dmg = 0x21,
		Issue_rcv = 0x22,

		White = 0x91,
		ObjDefault1 = 0x99,
	}

	public enum TextureType
	{
		crack = 0x01,
		baegtae = 0x02,
		bagli = 0x03,
		damage = 0x04,
		segul = 0x05,
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
