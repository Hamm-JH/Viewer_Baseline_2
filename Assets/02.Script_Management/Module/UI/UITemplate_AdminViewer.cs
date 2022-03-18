﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using AdminViewer;
	using Data.API;
	using Definition;
	using Management;
	using UnityEngine.Events;
	using UnityEngine.UI;
	using View;

	public partial class UITemplate_AdminViewer : AUI
	{
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
		/// 패널 정보들
		/// </summary>
		[SerializeField] private InPanels m_panels;

		public TitleData TitData { get => titData; set => titData=value; }
		public NavigationData NavData { get => navData; set => navData=value; }
		public BottomBarData BotBarData { get => botBarData; set => botBarData=value; }
		public BotData BotData { get => botData; set => botData=value; }
		public ItemsData Items { get => items; set => items=value; }
		public InPanels Panels { get => m_panels; set => m_panels=value; }

		public override void OnStart()
		{
			Debug.LogWarning("********** UITemplate OnStart");

			InitializeUI();

			ContentManager.Instance._API.RequestAddressData(GetAddressData);
		}

		private void GetAddressData(AAPI _data)
		{
			// 다시 언박싱
			DAddress data = (DAddress)_data;

			TitData.oName.text = data.nmTunnel;
			BotData.adrName.text = data.nmAddress;

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
			Debug.Log("안녀ㅓㅓㅓㅓㅓㅓㅓㅓㅓㅓ엉");

			items.mainPicture.texture = _image;
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
			throw new System.NotImplementedException();
		}
		#endregion

		public override void GetUIEvent(UIEventType _uType, UI_Selectable _setter)
		{
			Debug.Log($"Type : {_uType.ToString()}");
			switch(_uType)
			{
				case UIEventType.Ad_nav_state1:
					Ad_nav_state1_NAV();
					Ad_nav_state1_BOT();
					SetPanel_State1();
					SetPin(true);
					break;
				case UIEventType.Ad_nav_state2:
					Ad_nav_state2_NAV();
					Ad_nav_state2_BOT();
					SetPanel_State2();
					SetPin(false);
					break;
				case UIEventType.Ad_nav_state3:
					Ad_nav_state3_NAV();
					Ad_nav_state3_BOT();
					SetPanel_State3();
					SetPin(false);
					break;
				case UIEventType.Ad_nav_state4:
					Ad_nav_state4_NAV();
					Ad_nav_state4_BOT();
					SetPanel_State4();
					SetPin(false);
					break;
				case UIEventType.Ad_nav_state5:
					Ad_nav_state5_NAV();
					Ad_nav_state5_BOT();
					SetPanel_State5();
					SetPin(false);
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

				case UIEventType.Ad_Prev:

					break;

				case UIEventType.Ad_Next:

					break;

					
			}
		}
	}
}
