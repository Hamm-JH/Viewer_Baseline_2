using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Piglet;

	public static class Materials
	{
		static Material MAT_DEFAULT;
		static Material MAT_TRANSPARENT;
		
		public static Material Set(MaterialType type)
		{
			Material result = Resources.Load<Material>("3D/DefaultMat");

			RenderPipelineType pipeline = RenderPipelineUtil.GetRenderPipeline();

			switch(type)
			{
				case MaterialType.Default:
					if (pipeline == RenderPipelineType.BuiltIn)
					{
						MAT_DEFAULT = Resources.Load<Material>("3D/BltIn_Default");
						MAT_TRANSPARENT = Resources.Load<Material>("3D/BltIn_Transparent");
						
						result = MAT_DEFAULT;
					}
					else if (pipeline == RenderPipelineType.URP)
					{
						MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default");
						MAT_TRANSPARENT = Resources.Load<Material>("3D/URP_Transparent");
						//MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default2");
						//MAT_TRANSPARENT = Resources.Load<Material>("3D/URP_Transparent2");
						result = MAT_DEFAULT;
					}
					break;

				case MaterialType.Default1:
					result = Resources.Load<Material>("3D/DefaultMat");
					break;

				case MaterialType.ObjDefault1:
					result = Resources.Load<Material>("3D/ObjMat");
					break;
			}

			return result;
		}

		static public void Set(MeshRenderer _render, ColorType _colorType, float _trans)
		{
			Material mat = _render.material;
			Color colr = mat.color;
			string colrKey = "";

			RenderPipelineType pipeline = RenderPipelineUtil.GetRenderPipeline();
			if (pipeline == RenderPipelineType.URP)
			{
				colrKey = "_BaseColor";
			}
			else if (pipeline == RenderPipelineType.BuiltIn)
			{
				colrKey = "_Color";
			}
			else
			{
				colrKey = "_BaseColor";
			}

			_render.material.SetColor(colrKey, Colors.Set(_colorType, _trans));
		}

		public static void ToOpaqueMode(MeshRenderer render)
		{
			Material before = render.material;
			Color bColor = before.color;

			Material now = new Material(MAT_DEFAULT);
			now.color = bColor;

			// 기존 값 넘기기
			//material.SetOverrideTag("RenderType", "");
			//material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
			//material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
			//material.SetInt("_ZWrite", 1);
			//material.DisableKeyword("_ALPHATEST_ON");
			//material.DisableKeyword("_ALPHABLEND_ON");
			//material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			//material.renderQueue = -1;

			render.material = now;
		}

		public static void ToFadeMode(MeshRenderer render)
		{
			Material before = render.material;
			Color bColor = before.color;

			Material now = new Material(MAT_TRANSPARENT);
			now.color = bColor;

			// 기존 값 넘기기
			//material.SetOverrideTag("RenderType", "Transparent");
			//material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			//material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			//material.SetInt("_ZWrite", 0);
			//material.DisableKeyword("_ALPHATEST_ON");
			//material.EnableKeyword("_ALPHABLEND_ON");
			//material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			//material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

			render.material = now;
		}
	}
}
