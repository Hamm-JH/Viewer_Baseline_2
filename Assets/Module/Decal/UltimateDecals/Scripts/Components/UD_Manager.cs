using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bearroll.UltimateDecals {

	[ExecuteInEditMode]
	public partial class UD_Manager: UD_Component {

		public ForwardLightManager forwardLightManager { get; private set; }

		public DeferredCullingManager deferredCullingManager { get; private set; }

		public int maxPermanentMarks = 256;

		public bool perDecalLightProbes = false;
		public LayerMask layerMask = ~0;
		public UD_Error initError = UD_Error.None;

		public static readonly string version = "1.0.3"; 

		public static List<UltimateDecal> decals = new List<UltimateDecal>();

		[Range(-2,2)]
		public float globalMipBias = 0f;

		public static UD_Manager instance { get; private set;  }

		public int state { get; private set; }

		int lastPrepareFrame = -1;

		public UD_Pass markPass { get; private set; }
		public UD_Pass staticPass { get; private set; }
		public UD_Pass dynamicPass { get; private set; }
		
		public readonly List<UD_Pass> passes = new List<UD_Pass>();

		Dictionary<UltimateDecal, UD_Pass> passByDecal = new Dictionary<UltimateDecal, UD_Pass>();
		
		public LayerMask mainLayerMask {
			get { return layerMask; } 
		}

		void DoRestart() {
			OnDisable();
			OnEnable(); 
		}

		void Awake() {
			if (Application.isPlaying) {
				DontDestroyOnLoad(gameObject);
			}
		}

		public void DoDestroy()
        {
			Destroy(instance);
			instance = null;
        }

		void OnEnable() {

			if (instance != null && instance != this) {
				initError = UD_Error.SecondInstance;
				enabled = false;
				return;
			}

			instance = this;
			state = 0;

			markPass = new UD_Pass(UltimateDecalType.PermanentMark, this);
			staticPass = new UD_Pass(UltimateDecalType.Static, this);
			dynamicPass = new UD_Pass(UltimateDecalType.Dynamic, this);

			forwardLightManager = new ForwardLightManager(this);
			deferredCullingManager = new DeferredCullingManager(this);

			passes.Clear();
			passes.Add(staticPass);
			passes.Add(dynamicPass);
			passes.Add(markPass);

			passByDecal.Clear();

			foreach (var decal in decals) {
				DoAddDecal(decal);
			}

			for(var i = 0; i < UD_Camera.activeCount; i++) {
				UD_Camera.GetCamera(i).Enable();
			}

#if UNITY_EDITOR
#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui -= DrawDecalHandles;
			SceneView.duringSceneGui += DrawDecalHandles;
#else
			SceneView.onSceneGUIDelegate -= DrawDecalHandles;
			SceneView.onSceneGUIDelegate += DrawDecalHandles;
#endif
#endif

		}

		void OnDisable() {

			foreach (var e in passes) {
				e.Clean();
			}
			passes.Clear();
			
			if (forwardLightManager != null) {
				forwardLightManager.Clean();
				forwardLightManager = null;
			}

			if (deferredCullingManager != null) {
				deferredCullingManager.Clean();
				deferredCullingManager = null;
			}

			if (instance != null && instance != this) return;

			for(var i = 0; i < UD_Camera.activeCount; i++) {
				UD_Camera.GetCamera(i).Disable();
			}

			instance = null;

		}
		
		public void TryPrepare() {

			if (lastPrepareFrame == Time.frameCount) return;

			lastPrepareFrame = Time.frameCount;

			Prepare();

		}

		void Prepare() {

			if (Time.timeSinceLevelLoad == 0) return;

			if (!DynamicGI.isConverged) return;

			var update = false;

			foreach (var pass in passes) {

				if (!pass.Update()) continue;

				update = true;

				for(var i = 0; i < UD_Camera.activeCount; i++) {
					UD_Camera.GetCamera(i).RebuildPass(pass);
				}

			}

			if (!update) return;

			state++;

			for(var i = 0; i < UD_Camera.activeCount; i++) {
				UD_Camera.GetCamera(i).FinalizeRebuild();
			}

		}

		public static void Restart() {
			
			if (instance == null) return;

			instance.DoRestart();

		}

		public static void AddDecal(UltimateDecal decal) {

			decals.Add(decal);

			if (instance == null) return;

			instance.DoAddDecal(decal);

		}

		public static void AddPermanentMark(Material material, Transform t, float normalizedAtlasOffset = 0, int renderQueueOffset = 0) {

			if (instance == null) return;

			if (instance.markPass == null) return;

			instance.markPass.AddDecal(material, t, normalizedAtlasOffset, renderQueueOffset);

		}

		public static void AddPermanentMark(Material material, Matrix4x4 matrix, float normalizedAtlasOffset = 0, int renderQueueOffset = 0) {

			if (instance == null) return;

			if (instance.markPass == null) return;

			instance.markPass.AddDecal(material, matrix, normalizedAtlasOffset, renderQueueOffset);

		}

		public static void RemoveDecal(UltimateDecal decal) {

			decals.Remove(decal);

			if (instance == null) return;

			instance.DoRemoveDecal(decal); 

		}

		public static void UpdateDecal(UltimateDecal decal) {

			if (instance == null) return;

			instance.DoRemoveDecal(decal);
			instance.DoAddDecal(decal);

		}

		void DoRemoveDecal(UltimateDecal decal) {

			if (!passByDecal.ContainsKey(decal)) return;

			passByDecal[decal].RemoveDecal(decal);

			passByDecal.Remove(decal);

		}

		void DoAddDecal(UltimateDecal decal) {

			if (passByDecal.ContainsKey(decal)) {
				// warning?
				return;
			}

			var pass = markPass;
			var type = decal.type;

			if (!Application.isPlaying) {
				type = UltimateDecalType.Dynamic;
			}

			if (type == UltimateDecalType.Dynamic) {
				pass = dynamicPass;
			} else if (type == UltimateDecalType.Static) {
				pass = staticPass;
			}

			pass.AddDecal(decal);

			passByDecal[decal] = pass;

		}

	}
}