using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bearroll.UltimateDecals {

	public class UD_Setup: ScriptableObject {

		public TextAsset urpShaderSource;
		public List<Material> materials;

		#if UNITY_EDITOR

		void OnEnable() {

			#if UD_URP
			FixShader();
			#endif

			FixMaterials();

		}

		bool FixShader() {

			if (urpShaderSource == null) return false;

			if (Shader.Find("Hidden/UltimateDecals/ScreenSpaceShadows")) return false;

			var path = Application.dataPath + "/Bearroll/UltimateDecals/Resources/Shaders/";

			Directory.CreateDirectory(path);

			var filename = path + "UD_ScreenSpaceShadows.shader";

			File.WriteAllText(filename, urpShaderSource.text);

			Debug.Log("UD_Setup: created " + filename);

			AssetDatabase.ImportAsset("Assets/Bearroll/UltimateDecals/Resources/Shaders/UD_ScreenSpaceShadows.shader");

			UD_Manager.Restart(); 

			return true;

		}

		bool FixMaterials() {

			var shader = Shader.Find("Universal Render Pipeline/Lit");

			if (shader == null) {
				shader = Shader.Find("Lightweight Render Pipeline/Lit");
			}

			if (shader == null) {
				shader = Shader.Find("Standard");
			}

			if (shader == null) {
				Debug.Log("UD_Setup: Can't find any shader to convert materials.");
				return false;
			}

			var r = false;

			foreach (var material in materials) {

				if (material == null) continue;

				if (material.shader == shader) continue;

				material.shader = shader;

				if (material.HasProperty("_BaseMap")) {
					material.SetTexture("_BaseMap", material.GetTexture("_MainTex"));
				}

				Debug.Log(string.Format("UD_Setup: converting material {0} to {1}", material.name, shader.name));

				r = true;

			}

			return r;

		}

		#endif

	}

}