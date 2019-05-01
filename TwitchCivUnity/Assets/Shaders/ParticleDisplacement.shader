// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Particles/ParticleDisplacement"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisplacementAmp("Displacement amplifier", Float) = 1
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		GrabPass
		{
			"_BackgroundTexture"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
#else
				float scale = 1.0;
#endif

				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BackgroundTexture;
			float _DisplacementAmp;
			float4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				i.uvgrab.xy += (tex2D(_MainTex, i.uv).rg * 2 - 1) * (_DisplacementAmp * _Color.a);
				half4 col = tex2Dproj(_BackgroundTexture, UNITY_PROJ_COORD(i.uvgrab));
				return col;
			}
			ENDCG
		}
	}
}
