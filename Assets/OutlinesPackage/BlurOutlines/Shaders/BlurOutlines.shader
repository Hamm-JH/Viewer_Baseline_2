Shader "Hidden/BlurOutlines"
{
	Properties
	{
         _Intensity("_Intensity",float) = 0.83
		 _Color("_Color",Color) = (0,0,0,1)
		 _Width("_Width",float) = 1
	}

    HLSLINCLUDE

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		TEXTURE2D(_CameraColorTexture);
		SAMPLER(sampler_CameraColorTexture);

		TEXTURE2D(_OutlineObjectsTex);
		SAMPLER(sampler_OutlineObjectsTex);
		float2 _OutlineObjectsTex_TexelSize;

		TEXTURE2D(_BluerredObjectsTex);
		SAMPLER(sampler_BluerredObjectsTex);
		float2 _BluerredObjectsTex_TexelSize;

		float4 _Color;
		float _Intensity;
		int _Width;
		float _GaussSamples[32];

		struct Attributes
		{
			float4 positionOS : POSITION;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		
		struct Varyings
		{
			float4 positionCS : SV_POSITION;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		Varyings VertexSimple(Attributes input)
		{
			Varyings output = (Varyings)0;

			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
			output.uv = input.uv;

			return output;
		}

		float CalcIntensityN0(float2 uv, float2 offset, int k)
		{
			return SAMPLE_TEXTURE2D(_OutlineObjectsTex, sampler_OutlineObjectsTex, uv + k * offset).r * _GaussSamples[k];
		}

		float CalcIntensityN1(float2 uv, float2 offset, int k)
		{
			return SAMPLE_TEXTURE2D(_OutlineObjectsTex, sampler_OutlineObjectsTex, uv - k * offset).r * _GaussSamples[k];
		}

		float CalcIntensity(float2 uv, float2 offset)
		{
			float intensity = 0;

			// Accumulates horizontal or vertical blur intensity for the specified texture position.
			// Set offset = (tx, 0) for horizontal sampling and offset = (0, ty) for vertical.
			// 
			// NOTE: Unroll directive is needed to make the method function on platforms like WebGL 1.0 where loops are not supported.

			[unroll(32)]
			for (int k = 1; k <= _Width; ++k)
			{
				intensity += CalcIntensityN0(uv, offset, k);
				intensity += CalcIntensityN1(uv, offset, k);
			}

			intensity += CalcIntensityN0(uv, offset, 0);
			return intensity;
		}

		float CalcIntensityN0Blurred(float2 uv, float2 offset, int k)
		{
			return SAMPLE_TEXTURE2D(_BluerredObjectsTex, sampler_BluerredObjectsTex, uv + k * offset).r * _GaussSamples[k];
		}

		float CalcIntensityN1Blurred(float2 uv, float2 offset, int k)
		{
			return SAMPLE_TEXTURE2D(_BluerredObjectsTex, sampler_BluerredObjectsTex, uv - k * offset).r * _GaussSamples[k];
		}

		float CalcIntensityBlurred(float2 uv, float2 offset)
		{
			float intensity = 0;

			// Accumulates horizontal or vertical blur intensity for the specified texture position.
			// Set offset = (tx, 0) for horizontal sampling and offset = (0, ty) for vertical.
			// 
			// NOTE: Unroll directive is needed to make the method function on platforms like WebGL 1.0 where loops are not supported.

			[unroll(32)]
			for (int k = 1; k <= _Width; ++k)
			{
				intensity += CalcIntensityN0Blurred(uv, offset, k);
				intensity += CalcIntensityN1Blurred(uv, offset, k);
			}

			intensity += CalcIntensityN0Blurred(uv, offset, 0);
			return intensity;
		}


		float4 FragmentH(Varyings i) : SV_Target
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

			float2 uv = UnityStereoTransformScreenSpaceTex(i.uv);
			float intensity = CalcIntensity(uv, float2(_OutlineObjectsTex_TexelSize.x, 0));
			return float4(intensity, intensity, intensity, 1);
		}

		float4 FragmentV(Varyings i) : SV_Target
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

			float2 uv = UnityStereoTransformScreenSpaceTex(i.uv);
			
			float4 mask = SAMPLE_TEXTURE2D(_OutlineObjectsTex, sampler_OutlineObjectsTex, uv);//input.screenPos.xy / input.screenPos.w
			
            if (mask.r > 0)//input.positionCS.z
	        {
	    	    return SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);
	        }
			
			float intensity = CalcIntensityBlurred(uv, float2(0, _BluerredObjectsTex_TexelSize.y));

			intensity = _Intensity > 99 ? step(0.01, intensity) : intensity * _Intensity;

            //return SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv) + intensity * _Color;

			return float4(_Color.rgb, saturate(_Color.a * intensity)) + SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);
		}

	ENDHLSL

	// SM3.5+
	SubShader
	{
		Tags{ "RenderPipeline" = "UniversalPipeline" }

		//Cull Off
		//ZWrite Off
		//ZTest Always
		Lighting Off
		//Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Name "HPass"

			HLSLPROGRAM

			#pragma target 3.5
			#pragma multi_compile_instancing
			#pragma vertex VertexSimple
			#pragma fragment FragmentH

			ENDHLSL
		}

		Pass
		{
			Name "VPassBlend"
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM

			#pragma target 3.5
			#pragma multi_compile_instancing
			#pragma vertex VertexSimple
			#pragma fragment FragmentV

			ENDHLSL
		}
	}

}
