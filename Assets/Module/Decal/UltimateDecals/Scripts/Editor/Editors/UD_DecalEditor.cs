using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bearroll.UltimateDecals {

    [CustomEditor(typeof(UltimateDecal))]
    [CanEditMultipleObjects]
    public class UD_DecalEditor: Bearroll_Editor {

		MaterialEditor materialEditor;
		GUIStyle materialButtonStyle;
		GUIStyle labelCenterStyle;

		static bool isMaterialListOpen;
        static bool isMaterialListLoaded;
		static List<Material> materials = new List<Material>();

		static void LoadMaterials() {

			isMaterialListLoaded = true;

			materials.Clear();

			var assets = AssetDatabase.FindAssets("t:Material");

			foreach (var guid in assets) {

				var path = AssetDatabase.GUIDToAssetPath(guid);
				var m = AssetDatabase.LoadAssetAtPath<Material>(path);

				if (m == null) continue;

				if (m.shader == null) continue;

				if (!m.shader.name.StartsWith("UltimateDecals/")) continue;

				materials.Add(m);

			}

			Resources.UnloadUnusedAssets();

			materials.Sort((a,b) => string.CompareOrdinal(a.name, b.name));

		}

		public override void OnInspectorGUI() {

			var t = (UltimateDecal) target;

            EditorGUIUtility.labelWidth = 150;

			EditorGUI.BeginChangeCheck();

			FastPropertyField("renderingMode", "Mode");

			FastPropertyField("controlObjectName", "Control Object Name");

			FastPropertyField("type", "Type");

			FastPropertyField("material", "Material");

			if (t.material != null && t.material.HasProperty("_AtlasData")) {

				var m = t.material;

				var atlasData = m.GetVector("_AtlasData");
				var atlasX = Mathf.Max(1, (int) atlasData.x);
				var atlasY = Mathf.Max(1, (int) atlasData.y);
				var atlasLength = atlasX * atlasY;

				if (atlasLength > 1) {
					FastSliderField("atlasIndex", "Atlas Index", 0, atlasLength - 1);
				}

				var mainTex = m.GetTexture("_MainTex");

				if (mainTex != null) {

					var scaleOffsetProp = m.GetVector("_ScaleOffset");

					var textureRatio = (mainTex.width / (float) atlasX * scaleOffsetProp.x) / (mainTex.height / (float) atlasY * scaleOffsetProp.y);
					var scale = t.transform.localScale;
					var decalRatio = scale.x / scale.z;

					if (Mathf.Abs(textureRatio / decalRatio - 1) > 0.1f) {

						GUILayout.BeginHorizontal();

						var s = string.Format("Decal scale doesn't match texture/atlas aspect ratio.", textureRatio, decalRatio);
						EditorGUILayout.HelpBox(s, MessageType.Info);

						if (GUILayout.Button("Fix scale")) {

							scale.x = scale.z * textureRatio;

							t.transform.localScale = scale;

						}

						GUILayout.EndHorizontal();
					}
				}
			}

			FastPropertyField("order", "Render Queue Offset");

			if (materialButtonStyle == null) {

				materialButtonStyle = new GUIStyle();
				materialButtonStyle.margin = new RectOffset();
				materialButtonStyle.padding = new RectOffset();

			}

			if (labelCenterStyle == null) {
				labelCenterStyle = new GUIStyle(EditorStyles.label);
				labelCenterStyle.alignment = TextAnchor.MiddleCenter;
				labelCenterStyle.fontSize = 8;
			}

			if (isMaterialListOpen) {
				if (GUILayout.Button("Hide materials")) {
					isMaterialListOpen = false;
				}
			} else {
				if (GUILayout.Button("Show materials")) {
					isMaterialListOpen = true;
				}
			}

            if (isMaterialListOpen && !isMaterialListLoaded) {
                LoadMaterials();
            }

			if (isMaterialListOpen) {

				GUILayout.BeginVertical(EditorStyles.textArea);

				var width = EditorGUIUtility.currentViewWidth - 45;
				var colCount = Mathf.FloorToInt(width / 60f);
				var size = Mathf.FloorToInt(width / (float) colCount);

				for (var i = 0; i < Mathf.CeilToInt(materials.Count / (float) colCount); i++) {

					GUILayout.BeginHorizontal();

					for (var j = 0; j < colCount && i * colCount + j < materials.Count; j++) {

						var m = materials[i * colCount + j];

						GUILayout.BeginVertical(GUILayout.Width(size));

						var rect = EditorGUILayout.GetControlRect(false, size);

						// GUI.DrawTexture(rect, m.mainTexture);

						if (m == t.material) {

							var borderRect = new Rect(rect);
							borderRect.height += 20;

							GUI.Box(borderRect, Texture2D.blackTexture);

						}

						var texture = m.mainTexture ? m.mainTexture : Texture2D.whiteTexture;

						EditorGUI.DrawPreviewTexture(rect, texture, m);

						GUILayout.Label(m.name, labelCenterStyle, GUILayout.Width(size));

						GUILayout.EndVertical();

						if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.button == 0) {

							var usedRect = GUILayoutUtility.GetLastRect();

							if (usedRect.Contains(Event.current.mousePosition)) {

								serializedObject.FindProperty("material").objectReferenceValue = m;
								serializedObject.ApplyModifiedProperties();
								t.material = m;
								UD_Manager.UpdateDecal(t);

							}

						}

					}

					GUILayout.EndHorizontal();

					GUILayout.Space(5);

				}

				GUILayout.EndVertical();

				if (GUILayout.Button("Reload materials")) {
					LoadMaterials();
				}

			} 


			if (EditorGUI.EndChangeCheck()) {

				serializedObject.ApplyModifiedProperties();

				foreach (UltimateDecal e in targets) {
					UD_Manager.UpdateDecal(e);
				}

			}

			if (targets.Length == 1) {

				var material = t.material;

				if (material != null) {

					if (materialEditor == null) {
						materialEditor = (MaterialEditor) CreateEditor(material);
					} else {	
						if (materialEditor.target as Material != material) {
                            DestroyImmediate(materialEditor);
							materialEditor = (MaterialEditor) CreateEditor(material);
						}
					}

					materialEditor.DrawHeader();

					var isDefaultMaterial = !AssetDatabase.GetAssetPath(material).StartsWith("Assets");
 
					using (new EditorGUI.DisabledGroupScope(isDefaultMaterial)) {
						materialEditor.OnInspectorGUI(); 
					}

				}
			}


		}

        void OnDisable()
        {
            DestroyImmediate(materialEditor);
        }

	}

}