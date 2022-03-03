#if UD_LWRP || UD_URP

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	[InitializeOnLoad]
	public class FeatureManager {
	
		static FeatureManager() {

			SetUpFeature();

		}

		static void SetUpFeature() {

			var data = GetData();

			if (data == null) return;

			var serializedObject = new SerializedObject(data);
			var m_RenderPasses = serializedObject.FindProperty("m_RendererFeatures");
			var m_RenderPassMap = serializedObject.FindProperty("m_RendererFeatureMap");

			for (var i = 0; i < m_RenderPasses.arraySize; i++) {

				var prop = m_RenderPasses.GetArrayElementAtIndex(i);

				if (prop.objectReferenceValue is UltimateDecalsFeature) {
					// Debug.Log("Found UltimateDecalsFeature in " + data);
					return;
				}

			}

			serializedObject.Update();

			var feature = GetFeature();

			string guid;
			long localId;
			AssetDatabase.TryGetGUIDAndLocalFileIdentifier(feature, out guid, out localId);

			m_RenderPasses.arraySize++;
			var componentProp = m_RenderPasses.GetArrayElementAtIndex(m_RenderPasses.arraySize - 1);
			componentProp.objectReferenceValue = feature;

			m_RenderPassMap.arraySize++;
			var guidProp = m_RenderPassMap.GetArrayElementAtIndex(m_RenderPassMap.arraySize - 1);
			guidProp.longValue = localId;

			serializedObject.ApplyModifiedProperties();

			Debug.Log("Added UltimateDecalsFeature to " + data);

		}

		static Object GetData() {

			var asset = GraphicsSettings.renderPipelineAsset;

			if (asset == null) return null;

			var serializedObject = new SerializedObject(asset);

			var m_RendererDataProp = serializedObject.FindProperty("m_RendererDataList");
			var m_DefaultRendererProp = serializedObject.FindProperty("m_DefaultRendererIndex");

			var prop = m_RendererDataProp.GetArrayElementAtIndex(m_DefaultRendererProp.intValue);

			if (prop == null) return null;

			return prop.objectReferenceValue;

		}

		static UltimateDecalsFeature GetFeature() {

			var feature = Resources.Load<UltimateDecalsFeature>("UltimateDecalsFeature");

			if (feature != null) return feature;

			feature = ScriptableObject.CreateInstance<UltimateDecalsFeature>();

			var path = AssetDatabase.GenerateUniqueAssetPath("Assets/UltimateDecalsFeature.asset");
			AssetDatabase.CreateAsset(feature, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			return feature;

		}

	}
	

}

#endif