Shader "Scanner/Transparency" {
	Properties {
		_LightSweepVector ("Light Sweep Vector", Vector) = (0, 0, 1, 0)
		_LightSweepAmp ("Light Sweep Amp", Float) = 1
		_LightSweepExp ("Light Sweep Exp", Float) = 10
		_LightSweepInterval ("Light Sweep Interval", Float) = 20
		_LightSweepSpeed ("Light Sweep Speed", Float) = 10
		_LightSweepColor ("Light Sweep Color", Color) = (1, 0, 0, 0)
		_LightSweepAddColor ("Light Sweep Add Color", Color) = (0, 0, 0, 0)
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend Source", Int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend Destination", Int) = 10
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		ZWrite Off
		Blend [_BlendSrc] [_BlendDst]

		CGPROGRAM
		#pragma surface surf Unlit keepalpha
		#pragma multi_compile ALS_DIRECTIONAL ALS_SPHERICAL
		#include "Utils.cginc"
		void surf (Input IN, inout SurfaceOutput o)
		{
			float4 sonar = LightSweepColor(float4(IN.worldPos, 1));
			o.Albedo = sonar.rgb;
			o.Alpha = sonar.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}