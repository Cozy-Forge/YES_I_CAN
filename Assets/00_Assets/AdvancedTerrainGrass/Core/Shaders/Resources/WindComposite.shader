// http://www.iquilezles.org/www/articles/dynclouds/dynclouds.htm

Shader "WindComposite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert_imgMy
            #pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float3 _AtgWindDir;
			float2 _AtgWindUVs;
			float2 _AtgWindUVs1;
			float2 _AtgWindUVs2;
			float2 _AtgWindUVs3;
			float2 _AtgGust;
			float3 _AtgGustMixLayer;

			struct appdata_imgMy
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f_imgMy
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};


			v2f_imgMy vert_imgMy( appdata_imgMy v )
			{
				v2f_imgMy o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			half4 frag(v2f_imgMy i) : SV_Target {

				half4 n1 = tex2D(_MainTex, i.uv + _AtgWindUVs);
				half4 n2 = tex2D(_MainTex, (i.uv + _AtgWindUVs1)); 
				half4 n3 = tex2D(_MainTex, (i.uv + _AtgWindUVs2));
				half4 n4 = tex2D(_MainTex, i.uv * _AtgGust.x + _AtgWindUVs3);

				half4 sum = half4(n1.r, n1.g + n2.g, n1.b + n2.b + n3.b, n1.a + n2.a + n3.a + n4.a);
				//const half4 weights = half4(0.5000, 0.2500 , 0.1250 , 0.0625 );
				const half4 weights = half4(0.5000, 0.2500 , 0.1250 , 0.0625 );



            //  WindStrength
                half WindStrength = dot(sum, weights);
            //	Add some contrast...
            	//WindStrength = smoothstep(0, 1, WindStrength);
            //  GrassGustNoise                                
                half GustNoise = n4.a + dot(half3(n1.a, n2.a, n3.a), _AtgGustMixLayer);
            //	Into 0 - 1 range
            	GustNoise *= 0.5;
            //  Flatten Gust according to turbulence
            	//GustNoise = lerp(1, GustNoise * 2.0h - 0.75h, _AtgGust.y );
            	//GustNoise = lerp(1, 0.5 + GustNoise, _AtgGust.y );
            //	Final wind will be WindStrength * GustNoise. So we "center" Gustnoise around 1
            	GustNoise = 1 + (GustNoise * 2 - 0.75) * _AtgGust.y;
				return half4( WindStrength, GustNoise, 0, 0);

			}
			ENDCG
		}
	}
}
