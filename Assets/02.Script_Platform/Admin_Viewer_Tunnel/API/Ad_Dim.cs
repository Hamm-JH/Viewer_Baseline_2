using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer.API
{

	[System.Serializable]
	public class Ad_Dim
	{
		public Camera fr_camera;
		public Camera ba_camera;
		public Camera to_camera;
		public Camera bo_camera;
		public Camera le_camera;
		public Camera re_camera;

		public RenderTexture tex_fr;
		public RenderTexture tex_ba;
		public RenderTexture tex_to;
		public RenderTexture tex_bo;
		public RenderTexture tex_le;
		public RenderTexture tex_re;
	}
}
