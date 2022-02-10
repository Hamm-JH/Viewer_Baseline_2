using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Bearroll.UltimateDecals {

	public class DeferredCullingManager {

		UD_Manager manager;

		public Material cullingMaterial { get; private set; }

		Light cullingLight;
		bool[] layers = new bool[32];

		public DeferredCullingManager(UD_Manager manager) {

			this.manager = manager;

			if (cullingMaterial == null) {
				cullingMaterial = new Material(Shader.Find("Hidden/UltimateDecals/Culling"));
			}

			for (var i = 0; i < 32; i++) {
				layers[i] = !string.IsNullOrEmpty(LayerMask.LayerToName(i));
			}

			InitCullingLight();

		}

		public void Clean() {
			
			if (cullingLight != null) {
				Object.DestroyImmediate(cullingLight.gameObject);
			}

		}

		public void RequestCullingLight(UD_Camera udCamera) {

			var camera = udCamera.camera;

			camera.clearStencilAfterLightingPass = false;

			if (cullingLight == null) {
				InitCullingLight();
			}

			var pos = camera.transform.position + camera.transform.forward * 2;
			var matrix = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);

			cullingLight.cullingMask = ~manager.layerMask;
			cullingLight.transform.position = pos;

			for (var i = 0; i < 32; i++) {

				if ((manager.layerMask & (1 << i)) == 0 && !layers[i]) continue;

				Graphics.DrawMesh(ResourceManager.instanceMesh, matrix, cullingMaterial, i, null, 0, null, ShadowCastingMode.Off);

			}

		}

		void InitCullingLight() {

			if (cullingLight != null) {
				Object.DestroyImmediate(cullingLight.gameObject);
			}

			var lightObject = GameObject.Find("/UD_CullingLight");

			if (lightObject == null) {
				lightObject = new GameObject("UD_CullingLight");
			}

			if (Application.isPlaying) {
				Object.DontDestroyOnLoad(lightObject);
			}

			lightObject.transform.forward = Vector3.up;
			lightObject.hideFlags = HideFlags.HideAndDontSave;

			cullingLight = lightObject.GetComponent<Light>();

			if (cullingLight == null) {
				cullingLight = lightObject.AddComponent<Light>();
			}

			cullingLight.type = LightType.Point;
			cullingLight.range = 0.1f;
			cullingLight.intensity = 0.01f;
			cullingLight.color = new Color32(1,1,1,1);
			cullingLight.shadows = LightShadows.None;
			cullingLight.cullingMask = ~manager.layerMask;

		}

	}

}