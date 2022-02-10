using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bearroll.UltimateDecals_Demo {

	[ExecuteInEditMode]
    public class UD_LightAdjuster: MonoBehaviour {

		public float intensity = 2;

#if UNITY_EDITOR

		void Awake() {

			var lights = GetComponentsInChildren<Light>();

			var intensity = this.intensity;
			#if UD_URP || UD_LWRP
			intensity *= 10;
			#endif

			foreach (var light in lights) {

				if (light.type == LightType.Directional) continue;

				light.intensity = intensity;
			}

		}

#endif

	}


}