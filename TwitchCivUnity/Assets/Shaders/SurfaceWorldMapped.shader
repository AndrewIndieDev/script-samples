Shader "Custom/SurfaceWorldMapped" {
	Properties
	{
		_GrassTex("Grass Texture", 2D) = "white" {}
		_RockTex("Rock Texture", 2D) = "white" {}
		_SnowNoise("Snow noise", 2D) = "white" {}
		_SnowTex("Snow Texture", 2D) = "white" {}
		_GrassTint("Grass tint", Color) = (1,1,1,1)
		_RockTint("Rock tint", Color) = (1,1,1,1)
		_Scale("Scale", Float) = 1
		_GrassAngle("Grass angle", Range(1,100)) = 1
		_SnowHeight("Snow height", Float) = 1
		_SnowAmp("Snow Amplifier", Float) = 1
		_SnowScale("Snow Scale", Float) = 1
	}
		SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _GrassTex;
		sampler2D _RockTex;
		sampler2D _SnowTex;
		sampler2D _SnowNoise;
		float _Scale;
		float4 _GrassTint;
		float4 _RockTint;
		float _GrassAngle;
		float _SnowHeight;
		float _SnowAmp;
		float _SnowScale;

		struct Input{
			float4 wPos;
			float4 localPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex;
		}

		void surf (Input i, inout SurfaceOutputStandard o) {
			float3 worldNormal = o.Normal;
			float4 wPos = mul(unity_ObjectToWorld, i.localPos);
			fixed4 col1g = tex2D(_GrassTex, frac(wPos.zx * _Scale)) * _GrassTint;
			fixed4 col1r = tex2D(_RockTex, frac(wPos.zx * _Scale)) * _RockTint;
			fixed4 col2 = tex2D(_RockTex, frac(wPos.yx * _Scale)) * _RockTint;
			fixed4 col3 = tex2D(_RockTex, frac(wPos.zy * _Scale)) * _RockTint;
			fixed4 c = (((worldNormal.y > 0) ? col1g : col1r) * pow(abs(worldNormal.y), _GrassAngle) + col2 * abs(worldNormal.z) + col3 * abs(worldNormal.x));
			fixed4 snow = tex2D(_SnowTex, frac(wPos.zx * _Scale)) + tex2D(_SnowTex, frac(wPos.yx * _Scale)) + tex2D(_SnowTex, frac(wPos.zy * _Scale));
			snow /= 3;
			fixed4 finalcol = (wPos.y < _SnowHeight + tex2D(_SnowNoise, wPos.yx * _SnowScale).r * _SnowAmp) ? c : snow;
			//fixed4 c = (col1g * round(worldNormal.y - 0.4) + col1r * (1-round(worldNormal.y - 0.4))) + col2 * abs(worldNormal.z) + col3 * abs(worldNormal.x);
			//c.rgb = wPos.rgb;
			o.Albedo = finalcol.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
