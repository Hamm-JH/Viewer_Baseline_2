using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Piglet;

	public static class Textures
	{
		public static void Set(MeshRenderer _render, TextureType _tType)
		{
			string texKey = "";

			RenderPipelineType pipeline = RenderPipelineUtil.GetRenderPipeline();
			if(pipeline == RenderPipelineType.URP)
			{
				texKey = "_BaseMap";
			}
			else if(pipeline == RenderPipelineType.BuiltIn)
			{
				texKey = "_MainTex";
			}
			else
			{
				texKey = "_BaseMap";
			}

			_render.material.SetTexture(texKey, Set(_tType));
		}

		public static Texture Set(TextureType _type)
		{
			Texture result = null;

			switch(_type)
			{
				case TextureType.crack:
					result = Resources.Load<Texture>("Textures/crack");
					break;

				case TextureType.baegtae:
					result = Resources.Load<Texture>("Textures/baegtae");
					break;

				case TextureType.bagli:
					result = Resources.Load<Texture>("Textures/bagli");
					break;

				case TextureType.damage:
					result = Resources.Load<Texture>("Textures/damage");
					break;

				case TextureType.segul:
					result = Resources.Load<Texture>("Textures/segul");
					break;

				default:
					Debug.LogError("Texture type not match");
					break;
			}

			return result;
		}
	}
}
