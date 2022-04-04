using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	/// <summary>
	/// UI���� �̺�Ʈ�� �����Ҷ��� �б⸦ ����
	/// </summary>
	public enum UIEventType
	{
		Null = 0,

		/// <summary>
		/// UI ���
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
		/// ���� :: Ȩ 
		/// </summary>
		View_Home = 0x29,

		OrthoView = 0x30,
		OrthoView_Orthogonal = 0x31,
		OrthoView_Perspective = 0x32,

		Mode_ShowAll = 0x40,
		/// <summary>
		/// ��ü ���ý� Hide, Isolate
		/// Off�� ������ �ƿ� ��ü�� ����.
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

	}

	public enum ModuleID
	{
		NotDef = -1,
		/// <summary>
		/// Code :: ��
		/// </summary>
		Model = 0x10,

		/// <summary>
		/// Code :: ��ȣ�ۿ� ???
		/// </summary>
		Interaction = 0x20,

		/// <summary>
		/// Code :: �� ��ȣ�ۿ�
		/// </summary>
		WebAPI = 0x30,

		/// <summary>
		/// Code :: �׷���
		/// </summary>
		Graphic = 0x50,

		/// <summary>
		/// Code :: UI �Ӽ�
		/// </summary>
		Prop_UI = 0x50,

		/// <summary>
		/// Item :: ���������� Prefab ����
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
}
