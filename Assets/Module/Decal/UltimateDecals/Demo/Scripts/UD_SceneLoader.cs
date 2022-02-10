using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Bearroll.UltimateDecals_Demo {

	public class UD_SceneLoader: MonoBehaviour {
		public string scenePath;
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(UD_SceneLoader))]
	public class UD_SceneLoaderEditor: Editor {

		public override void OnInspectorGUI() {

			base.OnInspectorGUI();

			var t = target as UD_SceneLoader;

			if (GUILayout.Button("Load extra scene")) {
				EditorSceneManager.LoadSceneAsyncInPlayMode(t.scenePath, new LoadSceneParameters(LoadSceneMode.Additive));
			}

			if (GUILayout.Button("Unload active scene")) {
				
				var scene = EditorSceneManager.GetActiveScene();

				EditorSceneManager.UnloadSceneAsync(scene);
			}

		}

	}
	#endif

}