using UnityEngine;
using UnityEditor;

namespace Bearroll.UltimateDecals {

    [CustomEditor(typeof(UD_LightProbeManager))]
    [CanEditMultipleObjects]
    public class UD_LightProbeManagerEditor: Bearroll_Editor {

		void OnEnable() {
		}

		public override void OnInspectorGUI() {

			var manager = UD_Manager.instance;

			if (manager == null) {
				EditorGUILayout.HelpBox("UD_Manager not found", MessageType.Info);
				return;
			}

			var t = target as UD_LightProbeManager;

			GUILayout.Label("Light probes: " + t.probeCount);

			FastPropertyField("placeOnBake", "Auto place on bake");

			if (GUILayout.Button("Place probes")) {
				t.PlaceProbes(manager);
			}

			if (GUILayout.Button("Remove probes")) {
				t.RemoveProbes();
			}

		}

	}



}
