using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_ForwardRenderer: UD_LegacyRenderer {

		public CommandBuffer shadowBuffer { get; private set; }

		Light shadowBufferLight;

		LightEvent lightEvent {
			get {
				#if UD_MOBILE
				return LightEvent.AfterShadowMap;
				#else
				return LightEvent.AfterScreenspaceMask;
				#endif
			}
		}

		public UD_ForwardRenderer(UD_Camera camera): base(camera) {

			shadowBuffer = CreateCommandBuffer("UD_ShadowmaskRef");

			#if UD_MOBILE
			shadowBuffer.SetGlobalTexture("_ShadowMapTexture", BuiltinRenderTextureType.CurrentActive);
			#else
			shadowBuffer.ClearRenderTarget(false, false, Color.black);
			shadowBuffer.SetGlobalTexture("UD_ScreenSpaceShadowMask", BuiltinRenderTextureType.CurrentActive);
			#endif

		}

		protected override void SetBuffers() {
			
			AddCommandBuffer(CameraEvent.AfterForwardOpaque, bufferPrepare);

			foreach (var e in bufferByPassLit) { 
				AddCommandBuffer(CameraEvent.AfterForwardOpaque, e.Value);
			}

			foreach (var e in bufferByPassUnlit) {
				AddCommandBuffer(CameraEvent.AfterForwardOpaque, e.Value);
			}

		}

		protected override void OnBeforeClean() {

			base.OnBeforeClean();
			
			if (shadowBufferLight == null) return;

			if (shadowBuffer == null) return;

			shadowBufferLight.RemoveCommandBuffer(lightEvent, shadowBuffer);
			shadowBufferLight = null;

		}

		public override void BuildPrepareBuffer(CommandBuffer commandBuffer) {

			commandBuffer.Clear();

			var depth = camera.isUsingMSAA ? new RenderTargetIdentifier(depthTarget) : BuiltinRenderTextureType.CurrentActive;

			#if UD_MOBILE
			commandBuffer.SetRenderTarget(prepareTargets[0], depth);
			#else
			commandBuffer.SetRenderTarget(prepareTargets, depth);
			#endif

			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			commandBuffer.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, prepareMaterial, 0, 0);

			commandBuffer.SetRenderTarget(normalsTexture, depth);
			commandBuffer.SetGlobalTexture("_UD_Gradient", tempTexture);
			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			commandBuffer.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, prepareMaterial, 0, 2);

			commandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);

		}

		protected override void OnPrepareFrame() {

			if (camera.forwardDepthNormals == UD_ForwardNormalsSource.CameraDepthNormals && (camera.camera.depthTextureMode & DepthTextureMode.DepthNormals) == 0) {
				camera.camera.depthTextureMode |= DepthTextureMode.DepthNormals;
			}

			ResourceManager.ToggleKeyword(prepareMaterial, "_NORMALMAP", camera.forwardDepthNormals == UD_ForwardNormalsSource.CameraDepthNormals);

			manager.forwardLightManager.PrepareFrame(camera);

			var light = manager.forwardLightManager.mainDirectionalLight;

			if (light == null) return;

			if (light.bakingOutput.lightmapBakeType == LightmapBakeType.Baked) return;

			shadowBufferLight = light;

			light.AddCommandBuffer(lightEvent, shadowBuffer);

		}

		protected override void OnFinalizeFrame() {

			if (shadowBufferLight == null) return;

			if (shadowBuffer == null) return;

			shadowBufferLight.RemoveCommandBuffer(lightEvent, shadowBuffer);
			shadowBufferLight = null;

		}

	}

}