// UNITY_SHADER_NO_UPGRADE

sampler2D _Mask;
sampler2D UD_Normals;
sampler2D UD_Fix;
sampler2D _CameraDepthTexture;
sampler2D _CameraDepthNormalsTexture;
sampler2D _SMSR;
sampler2D _SeparateSR;
samplerCUBE UD_GlobalReflections;
float _ColorLerp;
float _NormalK;
float _MaskThreshold;
float _MaskSmoothing;
float _SR;
float _SM;
float4 _OriginPos;
float4 _ScaleOffset;
float _Lifetime;
float _LifetimePow;
float4 _FadeData;
float _NormalScale;
float _NormalSmooth;
float _AngleLimit;
float _AngleSmoothing; 
float _AlbedoOpacity;
float _Occlusion;
float4 _AtlasData;
float4 UD_ScreenSize;
float UD_GlobalMipBias;
float4 _BlendingFactors;

#ifdef _EMISSION
sampler2D _EmissionTex;
#endif

#define BATCH_DECAL_COUNT 64
#define UD_MAX_LIGHTS 8

#define _AtlasX _AtlasData.x
#define _AtlasY _AtlasData.y

UNITY_INSTANCING_BUFFER_START(Props)
UNITY_DEFINE_INSTANCED_PROP(float4, _SHAr)
UNITY_DEFINE_INSTANCED_PROP(float4, _SHAg)
UNITY_DEFINE_INSTANCED_PROP(float4, _SHAb)
UNITY_DEFINE_INSTANCED_PROP(float4, _SHBr)
UNITY_DEFINE_INSTANCED_PROP(float4, _SHBg)
UNITY_DEFINE_INSTANCED_PROP(float4, _SHBb)
UNITY_DEFINE_INSTANCED_PROP(float4, _SHC)
UNITY_INSTANCING_BUFFER_END(Props)

CBUFFER_START(UD_VERTEX)
#if defined(UD_FORWARD) && defined(UD_LIT)
float4 UD_DecalPos[BATCH_DECAL_COUNT];
#endif
float4 _Size[BATCH_DECAL_COUNT];
float _AtlasIndexOffset[BATCH_DECAL_COUNT];
float _CreateTime[BATCH_DECAL_COUNT];
CBUFFER_END

float _NormalSmoothEdge;
float _UnityAmbientStrength = 1;
float _MipBias;
float4 UD_SHAr;
float4 UD_SHAg;
float4 UD_SHAb;
float4 UD_SHBr;
float4 UD_SHBg;
float4 UD_SHBb;
float4 UD_SHC;
float UD_near;

half4 tex2DAuto(sampler2D tex, float4 uv, float4 dd) {
	return tex2D(tex, uv.xy, dd.xy, dd.zw);
}

half4 tex2DAutoColor(sampler2D tex, float4 uv, float4 dd) {
	half4 r = tex2DAuto(tex, uv, dd);
	#ifdef UD_GAMMA
	//r.rgb = pow(r.rgb, 2.2);
	#endif
	return r;
}

float3 GetColor(float3 color) {
	#ifdef UD_GAMMA
	//color.rgb = pow(color.rgb, 2.2);  
	#endif	
	return color;
}

float4 GetColor(float4 color){ 
	#ifdef UD_GAMMA
	//color.rgb = pow(color.rgb, 2.2);  
	#endif	
	return color;
}

float4 GetUV(v2f i, float3 objPos, float2 screenCoord, out float4 dd) {

	float2 pos = clamp(objPos.xz, float2(-1,-1), float2(1,1));

	float4 uv = float4(0.5 + pos, 0, 0);

	float atlasX = 1.0 / _AtlasX;
	float atlasY = 1.0 / _AtlasY;
	float atlasIndex = 0;

	float k = pow(2, _MipBias + UD_GlobalMipBias);
	float k2 = 1;

	float2 uvdd = objPos.xz * float2(atlasX, atlasY) * float2(_ScaleOffset.x, _ScaleOffset.y);

	dd.xy = ddx(uvdd);
	dd.zw = ddy(uvdd);

	dd *= k * k2;
	
	atlasIndex += round(i.data.y);

	atlasIndex = lerp(0, atlasIndex, any(min(_AtlasX, _AtlasY)));

	atlasIndex = clamp(atlasIndex, 0, _AtlasX * _AtlasY - 1);

	uv.x = atlasX * (atlasIndex % _AtlasX) + (uv.x * atlasX * _ScaleOffset.x) % atlasX;
	uv.y = atlasY * floor(atlasIndex / _AtlasX) + (uv.y * atlasY * _ScaleOffset.y) % atlasY;

	return uv;

}

float checkNormal(float3 worldNormal, float3 decalNormal) {

	float v = saturate(dot(decalNormal, worldNormal));
	float k_spread = _AngleSmoothing;
	float k_init = clamp(1 - _AngleLimit / 90, 0.01 + k_spread, 0.99 - k_spread);
	float k_min = k_init - k_spread;
	float k_max = k_init + k_spread;
	float k_angle = pow(saturate((v - k_min) / (k_max - k_min)), 1);
	clip(k_angle - 0.01);

	return k_angle;

}

float3 ApplyFog(float3 color, float3 worldPos) {

	float d = distance(_WorldSpaceCameraPos, worldPos);

#if defined(FOG_LINEAR)
	float unityFogFactor = d * unity_FogParams.z + unity_FogParams.w;
#elif defined(FOG_EXP)
	float unityFogFactor = unity_FogParams.y * d; unityFogFactor = exp2(-unityFogFactor);
#elif defined(FOG_EXP2)
	float unityFogFactor = unity_FogParams.x * d; unityFogFactor = exp2(-unityFogFactor * unityFogFactor);
#else
	float unityFogFactor = 1;
#endif

	return lerp(color, unity_FogColor, 1 - unityFogFactor);

}

float ProcessHeightAndMask(float4 uv, float4 dd) {

	float mask = tex2DAuto(_Mask, uv, dd).r;

	float maskMin = _MaskThreshold;
	float maskMax = saturate(_MaskThreshold + _MaskSmoothing);

	clip(mask - maskMin);

	return saturate((mask - maskMin) / (maskMax - maskMin));

}