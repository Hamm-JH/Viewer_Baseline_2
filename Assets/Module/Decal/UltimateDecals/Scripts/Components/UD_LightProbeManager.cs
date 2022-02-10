using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bearroll.UltimateDecals {

	[ExecuteInEditMode]
    public class UD_LightProbeManager: UD_Component {

		public bool placeOnBake = true;

		#if UNITY_EDITOR

		List<GameObject> rootObjects = new List<GameObject>(32);
		List<UltimateDecal> branchDecals = new List<UltimateDecal>(256);
		List<UltimateDecal> decals = new List<UltimateDecal>(256);
		Vector3[] positions;
		bool wasRunning;
		int lastState = -1;

		public int probeCount { get; private set; }

		public void RemoveProbes() {

			var group = GetComponent<LightProbeGroup>();

			if (group == null) return;
					
			#if UNITY_EDITOR
			group.probePositions = new Vector3[0];
			#endif

		}

		public void PlaceProbes(UD_Manager manager) {

			rootObjects.Clear();
			branchDecals.Clear();
			decals.Clear();

			var scene = gameObject.scene;
			scene.GetRootGameObjects(rootObjects);

			foreach (var root in rootObjects) {

				root.GetComponentsInChildren(true, branchDecals);

				foreach (var decal in branchDecals) {

					if (decal.type == UltimateDecalType.PermanentMark) continue;

					decals.Add(decal);

				}

			}

			probeCount = decals.Count;

			if (positions == null || positions.Length != probeCount) {
				positions = new Vector3[probeCount];
			}

			for (var i = 0; i < probeCount; i++) {
				var decal = decals[i];
				var position = decal.transform.position + decal.transform.up * decal.transform.localScale.y * 0.5f;
				positions[i] = position;
			}

			var group = GetComponent<LightProbeGroup>();

			if (group == null) {
				group = gameObject.AddComponent<LightProbeGroup>();
			}
			
			group.probePositions = positions;

			rootObjects.Clear();
			branchDecals.Clear();
			decals.Clear();
		

		}

		void Update() {

			if (!placeOnBake) return;

			var manager = UD_Manager.instance;

			if (manager == null) return;

			if (manager.state == lastState) return;

			lastState = manager.state;

			if (Lightmapping.isRunning && !wasRunning) {
				PlaceProbes(manager);
				Debug.Log("Placed light probes", gameObject);
			}

			wasRunning = Lightmapping.isRunning;

		}

		#endif

	}



}