// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Advertisement" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Speed ("Speed", Float) = 2
		_Frequency ("Frequency", Float) = 300
		_Wobble("Wobble", Float) = 2
		_EmissionAmp("Emission Amplifier", Float) = 1
		_NoiseAmp("Noise Amplifier", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		float _Speed;
		float _Frequency;
		float _Wobble;
		float _EmissionAmp;
		float _NoiseAmp;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)


		float rand(float3 co)
		{
			return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by 
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			float amp = ((sin(_Time.w * 3 * _Speed + IN.uv_MainTex.y * _Frequency) + 1) / 2.0f);
			amp = amp-((rand((round(IN.uv_MainTex.xyy*1000)/1000.0f)*_Time.y))*_NoiseAmp);
			float2 offset = IN.uv_MainTex + float2(+amp*_Wobble, 0)*0.0003f;
			c = tex2D(_MainTex, offset) * _Color;
			o.Emission = (c.rgb * (1-amp*0.5f))*_EmissionAmp;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
