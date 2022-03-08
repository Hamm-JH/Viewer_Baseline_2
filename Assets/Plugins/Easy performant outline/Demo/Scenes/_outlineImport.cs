using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	using EPOOutline;

	public class _outlineImport : MonoBehaviour
	{
		

		private void Start()
		{
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obj.name = "Outline";

			Outlinable outlinable = obj.AddComponent<Outlinable>();

			//outlineToUse.RenderStyle = RenderStyle.FrontBack;
			//outlinable.RenderStyle = RenderStyle.Single;
			//outlinable.DrawingMode = OutlinableDrawingMode.Normal;

			outlinable.AddAllChildRenderersToRenderingList();
		}
	}
}
