// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Depth/WorldNormals_BackFace"
{
	Properties
	{
		_MainTex("", 2D) = "white" {}
		_Cutoff("", Float) = 0.5
		_Color("", Color) = (1,1,1,1)
		//_GlassCustomDepthNormal("", int) = 1
	}

	SubShader{
		
		Tags { "RenderType" = "Opaque" }
		
		Pass {
		
		Cull Front
		ZWrite On
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
						
		/*
		Depth_1_0_UnityDepthCameras,									DEPTH_CAM_UNTY
		Depth_2_0_CustomDepthCameras,									DEPTH_CAM_CUSTOM
		
		FrontDepth_1_0_StandardDepth,									DEPTH_FRONT_SHADER_OFF
		FrontDepth_2_0_ShaderDepth										DEPTH_FRONT_SHADER_ON
		
		Normal_1_0_WorldCameraShader,									NORMAL_WORLD_CAM_SHADER
		Normal_1_1_CustomScreenNormalCameras,							NORMAL_CAM_CUSTOM
		Normal_2_0_UnityScreenNormalCameras								NORMAL_CAM_UNITY
		*/
		
		#pragma multi_compile DEPTH_FRONT_SHADER_ON DEPTH_FRONT_SHADER_OFF
		#pragma multi_compile NORMAL_CAM_CUSTOM NORMAL_CAM_UNITY NORMAL_WORLD_CAM_SHADER
		#pragma multi_compile DEPTH_CAM_CUSTOM DEPTH_CAM_UNTY
		
		struct v2f
		{
			float4 pos : SV_POSITION;
			float4 nz : TEXCOORD0;
		};
		
		int _GlassCustomDepthNormal;
		
		v2f vert(appdata_base v)
		{
			v2f o;

			UNITY_INITIALIZE_OUTPUT(v2f, o);
			
#if UNITY_VERSION >= 540
		o.pos = UnityObjectToClipPos(v.vertex); 
#else
		o.pos = UnityObjectToClipPos(v.vertex);
#endif
			
			#if NORMAL_CAM_CUSTOM
				o.nz.xyz = COMPUTE_VIEW_NORMAL;
			#endif	
			
			#if NORMAL_CAM_UNITY
				o.nz.xyz = COMPUTE_VIEW_NORMAL;
			#endif
			
			#if NORMAL_WORLD_CAM_SHADER
				o.nz.xyz = 0;
			#endif
			
			//o.nz.xyz = normalize( mul(float4(v.normal, 0.0), _Object2World).xyz);
			//o.nz.xyz = mul((float3x3)_Object2World, SCALED_NORMAL);
			
			#if DEPTH_CAM_CUSTOM
				o.nz.w = COMPUTE_DEPTH_01;
			#endif
			
			#if NORMAL_CAM_UNITY
				o.nz.w = COMPUTE_DEPTH_01;
			#endif

			#if NORMAL_WORLD_CAM_SHADER
				o.nz.w = COMPUTE_DEPTH_01;
			#endif
			
			return o;
		}
		
		float4 frag(v2f i) : SV_Target
		{
			#if NORMAL_CAM_CUSTOM
				return float4(i.nz.x, i.nz.y, i.nz.z, i.nz.w);
				//return float4(1,0,0,1);
			#endif
			
			#if NORMAL_CAM_UNITY
				return EncodeDepthNormal (i.nz.w, i.nz.xyz);
				//return float4(0,1,0,1);
			#endif
			
			#if NORMAL_WORLD_CAM_SHADER
				return float4(i.nz.w,0,0,1);
				//return float4(0,0,1,1);
			#endif

				//return float4(1,1,1,1);
		}
		
		ENDCG
		}
	}
	Fallback Off
}
