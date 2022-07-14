using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Piglet;

	public static class Textures
	{
		/// <summary>
		/// 대상 렌더러에 특정 텍스처를 덧입힘
		/// </summary>
		/// <param name="_render">대상 렌더러</param>
		/// <param name="_tType">텍스처 타입</param>
		public static void Set(MeshRenderer _render, TextureType _tType)
		{
			string texKey = GetTexKey();

			_render.material.SetTexture(texKey, Set(_tType));
		}

		/// <summary>
		/// 대상 Material에 특정 텍스처를 덧입힘
		/// </summary>
		/// <param name="_mat">대상 Material</param>
		/// <param name="_tType">텍스처 타입</param>
		public static void Set(Material _mat, TextureType _tType)
        {
			string texKey = GetTexKey();

			_mat.SetTexture(texKey, Set(_tType));
		}

		/// <summary>
		/// 현재 렌더링 파이프라인에 따라 Material에 할당해야 할 텍스처 키값을 반환한다.
		/// </summary>
		/// <returns>렌더링 파이프라인에 맞춘 텍스처 키</returns>
		private static string GetTexKey()
        {
			string texKey = "";

			RenderPipelineType pipeline = RenderPipelineUtil.GetRenderPipeline();
			if (pipeline == RenderPipelineType.URP)
			{
				texKey = "_BaseMap";
			}
			else if (pipeline == RenderPipelineType.BuiltIn)
			{
				texKey = "_MainTex";
			}
			else
			{
				texKey = "_BaseMap";
			}

			return texKey;
        }

		/// <summary>
		/// 손상 정보의 텍스처 리소스를 가져온다.
		/// </summary>
		/// <param name="_type">텍스처 타입</param>
		/// <returns>필요 텍스처 반환</returns>
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
