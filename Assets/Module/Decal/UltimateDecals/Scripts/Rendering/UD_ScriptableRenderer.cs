#if UD_LWRP || UD_URP

#if UD_LWRP
using UnityEngine.Rendering.LWRP;
#else
using UnityEngine.Rendering.Universal;
#endif

using UnityEngine.Rendering;

namespace Bearroll.UltimateDecals {

	public class UD_ScriptableRenderer: UD_Renderer {

		public UD_ScriptableRenderer(UD_Camera camera): base(camera) {

		}

		public virtual void ExecutePrepass(ScriptableRenderContext context) {

			context.ExecuteCommandBuffer(bufferPrepare);

		}

		public virtual void ExecuteLitPass(ScriptableRenderContext context) {

			foreach (var e in bufferByPassLit) {
				context.ExecuteCommandBuffer(e.Value); 
			}

		}

		public virtual void ExecuteUnlitPass(ScriptableRenderContext context) {

			foreach (var e in bufferByPassUnlit) {
				context.ExecuteCommandBuffer(e.Value);
			}

		}

	}

}


#endif