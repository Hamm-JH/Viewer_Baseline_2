Shader "Hidden/UltimateDecals/Culling" {
	SubShader {

		Tags { 
			"IgnoreProjector" = "True"
		}

		Pass {

			Tags { 
				"LightMode" = "Deferred"
				"Queue" = "Geometry-1500"
				"RenderType" = "Opaque"
				"DisableBatching" = "True"
			}

			ZWrite Off
			ZTest Always
			Cull Off

			CGPROGRAM
			#pragma target 4.0
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			struct appdata {
			};

			struct v2f {
				float4 pos: SV_POSITION;
			};

			v2f vert(appdata v) { 

				v2f o;

				o.pos = 0;

				return o;
			}

			half4 frag(v2f i): SV_Target {
				return 0;
			}

			ENDCG
		}

	}
	Fallback Off
}
