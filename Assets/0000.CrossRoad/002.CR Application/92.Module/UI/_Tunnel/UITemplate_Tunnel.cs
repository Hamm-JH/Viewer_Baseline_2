using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Data.API;
    using Definition;
    using Management;
    using TMPro;
    using UnityEngine.UI;
	using View;

	public partial class UITemplate_Tunnel : AUI
	{
		[System.Serializable]
		public class Panel_Hover
        {
			public GameObject m_hoverPanel;
			public TextMeshProUGUI m_title;
			public TextMeshProUGUI m_hoverText1;
			public TextMeshProUGUI m_hoverText2;
			public TextMeshProUGUI m_hoverText3;
			public TextMeshProUGUI m_hoverText4;
			public TextMeshProUGUI m_hoverText5;
		}

		[System.Serializable]
		public class Sliders
        {
			public Slider m_transparencySlider;
			public Slider m_scaleSlider;

			public void ResetSlider_transparency()
            {
				if (m_transparencySlider == null) return;

				m_transparencySlider.value = 1;
            }

			public void ResetSlider_scale()
			{
				if (m_scaleSlider == null) return;

				m_scaleSlider.value = 1;
			}
		}

		public GameObject m_buttonBar;

		[SerializeField] Text m_segment;
		[SerializeField] Text m_line;
		[SerializeField] Text m_description;
		[SerializeField] List<GameObject> childElements_1;

		[SerializeField] Panel_Hover m_panelHover;
		[SerializeField] Sliders m_sliders;

		//[SerializeField] GameObject m_hoverPanel;
		//[SerializeField] TextMeshProUGUI m_hoverText;

		public override void OnStart()
		{
			Debug.LogWarning("UITemplate OnStart");
		}

		public override void OnModuleComplete()
		{
			Debug.LogError("load complete");
		}

		public override void ReInvokeEvent()
		{
			
		}

        public override void GetUIEvent<T>(T _type, Interactable _setter)
        {
            if(typeof(T) == typeof(UIEventType))
            {
				Debug.Log(typeof(UIEventType).Name);
            }
			else if(typeof(T) == typeof(Compass_EventType))
            {
				Compass_EventType type = (Compass_EventType)(object)_type;
				GetUIEvent(type, _setter);
            }
			else
            {
				throw new System.Exception("EventType not defined");
            }
        }

        public override void GetUIEvent(float _value, UIEventType _uType, Interactable _setter)
		{
			switch (_uType)
			{
				case UIEventType.Slider_Model_Transparency:
					Event_Model_Transparency(_value);
					break;

				case UIEventType.Slider_Icon_Scale:
					Event_Icon_Scale(_value);
					break;
			}
		}

		public override void GetUIEvent(UIEventType _uType, Interactable _setter)
		{
			PlatformCode pCode = MainManager.Instance.Platform;

			switch (_uType)
			{
				case UIEventType.View_Home:
					// 초기 화면으로 복귀
					Event_View_Home();
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					if(Platforms.IsDemoWebViewer(pCode))
                    {
						Event_Mode_ShowAll();
					}
					else if(Platforms.IsSmartInspectPlatform(pCode))
                    {
						Debug.Log("11111");
                    }
					else
                    {
						throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
                    }
					break;

				case UIEventType.Toggle:
				case UIEventType.Viewport_ViewMode:
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Toggle_ChildPanel1:
					// ChildPanel 1번 토글
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.Viewport_ViewMode_ISO:
				case UIEventType.Viewport_ViewMode_TOP:
				case UIEventType.Viewport_ViewMode_SIDE:
				case UIEventType.Viewport_ViewMode_BOTTOM:
					Event_Toggle_ViewMode(_uType);
					break;

				case UIEventType.OrthoView_Orthogonal:
					Event_ToggleOrthoView(true);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				case UIEventType.OrthoView_Perspective:
					Event_ToggleOrthoView(false);
					Event_Toggle_ViewMode(_setter.ChildPanel);
					break;

				// 객체 모두 다시 켜기
				case UIEventType.Mode_ShowAll:
					Event_Mode_ShowAll();
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					break;

				case UIEventType.Mode_Hide:
				case UIEventType.Mode_Isolate:
					Event_Mode_HideIsolate(_uType);
					Event_Toggle_ChildPanel(1, _setter.ChildPanel);
					break;

				case UIEventType.Test_Surface:
					Event_Legacy_ChangeCameraDirection( ((UI_Selectable)_setter).t_surfaceCode);
					break;
			}
		}

        public override void GetUIEvent(Inspect_EventType _uType, Interactable _setter)
        {
            switch(_uType)
            {
				case Inspect_EventType.BtnBar_11_ZoomIn:
					ButtonBar_ZoomIn();
					break;

				case Inspect_EventType.BtnBar_12_ZoomOut:
					ButtonBar_ZoomOut();
					break;
			}
        }

        public void GetUIEvent(Compass_EventType _type, Interactable _setter)
        {
			switch(_type)
            {
				case Compass_EventType.Compass_FirstPosition:
				case Compass_EventType.Compass_LastPosition:
					Compass_SetPosition(_type, _setter);
					break;
            }
        }

        public override void API_GetAddress(AAPI _data)
        {
            throw new System.NotImplementedException();
        }

        public override void SetObjectData_Tunnel(GameObject selected)
		{
			if (selected == null) return;

			//Debug.LogError($"selected name : {selected.name}");
			string seg = "";
			string line = "";

			SetTunnelData(selected.name, out seg, out line);

			m_segment.text = seg;
			m_line.text = line;
		}

		public override void TogglePanelList(int _index, GameObject _exclusive)
		{
			if(_index == 1)
			{
				childElements_1.ForEach(x => x.SetActive( (x != _exclusive) ? false : x.activeSelf));
			}
		}

		private void SetTunnelData(string name, out string _seg, out string _line)
		{
			_seg = "";
			_line = "";

			string[] spl = name.Split(',');

			_seg = spl[0];
			_line = spl[1];
		}
	}
}
