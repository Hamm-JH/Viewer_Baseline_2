// UNITY_SHADER_NO_UPGRADE

half4 _Workflow;

#define METALLIC _Workflow.x
#define ROUGHNESS _Workflow.y
#define SEPARATE_SR _Workflow.z

struct appdata_t2 {
	uint instanceID: SV_InstanceID;
	uint vertexID: SV_VertexID;
	float4 vertex: POSITION;
};

struct v2f {

	uint instanceID: SV_InstanceID;

	float4 pos: SV_POSITION;
	float4 data: TEXCOORD0;
	float4 screenCoord: TEXCOORD1;
	float4 up: TEXCOORD4;

	#if defined(UD_FORWARD) && defined(UD_LIT) && !defined(UD_MOBILE)
	uint4 lightIndices: TEXCOORD5;
	#endif

	UNITY_VERTEX_OUTPUT_STEREO
};

#include "UD_Common.cginc"

#if defined(UD_LIT) && defined(UD_FORWARD)

half4 UD_MainLightDir;
half4 UD_MainLightColor;
half UD_LightCount;

#ifndef UD_MOBILE

CBUFFER_START(UD_LIGHTS)
float4 UD_LightPos[UD_MAX_LIGHTS];
half4 UD_LightColor[UD_MAX_LIGHTS];
half4 UD_LightDir[UD_MAX_LIGHTS];
half4 UD_LightData[UD_MAX_LIGHTS];
float4x4 UD_WorldToLight[UD_MAX_LIGHTS];
CBUFFER_END

#endif

#ifdef UD_SRP

half GetLightAtten(float3 worldPos, float4 pos, half4 dir, float4x4 worldToLight, float4 data) {

	float d = distance(pos.xyz, worldPos);
	float dd = d * d;
	float r = rcp(dd);

	half factor = dd * (1 / pos.w / pos.w);
	half smoothFactor = saturate(1 - factor * factor);
	smoothFactor = smoothFactor * smoothFactor;

	r *= smoothFactor;

	if (dir.w >= 0) {
		half SdotL = dot(dir.xyz, normalize(worldPos.xyz - pos.xyz));
		half atten = saturate(SdotL * data.x + data.y);
		r *= atten * atten;
	}

	return r;
}

#else

sampler2D UD_Attenuation;
sampler2D UD_Cookie;

half GetLightAtten(float3 worldPos, float4 pos, half4 dir, float4x4 worldToLight, float4 data) {

	float4 lightCoord = mul(worldToLight, float4(worldPos.xyz, 1)); 
	
	float r = tex2Dlod(UD_Attenuation, float4(dot(lightCoord.xyz, lightCoord.xyz).xx, 0, 0)).r;

	if (dir.w >= 0) {
		r *= (lightCoord.z > 0.1) * tex2Dlod(UD_Cookie, float4(lightCoord.xy / lightCoord.w + 0.5, 0, 0)).r;
	}	

	return r;
}

#endif
#endif

float4x4 UD_viewProjInverse[2];
float4x4 UD_WorldToMainCamera;
float4 UD_FrustumPlanes[6];
float4 UD_Data;

uint outside(float3 p, uint index) {
	float3 n = abs(UD_FrustumPlanes[index].xyz);
	float r = dot(0, n);
	float s = dot(float4(p, 1.0f), UD_FrustumPlanes[index]);
	return (s + r) < 0.0f;
}

