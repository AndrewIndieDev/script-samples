// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WorldTextured"
{
	Properties
	{
		_GrassTex("Grass Texture", 2D) = "white" {}
		_RockTex("Rock Texture", 2D) = "white" {}
		_GrassTint("Grass tint", Color) = (1,1,1,1)
		_RockTint("Rock tint", Color) = (1,1,1,1)
		_Scale ("Scale", Float) = 1
		_GrassAngle ("Grass angle", Range(1,100)) = 1
	}
	SubShader
	{

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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 wPos : TEXCOORD1;
				float3 normal : NORMAL;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.wPos = v.vertex;
				o.uv = v.uv;
				o.normal = normalize(mul(unity_ObjectToWorld, v.normal));
				return o;
			}
			
			sampler2D _GrassTex;
			sampler2D _RockTex;
			float _Scale;
			float4 _GrassTint;
			float4 _RockTint;
			float _GrassAngle;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 wPos = mul(unity_ObjectToWorld, i.wPos);
				fixed4 col1g = tex2D(_GrassTex, frac(wPos.zx * _Scale)) * _GrassTint;
				fixed4 col1r = tex2D(_RockTex, frac(wPos.zx * _Scale)) * _RockTint;
				fixed4 col2 = tex2D(_RockTex, frac(wPos.yx * _Scale)) * _RockTint;
				fixed4 col3 = tex2D(_RockTex, frac(wPos.zy * _Scale)) * _RockTint;
				return (((i.normal.y > 0) ? col1g : col1r) * pow(abs(i.normal.y), _GrassAngle) + col2 * abs(i.normal.z) + col3 * abs(i.normal.x));
			}
			ENDCG
		}
	}
}
