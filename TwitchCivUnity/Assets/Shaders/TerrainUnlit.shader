// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TerrainUnlit"
{
	Properties
	{
		_GrassTex("Texture", 2D) = "white" {}
		_RockTex("Texture", 2D) = "white" {}
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
				float3 normal : TEXCOORD1;
				float3 worldPosition : TEXCOORD2;
 
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = v.normal;
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			sampler2D _GrassTex;
			sampler2D _RockTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 grassCol =
					tex2D(_GrassTex, i.worldPosition.xy / 20) * i.normal.z +
					tex2D(_GrassTex, i.worldPosition.yz / 20) * i.normal.x +
					tex2D(_GrassTex, i.worldPosition.xz / 20) * i.normal.y;
				fixed4 rockCol =
					tex2D(_RockTex, i.worldPosition.xy / 20) * i.normal.z +
					tex2D(_RockTex, i.worldPosition.yz / 20) * i.normal.x +
					tex2D(_RockTex, i.worldPosition.xz / 20) * i.normal.y;
				float fac = abs(i.normal.y);
				fixed4 col = rockCol;// lerp(rockCol, grassCol, fac);

				return col;
			}
			ENDCG
		}
	}
}
