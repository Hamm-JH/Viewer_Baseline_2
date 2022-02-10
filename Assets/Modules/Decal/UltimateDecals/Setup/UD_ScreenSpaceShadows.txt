Shader "Hidden/UltimateDecals/ScreenSpaceShadows"
{
    SubShader
    {
        Tags{ "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}

        HLSLINCLUDE

        #define _MAIN_LIGHT_SHADOWS_CASCADE

        #pragma prefer_hlslcc gles
        #pragma exclude_renderers d3d11_9x
        #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
        TEXTURE2D_ARRAY_FLOAT(_CameraDepthTexture);
		#else
        TEXTURE2D_FLOAT(_CameraDepthTexture);
		#endif
        SAMPLER(sampler_CameraDepthTexture);

        struct Attributes
        {
            float4 positionOS   : POSITION;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            half4  positionCS   : SV_POSITION;
            half4  uv           : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        Varyings Vertex(Attributes input)
        {
            Varyings output;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            output.positionCS = float4(input.positionOS.xy, 0.0, 0.5);

			float4 projPos = output.positionCS * 0.5;
			projPos.xy = projPos.xy + projPos.w;

            output.uv.xy = UnityStereoTransformScreenSpaceTex(input.texcoord);
			output.uv.zw = projPos.xy;

            return output;
        }

		float4x4 UD_viewProjInverse[2];

        half4 Fragment(Varyings input) : SV_Target
        {
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

			input.uv.y = 1 - input.uv.y;
			
            float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, input.uv.xy).r;

			#ifdef UNITY_REVERSED_Z
			clip(depth - 0.0001);
			#else
			clip(0.9999 - depth);
			#endif

			#ifdef UNITY_UV_STARTS_AT_TOP
			input.uv.y = 1 - input.uv.y;
			#endif

			#ifdef UNITY_SINGLE_PASS_STEREO
			if(unity_StereoEyeIndex == 0) {
				input.uv.x *= 2;
			} else {
				input.uv.x = (input.uv.x - 0.5) * 2;
			}
			#endif

			#if defined(SHADER_API_OPENGL) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLES)
			depth = depth * 2 - 1;
			#endif

			float4 clipPos = float4(input.uv.x * 2 - 1, input.uv.y * 2 - 1, depth, 1);
			float4 d = mul(UD_viewProjInverse[unity_StereoEyeIndex], clipPos);
			float3 worldPos = d.xyz / d.w;			

            float4 coords = TransformWorldToShadowCoord(worldPos);

            ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
            half4 shadowParams = GetMainLightShadowParams();

            half r = SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), coords, shadowSamplingData, shadowParams, false);

			clip(0.99 - r);

			return r;

        }

        ENDHLSL

        Pass
        {
            Name "ScreenSpaceShadows"
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma multi_compile _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #pragma vertex   Vertex
            #pragma fragment Fragment
            ENDHLSL
        }
    }
}
