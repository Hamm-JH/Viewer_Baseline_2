using UnityEngine;
using UnityEditor;

namespace Bearroll.UltimateDecals {

    public class UD_LitShaderEditor: Bearroll_ShaderEditor {

		public override bool OnGUI() {

			EditorGUIUtility.labelWidth = 150;
			EditorGUIUtility.hierarchyMode = false;

			var material = materialEditor.target as Material;

			GUILayout.Label("Main", EditorStyles.boldLabel);
			GUILayout.Space(5);

			/*
			var workflowData = material.GetVector("_Workflow");

			var workflow = (UD_PBRWorkflow) (int) workflowData.x;

			var workflowNew = (UD_PBRWorkflow) EditorGUILayout.EnumPopup(c("Workflow"), workflow, EditorStyles.popup);

			if (workflowNew != workflow) {
				workflow = workflowNew;
			}
			*/
		
			var workflowData = material.GetVector("_Workflow");

			var metallic = workflowData.x > 0;
			var specular = !metallic;
			var roughness = workflowData.y > 0;

			var w1 = "Metallic";
			var w3 = " (R)";
			if (specular) {
				w1 = "Specular";
				w3 = " (RGB)";
			}
			var w2 = "Smoothness";
			if (roughness) {
				w2 = "Roughness";
			}

			UD_PBRWorkflow workflow;
			if (specular) {
				if (roughness) {
					workflow = UD_PBRWorkflow.SpecularRoughness;
				} else {
					workflow = UD_PBRWorkflow.SpecularSmoothness;
				}
			} else {
				if (roughness) {
					workflow = UD_PBRWorkflow.MetallicRoughness;
				} else {
					workflow = UD_PBRWorkflow.MetallicSmoothness;
				}
			}

			var workflowNew = (UD_PBRWorkflow) EditorGUILayout.EnumPopup(c("Workflow"), workflow, EditorStyles.popup);

			UD_SRMode srmode;
			UD_SRMode srmodeNew;

			if (metallic) {
				var srmodeMetallic = workflowData.z > 0 ? UD_SRModeMetallic.Separate : UD_SRModeMetallic.MetallicAlpha;
				var srmodeNewMetallic = (UD_SRModeMetallic) EditorGUILayout.EnumPopup(c(w2 + " source"), srmodeMetallic, EditorStyles.popup);
				srmode = (UD_SRMode) srmodeMetallic;
				srmodeNew = (UD_SRMode) srmodeNewMetallic;
			} else {
				srmode = workflowData.z > 0 ? UD_SRMode.Separate : UD_SRMode.SpecularAlpha;
				srmodeNew = (UD_SRMode) EditorGUILayout.EnumPopup(c(w2 + " source"), srmode, EditorStyles.popup);
			}

			if (workflowNew != workflow || srmodeNew != srmode) {

				workflow = workflowNew;
				srmode = srmodeNew;

				workflowData.x = (workflow == UD_PBRWorkflow.MetallicSmoothness || workflow == UD_PBRWorkflow.MetallicRoughness) ? 1 : 0;
				workflowData.y = (workflow == UD_PBRWorkflow.SpecularRoughness || workflow == UD_PBRWorkflow.MetallicRoughness) ? 1 : 0;
				workflowData.z = srmode == UD_SRMode.Separate ? 1 : 0;

				material.SetVector("_Workflow", workflowData);

			}

			GUILayout.Space(10);
			EditorGUIUtility.labelWidth = 200;

			materialEditor.TexturePropertySingleLine(c("Albedo (RGB) Opacity (A)"), FindProperty("_MainTex"), FindProperty("_Color"));
			materialEditor.TexturePropertySingleLine(c("Normal map (RGB)"), FindProperty("_BumpMap", properties), FindProperty("_NormalScale"));

			var smsrTex = FindProperty("_SMSR", properties);
			var srTex = FindProperty("_SeparateSR", properties);
			var wasSmsrEmpty = smsrTex.textureValue == null;
			var wasSrEmpty = srTex.textureValue == null;
			var sm = FindProperty("_SM");
			var sr = FindProperty("_SR");
			
			if (srmode == UD_SRMode.Separate) {
				materialEditor.TexturePropertySingleLine(c(w1 + w3), smsrTex);
				materialEditor.TexturePropertySingleLine(c(w2 + " (R)"), srTex);
			} else {
				materialEditor.TexturePropertySingleLine(c(w1 + w3 + " " + w2 + "(A)"), smsrTex);
			}

			if (wasSmsrEmpty && smsrTex.textureValue != null) {
				sm.floatValue = 1;
			}

			if (wasSrEmpty && srTex.textureValue != null) {
				sr.floatValue = 1;
			}

			materialEditor.ShaderProperty(sm, w1);
			materialEditor.ShaderProperty(sr, w2);

			GUILayout.Space(5);

			materialEditor.TexturePropertySingleLine(c("Occlusion (R)"), FindProperty("_OcclusionMap", properties), FindProperty("_Occlusion", properties));
			materialEditor.TexturePropertySingleLine(c("Mask (R)"), FindProperty("_Mask", properties));

			DrawAtlasInspector();

			DrawProjectionInspector();

			DrawRenderingInspector(true);

			return false;
		}

	}

}