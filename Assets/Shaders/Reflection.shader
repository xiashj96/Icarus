Shader "Custom/Reflection" 
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_RefCen("Reflect Center y", float) = 0.7
		_WTF1("Wave Time Freq 1", float) = 0.2
		_WSF1("Wave Spacial Freq 1", float) = 25
		_WA1("Wave Amplitude 1", float) = 0.5
		_WTF2("Wave Time Freq 2", float) = 3
		_WSF2("Wave Spacial Freq 2", float) = 100
		_WA2("Wave Amplitude 2", float) = 0.1
		_WTF3("Wave Time Freq 3", float) = 0
		_WSF3("Wave Spacial Freq 3", float) = 0
		_WA3("Wave Amplitude 3", float) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }

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
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _RefCen;
			float _WTF1, _WSF1, _WA1;
			float _WTF2, _WSF2, _WA2;
			float _WTF3, _WSF3, _WA3;

			fixed luminance(fixed3 color)
            {
                return color.r * 0.212 + color.g * 0.715 + color.b * 0.072;
            }

            fixed3 reflect(float2 uv, float2 uvgrab)
            {
            	if(uv.y > _RefCen)
            		return fixed3(0, 0, 0);
            	fixed3 tex = tex2D(_MainTex, uvgrab);
            	if(luminance(tex) < 0.5)
            		return fixed3(0, 0, 0);
            	return tex * (1 - uvgrab.y) * (uvgrab.y - _RefCen) / (1 - _RefCen) / (1 - _RefCen) * 2;
            }

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float _b = 0.3;
				float b = _RefCen + _b;
				float a = (b / _RefCen - 1) * (1 - _RefCen);
				float2 uvgrab = float2(i.uv.x, 1 - a * i.uv.y / (b - i.uv.y));

				uvgrab.x += sin(_Time.y * _WTF1 * 5 + _WSF1 * uvgrab.y * 10) * _WA1 * (_RefCen - uvgrab.y) * 0.1;
				uvgrab.x += sin(_Time.y * _WTF2 * 5 + _WSF2 * uvgrab.y * 10) * _WA2 * (_RefCen - uvgrab.y) * 0.1;
				uvgrab.x += sin(_Time.y * _WTF3 * 5 + _WSF3 * uvgrab.y * 10) * _WA3 * (_RefCen - uvgrab.y) * 0.1;

				return fixed4(reflect(i.uv, uvgrab) + tex2D(_MainTex, i.uv).rgb, 1);
				//return fixed4(i.uv.y, i.uv.y, i.uv.y, 1);
				//return fixed4(uvgrab.y, uvgrab.y, uvgrab.y, 1);
			}
			ENDCG
		}
	}
}
