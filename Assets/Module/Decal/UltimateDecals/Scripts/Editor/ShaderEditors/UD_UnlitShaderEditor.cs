using UnityEngine;
using UnityEditor;

namespace Bearroll.UltimateDecals {

    public class UD_UnlitShaderEditor: Bearroll_ShaderEditor {

		public override bool OnGUI() {

			EditorGUIUtility.labelWidth = 150;
			EditorGUIUtility.hierarchyMode = false;

			// var material = materialEditor.target as Material;

			GUILayout.Label("Main", EditorStyles.boldLabel);
			GUILayout.Space(5);

			EditorGUIUtility.labelWidth = 200;

			var blendProp = FindProperty("_Blend");
			var src = FindProperty("_SrcBlend");
			var dst = FindProperty("_DstBlend");

			var blend = (UD_UnlitBlendingMode) EditorGUILayout.EnumPopup(c("Color Mode"), (UD_UnlitBlendingMode) blendProp.floatValue, EditorStyles.popup);
			
			blendProp.floatValue = (float) blend;

			if (blend == UD_UnlitBlendingMode.AlphaBlending) {
				src.floatValue = (float) UnityEngine.Rendering.BlendMode.SrcAlpha;
				dst.floatValue = (float) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
			} else if (blend == UD_UnlitBlendingMode.Additive) {
				src.floatValue = (float) UnityEngine.Rendering.BlendMode.One;
				dst.floatValue = (float) UnityEngine.Rendering.BlendMode.One;
			} else if (blend == UD_UnlitBlendingMode.Multiplicative) {
				src.floatValue = (float) UnityEngine.Rendering.BlendMode.DstColor;
				dst.floatValue = (float) UnityEngine.Rendering.BlendMode.Zero;
			} else { // off
				src.floatValue = (float) UnityEngine.Rendering.BlendMode.One;
				dst.floatValue = (float) UnityEngine.Rendering.BlendMode.Zero;
			}

			GUILayout.Space(10);
			
			materialEditor.TexturePropertySingleLine(c("Color (RGB) Opacity (A)"), FindProperty("_MainTex"), FindProperty("_Color"));
			materialEditor.TexturePropertySingleLine(c("Mask (R)"), FindProperty("_Mask", properties));

			DrawAtlasInspector();

			DrawProjectionInspector();

			DrawRenderingInspector(false);

			return false;
		}

	}

}