using AdminViewer.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdminViewer
{
	#region UI Template

	[System.Serializable]
	public class ICON_Template
	{
		public Sprite m_bridge_icon;
		public Sprite m_tunnel_icon;

		public Sprite m_icon_detail;
		public Sprite m_icon_dmg;
		public Sprite m_icon_rcv;
		public Sprite m_icon_rein;
	}

	[System.Serializable]
	public class TitleData
	{
		public Image oImage;
		public TextMeshProUGUI oName;
		public Image pImage;
		public TextMeshProUGUI pName;
	}

	[System.Serializable]
	public class NavigationData
	{
		#region 네비
		[Header("nav1")]
		public Image			nv1_bg		;
		public Image			nv1_ic		;
		public TextMeshProUGUI	nv1_tx		;
		public Image			nv2_bg		;
		public Image			nv2_ic		;
		public TextMeshProUGUI	nv2_tx		;
		public Image			nv3_bg		;
		public Image			nv3_ic		;
		public TextMeshProUGUI	nv3_tx		;
		public Image			nv4_bg		;
		public Image			nv4_ic		;
		public TextMeshProUGUI	nv4_tx		;
		public Image			nv5_bg		;
		public Image			nv5_ic		;
		public TextMeshProUGUI	nv5_tx		;
		public Image			nv_ar1		;
		public Image			nv_ar2		;
		public Image			nv_ar3		;
		public Image			nv_ar4		;
		#endregion
	}

	[System.Serializable]
	public class BottomBarData
	{
		public GameObject state1;
		public GameObject state2;
		public GameObject state3;
		public GameObject state4;
		public GameObject state5;
	}

	[System.Serializable]
	public class BotData
	{
		public TextMeshProUGUI adrName;
	}

	[System.Serializable]
	public class ItemsData
	{
		public RawImage mainPicture;

		public GameObject State1_rootPin;
		public List<GameObject> State1_pins;
	}

	[System.Serializable]
	public class InPanels
	{
		[Header("GameObjects")]
		public GameObject mainLocation	;
		public GameObject mainPicture	;
		public GameObject keymap		;
		public GameObject bpm1			;
		public GameObject bpm2			;
		public GameObject bpmm			;
		public GameObject s5b1			;
		public GameObject s5b2			;
		public GameObject s5m1			;

		[Header("Panel Codes")]
		public Ad_Panel bpm1_code;
		public Ad_Panel bpm2_code;
		public Ad_Panel bpmm_code;
		public Ad_Panel s5b1_code;
		public Ad_Panel s5b2_code;
		public Ad_Panel s5m1_code;
	}

	#endregion

	#region AdminViewer in Ad_Panel

	[System.Serializable]
	public class B2_element
	{
		public Ad_Panel_Detail m_detailPanel;
	}

	[System.Serializable]
	public class S3_B2_Panel_Detail
	{
		public TextMeshProUGUI m_user;

		public TextMeshProUGUI m_partName;
		public TextMeshProUGUI m_width;
		public TextMeshProUGUI m_vertical;
		public TextMeshProUGUI m_depth;

		public TextMeshProUGUI m_description;
	}

	[System.Serializable]
	public class S5b1_element
	{
		public TextMeshProUGUI m_s5b1_allDmg;
		public TextMeshProUGUI m_s5b1_allRcv;

		public TextMeshProUGUI m_s5b1_1crack_dmg;
		public TextMeshProUGUI m_s5b1_1crack_rcv;

		public TextMeshProUGUI m_s5b1_2spalling_dmg;
		public TextMeshProUGUI m_s5b1_2spalling_rcv;

		public TextMeshProUGUI m_s5b1_3Efflorence_dmg;
		public TextMeshProUGUI m_s5b1_3Efflorence_rcv;

		public TextMeshProUGUI m_s5b1_4Breakage_dmg;
		public TextMeshProUGUI m_s5b1_4Breakage_rcv;

		//---------------------------------------------------------------

		public Image m_s5b1_image_allDmg;
		public Image m_s5b1_image_allRcv;
			   
		public Image m_s5b1_image_1crack_dmg;
		public Image m_s5b1_image_1crack_rcv;
			   
		public Image m_s5b1_image_2spalling_dmg;
		public Image m_s5b1_image_2spalling_rcv;
			   
		public Image m_s5b1_image_3Efflorence_dmg;
		public Image m_s5b1_image_3Efflorence_rcv;
			   
		public Image m_s5b1_image_4Breakage_dmg;
		public Image m_s5b1_image_4Breakage_rcv;
	}

	[System.Serializable]
	public class S5b2_element
	{
		public bool isMain;

		public RectTransform rcvElement;
		public RectTransform dmgElement;

		public Dictionary<int, Transform> yearDictionary;

		public Transform YearGridPanel;
	}

	[System.Serializable]
	public class S5m1_element
	{
		public TextMeshProUGUI m_s5m1_count_dmg;
		public TextMeshProUGUI m_s5m1_count_rcv;
		public TextMeshProUGUI m_s5m1_count_rein;
	}

	#endregion
}