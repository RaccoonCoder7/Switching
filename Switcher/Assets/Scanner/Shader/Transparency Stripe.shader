Shader "Scanner/Transparency Stripe" {
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
		_StripeTex     ("Stripe", 2D) = "white" {}
		_StripeColor   ("Stripe Color", Color) = (1, 0.8, 0, 1)
		_StripeWidth   ("Stripe Width", Float) = 0.1
		_StripeDensity ("Stripe Density", Float) = 5
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Standard keepalpha vertex:vert
		#pragma multi_compile ALS_DIRECTIONAL ALS_SPHERICAL
		#include "Utils.cginc"
		
		sampler2D _StripeTex;
		float4 _StripeColor;
		float _StripeWidth, _StripeDensity;
		
		void vert (inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.stripeUvw = v.vertex.xyz * _StripeDensity;
		}
		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 tc = tex2D(_MainTex, IN.uv_MainTex);
			float4 sonar = LightSweepColor(float4(IN.worldPos, 1));

			float stripex = tex2D(_StripeTex, float2(IN.stripeUvw.x, 1 - _StripeWidth)).x;
			float stripey = tex2D(_StripeTex, float2(IN.stripeUvw.y, 1 - _StripeWidth)).x;
			float stripez = tex2D(_StripeTex, float2(IN.stripeUvw.z, 1 - _StripeWidth)).x;
			float checker = stripex * stripey * stripez;
			fixed3 rgb = lerp(tc.rgb, _StripeColor.rgb, sonar.a);
			fixed a = lerp(tc.a, 1 - checker, sonar.a);

			o.Albedo = rgb * _Color.rgb;
			o.Alpha = a * _Color.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}