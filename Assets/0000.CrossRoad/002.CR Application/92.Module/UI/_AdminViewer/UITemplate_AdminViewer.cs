using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using AdminViewer;
	using Data.API;
	using Definition;
	using Management;
	using Management.Events;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using View;

	public partial class UITemplate_AdminViewer : AUI
	{
		[SerializeField] private ICON_Template icons;

		/// <summary>
		/// 타이틀 데이터
		/// </summary>
		[SerializeField] private TitleData titData;

		/// <summary>
		/// 네비 데이터
		/// </summary>
		[SerializeField] private NavigationData navData;

		/// <summary>
		/// 바텀 바 데이터
		/// </summary>
		[SerializeField] private BottomBarData botBarData;

		/// <summary>
		/// 바텀 바 텍스트 데이터
		/// </summary>
		[SerializeField] private BotData botData;

		/// <summary>
		/// 아이템들
		/// </summary>
		[SerializeField] private ItemsData items;

		/// <summary>
		/// 치수선 출력 결과
		/// </summary>
		[SerializeField] private DimData dimData;

		/// <summary>
		/// 패널 정보들
		/// </summary>
		[SerializeField] private InPanels m_panels;

		public ICON_Template Icons { get => icons; set => icons=value; }
		public TitleData TitData { get => titData; set => titData=value; }
		public NavigationData NavData { get => navData; set => navData=value; }
		public BottomBarData BotBarData { get => botBarData; set => botBarData=value; }
		public BotData BotData { get => botData; set => botData=value; }
		public ItemsData Items { get => items; set => items=value; }
		public InPanels Panels { get => m_panels; set => m_panels=value; }

		[Header("Keymap")]
		[SerializeField] RectTransform m_keymapPanel;

		public override void OnStart()
		{
			Debug.LogWarning("********** UITemplate OnStart");

			InitializeUI();

			ContentManager.Instance._API.RequestAddressData(GetAddressData);

			PlatformCode pCode = MainManager.Instance.Platform;
			if(Platforms.IsBridgePlatform(pCode))
			{
				TitData.oImage.sprite = Icons.m_bridge_icon;
			}
			else if(Platforms.IsTunnelPlatform(pCode))
			{
				TitData.oImage.sprite = Icons.m_tunnel_icon;
			}

			ContentManager.Instance.Ad_InitUI(m_keymapPanel);
			//if (MainManager.Instance.Platform)

			//Items.Init();
			//ContentManager.Instance.CreateNewModule(ModuleCode.Issue_Administration);

			// TODO 야매 CompCheck 3
			//ContentManager.Instance.CompCheck(3);
		}

		/// <summary>
		/// 사전 준비단계가 끝났을때 실행
		/// </summary>
		public override void OnModuleComplete()
		{
			Debug.LogWarning("load complete");

			items.Init();

			// 모듈 인스턴스 상태코드 이벤트 관리자에 올리기
			// 거대한 하나의 점검정보 관리 모듈의 형태이기 때문임.
			ContentManager.Instance.CreateNewModule(ModuleCode.Issue_Administration);
			// 모듈상태 변경 view1
			ChangeModuleStatus(ModuleStatus.Administration_view1);
		}

		private void GetAddressData(AAPI _data)
		{
			// 다시 언박싱
			DAddress data = (DAddress)_data;

			//TitData.oName.text = data.nmTunnel;
			//BotData.adrName.text = data.nmAddress;
			SetTitleText(data.nmTunnel);
			SetAddressText(data.nmAddress);

			//string _fid = data.mp_fid;
			//string _ftype = data.mp_ftype;
			//string _fgroup = data.mp_filename;

			//string argument = string.Format("fid={0}&ftype={1}&fgroup={2}", _fid, _ftype, _fgroup);
			GetTexture(data.mp_fid, data.mp_ftype, data.mp_fgroup, GetMainPicture);
		}

		private void GetTexture(string _fid, string _ftype, string _fgroup, UnityAction<Texture2D> _callback)
		{
			string argument = string.Format("fid={0}&ftype={1}&fgroup={2}", _fid, _ftype, _fgroup);

			ContentManager.Instance._API.RequestMainPicture(argument, _callback);
		}

		private void GetMainPicture(Texture2D _image)
		{
			items.mainPicture.texture = _image;
		}

		public void SetObject_IfNotSet()
		{
			GameObject obj = ContentManager.Instance._SelectedObj;

			if (obj == null)
			{
				List<GameObject> _objs = ContentManager.Instance._ModelObjects;
				int index = Random.Range(0, _objs.Count);

				obj = _objs[index];

				//EventData_Input
				EventManager.Instance.OnEvent(new EventData_API(
					InputEventType.API_SelectObject,
					obj,
					MainManager.Instance.cameraExecuteEvents.selectEvent
				));

				SetPartText(obj.name);
			}
			else
			{
				SetPartText(obj.name);
			}
		}

		/// <summary>
		/// UI 인스턴스 초기화
		/// </summary>
		private void InitializeUI()
		{
			Ad_nav_state1_NAV();
			Ad_nav_state1_BOT();
			SetPanel_State1();
			SetPin(true);
		}

		

		#region ** not implemented
		public override void SetObjectData_Tunnel(GameObject selected)
		{
			throw new System.NotImplementedException();
		}

		public override void TogglePanelList(int _index, GameObject _exclusive)
		{
			Debug.LogWarning("TogglePanelList");
		}
		#endregion

		public override void ReInvokeEvent()
		{
			UIEventType uType = UIEventType.Null;
			switch(m_status)
			{
				case ModuleStatus.Administration_view1:
					uType = UIEventType.Ad_nav_state1;
					break;

				case ModuleStatus.Administration_view2:
					uType = UIEventType.Ad_nav_state2;
					break;

				case ModuleStatus.Administration_view3:
					uType = UIEventType.Ad_nav_state3;
					break;

				case ModuleStatus.Administration_view4:
					uType = UIEventType.Ad_nav_state4;
					break;

				case ModuleStatus.Administration_view5:
					uType = UIEventType.Ad_nav_state5;
					break;
			}

			GetUIEvent(uType, null);
		}

		public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
		{
			throw new System.NotImplementedException();
		}

		public override void GetUIEvent(UIEventType _uType, Interactable _setter)
		{
			//Debug.Log($"Type : {_uType.ToString()}");
			switch(_uType)
			{
				case UIEventType.Ad_nav_state1:
					Ad_nav_state1_NAV();		// 상태바 표시변경
					Ad_nav_state1_BOT();		// 하단바 표시변경
					SetPanel_State1();			// 패널 표시변경
					SetPin(true);				// 1번상태 핀 표시여부
					ChangeModuleStatus(ModuleStatus.Administration_view1);
					ToggleDimension(false);
					break;
				case UIEventType.Ad_nav_state2:
					Ad_nav_state2_NAV();
					Ad_nav_state2_BOT();
					SetPanel_State2();
					SetPin(false);
					ChangeModuleStatus(ModuleStatus.Administration_view2);
					ToggleDimension(false);
					break;
				case UIEventType.Ad_nav_state3:
					Ad_nav_state3_NAV();
					Ad_nav_state3_BOT();
					SetPanel_State3();
					SetPin(false);
					ChangeModuleStatus(ModuleStatus.Administration_view3);
					ToggleDimension(false);
					break;
				case UIEventType.Ad_nav_state4:
					Ad_nav_state4_NAV();
					Ad_nav_state4_BOT();
					SetPanel_State4();
					SetPin(false);
					ChangeModuleStatus(ModuleStatus.Administration_view4);
					ToggleDimension(false);
					break;
				case UIEventType.Ad_nav_state5:
					Ad_nav_state5_NAV();
					Ad_nav_state5_BOT();
					SetPanel_State5();
					SetPin(false);
					ChangeModuleStatus(ModuleStatus.Administration_view5);
					ToggleDimension(false);
					break;

				case UIEventType.Toggle:
					_setter.ChildPanel.SetActive(!_setter.ChildPanel.activeSelf);
					break;

				case UIEventType.Ad_St3_Toggle:
					SetSt3_Toggle();
					break;

				case UIEventType.Ad_St4_Toggle:
					SetSt4_Toggle();
					break;

				case UIEventType.Ad_St5_Toggle:
					SetSt5_Toggle();
					break;

				case UIEventType.Ad_St5_Toggle_m1_dmg:
					SetSt5_m1_dmg_toggle();
					break;

				case UIEventType.Ad_St5_Toggle_m1_rcv:
					SetSt5_m1_rcv_toggle();
					break;

				case UIEventType.Ad_St5_Toggle_m1_rein:
					SetSt5_m1_rein_toggle();
					break;

				case UIEventType.Ad_St5_PrintExcel:
					ContentManager.Instance.Functin_PrintExcel();
					break;

				case UIEventType.Ad_Prev:

					break;

				case UIEventType.Ad_Next:

					break;

				case UIEventType.Ad_SetKeymapCenter:
					SetKeymapCenterPosition();
					break;

				case UIEventType.Ad_SetImageWindow:
					OpenImagePanel(_setter);
					break;

				case UIEventType.Ad_St_DimToggle:
					ToggleDimension();
					break;

				case UIEventType.Ad_St_PrintDim:
					PrintDimension();
					break;
			}
		}

		public void SetKeymapCenterPosition()
		{
			ContentManager.Instance.Function_SetCameraCenterPosition();
		}

		private void OpenImagePanel(Interactable _setter)
		{
			// 이미지 할당자를 
			UI_TableElement element = (UI_TableElement)_setter;

			// 점검정보를 가져온다.
			Definition._Issue.Issue issue = element.tRootElement.m_issue;

			// 이미지 패널 켜기
			Panels.image.SetActive(true);

			// 이미지 패널 실행
			Panels.image_code.OpenImagePanel(issue);
		}

        public override void API_GetAddress(AAPI _data)
        {
            throw new System.NotImplementedException();
        }
    }
}
