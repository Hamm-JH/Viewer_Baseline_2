// writes decal smoothness to gbuffer

Shader "Hidden/UltimateDecals/Finalize" {
	SubShader{

		Pass{

		ZWrite Off
		ZTest Always
		Cull Off
		ColorMask A
		Blend Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO
		};
		float4 _CameraNormalsTexture_ST;

		v2f vert(appdata v) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = float4(v.vertex.xy, 0.0, 0.5);
			o.uv = UnityStereoTransformScreenSpaceTex(TRANSFORM_TEX(v.texcoord,_CameraNormalsTexture));
			#if UNITY_UV_STARTS_AT_TOP
			o.uv.y = 1 - o.uv.y;
			#endif
			return o;
		}

		sampler2D _UD_Temp;

		void frag(v2f i, out half4 outSpecular: SV_Target1) {

			float4 temp = tex2D(_UD_Temp, i.uv);

			clip(temp.g - 0.001);

			float smoothness = temp.r;

			outSpecular = half4(0,0,0, smoothness);

		}
		ENDCG
		}

	}
	Fallback Off
}
