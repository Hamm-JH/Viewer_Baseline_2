using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Module;
	using Module.Interaction;
	using Module.Model;
	using Module.UI;
	using Module.WebAPI;
	using UnityEngine.UI;

	public partial class ContentManager : IManager<ContentManager>
	{
		//public Module_Model model;
		//public Module_WebAPI webAPI;
		//public Module_Interaction interaction;

		//private List<LayerCode> m_layerCodes;
		//public List<LayerCode> LayerCodes { get => m_layerCodes; set => m_layerCodes=value; }

		[SerializeField] List<AUI> m_uiList;

		/// <summary>
		/// 모듈 리스트
		/// </summary>
		[SerializeField] List<AModule> m_modules;

		public List<AModule> Modules { get => m_modules; set => m_modules=value; }

		/// <summary>
		/// 모델 요소의 중심축 값 반환
		/// </summary>
		public Bounds _CenterBounds
		{
			get
			{
				Module_Model mod = (Module_Model)Modules.Find(x => x.ID == ModuleID.Model);
				return mod.CenterBounds;
			}
		}

		public Canvas _Canvas
		{
			get
			{
				Module_Interaction mod = (Module_Interaction)Modules.Find(x => x.ID == ModuleID.Interaction);
				return mod.RootCanvas;
			}
		}

		public GraphicRaycaster _GrRaycaster
		{
			get
			{
				Module_Interaction mod = (Module_Interaction)Modules.Find(x => x.ID == ModuleID.Interaction);
				return mod.GrRaycaster;
			}
		}

	}
}
