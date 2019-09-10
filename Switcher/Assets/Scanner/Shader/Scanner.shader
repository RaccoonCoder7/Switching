Shader "Scanner/Scanner" {
	Properties {
		_MainTex      ("Texture", 2D) = "white" {}
		_ScanDistance ("Scan Distance", float) = 0
		_ScanWidth    ("Scan Width", float) = 10
		_LeadSharp    ("Leading Edge Sharpness", float) = 10
		_LeadColor    ("Leading Edge Color", Color) = (1, 1, 1, 0)
		_MidColor     ("Mid Color", Color) = (1, 1, 1, 0)
		_TrailColor   ("Trail Color", Color) = (1, 1, 1, 0)
		_HBarColor    ("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)
	}
	SubShader {
		Cull Off ZWrite Off ZTest Always
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv     : TEXCOORD0;
				float4 ray    : TEXCOORD1;
			};
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 uv       : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
				float4 ray      : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D_float _CameraDepthTexture;
			float4 _WorldSpaceScannerPos, _MainTex_TexelSize;
			float _ScanDistance, _ScanWidth, _LeadSharp;
			float4 _LeadColor, _MidColor, _TrailColor, _HBarColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;
#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
#endif				
				o.ray = v.ray;
				return o;
			}
			float4 horizBars (float2 p)
			{
				return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
			}
			half4 frag (v2f i) : SV_Target
			{
				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
				float linearDepth = Linear01Depth(rawDepth);
				float4 wsDir = linearDepth * i.ray;
				float3 wsPos = _WorldSpaceCameraPos + wsDir;
				half4 scannerCol = half4(0, 0, 0, 0);

				float dist = distance(wsPos, _WorldSpaceScannerPos);
				if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)
				{
					float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
					half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));
					scannerCol = lerp(_TrailColor, edge, diff) + horizBars(i.uv) * _HBarColor;
					scannerCol *= diff;
				}
				fixed4 base = tex2D(_MainTex, i.uv);
				return base + scannerCol;
			}
			ENDCG
		}
	}
	FallBack Off
}