v2f vert(appdata_t2 v) {

	v2f o = (v2f) 0;

	UNITY_SETUP_INSTANCE_ID(v);

	UNITY_TRANSFER_INSTANCE_ID(v, o);

	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	uint i; 
	uint id = v.instanceID;
	float3 size = _Size[id];
	v.vertex.xyz *= sign(size);

	#if defined(UD_FORWARD) && defined(UD_LIT) && !defined(UD_MOBILE)

	float s = 0.5;
	float3 p[8] = {
		mul(unity_ObjectToWorld, float4(s - 1, s - 1, s - 1, 1)).xyz,
		mul(unity_ObjectToWorld, float4(s, s - 1, s - 1, 1)).xyz,
		mul(unity_ObjectToWorld, float4(s - 1, s, s - 1, 1)).xyz,
		mul(unity_ObjectToWorld, float4(s - 1, s - 1, s, 1)).xyz,
		mul(unity_ObjectToWorld, float4(s, s - 1, s, 1)).xyz,
		mul(unity_ObjectToWorld, float4(s, s, s - 1, 1)).xyz,
		mul(unity_ObjectToWorld, float4(s - 1, s, s, 1)).xyz,
		mul(unity_ObjectToWorld, float4(s, s, s, 1)).xyz,
	};

	UNITY_LOOP
	for(uint j = 0; j < 6; j++) {
		uint r = 1;
		for(uint i = 0; i < 8; i++) {
			r = r && outside(p[i], j);
		}
		UNITY_BRANCH
		if(r) return o;
	}

	#endif

	o.up.xyz = normalize(mul(unity_ObjectToWorld, float3(0, 1, 0)));
	o.up.w = 0;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.screenCoord = ComputeScreenPos(o.pos);
	o.data.x = _CreateTime[id];
	o.data.y = _AtlasIndexOffset[id];

	#if defined(UD_FORWARD) && defined(UD_LIT)

	#if defined(UD_MOBILE)

	// do nothing?

	#else

	float3 decalPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
	float weights[4] = (float[4]) 0;
	uint r[4] = (uint[4]) 0;
	uint count = 0;

	UNITY_LOOP
	for(i = 0; i < (uint) UD_LightCount; i++) {

		float4 lightPos = UD_LightPos[i];
		float3 lightColor = UD_LightColor[i].rgb;

		float d = distance(lightPos.xyz, decalPos);
		float weight = (1 - saturate(d / lightPos.w)) * length(lightColor);

		if(count > 2 && weights[2] > weight) continue;

		bool first = count == 0 || weights[0] < weight;
		bool second = count == 1 || weights[1] < weight;
		bool third = count == 2 || weights[2] < weight;

		if(count < 3) count++;

		if(first) {
			weights[3] = weights[2];
			r[3] = r[2];
			weights[2] = weights[1];
			r[2] = r[1];
			weights[1] = weights[0];
			r[1] = r[0];
			weights[0] = weight;
			r[0] = i;
		} else if(second) {
			weights[3] = weights[2];
			r[3] = r[2];
			weights[2] = weights[1];
			r[2] = r[1];
			weights[1] = weight;
			r[1] = i;
		} else if(third) {
			weights[3] = weights[2];
			r[3] = r[2];
			weights[2] = weight;
			r[2] = i;
		}
		 
	}

	o.lightIndices = uint4(count, r[0], r[1], r[2]);

	#endif
	#endif
	
	return o;
}

struct Fragment {

	float2 screenCoord;
	float4 uv;
	float4 dd;

	half3 diffuse;

	half opacity;

	float3 worldPos;
	float3 worldNormal;

	half3 emission;

#ifdef UD_LIT
		
	half3 specular;
	half smoothness;
	half3 normal;

	#ifdef UD_DEFERRED
	half originalSmoothness;
	#endif

	#if defined(UD_FORWARD) && !defined(UD_MOBILE)
	uint4 lightIndices;
	#endif

#endif

};

half3 ProcessSMSR(half k, float4 uv, float4 dd, out half smoothness, inout half3 col) {

	half4 specularSmoothness = tex2DAutoColor(_SMSR, uv, dd);
	half3 specular = specularSmoothness.xyz * _SM * k;

	smoothness = lerp(specularSmoothness.a, tex2DAutoColor(_SeparateSR, uv, dd).r, SEPARATE_SR);

	smoothness *= _SR;

	smoothness = lerp(smoothness, 1 - smoothness, ROUGHNESS);

	smoothness *= k;

	if(METALLIC) {
		half metallic = specular.r;
		specular = lerp(unity_ColorSpaceDielectricSpec.rgb, col.rgb, metallic);
		half oneMinusReflectivity = OneMinusReflectivityFromMetallic(metallic);
		col *= oneMinusReflectivity;
	}
	 
	return specular;

}

