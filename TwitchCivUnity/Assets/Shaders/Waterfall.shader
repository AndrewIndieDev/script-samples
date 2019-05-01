// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Similar to regular FX/Glass/Stained BumpDistort shader
// from standard Effects package, just without grab pass,
// and samples a texture with a different name.

Shader "Custom/YetAnotherWaterfallShader" 
{
	Properties 
	{
		_BumpAmt  ("Distortion", range (0,64)) = 10
		_TintAmt ("Tint Amount", Range(0,1)) = 0.1
		_Tint ("Tint", Color) = (1,1,1,1)
		_MainTex ("Tint Color (RGB)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_Mask("Stream Mask", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "white" {}
		_DisplacementAmp("Displacement Amplifier", Float) = 1
		_StreamSpeed("Stream speed", Float) = 1
	}

	Category 
	{

		// We must be transparent, so other objects are drawn before this one.
		Tags { "Queue"="Transparent" "RenderType"="Opaque" }

		SubShader 
		{
			Cull Off

			Pass 
			{
				//Target 4.0
				Name "BASE"
				Tags { "LightMode" = "Always" }
				
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : POSITION;
					float4 uvgrab : TEXCOORD0;
					float2 uvbump : TEXCOORD1;
					float2 uvmain : TEXCOORD2;
					float3 normal : TEXCOORD4;
					UNITY_FOG_COORDS(3)
				};

				float _BumpAmt;
				half _TintAmt;
				float4 _BumpMap_ST;
				float4 _MainTex_ST;
				sampler2D _DisplacementTex;
				float _DisplacementAmp;
				float _StreamSpeed;

				v2f vert (appdata_t v)
				{
					v2f o;
					v.vertex.xyz += v.normal.xyz * tex2Dlod(_DisplacementTex, float4(v.texcoord + float2(_Time.x, _Time.y * _StreamSpeed), 0, 0)).r * _DisplacementAmp;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = mul(unity_ObjectToWorld, v.normal);

					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif

					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;

					o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
					o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );

					UNITY_TRANSFER_FOG(o,o.vertex);

					return o;
				}

				sampler2D _GrabBlurTexture;
				float4 _GrabBlurTexture_TexelSize;
				sampler2D _BumpMap;
				sampler2D _MainTex;
				sampler2D _Mask;
				float4 _Tint;
				

				half4 frag (v2f i) : SV_Target
				{
					// calculate perturbed coordinates
					// we could optimize this by just reading the x & y without reconstructing the Z

					

					float maskAmt = tex2D(_Mask, i.uvmain + float2(0, _Time.y * _StreamSpeed)).r;

					half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump + float2(_Time.x, _Time.y * _StreamSpeed) )).rg;

					float2 offset = bump * (_BumpAmt * maskAmt) * _GrabBlurTexture_TexelSize.xy;

					i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
					
					half4 col = tex2Dproj (_GrabBlurTexture, UNITY_PROJ_COORD(i.uvgrab));


					half4 tint = tex2D(_MainTex, i.uvmain + float2(_Time.x, _Time.y * _StreamSpeed)) * _Tint;

					col = lerp (col, tint, _TintAmt * maskAmt);

					UNITY_APPLY_FOG(i.fogCoord, col);

					return col;
				}
				ENDCG
			}
		}
	}
}