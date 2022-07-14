using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
	using Piglet;

	public static class Textures
	{
		/// <summary>
		/// ��� �������� Ư�� �ؽ�ó�� ������
		/// </summary>
		/// <param name="_render">��� ������</param>
		/// <param name="_tType">�ؽ�ó Ÿ��</param>
		public static void Set(MeshRenderer _render, TextureType _tType)
		{
			string texKey = GetTexKey();

			_render.material.SetTexture(texKey, Set(_tType));
		}

		/// <summary>
		/// ��� Material�� Ư�� �ؽ�ó�� ������
		/// </summary>
		/// <param name="_mat">��� Material</param>
		/// <param name="_tType">�ؽ�ó Ÿ��</param>
		public static void Set(Material _mat, TextureType _tType)
        {
			string texKey = GetTexKey();

			_mat.SetTexture(texKey, Set(_tType));
		}

		/// <summary>
		/// ���� ������ ���������ο� ���� Material�� �Ҵ��ؾ� �� �ؽ�ó Ű���� ��ȯ�Ѵ�.
		/// </summary>
		/// <returns>������ ���������ο� ���� �ؽ�ó Ű</returns>
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
		/// �ջ� ������ �ؽ�ó ���ҽ��� �����´�.
		/// </summary>
		/// <param name="_type">�ؽ�ó Ÿ��</param>
		/// <returns>�ʿ� �ؽ�ó ��ȯ</returns>
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
