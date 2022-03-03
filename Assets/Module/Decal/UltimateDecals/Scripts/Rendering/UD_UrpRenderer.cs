#if UD_URP

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Bearroll.UltimateDecals {

	public class UD_UrpRenderer: UD_ScriptableRenderer {

		Material shadowCopyMaterial;
		RenderTexture shadowCopy;

		public UD_UrpRenderer(UD_Camera camera): base(camera) {

			var urp = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;

			urp.supportsCameraDepthTexture = true;

			shadowCopyMaterial = CreateMaterial("Hidden/UltimateDecals/ScreenSpaceShadows");

			var data = camera.camera.GetUniversalAdditionalCameraData();

			if (data != null) {
				data.requiresDepthOption = CameraOverrideOption.On;
				data.requiresDepthTexture = true;
			}

		}

		protected override void OnAfterCreateTextures() {

			shadowCopy = CreateRenderTexture("UD_ShadowCopy", 0, RenderTextureFormat.ARGBHalf);

		}

		public override void StartPassLitBuffer(CommandBuffer commandBuffer) {
			
			commandBuffer.SetGlobalTexture("UD_ScreenSpaceShadowMask", shadowCopy);

		}

		public override void BuildPrepareBuffer(CommandBuffer commandBuffer) {

			commandBuffer.Clear();

			if (shadowCopyMaterial != null) {

				commandBuffer.SetRenderTarget(shadowCopy, depthTarget);
				commandBuffer.ClearRenderTarget(false, true, Color.white);
				commandBuffer.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, shadowCopyMaterial, 0, 0);

			}

			commandBuffer.SetRenderTarget(prepareTargets, depthTarget);
			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			commandBuffer.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, prepareMaterial, 0, 0);

			commandBuffer.SetRenderTarget(normalsTexture, depthTarget);
			commandBuffer.SetGlobalTexture("_UD_Gradient", tempTexture);
			commandBuffer.ClearRenderTarget(false, true, Color.clear);
			commandBuffer.DrawMesh(ResourceManager.quadMesh, Matrix4x4.identity, prepareMaterial, 0, 3);

		}

		protected override void OnPrepareFrame() {
			camera.manager.forwardLightManager.PrepareFrame(camera);
		}

	}

}

#endif
