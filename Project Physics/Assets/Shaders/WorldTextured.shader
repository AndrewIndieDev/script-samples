// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/WorldTextured"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 wPos = mul(unity_ObjectToWorld, i.wPos);
				fixed4 col1 = tex2D(_MainTex, frac(wPos.zx));
				fixed4 col2 = tex2D(_MainTex, frac(wPos.yx));
				fixed4 col3 = tex2D(_MainTex, frac(wPos.zy));
				return (col1 * abs(i.normal.y) + col2 * abs(i.normal.z) + col3 * abs(i.normal.x));
			}
			ENDCG
		}
	}
}
