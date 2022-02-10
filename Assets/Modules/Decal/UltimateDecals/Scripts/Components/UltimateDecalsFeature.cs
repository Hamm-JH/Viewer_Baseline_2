#if UD_URP || UD_LWRP

using UnityEngine;
using UnityEngine.Rendering;

#if UD_LWRP
using UnityEngine.Rendering.LWRP;
#else
using UnityEngine.Rendering.Universal;
#endif

namespace Bearroll.UltimateDecals {

	public class UltimateDecalsFeature: ScriptableRendererFeature {

		public class UD_PreparePass: ScriptableRenderPass {

			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
				
				var camera = UD_Camera.currentCamera;

				if (camera == null) return;

				ConfigureTarget(BuiltinRenderTextureType.CurrentActive, BuiltinRenderTextureType.CurrentActive);

			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

				var camera = UD_Camera.currentCamera;

				if (camera == null) return;

				camera.decalRenderer.ExecutePrepass(context);
			}

		}

		public class UD_RenderPass: ScriptableRenderPass {

			bool isLit;

			public UD_RenderPass(bool isLit) {
				this.isLit = isLit;
			}

			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

				var camera = UD_Camera.currentCamera;

				if (camera == null) return;

				if (isLit) {
					camera.decalRenderer.ExecuteLitPass(context);
				} else {
					camera.decalRenderer.ExecuteUnlitPass(context);
				}

			}

		}

		UD_PreparePass preparePass;
		UD_RenderPass renderPass1;
		UD_RenderPass renderPass2;

		public override void Create() {

			preparePass = new UD_PreparePass();
			preparePass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

			renderPass1 = new UD_RenderPass(true);
			renderPass1.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;

			renderPass2 = new UD_RenderPass(false);
			renderPass2.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {

			if (UD_Camera.currentCamera == null) return;

			renderer.EnqueuePass(preparePass);
			renderer.EnqueuePass(renderPass1);
			renderer.EnqueuePass(renderPass2);
		}

	}

}

#endif