// UNITY_SHADER_NO_UPGRADE

sampler2D _UD_Gradient;
float4x4 UD_viewProjInverse[2];

float3 RestoreSurfaceNormal(float2 uv0, uint unity_StereoEyeIndex, out float edge) {

	float depth0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv0);

	#ifdef UNITY_REVERSED_Z
	clip(depth0 - 0.0001);
	#else
	clip(0.9999 - depth0);
	#endif

	float2 uv1 = uv0 + float2(UD_ScreenSize.z, 0);
	float2 uv2 = uv0 + float2(0, UD_ScreenSize.w);
	float2 uv3 = uv0 - float2(UD_ScreenSize.z, 0);
	float2 uv4 = uv0 - float2(0, UD_ScreenSize.w);

	float4 g0 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_UD_Gradient, uv0);

	float4 g1 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_UD_Gradient, uv1);
	float4 g2 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_UD_Gradient, uv2);
	float4 g3 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_UD_Gradient, uv3);
	float4 g4 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_UD_Gradient, uv4);

	float rightDiff = abs(g1.x - g0.x); 
	float leftDiff = abs(g3.z - g0.z);
	float upDiff = abs(g2.y - g0.y);
	float downDiff = abs(g4.w - g0.w);

	float depth1 = depth0 + g0.x;
	float depth2 = depth0 + g0.y;

	if(leftDiff < rightDiff - 0.000001) {
		depth1 = depth0 - g0.z;
		g1 = g3;
	}
	if(downDiff < upDiff - 0.000001) {
		depth2 = depth0 - g0.w;
		g2 = g4;
	}

	float h = max(leftDiff, rightDiff) - min(leftDiff, rightDiff);
	float v = max(upDiff, downDiff) - min(upDiff, downDiff);
	float hv = max(h, v);

	edge = 0; //hv >	0.00005;
		
	#ifdef UNITY_UV_STARTS_AT_TOP
	uv0.y = 1 - uv0.y;
	uv1.y = 1 - uv1.y;
	uv2.y = 1 - uv2.y;
	#endif

#ifdef UNITY_SINGLE_PASS_STEREO
	if(unity_StereoEyeIndex == 0) {
		uv0.x *= 2;
		uv1.x *= 2;
		uv2.x *= 2;
	} else {
		uv0.x = (uv0.x - 0.5) * 2;
		uv1.x = (uv1.x - 0.5) * 2;
		uv2.x = (uv2.x - 0.5) * 2;
	}
#endif	

#if defined(SHADER_API_OPENGL) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLES)
	depth0 = depth0 * 2 - 1;
	depth1 = depth1 * 2 - 1;
	depth2 = depth2 * 2 - 1;
#endif

	float4 clipPos0 = float4(uv0.x * 2 - 1, uv0.y * 2 - 1, depth0, 1);
	float4 clipPos1 = float4(uv1.x * 2 - 1, uv1.y * 2 - 1, depth1, 1);
	float4 clipPos2 = float4(uv2.x * 2 - 1, uv2.y * 2 - 1, depth2, 1);

	float4x4 m = UD_viewProjInverse[unity_StereoEyeIndex];

	float4 d0 = mul(m, clipPos0);
	float4 d1 = mul(m, clipPos1);
	float4 d2 = mul(m, clipPos2);

	float3 worldPos0 = d0.xyz / d0.w;
	float3 worldPos1 = d1.xyz / d1.w;
	float3 worldPos2 = d2.xyz / d2.w;

	return normalize(cross(worldPos1 - worldPos0, worldPos0 - worldPos2));

}