using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Module.Interaction;
	using Module.Model;
	using Module.WebAPI;

	public partial class ContentManager : IManager<ContentManager>
	{
		public Module_Model model;
		public Module_WebAPI webAPI;
		public Module_Interaction interaction;

		// Start is called before the first frame update
		void Start()
		{
			OnCreate();
		}
	}
}
