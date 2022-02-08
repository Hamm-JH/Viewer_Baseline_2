using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	public partial class ContentManager : IManager<ContentManager>
	{
		public void SetCameraCenterPosition()
		{
			Bounds _b = model.CenterBounds;
			Canvas _canvas = interaction.rootCanvas;

			MainManager.Instance.SetCameraPosition(_b, _canvas);
		}
	}
}