Fragment PrepareFragment(v2f i) {
	  
	Fragment r;

	UNITY_INITIALIZE_OUTPUT(Fragment, r);

	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

	r.screenCoord = i.screenCoord.xy / i.screenCoord.w;

	#if defined(UD_FORWARD) && defined(UD_LIT) && !defined(UD_MOBILE)
	r.lightIndices = i.lightIndices;
	#endif

	float2 screenPos = r.screenCoord.xy;
	#ifdef UNITY_UV_STARTS_AT_TOP
	screenPos.y = 1 - screenPos.y;
	#endif
	#ifdef UNITY_SINGLE_PASS_STEREO
	if(unity_StereoEyeIndex == 0) {
		screenPos.x *= 2;
	} else {
		screenPos.x = (screenPos.x - 0.5) * 2;
	}
	#endif	

	float rawDepth = tex2D(_CameraDepthTexture, r.screenCoord).r;
	#if defined(SHADER_API_OPENGL) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLES)
	rawDepth = rawDepth * 2 - 1;
	#endif
	float4 clipPos = float4(screenPos.x * 2 - 1, screenPos.y * 2 - 1, rawDepth, 1);
	float4x4 m = UD_viewProjInverse[unity_StereoEyeIndex];
	float4 d = mul(m, clipPos);
	r.worldPos = d.xyz / d.w;

	float3 objPos = mul(unity_WorldToObject, float4(r.worldPos, 1)).xyz;

	r.uv = GetUV(i, objPos, r.screenCoord, /* out */ r.dd);

	clip(0.5 - abs(objPos.xyz));
	
	half4 enc = UNITY_SAMPLE_SCREENSPACE_TEXTURE(UD_Normals, r.screenCoord);

	#if !defined(UD_MOBILE)
	half2 fix = UNITY_SAMPLE_SCREENSPACE_TEXTURE(UD_Fix, r.screenCoord).rg;
	r.dd = lerp(r.dd, 0, fix.r);
	#endif
	
	#ifdef UD_DEFERRED
	r.originalSmoothness = enc.w;
	#endif

	r.worldNormal = lerp(normalize(enc.xyz * 2.0 - 1.0), i.up, _NormalSmooth);

	half k_angle = checkNormal(r.worldNormal, i.up);

	half4 mainTex = tex2DAutoColor(_MainTex, r.uv, r.dd);
	half4 color = GetColor(_Color);

	half mask = lerp(tex2DAuto(_Mask, r.uv, r.dd).r, mainTex.a * color.a, _AlbedoOpacity);

	clip(mask - _MaskThreshold);

	half k = saturate((mask - _MaskThreshold) / _MaskSmoothing) * k_angle;

	r.diffuse = lerp(mainTex.rgb, _Color, _ColorLerp) * color;

	k *= 1 - any(_Lifetime) * pow(saturate((_Time.y - i.data.x) / _Lifetime), _LifetimePow);
	
	r.opacity = k;

	#ifdef UD_LIT

	half3 worldTangent = normalize(mul((half3x3) unity_ObjectToWorld, half3(1, 0, 0)));
	half3 worldBinormal = normalize(cross(r.worldNormal, worldTangent)) * -1;

	half3x3 t2w;

	t2w[0] = half3(worldTangent.x, worldBinormal.x, r.worldNormal.x);
	t2w[1] = half3(worldTangent.y, worldBinormal.y, r.worldNormal.y);
	t2w[2] = half3(worldTangent.z, worldBinormal.z, r.worldNormal.z);

	half3 normalMap = UnpackScaleNormal(tex2DAuto(_BumpMap, r.uv, r.dd), _NormalScale);

	r.normal = normalize(mul(t2w, normalMap).xyz);

	r.specular = ProcessSMSR(k, r.uv, r.dd, /* out */ r.smoothness, /* inout */ r.diffuse);

	#endif

	#ifdef _EMISSION
	half3 emission = tex2DAutoColor(_EmissionTex, r.uv, r.dd).rgb * GetColor(_EmissionColor.rgb) * _EmissionColor.a;
	r.emission = emission;

	#endif

	#ifdef UD_LIT

	#ifdef UD_PER_DECAL_PROBES 
	unity_SHAr = UNITY_ACCESS_INSTANCED_PROP(Props, _SHAr);
	unity_SHAg = UNITY_ACCESS_INSTANCED_PROP(Props, _SHAg);
	unity_SHAb = UNITY_ACCESS_INSTANCED_PROP(Props, _SHAb);
	unity_SHBr = UNITY_ACCESS_INSTANCED_PROP(Props, _SHBr);
	unity_SHBg = UNITY_ACCESS_INSTANCED_PROP(Props, _SHBg);
	unity_SHBb = UNITY_ACCESS_INSTANCED_PROP(Props, _SHBb);
	unity_SHC = UNITY_ACCESS_INSTANCED_PROP(Props, _SHC);
	#else
	unity_SHAr = UD_SHAr;
	unity_SHAg = UD_SHAg;
	unity_SHAb = UD_SHAb;
	unity_SHBr = UD_SHBr;
	unity_SHBg = UD_SHBg;
	unity_SHBb = UD_SHBb;
	unity_SHC = UD_SHC;
	#endif
	
	half occlusion = tex2DAutoColor(_OcclusionMap, r.uv, r.dd).r * _Occlusion;

	half3 ambient = occlusion * r.diffuse * ShadeSH9(half4(r.normal, 1));

	r.emission += ambient;

	#endif

	return r;

}

