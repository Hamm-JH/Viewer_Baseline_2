#if UD_LWRP

using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;



namespace Bearroll.UltimateDecals {

	public class UD_LwrpRenderer: UD_ScriptableRenderer {

		public UD_LwrpRenderer(UD_Camera camera) : base(camera) {

			var lwrp = GraphicsSettings.renderPipelineAsset as LightweightRenderPipelineAsset;

			lwrp.supportsCameraDepthTexture = true;

		}

		public override void BuildPrepareBuffer(CommandBuffer commandBuffer) {

			commandBuffer.Clear(); 

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