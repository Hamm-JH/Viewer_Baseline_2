using UnityEngine;
using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_DeferredRenderer: UD_LegacyRenderer {

		public Material fixMaterial { get; private set; }

		public CommandBuffer bufferFix { get; private set; }

		public RenderTargetIdentifier[] deferredTargets = {
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.GBuffer1,
			BuiltinRenderTextureType.GBuffer2,
			BuiltinRenderTextureType.CameraTarget,
			0
		};

		public RenderTargetIdentifier[] deferredTargets4 = {
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.GBuffer1,
			BuiltinRenderTextureType.GBuffer2, 
			BuiltinRenderTextureType.CameraTarget,
		};

		public UD_DeferredRenderer(UD_Camera camera) : base(camera) {
			
			fixMaterial = CreateMaterial("Hidden/UltimateDecals/Finalize");

			bufferFix = CreateCommandBuffer("UD_Finalize");

			FillFixBuffer();

		}

		protected override void SetBuffers() {
			
			AddCommandBuffer(CameraEvent.BeforeReflections, bufferPrepare);

			foreach (var e in bufferByPassLit) {
				AddCommandBuffer(CameraEvent.BeforeReflections, e.Value);
			}

			AddCommandBuffer(CameraEvent.BeforeReflections, bufferFix);

			foreach (var e in bufferByPassUnlit) {
				AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, e.Value);
			}

		}

		void FillFixBuffer() {

			// applying smoothness
			bufferFix.Clear();
			bufferFix.SetRenderTarget(deferredTargets4, BuiltinRenderTextureType.CurrentActive);
			bufferFix.SetGlobalTexture("_UD_Temp", tempTexture);
			bufferFix.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, fixMaterial, 0, 0);

		}

		public override void BuildPrepareBuffer(CommandBuffer commandBuffer) {

			commandBuffer.Clear();

			deferredTargets[3] = camera.camera.allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3;
			deferredTargets[4] = tempTexture;
			deferredTargets4[3] = deferredTargets[3];

			// Depth gradient + discontinuities
			commandBuffer.SetRenderTarget(prepareTargets, BuiltinRenderTextureType.CurrentActive);
			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			commandBuffer.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, prepareMaterial, 0, 0);

			// Normals + storing original smoothness
			commandBuffer.SetRenderTarget(normalsTexture, BuiltinRenderTextureType.CurrentActive);
			commandBuffer.SetGlobalTexture("_UD_Gradient", tempTexture);
			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			commandBuffer.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, prepareMaterial, 0, 1);


		}

		public override void StartPassLitBuffer(CommandBuffer commandBuffer) {
			
			commandBuffer.SetRenderTarget(deferredTargets, BuiltinRenderTextureType.CameraTarget);

		}

		protected override void OnPrepareFrame() {

			ResourceManager.ToggleKeyword(prepareMaterial, "_NORMALMAP", camera.normalSmoothing > 0);
			prepareMaterial.SetFloat("_PerPixelNormalWeight", camera.normalSmoothing);

			manager.deferredCullingManager.RequestCullingLight(camera);

		}

	}

}