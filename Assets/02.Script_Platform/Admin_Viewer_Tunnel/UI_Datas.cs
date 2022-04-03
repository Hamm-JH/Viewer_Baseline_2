using System.Collections;
using System.Collections.Generic;

namespace AdminViewer
{
	using AdminViewer.UI;
	using Definition;
	using Management;
	using Platform.Tunnel;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;
	using Items.UIPin;

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
		public List<UIPin> pins;
		public List<GameObject> State1_pins;

		public void Init()
		{
			GameObject root = new GameObject("pin root");
			Controller_UIPin rootPin = root.AddComponent<Controller_UIPin>();

			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsTunnelPlatform(pCode))
			{
				#region 객체 34개 준비

				List<GameObject> objList = new List<GameObject>();
				for (int i = 0; i < 34; i++)
				{
					GameObject _obj = new GameObject($"pin pos {i+1}");
					_obj.transform.SetParent(root.transform);
					objList.Add(_obj);
				}
				rootPin.m_objectList = objList;

				#endregion

				#region 손상, 보수정보 구하기
				// 34개 세팅
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

				#endregion

				// 다 끄기
				pins.ForEach(x => x.gameObject.SetActive(false));

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

					// 표시객체 위치할당
					objList[i].transform.position = pos;
					//State1_pins[i].transform.position = _cam.WorldToScreenPoint(pos);

					// 표시객체 pin에 할당
					pins[i].m_targetObject = objList[i].transform;

					if (lst[i] != 0)
					{
						pins[i].gameObject.SetActive(true);
					}
				}
			}
			else if(Platforms.IsBridgePlatform(pCode))
            {
				pins.ForEach(x => x.gameObject.SetActive(false));
            }

		}
	}

	[System.Serializable]
	public class DimData
	{
		public string	fr_result1;
		public string	fr_result2;
		public bool		fr_complete;

		public string	ba_result1;
		public string	ba_result2;
		public bool		ba_complete;

		public string	to_result1;
		public string	to_result2;
		public bool		to_complete;

		public string	bo_result1;
		public string	bo_result2;
		public bool		bo_complete;

		public string	le_result1;
		public string	le_result2;
		public bool		le_complete;

		public string	re_result1;
		public string	re_result2;
		public bool		re_complete;
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
		public GameObject image			;

		[Header("Panel Codes")]
		public Ad_Panel bpm1_code;
		public Ad_Panel bpm2_code;
		public Ad_Panel bpmm_code;
		public Ad_Panel s5b1_code;
		public Ad_Panel s5b2_code;
		public Ad_Panel s5m1_code;
		public Ad_Panel image_code;
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


		public RectTransform m_s5b1_image_1crack_dmgRect;
		public RectTransform m_s5b1_image_1crack_rcvRect;

		public RectTransform m_s5b1_image_2spalling_dmgRect;
		public RectTransform m_s5b1_image_2spalling_rcvRect;

		public RectTransform m_s5b1_image_3Efflorence_dmgRect;
		public RectTransform m_s5b1_image_3Efflorence_rcvRect;

		public RectTransform m_s5b1_image_4Breakage_dmgRect;
		public RectTransform m_s5b1_image_4Breakage_rcvRect;

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

	[System.Serializable]
	public class ImgPanel_element
	{
		public GameObject img_cRoot;
		
		public GameObject img_enlargePanel;
		public RawImage img_enlargeImage;

		public List<Ad_Panel> imgContents;
	}

	#endregion
}