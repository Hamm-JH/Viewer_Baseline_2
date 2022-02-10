using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Bearroll.UltimateDecals {

    [CustomEditor(typeof(UD_Manager))]
    [CanEditMultipleObjects]
    public class UD_ManagerEditor: Bearroll_Editor {

		public override void OnInspectorGUI() {

			var t = target as UD_Manager;

			if (t == null) return;

			EditorGUI.BeginChangeCheck();

			GUILayout.Label("Stats", EditorStyles.boldLabel);

			var count = t.passes.Sum(e => e.decalCount);

			GUILayout.Label("Decals: " + count);

			GUILayout.Label("Batches: " + t.passes.Sum(e => e.batchCount));

			GUILayout.Label("General", EditorStyles.boldLabel);

			FastPropertyField("maxPermanentMarks", "Max Permanent Marks");

			FastPropertyField("globalMipBias", "Global Mip Bias");

			FastPropertyField("perDecalLightProbes", "Per Decal Light Probes");

			GUILayout.Label("Deferred", EditorStyles.boldLabel);

			FastPropertyField("layerMask", "Layer Mask");

            GUILayout.Label("Editor", EditorStyles.boldLabel);

			FastPropertyField("drawHandles", "Draw Handles");

			if (EditorGUI.EndChangeCheck()) {
				serializedObject.ApplyModifiedProperties();
			}
		}

	}



}
