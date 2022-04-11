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

		static Material MAT_ISSUE;
		static Material MAT_DAMAGE;
		static Material MAT_RECOVER;
		
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
						//MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default2");  // 홀로그램
						MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default");		// 기본
						MAT_TRANSPARENT = Resources.Load<Material>("3D/URP_Transparent");


						//MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default2");
						//MAT_TRANSPARENT = Resources.Load<Material>("3D/URP_Transparent2");
						result = MAT_DEFAULT;
					}
					break;

				case MaterialType.Issue:
					{
						if(pipeline == RenderPipelineType.BuiltIn)
						{
							MAT_ISSUE = Resources.Load<Material>("3D/Issue/BltIn/Issue");
						}
						else if(pipeline == RenderPipelineType.URP)
						{
							MAT_ISSUE = Resources.Load<Material>("3D/Issue/URP/Issue");
						}
						result = MAT_ISSUE;
					}
					break;

				case MaterialType.Issue_dmg:
					{
						if (pipeline == RenderPipelineType.BuiltIn)
						{
							MAT_DAMAGE = Resources.Load<Material>("3D/Issue/BltIn/Issue_dmg");
						}
						else if (pipeline == RenderPipelineType.URP)
						{
							MAT_DAMAGE = Resources.Load<Material>("3D/Issue/URP/Issue_dmg");
						}
						result = MAT_DAMAGE;
					}
					break;

				case MaterialType.Issue_rcv:
					{
						if (pipeline == RenderPipelineType.BuiltIn)
						{
							MAT_RECOVER = Resources.Load<Material>("3D/Issue/BltIn/Issue_rcv");
						}
						else if (pipeline == RenderPipelineType.URP)
						{
							MAT_RECOVER = Resources.Load<Material>("3D/Issue/URP/Issue_rcv");
						}
						result = MAT_RECOVER;
					}
					break;



				case MaterialType.Default1:
					result = Resources.Load<Material>("3D/DefaultMat");
					break;

				case MaterialType.White:
					throw new System.Exception("White 구현");
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
				//colrKey = "_MainColor";
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

			render.material = now;
		}

		public static void ToFadeMode(MeshRenderer render)
		{
			Material before = render.material;
			Color bColor = before.color;

			Material now = new Material(MAT_TRANSPARENT);
			now.color = bColor;

			render.material = now;
		}
	}
}
