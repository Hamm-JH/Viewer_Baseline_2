using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bearroll.UltimateDecals {

	public class Bearroll_ShaderEditor: ShaderGUI {

		int lastMainTexId;
		bool hasAlpha;

		Dictionary<string, GUIContent> guiContentCache = new Dictionary<string, GUIContent>();

		protected MaterialProperty[] properties { get; private set; }
		protected MaterialEditor materialEditor { get; private set; }
		static protected string[] GBufferName = new string[] { "Albedo", "Specular", "Normals", "Emission" };

		public sealed override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {

			this.properties = properties;
			this.materialEditor = materialEditor;

			if (OnGUI()) {
				base.OnGUI(materialEditor, properties);
			}

		}

		public virtual bool OnGUI() {
			return true;
		}

		protected MaterialProperty FindProperty(string name) {
			return FindProperty(name, properties);
		}

		protected void SetKeyword(string name, bool set) {

			if (materialEditor == null) return;

			var material = materialEditor.target as Material;

			if (material == null) return;

			if (material.IsKeywordEnabled(name) == set) return;

			if (set) {
				material.EnableKeyword(name);
			} else {
				material.DisableKeyword(name);
			}

		}

		protected void DrawAtlasInspector() {
			
			var material = materialEditor.target as Material;

			var emissionTex = FindProperty("_EmissionTex");
			var emissionColor = FindProperty("_EmissionColor");

			materialEditor.TexturePropertySingleLine(c("Emission (RGB)"), emissionTex, emissionColor);

			SetKeyword("_EMISSION", emissionTex.textureValue != null || emissionColor.colorValue.maxColorComponent > 0.001f);

			GUILayout.Space(10);
			GUILayout.Label("Atlas", EditorStyles.boldLabel);
			GUILayout.Space(5);

			EditorGUIUtility.labelWidth = 150;
			
			var atlasProp = material.GetVector("_AtlasData");

			EditorGUI.BeginChangeCheck();

			atlasProp.x = EditorGUILayout.IntField("Cols", (int) atlasProp.x);
			atlasProp.y = EditorGUILayout.IntField("Rows", (int) atlasProp.y);

			if (EditorGUI.EndChangeCheck()) {
				material.SetVector("_AtlasData", atlasProp);
			}

			EditorGUI.BeginChangeCheck();
			EditorGUIUtility.wideMode = true;
			var scaleOffset = EditorGUILayout.Vector2Field("Tiling", FindProperty("_ScaleOffset").vectorValue);
			EditorGUIUtility.wideMode = false;
			if (EditorGUI.EndChangeCheck()) {
				material.SetVector("_ScaleOffset", scaleOffset);
			}

		}

		protected void DrawProjectionInspector() {

			GUILayout.Space(10);
			GUILayout.Label("Projection", EditorStyles.boldLabel);
			GUILayout.Space(5);

			var stencilProp = FindProperty("_StencilMode");
			var stencilRefProp = FindProperty("_StencilRef");
			var stencilMaskProp = FindProperty("_StencilMask");

			var stencil = (UD_StencilMode) EditorGUILayout.EnumPopup(c("Layer Limit"), (UD_StencilMode) stencilProp.floatValue, EditorStyles.popup);
			stencilProp.floatValue = (float) stencil;

			if (stencil == UD_StencilMode.Disabled) {
				stencilRefProp.floatValue = 0;
				stencilMaskProp.floatValue = 0;
			} else if (stencil == UD_StencilMode.LayerMask) {
				stencilRefProp.floatValue = 0;
				stencilMaskProp.floatValue = 8;
			} else if (stencil == UD_StencilMode.InvertedLayerMask) {
				stencilRefProp.floatValue = 255;
				stencilMaskProp.floatValue = 8;
			} else {
				materialEditor.ShaderProperty(stencilRefProp, "Stencil Ref");
				materialEditor.ShaderProperty(stencilMaskProp, "Stencil Mask");
			}

			materialEditor.ShaderProperty(FindProperty("_AngleLimit"), "Angle Limit");
			materialEditor.ShaderProperty(FindProperty("_AngleSmoothing"), "Angle Smoothing");
			materialEditor.ShaderProperty(FindProperty("_NormalSmooth"), "Normal Smoothing");

		}

		void DrawBlendingEnumPopup() {

			GUILayout.Space(10);
			GUILayout.Label("Output", EditorStyles.boldLabel);
			GUILayout.Space(5);

			var blendProp = FindProperty("_Blend");
			var src = FindProperty("_SrcBlend");
			var dst = FindProperty("_DstBlend");

			var blend = (UD_BlendingMode) EditorGUILayout.EnumPopup(c("Blending"), (UD_BlendingMode) blendProp.floatValue, EditorStyles.popup);
			
			blendProp.floatValue = (float) blend;

			if (blend == UD_BlendingMode.On) {
				src.floatValue = (float) UnityEngine.Rendering.BlendMode.SrcAlpha;
				dst.floatValue = (float) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
			} else { // off
				src.floatValue = (float) UnityEngine.Rendering.BlendMode.One;
				dst.floatValue = (float) UnityEngine.Rendering.BlendMode.Zero;
			}

			#if UD_LEGACY

			var warning = UD_Camera.main != null && UD_Camera.main.isForward;

			if (warning) {
				EditorGUILayout.HelpBox("Main camera uses Forward rendering path and will ignore per-channel settings.", MessageType.Info);
			}

			var blendingProp = FindProperty("_BlendingFactors");
			var blendingFactors = blendingProp.vectorValue;

			EditorGUI.BeginChangeCheck();

			blendingFactors.x = EditorGUILayout.Slider(c("Color"), blendingFactors.x, 0, 1);
			blendingFactors.y = EditorGUILayout.Slider(c("Normals"), blendingFactors.y, 0, 1);
			blendingFactors.z = EditorGUILayout.Slider(c("Specular"), blendingFactors.z, 0, 1);
			blendingFactors.w = EditorGUILayout.Slider(c("Smoothness"), blendingFactors.w, 0, 1);

			if (EditorGUI.EndChangeCheck()) {
				blendingProp.vectorValue = blendingFactors;
			}

			#endif

		}

		protected void DrawRenderingInspector(bool isLit) {

			if(isLit) {
				DrawBlendingEnumPopup();
			}

			GUILayout.Space(10);
			GUILayout.Label("Rendering", EditorStyles.boldLabel);
			GUILayout.Space(5);

            materialEditor.RenderQueueField();
			
			materialEditor.ShaderProperty(FindProperty("_MaskThreshold"), "Alpha Cutout");
			materialEditor.ShaderProperty(FindProperty("_MaskSmoothing"), "Alpha Smoothing");
			materialEditor.ShaderProperty(FindProperty("_ColorLerp"), "Colorization");

			GUILayout.Space(10);
			GUILayout.Label("Advanced", EditorStyles.boldLabel);
			GUILayout.Space(5);

			materialEditor.ShaderProperty(FindProperty("_Lifetime"), "Lifetime (sec)");
			materialEditor.ShaderProperty(FindProperty("_LifetimePow"), "Lifetime Power");
			materialEditor.ShaderProperty(FindProperty("_MipBias"), "Mip Bias");

			var colorProp = FindProperty("_Color", properties);
			var mainTex = FindProperty("_MainTex", properties);

			if (mainTex.textureValue != null) {

				if (mainTex.textureValue.GetInstanceID() != lastMainTexId) {

					lastMainTexId = mainTex.textureValue.GetInstanceID();

					var path = AssetDatabase.GetAssetPath(mainTex.textureValue);
					var importer = AssetImporter.GetAtPath(path) as TextureImporter;

					hasAlpha = importer != null && importer.DoesSourceTextureHaveAlpha();

				}

			}

			EditorGUI.BeginDisabledGroup(!hasAlpha);

			var albedoOpacity = FindProperty("_AlbedoOpacity");

			if (!hasAlpha) {
				albedoOpacity.floatValue = 0;
			}

			materialEditor.ShaderProperty(albedoOpacity, "Albedo Opacity");

			EditorGUI.EndDisabledGroup();

			if (!hasAlpha && colorProp.colorValue.a >= 1) {
				EditorGUILayout.HelpBox("Albedo texture has no alpha.", MessageType.Info);
			}

			var material = materialEditor.target as Material;

			GUILayout.Label(string.Join("\n", material.shaderKeywords));

		}

		protected GUIContent c(string s) {
			if (!guiContentCache.ContainsKey(s)) {
				guiContentCache[s] = new GUIContent(s);
			}

			return guiContentCache[s];
		}

	}

}