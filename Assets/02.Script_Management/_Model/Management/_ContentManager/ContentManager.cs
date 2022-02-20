using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Module.Interaction;
	using Module.Model;
	using Module.UI;
	using Module.WebAPI;
	using UnityEngine.UI;

	public partial class ContentManager : IManager<ContentManager>
	{
		public Module_Model model;
		public Module_WebAPI webAPI;
		public Module_Interaction interaction;

		//private List<LayerCode> m_layerCodes;
		//public List<LayerCode> LayerCodes { get => m_layerCodes; set => m_layerCodes=value; }

		[SerializeField] List<AUI> m_uiList;

		public Canvas _Canvas
		{
			get => interaction.rootCanvas;
		}

		public GraphicRaycaster _GrRaycaster
		{
			get => interaction.grRaycaster;
		}

	}
}
