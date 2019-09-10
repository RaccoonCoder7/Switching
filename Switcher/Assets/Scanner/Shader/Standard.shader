Shader "Scanner/Standard" {
	Properties {
		_MainTex ("Main", 2D) = "white" {}
		_BumpMap ("Bump", 2D) = "bump" {}
		_Color ("Tint", Color) = (1, 1, 1, 1)
		_Glossiness ("Smoothness", Range(0, 1)) = 0
		_Metallic ("Metallic", Range(0, 1)) = 0
		_LightSweepVector ("Light Sweep Vector", Vector) = (0, 0, 1, 0)
		_LightSweepAmp ("Light Sweep Amp", Range(0, 2)) = 1
		_LightSweepExp ("Light Sweep Exp", Range(0, 32)) = 10
		_LightSweepInterval ("Light Sweep Interval", Range(0, 32)) = 20
		_LightSweepSpeed ("Light Sweep Speed", Range(0, 16)) = 10
		_LightSweepColor ("Light Sweep Color", Color) = (1, 0, 0, 0)
		_LightSweepAddColor ("Light Sweep Add Color", Color) = (0, 0, 0, 0)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Standard finalcolor:scanColor
		#pragma multi_compile ALS_DIRECTIONAL ALS_SPHERICAL
		#include "Utils.cginc"
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 tc = tex2D(_MainTex, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Albedo = tc.rgb * _Color.rgb;
			o.Alpha = tc.a * _Color.a;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		void scanColor (Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			float4 sonar = LightSweepColor(float4(IN.worldPos, 1));
			color += sonar;
		}
		ENDCG
	}
	FallBack "Diffuse"
}