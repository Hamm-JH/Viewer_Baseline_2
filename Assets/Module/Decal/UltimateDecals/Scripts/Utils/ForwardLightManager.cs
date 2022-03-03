using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace Bearroll.UltimateDecals {

	public class ForwardLightManager {

		public Light mainDirectionalLight { get; private set; }

		List<Light> lights = new List<Light>();

		static int maxLights = 8;

		Vector4[] lightPos = new Vector4[maxLights];
		Vector4[] lightColor = new Vector4[maxLights];
		Vector4[] lightDir = new Vector4[maxLights];
		Vector4[] lightData = new Vector4[maxLights];

		#if !UD_URP && !UD_LWRP
		Matrix4x4[] worldToLight = new Matrix4x4[maxLights];
		#endif

		bool requiresLightUpdate;
		float[] weights = new float[maxLights];
		Plane[] planes = new Plane[6];

		Texture2D cookieTex;
		Texture2D attenTex;

		private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
			requiresLightUpdate = true;
		}

		public ForwardLightManager(UD_Manager manager) {

			attenTex = Resources.Load<Texture2D>("UD_Attenuation");
			cookieTex = Resources.Load<Texture2D>("UD_Cookie");

			requiresLightUpdate = true;

			SceneManager.sceneLoaded += OnSceneLoaded;

		}

		public void Clean() {
			lights.Clear();
		}

		public void PrepareFrame(UD_Camera camera) {

			Profiler.BeginSample("PrepareFrame", camera.gameObject);

			if (requiresLightUpdate) {
				lights.Clear();
				lights.AddRange(FindObjectsOfTypeAll<Light>());
				requiresLightUpdate = false;
			}

			var count = 0;
			Light directionalLight = null;

			GeometryUtility.CalculateFrustumPlanes(camera.camera, planes);

			for(var i = lights.Count - 1; i >= 0; i--) {

				var light = lights[i];

				if(light == null) {
					lights.RemoveAt(i);
					continue;
				}

				if(!light.isActiveAndEnabled) continue;

				if(light.intensity < 0.01f) continue;

				if(light.type == LightType.Directional) {

					if(directionalLight == null || light.intensity > directionalLight.intensity) {
						directionalLight = light;
					}

					continue;

				}

				var transform = light.transform;
				var position = transform.position;
				var bounds = new Bounds(position, Vector3.one * light.range * 2);

				if (!GeometryUtility.TestPlanesAABB(planes, bounds)) continue;

				var weight = light.intensity * light.color.maxColorComponent * light.range * light.spotAngle;

				weight /= Vector3.Distance(light.transform.position, camera.transform.position);

				if(count < maxLights) count++;

				var index = count - 1;
				while(index > 0 && weights[index] < weight) {
					index--;
				}

				for(var j = count - 1; j > index; j--) {

					weights[j] = weights[j - 1];

					lightPos[j] = lightPos[j - 1];
					lightDir[j] = lightDir[j - 1];
					lightColor[j] = lightColor[j - 1];
					lightData[j] = lightData[j - 1];
					#if !UD_URP && !UD_LWRP
					worldToLight[j] = worldToLight[j - 1];
					#endif
				}

				var pos = (Vector4) position;
				pos.w = light.range;

				var dir = (Vector4) transform.forward;

				if(light.type == LightType.Spot) {
					dir.w = 1 - Mathf.Cos(Mathf.Deg2Rad * light.spotAngle / 2f);
				} else {
					dir.w = -1;
				}

				weights[index] = weight;
				lightPos[index] = pos;
				lightDir[index] = dir;
				lightColor[index] = GetLightColor(light);

				var cosOuterAngle = Mathf.Cos(Mathf.Deg2Rad * light.spotAngle * 0.5f);
#if UNITY_2019_1_OR_NEWER
				var cosInnerAngle = Mathf.Cos(light.innerSpotAngle * Mathf.Deg2Rad * 0.5f);
#else
				var cosInnerAngle = Mathf.Cos((2.0f * Mathf.Atan(Mathf.Tan(light.spotAngle * 0.5f * Mathf.Deg2Rad) * (64.0f - 18.0f) / 64.0f)) * 0.5f);
#endif
				var smoothAngleRange = Mathf.Max(0.001f, cosInnerAngle - cosOuterAngle);
				var invAngleRange = 1.0f / smoothAngleRange;
				var add = -cosOuterAngle * invAngleRange;

				lightData[index] = new Vector4(invAngleRange, add);

#if !UD_URP && !UD_LWRP

				worldToLight[index] = Matrix4x4.Scale(Vector3.one / light.range) * Matrix4x4.Rotate(Quaternion.Inverse(light.transform.rotation)) * Matrix4x4.Translate(-light.transform.position);

				if (light.type == LightType.Spot) {

					var proj = Matrix4x4.identity;
					var d = Mathf.Deg2Rad * light.spotAngle * 0.5f;
					var r = Mathf.Sin(d) / Mathf.Cos(d);

					proj[3, 2] = 2 * r;
					proj[3, 3] = 0;

					worldToLight[index] = proj * worldToLight[index];
				}

#endif

			}

			if(directionalLight != null && directionalLight.bakingOutput.lightmapBakeType != LightmapBakeType.Baked) {

				mainDirectionalLight = directionalLight;

				Shader.SetGlobalVector("UD_MainLightDir", mainDirectionalLight.transform.forward);

				var color = GetLightColor(mainDirectionalLight);
				color.w = mainDirectionalLight.shadowStrength;

				Shader.SetGlobalVector("UD_MainLightColor", color);

			} else {

				mainDirectionalLight = null;

				Shader.SetGlobalVector("UD_MainLightColor", Vector3.zero);

			}

			Shader.SetGlobalFloat("UD_LightCount", count);

			Shader.SetGlobalVectorArray("UD_LightPos", lightPos);
			Shader.SetGlobalVectorArray("UD_LightDir", lightDir);
			Shader.SetGlobalVectorArray("UD_LightColor", lightColor);
			Shader.SetGlobalVectorArray("UD_LightData", lightData);

#if !UD_URP && !UD_LWRP
			Shader.SetGlobalMatrixArray("UD_WorldToLight", worldToLight);
			Shader.SetGlobalTexture("UD_Attenuation", attenTex);
			Shader.SetGlobalTexture("UD_Cookie", cookieTex);
#endif

			Profiler.EndSample();

		}

		public static List<T> FindObjectsOfTypeAll<T>() {
			var results = new List<T>();
			for(var i = 0; i < SceneManager.sceneCount; i++) {
				var s = SceneManager.GetSceneAt(i);
				if(s.isLoaded) {
					var allGameObjects = s.GetRootGameObjects();
					for(var j = 0; j < allGameObjects.Length; j++) {
						var go = allGameObjects[j];
						results.AddRange(go.GetComponentsInChildren<T>(true));
					}
				}
			}
			return results;
		}

		static Vector4 GetLightColor(Light light) {
#if UD_URP || UD_LWRP
			return light.color.linear * light.intensity;
#else
			if (QualitySettings.activeColorSpace == ColorSpace.Gamma) {
				return light.color * light.intensity;				
			} else {
				return light.color.linear * Mathf.Pow(light.intensity, 2.2f);
			}
#endif

		}

	}

}