// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/UltimateDecals/Prepare" {

SubShader {

	Pass{

		Name "Gradient"

		ZWrite Off
		ZTest Always
		Cull Off

		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		#if defined(SHADER_API_GLES3) || defined(SHADER_API_GLES)
		#define UD_MOBILE
		#endif

		struct appdata {
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 offset: TEXCOORD1;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
		float4 UD_ScreenSize;

		v2f vert(appdata v) {
			v2f o;

			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = float4(v.vertex.xy, 0.0, 0.5);

			o.uv = UnityStereoTransformScreenSpaceTex(v.texcoord);

			#if UNITY_UV_STARTS_AT_TOP
			o.uv.y = 1 - o.uv.y;
			#endif

			o.offset = float4(UD_ScreenSize.z, 0, 0, UD_ScreenSize.w);

			return o;
		}		

		#ifdef UD_MOBILE
		void frag(v2f i, out half4 gradient: SV_Target0) {
		#else
		void frag(v2f i, out half4 gradient: SV_Target0, out fixed4 br: SV_Target1)  {
		#endif

			float2 uv0 = i.uv;

			float2 uv1 = uv0 + i.offset.xy;
			float2 uv2 = uv0 + i.offset.zw;
			float2 uv3 = uv0 - i.offset.xy;
			float2 uv4 = uv0 - i.offset.zw;

			float depth0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv0);

			#ifdef UNITY_REVERSED_Z
			clip(depth0 - 0.0001);
			#else
			clip(0.9999 - depth0);
			#endif

			float depth1 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv1); 
			float depth2 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv2);
			float depth3 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv3);
			float depth4 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv4);

			gradient = half4(depth1 - depth0, depth2 - depth0, depth3 - depth0, depth4 - depth0);

			#ifndef UD_MOBILE

			float2 diffx = abs(float2(depth1 - depth0, depth3 - depth0));
			float2 diffy = abs(float2(depth2 - depth0, depth4 - depth0));

			float mindx = min(diffx.x, diffx.y);
			float maxdx = max(diffx.x, diffx.y);
			float mindy = min(diffy.x, diffy.y);
			float maxdy = max(diffy.x, diffy.y);

			float ax = maxdx / mindx;
			float bx = maxdx - mindx;
			float ay = maxdy / mindy;
			float by = maxdy - mindy;

			float a = max(ax, ay); 
			float b = max(bx, by);

			br = fixed4(a > 5 && b > 0.0001, 0, 0, 0);

			#endif

		}
	ENDCG
}

Pass {

	Name "Deferred"

	ZWrite Off
	ZTest Always
	Cull Off

	CGPROGRAM
	#pragma vertex vert 
	#pragma fragment frag
	#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO
	#pragma multi_compile _ _NORMALMAP
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float4 offset: TEXCOORD1;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	sampler2D _CameraGBufferTexture1;
	sampler2D _CameraDepthTexture;
	float4 UD_ScreenSize;

	v2f vert (appdata v) {

		v2f o;

		UNITY_SETUP_INSTANCE_ID(v);

		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.pos = float4(v.vertex.xy, 0.0, 0.5);
		o.uv = UnityStereoTransformScreenSpaceTex(v.texcoord);

		#if UNITY_UV_STARTS_AT_TOP
		o.uv.y = 1 - o.uv.y;   
		#endif

		o.offset = float4(UD_ScreenSize.z, 0, 0, UD_ScreenSize.w);

		return o;
	}

	#include "../../Shaders/Includes/UD_Utils.cginc"

	#ifdef _NORMALMAP
		sampler2D _CameraGBufferTexture2;
		float _PerPixelNormalWeight;
	#endif    


	void frag(v2f i, out half4 outNormal: SV_Target) {

		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

		float edge;
		float3 resultNormal = RestoreSurfaceNormal(i.uv, unity_StereoEyeIndex, edge); 
		 
		#ifdef _NORMALMAP
		float3 pixelNormal = tex2D(_CameraGBufferTexture2, i.uv).rgb * 2 - 1;
		resultNormal = lerp(resultNormal, pixelNormal, _PerPixelNormalWeight);
		#endif

		float a = tex2D(_CameraGBufferTexture1, i.uv).a;
	
		outNormal = half4(resultNormal * 0.5 + 0.5, a);

	}
	ENDCG
}

Pass {

		Name "Forward"

		ZWrite Off
		ZTest Always
		Cull Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		#pragma multi_compile _ _NORMALMAP
		#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

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

		sampler2D _CameraDepthTexture;
		float4 _CameraDepthTexture_ST;
		float4 UD_ScreenSize;

		#ifdef _NORMALMAP
		sampler2D _CameraDepthNormalsTexture;
		float _PerPixelNormalWeight;
		float4x4 UD_CameraToWorld[2];
		#endif

		#include "../../Shaders/Includes/UD_Utils.cginc"

		v2f vert(appdata v) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = float4(v.vertex.xy, 0.0, 0.5);
			o.uv = UnityStereoTransformScreenSpaceTex(TRANSFORM_TEX(v.texcoord, _CameraDepthTexture));
			#if UNITY_UV_STARTS_AT_TOP
			o.uv.y = 1 - o.uv.y;
			#endif

			return o;
		}

		half4 frag(v2f i): SV_Target {

			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

			#ifdef _NORMALMAP

				float depth0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
				#ifdef UNITY_REVERSED_Z
				clip(depth0 - 0.0001);
				#else
				clip(0.9999 - depth0);
				#endif

				float4 dn = tex2D(_CameraDepthNormalsTexture, i.uv);
				float3 viewNormal = DecodeViewNormalStereo(dn);
				float3 resultNormal = mul((float3x3) (UD_CameraToWorld[unity_StereoEyeIndex]), viewNormal);
			#else
				float edge;
				float3 resultNormal = RestoreSurfaceNormal(i.uv, unity_StereoEyeIndex, edge);
			#endif
  
			return half4(resultNormal * 0.5 + 0.5, 0);

		}
	ENDCG
	}

	Pass {

		Name "SRP" 

		ZWrite Off
		ZTest Always
		Cull Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO
		#include "UnityCG.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float3 texcoord : TEXCOORD0;
			uint id: SV_VertexID;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos: SV_POSITION;
			float4 objPos: TEXCOORD1;
			float2 uv: TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		sampler2D _CameraDepthTexture;
		float4 _CameraDepthTexture_ST;
		float4 UD_ScreenSize;

		#include "../../Shaders/Includes/UD_Utils.cginc"

		v2f vert(appdata v) {
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = float4(v.vertex.xy, 0.0, 0.5);
			o.objPos = v.vertex;
			o.uv = UnityStereoTransformScreenSpaceTex(TRANSFORM_TEX(v.texcoord, _CameraDepthTexture));
			#if UNITY_UV_STARTS_AT_TOP
			o.uv.y = 1 - o.uv.y;
			#endif
			return o;
		}

		float4 frag(v2f i) : SV_Target0 {

			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

			float edge;
			float3 resultNormal = RestoreSurfaceNormal(i.uv, unity_StereoEyeIndex, edge);

			return float4(resultNormal * 0.5 + 0.5, edge);

		}

		ENDCG
	}

	

}
Fallback Off
}
