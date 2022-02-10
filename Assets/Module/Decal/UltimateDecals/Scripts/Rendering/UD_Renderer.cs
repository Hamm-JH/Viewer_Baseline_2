using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_Renderer {

		public UD_Manager manager {
			get { return camera != null ? camera.manager : null; }
		}

		public UD_Camera camera { get; private set; }

		protected CommandBuffer bufferPrepare { get; private set; }
		protected Material prepareMaterial { get; private set; }

		public RenderTexture normalsTexture { get; private set; }
		public RenderTexture tempTexture { get; private set; }
		public RenderTexture fixTexture { get; private set; }

		public RenderTexture depthTarget { get; private set; }

		List<Material> materials = new List<Material>(); 
		List<RenderTexture> textures = new List<RenderTexture>();
		List<CommandBuffer> commandBuffers = new List<CommandBuffer>();

		public RenderTargetIdentifier[] prepareTargets = {
			0,
			0
		};

		public RenderTargetIdentifier[] prepareTargets2 = {
			0,
			0
		};

		Matrix4x4[] viewProjInverse = new Matrix4x4[2];
		Matrix4x4[] cameraToWorld = new Matrix4x4[2];
		Plane[] planes = new Plane[6];
		Vector4[] planeData = new Vector4[6];

		Stopwatch sw = new Stopwatch();
		public float lastPrepareTime { get; private set; }

		public string name { get; private set; }

		protected Dictionary<UD_Pass, CommandBuffer> bufferByPassLit = new Dictionary<UD_Pass, CommandBuffer>();
		protected Dictionary<UD_Pass, CommandBuffer> bufferByPassUnlit = new Dictionary<UD_Pass, CommandBuffer>();

		public UD_Renderer(UD_Camera camera) {

			this.camera = camera;

			name = GetType().Name;

			prepareMaterial = CreateMaterial("Hidden/UltimateDecals/Prepare");

			CreateTextures();

			bufferPrepare = CreateCommandBuffer("UD_Prepare");

		}

		public void Init() {

			BuildPrepareBuffer(bufferPrepare);

			RebuildAll();

			OnInit();

		}

		protected virtual void OnInit() {

		}

		public void Clean() {

			OnBeforeClean();

			ReleaseTextures();

			foreach (var e in materials) {
				if (e == null) continue;

				Object.DestroyImmediate(e);
			}

			materials.Clear();

			foreach (var e in commandBuffers) {
				if (e == null) continue;

				e.Release();
			}

			commandBuffers.Clear();

		}

		protected virtual void OnBeforeClean() {

		}

		CommandBuffer GetCommandBufferByPassLit(UD_Pass pass) {

			if (!bufferByPassLit.ContainsKey(pass)) {
				bufferByPassLit[pass] = CreateCommandBuffer("UD_" + pass.type + " (Lit)");
			}

			return bufferByPassLit[pass];

		}

		CommandBuffer GetCommandBufferByPassUnlit(UD_Pass pass) {

			if (!bufferByPassUnlit.ContainsKey(pass)) {
				bufferByPassUnlit[pass] = CreateCommandBuffer("UD_" + pass.type + " (Unlit)");
			}

			return bufferByPassUnlit[pass];

		}


		public void RebuildAll() {

			foreach (var pass in manager.passes) {

				var buffer = GetCommandBufferByPassLit(pass);

				pass.RebuildCommandBuffer(camera, buffer);

				var unlitBuffer = GetCommandBufferByPassUnlit(pass);

				pass.RebuildUnlitCommandBuffer(camera, unlitBuffer);

			}

			FinalizeRebuild();

		}

		public void RebuildPass(UD_Pass pass) {

			var buffer = GetCommandBufferByPassLit(pass);

			pass.RebuildCommandBuffer(camera, buffer);

			var unlitBuffer = GetCommandBufferByPassUnlit(pass);

			pass.RebuildUnlitCommandBuffer(camera, unlitBuffer);

		}

		public void FinalizeRebuild() {
			OnFinalizeRebuild();
		}

		protected virtual void OnFinalizeRebuild() {

		}


		public virtual void FillPassLitBuffer(CommandBuffer commandBuffer) {


		}

		public virtual void BuildPrepareBuffer(CommandBuffer commandBuffer) {
		}

		public virtual void StartPassLitBuffer(CommandBuffer commandBuffer) {
		}

		public virtual void StartPassUnlitBuffer(CommandBuffer commandBuffer) {
		}



		public void PrepareFrame() {

			sw.Reset();
			sw.Start();

			if (normalsTexture == null) return;

			var data = new Vector4();
			data.x = camera.GetNearPlaneRadius();
			data.y = RenderSettings.ambientIntensity;
			Shader.SetGlobalVector("UD_Data", data);

			// var c = Camera.main;
			GeometryUtility.CalculateFrustumPlanes(camera.camera, planes);
			
			for (var i = 0; i < 6; i++) {
				planeData[i] = planes[i].normal;
				planeData[i].w = planes[i].distance;
			}

			Shader.SetGlobalVectorArray("UD_FrustumPlanes", planeData);

			Shader.SetGlobalMatrix("UD_WorldToMainCamera", camera.camera.worldToCameraMatrix);

			Shader.SetGlobalTexture("UD_Fix", fixTexture);
			Shader.SetGlobalTexture("UD_Normals", normalsTexture);
			Shader.SetGlobalVector("UD_ScreenSize", new Vector4(normalsTexture.width, normalsTexture.height, 1f / normalsTexture.width, 1f / normalsTexture.height));
			Shader.SetGlobalFloat("UD_GlobalMipBias", manager.globalMipBias);

			if (camera.isStereo) {

				viewProjInverse[0] = camera.GetViewProjInverse(Camera.StereoscopicEye.Left);
				viewProjInverse[1] = camera.GetViewProjInverse(Camera.StereoscopicEye.Right);

				cameraToWorld[0] = camera.GetViewInverse(Camera.StereoscopicEye.Left);
				cameraToWorld[1] = camera.GetViewInverse(Camera.StereoscopicEye.Right);

			} else {
				viewProjInverse[0] = camera.GetViewProjInverse();
				cameraToWorld[0] = camera.GetViewInverse();
			}

			Shader.SetGlobalMatrixArray("UD_viewProjInverse", viewProjInverse);
			Shader.SetGlobalMatrixArray("UD_CameraToWorld", cameraToWorld);
			
			LightProbeUtility.SetAmbientSHCoefficients();

			ResourceManager.ToggleKeyword("UD_PER_DECAL_PROBES", manager.perDecalLightProbes);

			OnPrepareFrame();

			lastPrepareTime = (float) sw.ElapsedTicks / Stopwatch.Frequency;

		}

		protected virtual void OnPrepareFrame() {
		}



		public void FinalizeFrame() {

			OnFinalizeFrame();

		}

		protected virtual void OnFinalizeFrame() {
		}

		protected CommandBuffer CreateCommandBuffer(string name) {

			var cb = new CommandBuffer();
			cb.name = name;

			commandBuffers.Add(cb);

			return cb;

		}

		protected Material CreateMaterial(string shaderName, bool silent = true) {

			var shader = Shader.Find(shaderName);

			if (shader == null) {
				if (!silent) {
					UnityEngine.Debug.Log("Failed to create material with shader " + shaderName);
				}
				return null;
			}

			var material = new Material(shader);

			materials.Add(material);

			return material;

		}

		protected RenderTexture CreateRenderTexture(string name, int depth, RenderTextureFormat format) {

			int w, h;
			var dimension = camera.GetScreenSize(out w, out h);

			var r = new RenderTexture(w, h, depth, format);

			r.dimension = dimension;

			if (r.dimension == TextureDimension.Tex2DArray) {
				r.volumeDepth = 2;
			}

			r.name = name;

			textures.Add(r);

			return r;

		}

		public void CreateTextures() {

			ReleaseTextures();

			normalsTexture = CreateRenderTexture("UD_Normals", 0, RenderTextureFormat.ARGBHalf);

			tempTexture = CreateRenderTexture("UD_Temp", 0, RenderTextureFormat.ARGBHalf);

			fixTexture = CreateRenderTexture("UD_Fix", 0, RenderTextureFormat.ARGB32);

			depthTarget = CreateRenderTexture("UD_DepthTarget", 16, RenderTextureFormat.R8);

			prepareTargets[0] = new RenderTargetIdentifier(tempTexture);
			prepareTargets[1] = new RenderTargetIdentifier(fixTexture);

			prepareTargets2[0] = new RenderTargetIdentifier(normalsTexture);
			prepareTargets2[1] = new RenderTargetIdentifier(fixTexture);

			OnAfterCreateTextures();

		}

		protected virtual void OnAfterCreateTextures() {

		}

		public bool requiresRestart {
			get {

				if (textures.Count == 0) return false;

				int w, h;
				var dimension = camera.GetScreenSize(out w, out h);

				var e = textures[0];

				if (e.width != w || e.height != h || e.dimension != dimension) return true;

				return false;

			}

		}

		void ReleaseTextures() {

			foreach (var e in textures) {
				if (e == null) continue;
				e.Release();
			}
			textures.Clear();

		}

	}

}
