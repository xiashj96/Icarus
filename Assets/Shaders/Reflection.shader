﻿Shader "Custom/Reflection" 
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}

		_BlueLine ("Blue Line", Range(0, 0.5)) = 0.35
		_Compression ("Compression", Range(0, 1)) = 0
		_SightPoint ("Sight Point", Range(0, 1)) = 0.3

		_WTF1("Wave Time Freq 1", float) = 1
		_WSF1("Wave Spacial Freq 1", float) = 250
		_WA1("Wave Amplitude 1", float) = 0.05
		_WTF2("Wave Time Freq 2", float) = 15
		_WSF2("Wave Spacial Freq 2", float) = 1000
		_WA2("Wave Amplitude 2", float) = 0.01
		_WTF3("Wave Time Freq 3", float) = 0
		_WSF3("Wave Spacial Freq 3", float) = 0
		_WA3("Wave Amplitude 3", float) = 0

		_BlurOffsetX ("Blur Offset X", Range(0, 0.01)) = 0.0035
        _BlurOffsetY ("Blur Offset Y", Range(0, 0.01)) = 0.0017
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
			float _BlueLine, _Compression, _SightPoint;
			float _WTF1, _WSF1, _WA1;
			float _WTF2, _WSF2, _WA2;
			float _WTF3, _WSF3, _WA3;

			fixed luminance(fixed3 color)
            {
                return color.r * 0.212 + color.g * 0.715 + color.b * 0.072;
            }

            fixed3 reflect(float2 uv)
            {
            	if(_Compression == 1)
            		return fixed3(0, 0, 0);

            	float _RefCen = _BlueLine * (2 - _Compression);

            	if(uv.y > _RefCen)
            		return fixed3(0, 0, 0);

				float b = _RefCen + _SightPoint;
				float a = (b / _RefCen - 1) * (1 - _RefCen);
				float y2 = _RefCen - (_RefCen - uv.y) / (_RefCen - _BlueLine * _Compression) * _RefCen;
				float2 uvgrab = float2(uv.x, 1 - a * y2 / (b - y2));
				if(uvgrab.y > 1)
					return fixed3(0, 0, 0);

				uvgrab.x += sin(_Time.y * _WTF1 + _WSF1 * uvgrab.y) * _WA1 * (_RefCen - uvgrab.y);
				uvgrab.x += sin(_Time.y * _WTF2 + _WSF2 * uvgrab.y) * _WA2 * (_RefCen - uvgrab.y);
				//uvgrab.x += sin(_Time.y * _WTF3 + _WSF3 * uvgrab.y) * _WA3 * (_RefCen - uvgrab.y);
            	
            	fixed3 tex = tex2D(_MainTex, uvgrab);
            	if(luminance(tex) < 0.5)
            		return fixed3(0, 0, 0);
            	return tex * (1 - uvgrab.y) * (uvgrab.y - _RefCen) / (1 - _RefCen) / (1 - _RefCen) * (1 - uvgrab.y) / (1 - _RefCen) * (3 + _Compression * 5);
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
				return fixed4(reflect(i.uv), 1);
			}
			ENDCG
		}

		Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float2 _Offsets;

            struct v2f
            {
                float4 pos : POSITION;
                half2 uv : TEXCOORD0;
                half4 uv01 : TEXCOORD1;
                half4 uv23 : TEXCOORD2;
                half4 uv45 : TEXCOORD3;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;

                o.uv01 = v.texcoord.xyxy + _Offsets.xyxy * float4(1, 1, -1, -1);
                o.uv23 = v.texcoord.xyxy + _Offsets.xyxy * float4(1, 1, -1, -1) * 2;
                o.uv45 = v.texcoord.xyxy + _Offsets.xyxy * float4(1, 1, -1, -1) * 3;

                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                fixed4 color = fixed4(0, 0, 0, 0);
                color += 0.40 * tex2D(_MainTex, i.uv);
                color += 0.15 * tex2D(_MainTex, i.uv01.xy);
                color += 0.15 * tex2D(_MainTex, i.uv01.zw);
                color += 0.10 * tex2D(_MainTex, i.uv23.xy);
                color += 0.10 * tex2D(_MainTex, i.uv23.zw);
                color += 0.05 * tex2D(_MainTex, i.uv45.xy);
                color += 0.05 * tex2D(_MainTex, i.uv45.zw);
                return color;
            }

            ENDCG
        }

        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _OriginTex;
            float4 _OriginTex_ST;

            struct v2f
            {
                float4 pos : POSITION;
                half2 uv : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                return tex2D(_OriginTex, i.uv) + tex2D(_MainTex, i.uv);
            }

            ENDCG
        }
	}
}