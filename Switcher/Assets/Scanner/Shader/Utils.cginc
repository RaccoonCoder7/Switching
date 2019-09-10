#ifndef UTILS_INCLUDED
#define UTILS_INCLUDED

struct Input
{
	float2 uv_MainTex;
	float2 uv_BumpMap;
	float3 worldPos;
	float3 stripeUvw;
};
inline fixed4 LightingUnlit (SurfaceOutput s, fixed3 lightDir, fixed atten)
{
	return fixed4(s.Albedo, s.Alpha);
}

sampler2D _MainTex, _BumpMap;
float _Metallic, _Glossiness;
float4 _LightSweepVector, _LightSweepColor, _LightSweepAddColor, _Color;
float  _LightSweepAmp, _LightSweepExp, _LightSweepInterval, _LightSweepSpeed;

float4 LightSweepColor (in float4 v)
{
#ifdef ALS_DIRECTIONAL
	float w = dot(v, _LightSweepVector);
#endif
#ifdef ALS_SPHERICAL
	float w = length(v - _LightSweepVector);
#endif

	w -= _Time.y * _LightSweepSpeed;
	w /= _LightSweepInterval;
	w = w - floor(w);

	float p = _LightSweepExp;
	w = (pow(w, p) + pow(1 - w, p * 4)) * 0.5;	
	w *= _LightSweepAmp;
	return _LightSweepColor * w + _LightSweepAddColor;
}

#endif