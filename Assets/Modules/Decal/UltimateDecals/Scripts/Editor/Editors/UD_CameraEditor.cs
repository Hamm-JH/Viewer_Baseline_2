using UnityEngine;
using UnityEditor;
using UnityEngine.XR;

#pragma warning disable 618

namespace Bearroll.UltimateDecals {

    [CustomEditor(typeof(UD_Camera))]
    [CanEditMultipleObjects]
    public class UD_CameraEditor: Bearroll_Editor {

		public override void OnInspectorGUI() {

			var t = target as UD_Camera;

			if (t.initError != UD_Error.None) {
				EditorGUILayout.HelpBox("Init error: " + t.initError, MessageType.Error);
				return;
			}

			EditorGUI.BeginChangeCheck();

			if (PlayerSettings.virtualRealitySupported) {
				FastPropertyField("stereoRenderingMode", "Stereo Mode");
			}

			#if !UD_URP && !UD_LWRP

			if (t.isDeferred) {

				FastPropertyField("normalSmoothing", "Per-pixel Normals");

			} else {

				FastPropertyField("forwardDepthNormals", "Normals Source");

				if (t.forwardDepthNormals == UD_ForwardNormalsSource.CameraDepthNormals) {
					EditorGUILayout.HelpBox("Camera's DepthNormals in Forward reduces performance. It's not recommended to use it for decals only.", MessageType.Info);
				}

				if (t.forwardDepthNormals != UD_ForwardNormalsSource.CameraDepthNormals && (t.camera.depthTextureMode & DepthTextureMode.DepthNormals) > 0) {

					EditorGUILayout.HelpBox("Camera renders DepthNormals texture but it's not used by UD. Consider disabling it if other assets don't need it.", MessageType.Info);

					if (GUILayout.Button("Disable DepthNormals")) {
						t.camera.depthTextureMode &= ~DepthTextureMode.DepthNormals;
					}
				}

			}

			#endif

			FastPropertyField("debugMode", "Debug View");

			// EditorGUILayout.HelpBox("Working", MessageType.Info);

			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}

	}

}