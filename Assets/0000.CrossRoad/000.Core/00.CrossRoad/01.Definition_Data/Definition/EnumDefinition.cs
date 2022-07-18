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

		WebGL_Template1 = 0x10,				// 웹 템플릿(기준코드)
		WebGL_Template2 = 0x11,             // 웹 템플릿(기준코드)
		WebGL_AdminViewer_Tunnel = 0x12,	// 명지대 터널 관리자뷰어
		WebGL_AdminViewer_Bridge = 0x13,	// 명지대 교량 관리자뷰어

		Mobile_Template1 = 0x20,			// 모바일 템플릿(기준코드)
		Mobile_Template2 = 0x21,            // 모바일 템플릿(기준코드)

		PC_Maker1 = 0x30,					// 메이커 템플릿(기준코드)
		PC_Viewer_Tunnel = 0x31,            // 명지대 터널 메이커(미작업)
		PC_Viewer_Bridge = 0x32,			// 명지대 교량 메이커(미작업)

		WebGL_SmartInspect_Tunnel = 0x101,	// 스마트 인스펙트 터널 뷰어
		WebGL_SmartInspect_Bridge = 0x102,	// 스마트 인스펙트 교량 뷰어
	}

	/// <summary>
	/// 그래픽 코드
	/// </summary>
	public enum GraphicCode
	{
		NotDef = -1,
		Null = 0,

		/// <summary>
		/// 지정된 단일 색상으로 처리
		/// </summary>
		Single_Color,

		/// <summary>
		/// 플랫폼별 텍스처링
		/// </summary>
		Platform_Texturing,

	}

	/// <summary>
	/// 씬 이름을 관리하는 코드
	/// </summary>
	public enum SceneName
	{
		NotDef = -1,
		Maker = 0x00,
		Viewer = 0x01,
		AdminViewer = 0x02,

		SmartInspectViewer = 0x03,

		Mapbox_Demo,
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
		/// 호버링 체크
		/// </summary>
		Input_hover,

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

	/// <summary>
	/// 
	/// </summary>
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
	/// 객체의 토글 방식
	/// </summary>
	public enum ToggleType
	{
		ALL = 0x00,
		Hide = 0x01,		// 지정된 대상 숨김

		Isolate = 0x11,		// 지정된 대상 이외 숨김
	}

	/// <summary>
	/// 색상 타입
	/// </summary>
	public enum ColorType
	{
		White = 0x01,

		Default1 = 0x10,
		Selected1 = 0x20,
		
		UI_Default = 0x30,
		UI_Highlight = 0x31,

		UI_Ad_Img_Default = 0x200,
		UI_Ad_Img_Highlight = 0x201,

		UI_Ad_Txt_Default = 0x202,
		UI_Ad_Txt_Highlight = 0x203,

		UI_dmg = 0x300,
		UI_rcv = 0x301,
		UI_rein = 0x302,
	}
	
	/// <summary>
	/// Material 타입
	/// </summary>
	public enum MaterialType
	{
		Default = 0x01,

		Default1 = 0x10,

		Issue = 0x20,
		Issue_dmg = 0x21,
		Issue_rcv = 0x22,

		UltimateDecal = 0x30,
		UltimateDecal_dmg = 0x31,
		UltimateDecal_rcv = 0x32,

		EasyDecal = 0x40,
		EasyDecal_dmg = 0x41,
		EasyDecal_rcv = 0x42,

		White = 0x91,
		ObjDefault1 = 0x99,
	}

	/// <summary>
	/// 손상정보에 붙일 텍스처 타입 (기존 분류에서 가져옴)
	/// </summary>
	public enum TextureType
	{
		crack = 0x01,
		baegtae = 0x02,
		bagli = 0x03,
		damage = 0x04,
		segul = 0x05,
	}

	/// <summary>
	/// 아이템 프리팹 분류
	/// </summary>
	public enum PrefabType
	{
		UltimateDecal = 0x10,
		EasyDecal = 0x11,
	}

	/// <summary>
	/// Unity 레이어 분류
	/// </summary>
	public enum LayerNames
	{
		Default = 0,
		TransparentFX = 1,

		Water = 4,
		UI = 5,
		DecalPoint = 6
	}

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
