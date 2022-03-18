using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdminViewer
{
	[System.Serializable]
	public class TitleData
	{
		public TextMeshProUGUI oName;
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
		public GameObject mainLocation	;
		public GameObject mainPicture	;
		public GameObject keymap		;
		public GameObject bpm1			;
		public GameObject bpm2			;
		public GameObject bpmm			;
		public GameObject s5b1			;
		public GameObject s5b2			;
		public GameObject s5m1			;
	}
}