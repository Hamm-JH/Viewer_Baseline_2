using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bearroll.UltimateDecals {

	[ExecuteInEditMode]
	public class UD_Camera: UD_Component {

		public UD_StereoMode stereoRenderingMode = UD_StereoMode.SinglePass;

		public new Camera camera { get; private set; }

		static List<UD_Camera> instances = new List<UD_Camera>();

		static public int activeCount {
			get { return instances.Count; }
		}

		static public UD_Camera GetCamera(int index) {
			if (index < 0 || index >= instances.Count) return null;
			return instances[index];
		}

#if UD_URP || UD_LWRP
	public UD_ScriptableRenderer decalRenderer { get; private set; }
#else
	public UD_LegacyRenderer decalRenderer { get; private set; }
#endif

		static public UD_Camera currentCamera { get; private set; }

		static public UD_Camera main { get; private set; }

		public UD_Error initError { get; private set; }

		[Range(0f, 1f)]
		public float normalSmoothing = 0.0f;

		public UD_ForwardNormalsSource forwardDepthNormals = UD_ForwardNormalsSource.Restored;

		public UD_DebugMode debugMode;


		public UD_Manager manager {
			get { return UD_Manager.instance; }
		}

		bool cameraAllowHDR;
		bool usingMSAA;
		bool usingPerDecalLightProbes;
		RenderingPath cameraRenderingPath;

		public bool isUsingMSAA {
			get { return QualitySettings.antiAliasing > 0 && camera != null && camera.allowMSAA; }
		}

		public bool isDeferred {
			get {
#if UD_URP || UD_LWRP
				return false;
#else
				return camera != null && camera.actualRenderingPath == RenderingPath.DeferredShading;
#endif
			}
		}

		public bool isStereo {
			get {
				return camera.stereoTargetEye == StereoTargetEyeMask.Both && camera.stereoActiveEye != Camera.MonoOrStereoscopicEye.Mono;
			}
		}

		public bool isForward {
			get { return !isDeferred; }
		}

		public Matrix4x4 GetViewInverse() {
			return camera.cameraToWorldMatrix;
		}

		public Matrix4x4 GetViewInverse(Camera.StereoscopicEye eye) {
			return camera.GetStereoViewMatrix(eye).inverse;
		}

		public Matrix4x4 GetViewProjInverse() {
			var p = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true);
			var v = camera.worldToCameraMatrix;
			return (p * v).inverse;
		}

		public float GetNearPlaneRadius() {
			var a = camera.nearClipPlane;
			var A = camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
			var h = (Mathf.Tan(A) * a);
			var w = (h / camera.pixelHeight) * camera.pixelWidth;
			var v = new Vector2(w, h);
			return v.magnitude;
		}

		public Matrix4x4 GetViewProjInverse(Camera.StereoscopicEye eye) {

			var projection = GL.GetGPUProjectionMatrix(camera.GetStereoProjectionMatrix(eye), true);
			var view = camera.GetStereoViewMatrix(eye);

			return (projection * view).inverse;

		}

		public void Enable() {

			initError = UD_Error.None;

			if (UD_Manager.instance == null) {
				initError = UD_Error.NoManager;
				return;
			}

			camera = GetComponent<Camera>();

			if (camera == null) {
				initError = UD_Error.NoCamera;
				return;
			}

			cameraRenderingPath = camera.actualRenderingPath;
			cameraAllowHDR = camera.allowHDR;
			usingMSAA = isUsingMSAA;
			usingPerDecalLightProbes = manager.perDecalLightProbes;

#if UD_URP || UD_LWRP
			RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
			RenderPipelineManager.endCameraRendering += EndCameraRendering;
#if UD_LWRP
			decalRenderer = new UD_LwrpRenderer(this);
#else
			decalRenderer = new UD_UrpRenderer(this);
#endif

#else
			if (isDeferred) {
				decalRenderer = new UD_DeferredRenderer(this);
			} else {
				decalRenderer = new UD_ForwardRenderer(this);
			}
#endif

			decalRenderer.Init();

		}

		void OnEnable() {

			instances.Add(this);

			if(CompareTag("MainCamera")) {
				main = this;
			}

			Enable();

		}

		public void Disable() {
			
			if (decalRenderer != null) {
				decalRenderer.Clean();
				decalRenderer = null;
			}

#if UD_URP || UD_LWRP
			RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= EndCameraRendering;	
#endif

		}

		void OnDisable() {

			instances.Remove(this);

			if (main == this) {
				main = null;
			}

			Disable();

		}
		
		public TextureDimension GetScreenSize(out int w, out int h) {

			w = camera.pixelWidth;
			h = camera.pixelHeight;
			var r = TextureDimension.Tex2D;

			if (camera.stereoTargetEye == StereoTargetEyeMask.Both && camera.stereoActiveEye != Camera.MonoOrStereoscopicEye.Mono) {

				if (stereoRenderingMode == UD_StereoMode.SinglePass) {
					w *= 2;
				}

			}

			return r;

		}

		bool requiresRestart {
			get {

				if (camera.actualRenderingPath != cameraRenderingPath) return true;

				if (camera.allowHDR != cameraAllowHDR) return true;

				if (isUsingMSAA != usingMSAA) return true;

				if (usingPerDecalLightProbes != manager.perDecalLightProbes) return true;

				if (decalRenderer.requiresRestart) return true;

				return false;

			}
		}

		void LateUpdate() {

			if (manager != null && manager.enabled) {
				manager.TryPrepare();
			}

		}

		public void Rebuild() {
			decalRenderer.RebuildAll();
		}

		public void RebuildPass(UD_Pass pass) {
			
			decalRenderer.RebuildPass(pass);

		}

		public void FinalizeRebuild() {
			decalRenderer.FinalizeRebuild();
		}

		void PrepareFrame() {

			if (manager == null) return;

			if (requiresRestart) {
				// Debug.Log("Restart");
				OnDisable();
				OnEnable();
			}

			currentCamera = this;

			camera.depthTextureMode |= DepthTextureMode.Depth;

			decalRenderer.PrepareFrame();

		}

		void FinalizeFrame() {

			if (manager == null) return;

			currentCamera = null;

			decalRenderer.FinalizeFrame();

		}

#if UD_LWRP || UD_URP

		void BeginCameraRendering(ScriptableRenderContext src, Camera camera) {

			if (camera != this.camera) return;

			PrepareFrame();

		}

		void EndCameraRendering(ScriptableRenderContext src, Camera camera) {

			if (camera != this.camera) return;

			FinalizeFrame();

		}

#else

		public void OnPreCull() {
			PrepareFrame();
		}

		void OnPostRender() {
			FinalizeFrame();
		}

#endif

#if UNITY_EDITOR
		void OnGUI() {

			if (debugMode == UD_DebugMode.Normals) {

				var t = decalRenderer.normalsTexture;

				GUI.DrawTexture(new Rect(0, 0, t.width, t.height), t, ScaleMode.ScaleAndCrop, false);

			} else if (debugMode == UD_DebugMode.Edges) {

				var t = decalRenderer.fixTexture;

				GUI.DrawTexture(new Rect(0, 0, t.width, t.height), t, ScaleMode.ScaleAndCrop, false);

			}

		}
#endif


	}

}