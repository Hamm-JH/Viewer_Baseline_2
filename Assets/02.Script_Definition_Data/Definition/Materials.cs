using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	public static class Materials
	{
		public static Material Set(MaterialType type)
		{
			Material result = Resources.Load<Material>("3D/DefaultMat");

			switch(type)
			{
				case MaterialType.Default1:
					result = Resources.Load<Material>("3D/DefaultMat");
					break;

				case MaterialType.ObjDefault1:
					result = Resources.Load<Material>("3D/ObjMat");
					break;
			}

			return result;
		}

		public static void ToOpaqueMode(Material material)
		{
			material.SetOverrideTag("RenderType", "");
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
		}

		public static void ToFadeMode(Material material)
		{
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
		}
	}
}
