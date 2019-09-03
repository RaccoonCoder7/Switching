// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Glass" {

	Properties{

		//	ALbedo
		_Color("Albedo Colour (RGBA)", Color) = (1,1,1,0)
		_MainTex("Albedo Texture (RGBA)", 2D) = "clear" {}
	_MainTexIntensity("Texture Colour (RGBA)", Color) = (1,1,1,0)
		_Opacity("Opacity", Range(0,1)) = 0.0
		_Alpha("Alpha", Range(0,1)) = 1.0
		//	Distortion
		[Toggle]_EnableDistortion("Enable Distortion (quality++)", int) = 1
		_DistortionIntensity("Distortion Intensity (Texture, Mesh Normal, Multiplier, Max)", vector) = (0.1, 0.1, 1.0, 1.0)
		_DistortionTexture("Distortion / Bump", 2D) = "bump" {}
	_DistortionEdgeBend("Distortion Edge Bend", float) = 0.3
		_DistortionDepthFade("Distortion Depth Fade", float) = 0.1
		[Toggle]_FlipUV_Grab("Flip Grab Refraction UV", int) = 0
		_BumpMagnitude("Bump Magnitude", float) = 1.0
		[Toggle]_DoubleDepthPass("Double Depth Pass (quality++/FrontFace)", int) = 1

		//	Extinction
		[Toggle]_EnableColourExtinction("Enable Colour Extinction (quality++)", int) = 1
		_ColourExtinction("Colour Extinction (RGBA)", Color) = (0.1, 0.033, 0.00666, 1.0)
		_ColourExtinctionMagnitude("Extinction Magnitude (Multiplier, Min, Max)", vector) = (1.0,0.0,1.0)
		_ColourExtinctionTexture("Extinction Texture (RGBA)", 2D) = "white" {}
	[Toggle]_CapExtinctionValues("Cap Extinction Values (min,max)", int) = 1

		//	ABBERATION
		[Toggle]_EnableAberration("Enable Chromatic Distortion (quality++)", int) = 1
		_Aberration("Chromatic Aberration (RGBA)", Color) = (0.1, 0.033, 0.00666, 1.0)
		_AberrationMagnitude("Aberration Magnitude (Multiplier, Min, Max)", vector) = (1.0,0.0,0.1)
		_AberrationTexture("Aberration Texture (RGBA)", 2D) = "white" {}
	[Toggle]_CapAberrationValues("Cap Aberration Values (min,max)", int) = 1

		//	Fog
		[Toggle]_EnableFog("Enable Fog", int) = 1
		_FogMagnitude("Fog Magnitude", float) = 1.0
		_DepthColourNear("Fog Near", Color) = (0,0,0,0)
		_DepthColourFar("Fog Far", Color) = (0,0,0,0)

		//	Depth
		[Toggle]_DisableBackFace("Is Back Face (quality--/BackFace)", float) = 0.0
		[Enum(FantasticGlass.GlassDepthTechnique)] _GlassShader_DepthTechnique("Depth Technique", int) = 2
		[Enum(FantasticGlass.GlassNormalTechnique)] _GlassShader_NormalTechnique("Normal Technique", int) = 2
		_DepthFront("Depth Front (R)", 2D) = "white" {}
	_DepthBack("Depth Back (R)", 2D) = "white" {}
	_DepthOther("Depth Other (R)", 2D) = "white" {}
	_DepthFrontDistance("Depth Back Distance", float) = 5000.0
		_DepthBackDistance("Depth Back Distance", float) = 5000.0
		_DepthOtherDistance("Depth Other Distance", float) = 5000.0
		_DepthMagnitude("Depth Magnitude", float) = 1.0
		_DepthOffset("Depth Offset", float) = 0.0
		[Toggle]_FlipUV_Depth("Flip Grab/Depth UV", int) = 0

		//	Camera
		//	NORMAL TECHNIQUE 1
		[Toggle]_UseCameraNormal("Use Camera Normal (quality++)", int) = 1
		_CameraNormal("Camera Normal", vector) = (0.0, 1.0, 0.0, 1.0)

		//	Surface
		_Glossiness("Glossiness", Range(0,1)) = 0.5
		_GlossinessTexture("Glossiness Texture (G)", 2D) = "white" {}
	_Metallic("Metallic", Range(0,1)) = 0.0
		_MetallicTexture("Metallic Texture (G)", 2D) = "white" {}
	_Glow("Glow", Range(-100,100)) = 0.0
		_GlowTexture("Glow Texture (RGBA)", 2D) = "white" {}

	//	BLEND & Z WRITE
	[Toggle]_ZWrite("__zw", int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _Blend_Source("Source Blend mode", int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _Blend_Dest("Destination Blend mode", int) = 10
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", int) = 0
	}
		SubShader{
		Tags{ "Queue" = "Transparent+10" "RenderType" = "Opaque" }	//	RenderType must be Opaque so as to render to the depth buffer
		Blend[_Blend_Source][_Blend_Dest]
		ZWrite[_ZWrite]
		Cull[_CullMode]
		LOD 200

		//	GRAB

		GrabPass{
		Name "BASE"
		Tags{ "LightMode" = "Always" }
	}

		CGPROGRAM
#pragma surface surf Standard vertex:vert
#pragma target 3.0
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

		//#pragma multi_compile NORMAL_SHADER_OFF NORMAL_SHADER_OFF
		//#pragma multi_compile DEPTH_SHADER_OFF DEPTH_SHADER_OFF
#pragma multi_compile DEPTH_FRONT_SHADER_OFF DEPTH_FRONT_SHADER_ON
#pragma multi_compile NORMAL_CAM_CUSTOM NORMAL_CAM_UNITY NORMAL_WORLD_CAM_SHADER
#pragma multi_compile DEPTH_CAM_CUSTOM DEPTH_CAM_UNTY

		//	DATA

		//	Albedo
		sampler2D _MainTex;
	half4 _MainTexIntensity;
	half4 _Color;
	half _Opacity;
	half _Alpha;
	//	Camera
	//	NORMAL TECHNIQUE 1
	int _UseCameraNormal;
	half4 _CameraNormal;
	//	Distortion
	int _EnableDistortion;
	sampler2D _DistortionTexture;
	half4 _DistortionIntensity;
	sampler2D _GrabTexture;
	int _FlipUV_Grab;
	half _BumpMagnitude;
	half _DistortionEdgeBend;
	half _DistortionDepthFade;
	//	Extinction
	int _EnableColourExtinction;
	int _CapExtinctionValues;
	half4 _ColourExtinction;
	half3 _ColourExtinctionMagnitude;
	sampler2D _ColourExtinctionTexture;
	//	Aberration
	int _EnableAberration;
	int _CapAberrationValues;
	half4 _Aberration;
	half3 _AberrationMagnitude;
	sampler2D _AberrationTexture;
	//	Surface
	half _Glossiness;
	sampler2D _GlossinessTexture;
	half _Metallic;
	sampler2D _MetallicTexture;
	half _Glow;
	sampler2D _GlowTexture;
	//	Depth
	int _DisableBackFace;
	//	DEPTH TECHNIQUE 1.1
	sampler2D _DepthFront;
	sampler2D _DepthBack;
	sampler2D _DepthOther;
	//	DEPTH TECHNIQUE 1.1
	float _DepthFrontDistance;
	float _DepthBackDistance;
	float _DepthOtherDistance;
	int _DoubleDepthPass;
	half _DepthMagnitude;
	half _DepthOffset;
	int _FlipUV_Depth;
	//	Fog
	int _EnableFog;
	half _FogMagnitude;
	half4 _DepthColourNear;
	half4 _DepthColourFar;

	struct Input {
		float2 uv_MainTex;
		float2 uv_DistortionTexture;
		float4 grabUV;
		//float4 grabUV_flip;
#if DEPTH_FRONT_SHADER_ON
		float4 screenNormalDepth;
#endif
		INTERNAL_DATA
	};

	//	FUNCTIONS

	//	Vertex								
	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input, o);

		//	Grab Screen UV
#if UNITY_VERSION >= 540
		float4 hPos = UnityObjectToClipPos(v.vertex); 
#else
		float4 hPos = UnityObjectToClipPos(v.vertex);
#endif
		
		if (_FlipUV_Depth)
		{
			hPos.y = 1 - hPos.y;
		}
		o.grabUV = ComputeGrabScreenPos(hPos);
		//o.grabUV_flip = ComputeGrabScreenPos(hPos);

#if DEPTH_FRONT_SHADER_ON
		//	screen normal and depth (replaces front face depth normal camera)
		o.screenNormalDepth.xyz = COMPUTE_VIEW_NORMAL;
		o.screenNormalDepth.w = COMPUTE_DEPTH_01;
#endif
	}

	//	Surface
	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		//	DEPTH
		float4 uv_depth = IN.grabUV;
		//if (_FlipUV_Depth)
		//	uv_depth = IN.grabUV_flip;


		float3 frontNormal;
		float3 backNormal;
		float3 otherNormal;

		float3 worldNormal;

		float frontDepth = 0.001;
		float backDepth = 0.001;
		float otherDepth = 0.001;

		float4 frontTexture = 1;
		float4 backTexture = 1;
		float4 otherTexture = 1;

		float averageDepthDistance = (_DepthFrontDistance + _DepthBackDistance + _DepthOtherDistance) * 3;
		float depthOffset = _DepthOffset * averageDepthDistance * 0.01f;


#if DEPTH_FRONT_SHADER_OFF
		frontTexture = tex2Dproj(_DepthFront, UNITY_PROJ_COORD(uv_depth));
#endif

		backTexture = tex2Dproj(_DepthBack, UNITY_PROJ_COORD(uv_depth));
		otherTexture = tex2Dproj(_DepthOther, UNITY_PROJ_COORD(uv_depth));


#if DEPTH_CAM_CUSTOM
#if DEPTH_FRONT_SHADER_OFF
		frontDepth = frontTexture.a * _DepthFrontDistance;
#endif
		backDepth = backTexture.a * _DepthBackDistance;
		otherDepth = otherTexture.a * _DepthOtherDistance;
#endif


#if NORMAL_CAM_UNITY || DEPTH_CAM_UNITY
#if DEPTH_FRONT_SHADER_OFF
		DecodeDepthNormal(frontTexture, frontDepth, frontNormal);
		frontDepth *= _DepthFrontDistance;
#endif

		DecodeDepthNormal(backTexture, backDepth, backNormal);
		backDepth *= _DepthBackDistance;

		DecodeDepthNormal(otherTexture, otherDepth, otherNormal);
		otherDepth *= _DepthOtherDistance;

		worldNormal = frontNormal;
#endif		


#if NORMAL_CAM_CUSTOM
#if DEPTH_FRONT_SHADER_OFF
		frontNormal = frontTexture.rgb;
#endif
		backNormal = backTexture.rgb;
		otherNormal = otherTexture.rgb;
#endif


#if DEPTH_FRONT_SHADER_ON
		frontDepth = IN.screenNormalDepth.w * _DepthBackDistance;
		frontNormal = IN.screenNormalDepth.xyz;
#endif

		//	->	Ensures correct depth chosen, especially with Complex Other depth and Front/Back Render Order
		//	TODO: consider keyword enable; test performance with & without
		#if DEPTH_FRONT_SHADER_OFF
		if (backDepth < frontDepth)
		{
		backDepth = max(backDepth, otherDepth);
		}


		if (otherDepth < frontDepth)
		{
		otherDepth = max(backDepth, otherDepth);
		}
		#endif
		//	<-


		float internalBackDepth = min(backDepth, otherDepth);


		if (_DisableBackFace)
		{
			backDepth = otherDepth;

			//	TODO: test suitability to Normal Technique 2:
			//#if NORMAL_CAM_UNITY
			//	NORMAL TECHNIQUE 2
			//worldNormal = backNormal - frontNormal;
			//#endif
		}
		else
		{
			backDepth = internalBackDepth;
		}

		float externalDepthDiff = ((backDepth - frontDepth) * _DepthMagnitude) + depthOffset;

		float internalDepthDiff = ((internalBackDepth - frontDepth) * _DepthMagnitude) + depthOffset;

#if NORMAL_CAM_CUSTOM || NORMAL_CAM_UNITY
		worldNormal = lerp(frontNormal, backNormal - frontNormal, internalDepthDiff);
#endif

		//	GRAB
		float4 uv_grab = IN.grabUV;
		//if (_FlipUV_Grab)
		//	uv_grab = IN.grabUV_flip;

		//	DISTORTION / BUMP
		float3 distortion = UnpackNormal(tex2D(_DistortionTexture, IN.uv_DistortionTexture));

		o.Normal = lerp(float3(0.5, 0.5, 0.5), distortion, _BumpMagnitude);

#if NORMAL_WORLD_CAM_SHADER
		worldNormal = WorldNormalVector(IN, o.Normal);
#endif

		if (_EnableDistortion)
		{
			distortion *= _DistortionIntensity.x;

#if NORMAL_CAM_UNITY || NORMAL_CAM_CUSTOM
			distortion += worldNormal * _DistortionIntensity.y;
#endif

#if NORMAL_WORLD_CAM_SHADER
			distortion += dot(_CameraNormal, worldNormal) * _DistortionIntensity.y;
#endif

			distortion /= (1 + ((1 / internalDepthDiff) * _DistortionDepthFade));

			distortion = lerp(distortion, distortion / internalDepthDiff, _DistortionEdgeBend);

			distortion.x = max(min(distortion.x, _DistortionIntensity.w), -_DistortionIntensity.w);
			distortion.y = max(min(distortion.y, _DistortionIntensity.w), -_DistortionIntensity.w);
			//distortion.z = max(min(distortion.z, _DistortionIntensity.w), -_DistortionIntensity.w);

			distortion *= _DistortionIntensity.z;
		}
		else
		{
			distortion = float3(0, 0, 0);
		}

		float4 distortion4 = float4(0, 0, 0, 0);
		distortion4.xyz = distortion;

		float4 distortedUV_depth = uv_depth + distortion4;
		float4 distortedUV_grab = uv_grab + distortion4;

		//distortedUV_depth = UnityStereoTransformScreenSpaceTex(distortedUV_depth);
		//distortedUV_grab = UnityStereoTransformScreenSpaceTex(distortedUV_grab);

		//	DEPTH - SECOND PASS
		if (_DoubleDepthPass)
		{
#if DEPTH_FRONT_SHADER_OFF
			frontTexture = tex2Dproj(_DepthFront, UNITY_PROJ_COORD(distortedUV_depth));
#endif
			backTexture = tex2Dproj(_DepthBack, UNITY_PROJ_COORD(distortedUV_depth));
			otherTexture = tex2Dproj(_DepthOther, UNITY_PROJ_COORD(distortedUV_depth));



#if DEPTH_CAM_UNITY

#if DEPTH_FRONT_SHADER_OFF
			DecodeDepthNormal(frontTexture, frontDepth, frontNormal);
			frontDepth *= _DepthFrontDistance;
#endif

			DecodeDepthNormal(backTexture, backDepth, backNormal);
			backDepth *= _DepthBackDistance;
			DecodeDepthNormal(otherTexture, otherDepth, otherNormal);
			otherDepth *= _DepthOtherDistance;

#endif



#if DEPTH_CAM_CUSTOM

#if DEPTH_FRONT_SHADER_OFF
			frontDepth = frontTexture.a * _DepthFrontDistance;
#endif

			backDepth = backTexture.a * _DepthBackDistance;
			otherDepth = otherTexture.a * _DepthOtherDistance;

#endif

			//	->	Ensures correct depth chosen, especially with Complex Other depth and Front/Back Render Order
			//	TODO: consider keyword enable; test performance with & without
			#if DEPTH_FRONT_SHADER_OFF
			if (backDepth < frontDepth)
			{
					backDepth = max(backDepth, otherDepth);
			}


			if (otherDepth < frontDepth)
			{
				otherDepth = max(backDepth, otherDepth);
			}
			#endif
			//	<-


			//internalBackDepth = max(frontDepth, min(backDepth, otherDepth));
			internalBackDepth = min(backDepth, otherDepth);
			//internalBackDepth = min(backDepth, max(frontDepth, otherDepth));

			if (_DisableBackFace)
			{
				backDepth = otherDepth;
			}
			else
			{
				backDepth = internalBackDepth;
			}

			externalDepthDiff = ((backDepth - frontDepth) * _DepthMagnitude) + depthOffset;

			internalDepthDiff = ((internalBackDepth - frontDepth) * _DepthMagnitude) + depthOffset;
		}

		//	GRAB
		half4 grabColour = half4(0, 0, 0, 1);
		half4 grabColour_Flat = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(uv_grab));


		//	ABBERATION
		if (_EnableAberration)
		{
			half4 aberrationTextureColour = tex2D(_AberrationTexture, IN.uv_MainTex);

			//	external depth used, in case of backless geometry
			//half aberrationMagnitude = externalDepthDiff * _AberrationMagnitude.x * _Aberration.a;
			float aberrationMagnitude = internalDepthDiff * _Aberration.a * _AberrationMagnitude.x * aberrationTextureColour.a;
			aberrationMagnitude = lerp(_AberrationMagnitude.y, _AberrationMagnitude.z, aberrationMagnitude);

			half3 aberration = aberrationMagnitude * distortion;

			if (_CapAberrationValues)
			{
				aberration = max(min(_AberrationMagnitude.z, _AberrationMagnitude.y), aberration);
				aberration = min(max(_AberrationMagnitude.z, _AberrationMagnitude.y), aberration);
			}

			float4 uv_refraction_aberration_r = distortedUV_grab;
			float4 uv_refraction_aberration_g = distortedUV_grab;
			float4 uv_refraction_aberration_b = distortedUV_grab;

			uv_refraction_aberration_r.xyz += aberration * _Aberration.r * aberrationTextureColour.r;
			uv_refraction_aberration_g.xyz += aberration * _Aberration.g * aberrationTextureColour.g;
			uv_refraction_aberration_b.xyz += aberration * _Aberration.b * aberrationTextureColour.b;

			grabColour.r = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(uv_refraction_aberration_r)).r;
			grabColour.g = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(uv_refraction_aberration_g)).g;
			grabColour.b = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(uv_refraction_aberration_b)).b;
		}
		else
		{
			grabColour = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(distortedUV_grab));
		}

		//	FOG
		if (_EnableFog)
		{
			float fogDepth = internalDepthDiff * _FogMagnitude;
			fogDepth = max(0, min(1, fogDepth));
			half4 fogColour = lerp(_DepthColourNear, _DepthColourFar, fogDepth);
			grabColour = lerp(grabColour, fogColour, fogColour.a);
		}

		//	OPACITY
		half4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _MainTexIntensity;
		albedo = lerp(albedo, _Color, _Color.a);

		grabColour = lerp(grabColour, albedo, min(1,max(0, _Color.a * _Opacity)));

		//	EXTINCTION
		if (_EnableColourExtinction)
		{
			half4 extinctionTextureColour = tex2D(_ColourExtinctionTexture, IN.uv_MainTex);

			float extinction = internalDepthDiff * _ColourExtinction.a * extinctionTextureColour.a * _ColourExtinctionMagnitude.x;

			extinction = lerp(_ColourExtinctionMagnitude.y, _ColourExtinctionMagnitude.z, extinction);

			if (_CapExtinctionValues)
			{
				extinction = max(min(_ColourExtinctionMagnitude.z, _ColourExtinctionMagnitude.y), extinction);
				extinction = min(max(_ColourExtinctionMagnitude.z, _ColourExtinctionMagnitude.y), extinction);
			}

			grabColour.r -= _ColourExtinction.r * extinctionTextureColour.r * extinction;
			grabColour.g -= _ColourExtinction.g * extinctionTextureColour.g * extinction;
			grabColour.b -= _ColourExtinction.b * extinctionTextureColour.b * extinction;
		}

		//	ALBEDO
		o.Albedo = grabColour.rgb;

		albedo = tex2D(_MainTex, IN.uv_MainTex) * _MainTexIntensity;

		o.Albedo = lerp(o.Albedo, albedo.rgb, _Opacity * albedo.a);

		o.Albedo = lerp(o.Albedo, _Color.rgb, _Color.a);

		//	GLOW