#if defined(UD_LIT) && defined(UD_FORWARD)

sampler2D UD_ScreenSpaceShadowMask;

#ifdef UD_MOBILE

half3 ForwardLighting(uint instanceID, Fragment f) {

	UnityLight light;
	UNITY_INITIALIZE_OUTPUT(UnityLight, light);

	UnityIndirect indirect;
	UNITY_INITIALIZE_OUTPUT(UnityIndirect, indirect);

	half3 worldViewDir = normalize(_WorldSpaceCameraPos - f.worldPos);
	half oneMinusReflectivity = 0;

	f.diffuse = EnergyConservationBetweenDiffuseAndSpecular(f.diffuse, f.specular, oneMinusReflectivity);

	light.dir = -UD_MainLightDir.xyz;
	light.color = UD_MainLightColor.rgb;

	#ifdef UD_SRP
	half shadow = tex2D(UD_ScreenSpaceShadowMask, f.screenCoord);
	#else
	half shadow = tex2D(UD_ScreenSpaceShadowMask, f.screenCoord);
	#endif

	shadow = lerp(shadow, 1, 1 - UD_MainLightColor.w);

	half3 r = shadow * UNITY_BRDF_PBS(f.diffuse, f.specular, oneMinusReflectivity, f.smoothness, f.normal, worldViewDir, light, indirect);

	UnityGIInput d;
	d.worldPos = f.worldPos;
	d.worldViewDir = worldViewDir;
	d.probeHDR[0] = unity_SpecCube0_HDR;
	d.boxMin[0].w = 1;

	half blendDistance = unity_SpecCube1_ProbePosition.w;
	#ifdef UNITY_SPECCUBE_BOX_PROJECTION
	d.probePosition[0] = unity_SpecCube0_ProbePosition;
	d.boxMin[0].xyz = unity_SpecCube0_BoxMin - half4(blendDistance, blendDistance, blendDistance, 0);
	d.boxMax[0].xyz = unity_SpecCube0_BoxMax + half4(blendDistance, blendDistance, blendDistance, 0);
	#endif

	Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(f.smoothness, worldViewDir, f.normal, f.specular);

	half3 env0 = UnityGI_IndirectSpecular(d, 1, g);

	light.color = half3(0, 0, 0);
	light.dir = half3(0, 1, 0);

	indirect.diffuse = 0;
	indirect.specular = env0;

	half3 reflection = UNITY_BRDF_PBS(0, f.specular, oneMinusReflectivity, f.smoothness, f.normal, -worldViewDir, light, indirect).rgb;

	r.rgb += reflection;

	return r;

}

