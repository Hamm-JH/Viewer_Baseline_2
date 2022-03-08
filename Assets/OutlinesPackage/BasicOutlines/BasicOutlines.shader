
Shader "Outlines/BasicOutlines" {
    Properties{
         [Header(Main Settings)]
           [Space]
        _Thickness("Base Thickness", Range(0, 0.25)) = 1 // The amount to extrude the outline mesh
        _Color("Color", Color) = (1, 1, 1, 1) // The outline color
        _DepthOffset("Depth offset", Range(0, 1)) = 0 // An offset to the clip space Z, pushing the outline back
        [KeywordEnum(NML,POS)] _OUTLINE("OUTLINE MODE", Float) = 0

        [Header(Precalculated Normals)]
          [Space]

        [KeywordEnum(NONE,UVBAKED,TEXBAKED)] _BAKEDNORMALS("Baked Normals", Float) = 0

        // If enabled, this shader will use "smoothed" normals stored in TEXCOORD to extrude along
        //[Toggle(USE_PRECALCULATED_OUTLINE_NORMALS)]_PrecalculateNormals("Use normals stored in UV channel ", Float) = 0
        [KeywordEnum(UV2,UV3,UV4)] _UvChannel("UV Channel With Baked Normals", int) = 2
            [Space]
        //[Toggle(USE_BAKEDNORMAL_TEX)] _Is_BakedNormal ("Use Baked Normal", Float ) = 0
        _BakedNormal ("Texture Baked Normals", 2D) = "white" {}

        [Header(Outline width dependent on camera position)]
          [Space]
        [Toggle(CONSTANT_OUTLINE_WIDTH)]_ConstantOutlineWidth("Outline width change over distance", Float) = 0
        _Limiter ("Max thickness", Range(0, 0.35)) = 0
        //_ChangeMul("Distance Change Multiplier (Speed)", Range(0,1.5)) = 0
        _StartChangeDist("Base Thickness distance", float) = 0
        _FinalChangeDist("Max thickness distance", float) = 0

    }
    SubShader{
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"  "LightMode" = "SRPDefaultUnlit"}

        Pass {
            Name "BasicOutlines"


            // Cull front faces
            Cull Front

            HLSLPROGRAM
            
  
            // Standard URP requirements
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // Register our material keywords
            #pragma shader_feature CONSTANT_OUTLINE_WIDTH

            #pragma multi_compile _UVCHANNEL_UV2 _UVCHANNEL_UV3 _UVCHANNEL_UV4
            #pragma multi_compile _OUTLINE_NML _OUTLINE_POS
            #pragma multi_compile _BAKEDNORMALS_NONE _BAKEDNORMALS_UVBAKED _BAKEDNORMALS_TEXBAKED

            // Register our functions
            #pragma vertex Vertex
            #pragma fragment Fragment

            // Include our logic file
           // Include helper functions from URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"


            // Data from the meshes
            struct Attributes {
                float4 positionOS       : POSITION; // Position in object space
                float3 normalOS         : NORMAL; // Normal vector in object space
                float4 tangent          : TANGENT;
                float2 texcoord0        : TEXCOORD0;

            #ifdef _BAKEDNORMALS_UVBAKED
            // Calculated "smooth" normals to extrude along in object space
                #if _UVCHANNEL_UV2
                    float3 smoothNormalOS   : TEXCOORD2; 
                #elif _UVCHANNEL_UV3
                    float3 smoothNormalOS   : TEXCOORD3; 
                #elif _UVCHANNEL_UV4
                    float3 smoothNormalOS   : TEXCOORD4; 
                #endif
            #endif

            UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            // Output from the vertex function and input to the fragment function
            struct VertexOutput {
                float4 positionCS   : SV_POSITION; // Position in clip space
                float4 positionNDC  : TEXCOORD5;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO 
            };
            
            CBUFFER_START(UnityPerMaterial)
            // Properties
            float _Thickness;
            float4 _Color;
            float _DepthOffset;
            float _Limiter;
            float _StartChangeDist;
            float _FinalChangeDist;
            sampler2D _BakedNormal;
            float4 _BakedNormal_ST;
            float _Opacity;
            float _Scale;
            CBUFFER_END

            VertexOutput Vertex(Attributes input) {
                VertexOutput output = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID(input);
	            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                float2 uv0 = input.texcoord0;

                float3 normalOS;
            #ifdef _BAKEDNORMALS_UVBAKED
                normalOS = input.smoothNormalOS;
            #elif _BAKEDNORMALS_NONE
                normalOS = input.normalOS;
            #elif _BAKEDNORMALS_TEXBAKED
                 float3 normalDir = 	TransformObjectToWorldNormal(input.normalOS);
                float3 tangentDir = normalize( mul( unity_ObjectToWorld, float4( input.tangent.xyz, 0.0 ) ).xyz );
                float3 bitangentDir = normalize(cross(normalDir, tangentDir) * input.tangent.w);
                float3x3 tangentTransform = float3x3( tangentDir, bitangentDir,normalDir);

                float4 _BakedNormal_var = (tex2Dlod(_BakedNormal,float4(TRANSFORM_TEX(uv0, _BakedNormal),0.0,0)) * 2 - 1);
                float3 _BakedNormalDir = normalize(mul(_BakedNormal_var.rgb, tangentTransform));
                normalOS = _BakedNormalDir;
            #endif
            
            #ifdef CONSTANT_OUTLINE_WIDTH
            
                //Camera position in world
            	float3 targetPos = _WorldSpaceCameraPos;
            
            	//Object position in world
            	float3 objectPos = mul (unity_ObjectToWorld, input.positionOS).xyz;
            
            	//Vertex offset from center of the object
            	float3 offset = input.positionOS.xyz;
            
            	//Distance from the object to the camera
            	float dist = distance(objectPos, targetPos + offset);  
                
                //normalization the distance value between _StartChangeDist and _FinalChangeDist
                float distNormalized = (dist - _StartChangeDist)/(_FinalChangeDist-_StartChangeDist);
            
                if(distNormalized < 0) distNormalized = 0;
            
            	//Real distance (to the plane 90 degrees to camera forward and object)
            	_Thickness = lerp(_Thickness,_Limiter,distNormalized)  ;//maybe --> * cos(degrees(angle)) ??
            
            #endif

            #ifdef _OUTLINE_NML
                 float3 posOS = input.positionOS.xyz + normalOS * _Thickness;
            #elif _OUTLINE_POS
                float signVar = dot(normalize(input.positionOS),normalize(input.positionOS))<0 ? -1 : 1;
                float3 posOS = input.positionOS.xyz + signVar*normalize(input.positionOS).xyz * _Thickness;
            #endif

                // Convert this position to world and clip space
                output.positionCS = GetVertexPositionInputs(posOS).positionCS;
                output.positionNDC = GetVertexPositionInputs(posOS).positionNDC;
                

                float depthOffset = _DepthOffset;
                float4 _ClipCameraPos = mul(UNITY_MATRIX_VP, float4(_WorldSpaceCameraPos.xyz, 1));

                // If depth is reversed on this platform, reverse the offset
            #ifdef UNITY_REVERSED_Z
                depthOffset = -depthOffset;
            #endif
                output.positionCS.z += depthOffset* _ClipCameraPos.z;
            
                return output;
            }
            
            float4 Fragment(VertexOutput input) : SV_Target {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                return _Color;
            }
  

            ENDHLSL
        }
    }
}