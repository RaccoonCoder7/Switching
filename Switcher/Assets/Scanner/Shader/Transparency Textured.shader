Shader "Scanner/Transparency Textured" {
	Properties {
		_MainTex ("Main", 2D) = "white" {}
		_BumpMap ("Bump", 2D) = "bump" {}
		_Color ("Tint", Color) = (1, 1, 1, 1)
		_Glossiness ("Smoothness", Range(0, 1)) = 0
		_Metallic ("Metallic", Range(0, 1)) = 0
		_LightSweepVector ("Light Sweep Vector", Vector) = (0, 0, 1, 0)
		_LightSweepAmp ("Light Sweep Amp", Float) = 1
		_LightSweepExp ("Light Sweep Exp", Float) = 10
		_LightSweepInterval ("Light Sweep Interval", Float) = 20
		_LightSweepSpeed ("Light Sweep Speed", Float) = 10
		_LightSweepColor ("Light Sweep Color", Color) = (1, 0, 0, 0)
		_LightSweepAddColor ("Light Sweep Add Color", Color) = (0, 0, 0, 0)	
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha One

		CGPROGRAM
		#pragma surface surf Standard keepalpha
		#pragma multi_compile ALS_DIRECTIONAL ALS_SPHERICAL
		#include "Utils.cginc"
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 tc = tex2D(_MainTex, IN.uv_MainTex);
			float4 sonar = LightSweepColor(float4(IN.worldPos, 1));
			o.Albedo = tc.rgb * _LightSweepAmp * _Color.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Alpha = sonar.a * _Color.a;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}