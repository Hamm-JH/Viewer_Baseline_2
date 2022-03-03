// UNITY_SHADER_NO_UPGRADE

Shader "UltimateDecals/Lit" { 

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

		// PBR

		_Workflow("_Workflow", Vector) = (1, 0, 0, 0)

		_SMSR("Specular/Metallic(RGB) Smoothness/Roughness (A)", 2D) = "white" {}
		_SeparateSR("Separate Smoothness/Roughness", 2D) = "white" {}
		_SM("Specular/Metallic", Range(0,1)) = 0.00                                  
		_SR("Smoothness/Roughness", Range(0,1)) = 0.15

		_OcclusionMap("Occlusion (R)", 2D) = "white" {}
		_Occlusion("Occlusion", Range(0,1)) = 1
				
		[Normal] _BumpMap("Normal Map", 2D) = "bump" {}
		_NormalScale("Normal Scale", Range(0,2)) = 1 

		_BlendingFactors("Blending Factors", Vector) = (1,1,1,1)

		[HideInInspector] _ScreenSpaceShadowmapTexture("Shadowmap", 2D) = "white" { }
	
	}
		  
	SubShader {

		Tags{
			"Queue" = "Geometry" 
			"RenderType" = "Transparent"
			"DisableBatching" = "True"
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
		}  
		 
		LOD 100		
		
		Pass {    

			Name "Forward"     
			Tags{
				"Queue" = "Geometry"
				"RenderType" = "Transparent"
				"DisableBatching" = "True"
				"IgnoreProjector" = "True"
				"PreviewType" = "Plane"
				"LightMode" = "ForwardBase"
			}

			ZWrite Off
			Cull Front
			ZTest Greater
			Blend [_SrcBlend] [_DstBlend]

			Stencil{
				Ref[_StencilRef]
				Comp Equal
				Pass Keep
				Fail Keep
				ZFail Keep
				ReadMask[_StencilMask]
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
			#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

			#pragma multi_compile _ _EMISSION
			#pragma multi_compile _ UD_PER_DECAL_PROBES

			#if defined(SHADER_API_GLES3) || defined(SHADER_API_GLES)
			#define UD_MOBILE
			#define SHADOWS_SCREEN
			#define UNITY_NO_SCREENSPACE_SHADOWS
			#endif

			#include "UnityCG.cginc"  
			#include "AutoLight.cginc"
			#include "UnityStandardCore.cginc"

			#define UD_LIT
			#define UD_FORWARD

			#include "Includes/UD_PBR.cginc"     

			#ifdef SHADER_API_D3D11  
			[earlydepthstencil]  
			#endif
			half4 frag(v2f i): SV_Target {
				     
				UNITY_SETUP_INSTANCE_ID(i);

				Fragment f = PrepareFragment(i);   

				half3 c = 0;

				c = ForwardLighting(i.instanceID, f);
			 
				c += f.emission;
				 
				c = ApplyFog(c, f.worldPos);

				return half4(c, f.opacity);

			}

	ENDCG  
	}

	Pass {

		Name "SRP"
		
			Tags{
				"Queue" = "Geometry"
				"RenderType" = "Transparent"
				"DisableBatching" = "True"
				"IgnoreProjector" = "True"
				"PreviewType" = "Plane"
			}

			ZWrite Off
			Cull Front
			ZTest Greater
			Blend [_SrcBlend] [_DstBlend]

			Stencil{
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
			#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

			#pragma multi_compile _ _EMISSION
			#pragma multi_compile _ UD_PER_DECAL_PROBES 

			#include "UnityCG.cginc"  
			#include "AutoLight.cginc"
			#include "UnityStandardCore.cginc"          

			#define UD_LIT
			#define UD_FORWARD
			#define UD_SRP

			#if defined(SHADER_API_GLES3) || defined(SHADER_API_GLES)
			#define UD_MOBILE
			#endif

			#include "Includes/UD_PBR.cginc"

			#ifdef SHADER_API_D3D11  
			[earlydepthstencil]
			#endif
			half4 frag(v2f i): SV_Target{

				UNITY_SETUP_INSTANCE_ID(i);

				Fragment f = PrepareFragment(i);

				half3 c = 0;

				c = ForwardLighting(i.instanceID, f);

				c += f.emission;

				c = ApplyFog(c, f.worldPos);

				return half4(c, f.opacity);

			}

		ENDCG
	}
	
	Pass {

		Name "Deferred"
		Tags {
			"LightMode" = "Deferred"
		}

		Stencil{
			Ref[_StencilRef]
			Comp Equal
			Pass Keep 
			Fail Keep        
			ZFail Keep
			ReadMask[_StencilMask]    
		}

		ZWrite Off	
		Cull Front
		ZTest Greater
		Blend 0 [_SrcBlend] [_DstBlend]
		Blend 1 [_SrcBlend] [_DstBlend]
		Blend 2 [_SrcBlend] [_DstBlend]
		Blend 3 [_SrcBlend] [_DstBlend]
		Blend 4 Off
		ColorMask RGBA 0
		ColorMask RGB 1
		ColorMask RGB 2
		ColorMask RGB 3
		ColorMask RG 4

		CGPROGRAM

		#pragma target 4.0
		#pragma vertex vert
		#pragma fragment frag

		#pragma exclude_renderers d3d9
		#pragma exclude_renderers d3d11_9x

		#pragma multi_compile_instancing    
		#pragma multi_compile _ UNITY_COLORSPACE_GAMMA
		#pragma multi_compile _ UNITY_HDR_ON
		#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

		#pragma multi_compile _ _EMISSION
		#pragma multi_compile _ UD_PER_DECAL_PROBES

		#include "UnityCG.cginc"
		#include "UnityStandardCore.cginc"

		#define UD_LIT
		#define UD_DEFERRED

		#include "Includes/UD_PBR.cginc"    

		#ifdef SHADER_API_D3D11 
		[earlydepthstencil]
		#endif			
		void frag(v2f i, out half4 outDiffuse: SV_Target0, out half4 outSpecular: SV_Target1, out half4 outNormal: SV_Target2, out half4 outEmission: SV_Target3, out half4 outTemp: SV_Target4) {
	
			UNITY_SETUP_INSTANCE_ID(i);

			Fragment f = PrepareFragment(i);

			#if !defined(UNITY_HDR_ON)  
			f.emission = exp2(-f.emission);
			#endif

			outDiffuse = half4(f.diffuse, f.opacity * _BlendingFactors.x);
			outSpecular = half4(f.specular, f.opacity * _BlendingFactors.z);
			outEmission = half4(f.emission, f.opacity * _BlendingFactors.x);
			outNormal = half4(0.5 + f.normal* 0.5, f.opacity * _BlendingFactors.y);
			outTemp = half4(lerp(f.originalSmoothness, f.smoothness, f.opacity * _BlendingFactors.w), 1, 0, 0);
				
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

			fixed4 frag(v2f i): SV_Target {

				float mask = tex2D(_Mask, i.uv).r;

				clip(mask - _MaskThreshold);

				mask = saturate((mask - _MaskThreshold) / _MaskSmoothing);

				half4 col = tex2D(_MainTex, i.uv) * _Color;
				
				half3 emission = tex2D(_EmissionTex, i.uv) * _EmissionColor * 2;

				col.rgb += emission;

				col.rgb = pow(col.rgb, 0.5);

				half k = col.a * mask;
	
				half4 bg = lerp(0, half4(0,0,0,1), any(_EmissionColor.rgb));

				return lerp(bg, col, k);
			}

		ENDCG
		}
	
	}

	CustomEditor "Bearroll.UltimateDecals.UD_LitShaderEditor"
	Fallback "Unlit/Texture"
}