#else

half3 ForwardLighting(uint instanceID, Fragment f) {

	UnityLight light;
	UNITY_INITIALIZE_OUTPUT(UnityLight, light);

	UnityIndirect indirect;
	UNITY_INITIALIZE_OUTPUT(UnityIndirect, indirect);

	half3 worldViewDir = normalize(_WorldSpaceCameraPos - f.worldPos);
	half oneMinusReflectivity = 0;
	
	f.diffuse = EnergyConservationBetweenDiffuseAndSpecular(f.diffuse, f.specular, oneMinusReflectivity);

	light.dir = -UD_MainLightDir.xyz;
	light.color = UD_MainLightColor.rgb;

	#ifdef UD_SRP
	half shadow = tex2D(UD_ScreenSpaceShadowMask, f.screenCoord);
	#else
	half shadow = tex2D(UD_ScreenSpaceShadowMask, f.screenCoord);
	#endif

	shadow = lerp(shadow, 1, 1 - UD_MainLightColor.w);

	half3 r = shadow * UNITY_BRDF_PBS(f.diffuse, f.specular, oneMinusReflectivity, f.smoothness, f.normal, worldViewDir, light, indirect);

	uint n = min(3, f.lightIndices[0]);

	[loop]
	for(uint i = 0; i < n; i++) {

		uint index = f.lightIndices[i + 1];

		float4 pos = UD_LightPos[index];
		half4 dir = UD_LightDir[index];

		light.dir = normalize(pos.xyz - f.worldPos);
		light.color.rgb = UD_LightColor[index].rgb * GetLightAtten(f.worldPos, pos, dir, UD_WorldToLight[index], UD_LightData[index]);

		r += UNITY_BRDF_PBS(f.diffuse, f.specular, oneMinusReflectivity, f.smoothness, f.normal, worldViewDir, light, indirect);

	}

	UnityGIInput d;
	d.worldPos = f.worldPos;
	d.worldViewDir = worldViewDir;
	d.probeHDR[0] = unity_SpecCube0_HDR;
	d.boxMin[0].w = 1;

	half blendDistance = unity_SpecCube1_ProbePosition.w;
	#ifdef UNITY_SPECCUBE_BOX_PROJECTION
	d.probePosition[0] = unity_SpecCube0_ProbePosition;
	d.boxMin[0].xyz = unity_SpecCube0_BoxMin - half4(blendDistance, blendDistance, blendDistance, 0);
	d.boxMax[0].xyz = unity_SpecCube0_BoxMax + half4(blendDistance, blendDistance, blendDistance, 0);
	#endif

	Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(f.smoothness, worldViewDir, f.normal, f.specular);

	half3 env0 = UnityGI_IndirectSpecular(d, 1, g);

	light.color = half3(0, 0, 0);
	light.dir = half3(0, 1, 0);

	indirect.diffuse = 0;
	indirect.specular = env0; 

	half3 reflection = UNITY_BRDF_PBS(0, f.specular, oneMinusReflectivity, f.smoothness, f.normal, -worldViewDir, light, indirect).rgb;	

	r.rgb += reflection;

	return r;

}

#endif

#endif