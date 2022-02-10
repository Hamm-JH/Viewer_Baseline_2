// UNITY_SHADER_NO_UPGRADE

Shader "UltimateDecals/Unlit" { 

	Properties 	{  

		// Atlas

		_AtlasData("Atlas Data", Vector) = (1,1,0,0)
		_ScaleOffset("Scale/Offset", Vector) = (1,1,0,0)

		// Projection

		_AngleLimit("Angle Limit", Range(1,89)) = 60
		_AngleSmoothing("Angle Smoothing", Range(0,0.5)) = 0.02
		_NormalSmooth("Normal Smooth", Range(0,1)) = 0.2

		// Rendering

		_Blend("", Float) = 1
		_SrcBlend("", Float) = 5
		_DstBlend("", Float) = 10

		_StencilMode("Stencil Mode", Float) = 0
		_StencilRef("Stencil Ref", Float) = 0
		_StencilMask("Stencil Mask", Float) = 8

		// Advanced

		_AlbedoOpacity("Albedo Control", Range(0,1)) = 0
		_Lifetime("Lifetime", Float) = 0
		_LifetimePow("Litetime Pow", Float) = 1
		_FadeData("FadeData", Vector) = (0,0,0,0)
		_MipBias("Mip Bias", Range(-10,10)) = 0

		// Common maps 

		_MainTex("Texture", 2D) = "white" {}
		[HDR] _Color("Color", Color) = (1,1,1,1)
		_ColorLerp("ColorLerp", Range(0,2)) = 0

		_Mask("Mask", 2D) = "white" {}
		_MaskThreshold("Height Threshold", Range(0,1)) = 0.5
		_MaskSmoothing("Height Smoothing", Range(0,1)) = 0.5 

		_EmissionTex("Emission Map", 2D) = "white" {}
		[HDR] _EmissionColor("Emission Color", Color) = (0,0,0,0)

		[HideInInspector] _ScreenSpaceShadowmapTexture("Shadowmap", 2D) = "white" { }
	
	}
		  
	SubShader {

		Tags{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"DisableBatching" = "True"
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
		}  
		
		LOD 100

		Pass {

			Name "Forward"  
			Tags { 
			}

			ZWrite Off
			Cull Front
			ZTest Greater 
			Blend [_SrcBlend][_DstBlend] 
			ColorMask RGB

			Stencil {
				Ref [_StencilRef]
				Comp Equal
				Pass Keep
				Fail Keep
				ZFail Keep
				ReadMask [_StencilMask]
			}

			CGPROGRAM

			#pragma target 4.0
			#pragma vertex vert
			#pragma fragment frag

			#pragma exclude_renderers d3d9
			#pragma exclude_renderers d3d11_9x

			#pragma multi_compile_instancing 
			#pragma multi_compile_fog 
			#pragma multi_compile _ UNITY_COLORSPACE_GAMMA
			#pragma multi_compile _ UNITY_HDR_ON

			#pragma multi_compile _ _EMISSION

			#include "UnityCG.cginc"   
			#include "AutoLight.cginc"
			#include "UnityStandardCore.cginc"

			#define UD_UNLIT 

			#if defined(SHADER_API_GLES3) || defined(SHADER_API_GLES)
			#define UD_MOBILE
			#endif

			#include "Includes/UD_PBR.cginc"

			#ifdef SHADER_API_D3D11
			[earlydepthstencil]
			#endif
			half4 frag(v2f i): SV_Target {

				UNITY_SETUP_INSTANCE_ID(i);  

  				Fragment f = PrepareFragment(i);

				half3 c = f.diffuse + f.emission;

				c = ApplyFog(c, f.worldPos);
				
				return half4(c, f.opacity); 

			}

	ENDCG
	}
	
	Pass {

			Name "Preview"
			Tags{
				"PreviewType" = "Plane"
			}  

			ZWrite Off
			Cull Off
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma exclude_renderers d3d9
			#pragma exclude_renderers d3d11_9x
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "UnityStandardCore.cginc" 

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv: TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			sampler2D _Mask;
			sampler2D _EmissionTex;
			half _MaskThreshold;
			half _MaskSmoothing; 

			fixed4 frag(v2f i) : SV_Target{

				half4 col = tex2D(_MainTex, i.uv) * _Color;
				half mask = tex2D(_Mask, i.uv).r;
				half3 emission = tex2D(_EmissionTex, i.uv) * _EmissionColor * 2;

				col.rgb += emission;

				half k = col.a * step(_MaskSmoothing, mask);

				half4 bg = lerp(0, half4(0,0,0,1), any(_EmissionColor.rgb));

				return lerp(bg, col, k);
			}

		ENDCG
		}
	
	}

	CustomEditor "Bearroll.UltimateDecals.UD_UnlitShaderEditor"
	Fallback "Unlit/Texture"
}