#if UNITY_VERSION >= 540
		o.Emission = 0.5  * o.Albedo + (o.Albedo * (_Glow*0.125) * 0.01 * (0.1 + (_Glossiness)));
#elif UNITY_VERSION >= 520
#if UNITY_VERSION < 530
		o.Emission = 0.25  * o.Albedo + (o.Albedo * (_Glow*0.125) * 0.01 * (0.1 + (_Glossiness)));
#endif
#else
		o.Emission = (o.Albedo * _Glow * 0.01 * (0.1 + _Glossiness));
#endif

				half3 emissionTexture = tex2D(_GlowTexture, IN.uv_MainTex).rgb;
				o.Emission *= emissionTexture;

				//	SURFACE
				o.Metallic = _Metallic * tex2D(_MetallicTexture, IN.uv_MainTex).g;
				o.Smoothness = _Glossiness * tex2D(_GlossinessTexture, IN.uv_MainTex).g;

				//	ALPHA
				o.Albedo = lerp(grabColour_Flat, o.Albedo, _Alpha);
				o.Emission *= _Alpha;
				o.Smoothness *= _Alpha;
				o.Metallic *= _Alpha;
			}
			ENDCG
	}
		Fallback "VertexLit"
				CustomEditor "FantasticGlass.GlassMaterialInspector"
}
