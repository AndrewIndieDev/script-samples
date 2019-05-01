Shader "Custom/YetAnotherWaterShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_xFrequency("X Frequency", Float) = 5
		_yFrequency("Y Frequency", Float) = 7
		_WaveAmplifier("Wave Amplifier", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		const float pi = 3.141592;

		float _xFrequency;
		float _yFrequency;
		float _WaveAmplifier;

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;

		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = (UnpackNormal(1- tex2D(_NormalTex, IN.uv_MainTex + float2(1,1) * _Time.x/5)) + UnpackNormal(1- tex2D(_NormalTex, IN.uv_MainTex * 0.3 + float2(0.6, -1.4) * _Time.x/2.5)) + UnpackNormal(1 - tex2D(_NormalTex, IN.uv_MainTex * 0.1 + float2(-1, 1.2) * _Time.x*0.1))) / 4;
		}

		/*float4x4 rotationMatrix(float3 axis, float angle)
		{
			axis = normalize(axis);
			float s = sin(angle);
			float c = cos(angle);
			float oc = 1.0 - c;

			return float4x4(oc * axis.x * axis.x + c, oc * axis.x * axis.y - axis.z * s, oc * axis.z * axis.x + axis.y * s, 0.0,
				oc * axis.x * axis.y + axis.z * s, oc * axis.y * axis.y + c, oc * axis.y * axis.z - axis.x * s, 0.0,
				oc * axis.z * axis.x - axis.y * s, oc * axis.y * axis.z + axis.x * s, oc * axis.z * axis.z + c, 0.0,
				0.0, 0.0, 0.0, 1.0);
		}*/

		/*float3x3 rotateNormal(float angle)
		{
			float x1 = cos(angle);
			float x2 = sin(angle);
			return float3x3(x1, -x2, 0,
				x2, x1, 0,
				0, 0, 1);
		}*/

		float2x2 rotateNormal(float angle)
		{
			float x1 = cos(angle);
			float x2 = sin(angle);
			return float2x2(x1, -x2,
				x2, x1);
		}

		void vert (inout appdata v)
		{
			v.vertex.y += (0.25*sin(v.vertex.x*_xFrequency + _Time.y) + 0.25) * (0.25*sin(v.vertex.z *_yFrequency + _Time.y) + 0.25) * _WaveAmplifier;
			//v.vertex.y += (0.25*sin(v.vertex.x * 2 + _Time.y) + 0.25);
			//float deg = (0.5 * sin(v.vertex.x * 2 + (pi / 2) + _Time.y)) * 90;
			//v.normal = mul(unity_WorldToObject,float3(sin(deg), cos(deg), 0));
			//v.normal.x = (0.25*sin(v.vertex.x * _xFrequency + _Time.y) + 0.25) * (0.25*sin(v.vertex.z * _yFrequency + _Time.y) + 0.25)/2;
		}


		

		ENDCG
	}
	FallBack "Diffuse"
}