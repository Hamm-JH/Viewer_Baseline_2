using AdminViewer.UI;
using Definition;
using Management;
using Platform.Tunnel;
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

		public void Init()
		{
			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(pCode))
			{
				List<int> lst = new List<int>
				{ 
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0 };

				ContentManager.Instance._Model.DmgData.ForEach(x =>
				{
					int index = (int)Tunnels.GetPartCode(x.CdBridgeParts);
					lst[index]++;
				});

				ContentManager.Instance._Model.RcvData.ForEach(x =>
				{
					int index = (int)Tunnels.GetPartCode(x.CdBridgeParts) + 17;
					lst[index]++;
				});

				// 다 끄기
				State1_pins.ForEach(x => x.SetActive(false));

				// 모델 경계값
				Bounds _b = ContentManager.Instance._Model.CenterBounds;
				Camera _cam = MainManager.Instance.MainCamera;

				// 리스트 변환
				int _index = lst.Count;
				for (int i = 0; i < _index; i++)
				{
					// x 17 나머지
					int xFactor = i % 17;
					// 17 이상이면 1, 밑이면 -1
					int zFactor = i > 16 ? 1 : -1;

					Vector3 center = _b.center;
					Vector3 pos = new Vector3(
						center.x - 48 + xFactor * 5,
						center.y,
						center.z + zFactor * 5);

					State1_pins[i].transform.position = _cam.WorldToScreenPoint(pos);

					if (lst[i] != 0)
					{
						State1_pins[i].SetActive(true);
					}
				}



			}

		}
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