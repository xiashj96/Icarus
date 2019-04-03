Shader "Custom/EdgeRays"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _DifferencingScaleX ("Differencing Scale X", Range(0, 0.01)) = 0
        _DifferencingScaleY ("Differencing Scale Y", Range(0, 0.01)) = 0.00239
        _BlurOffsetX ("Blur Offset X", Range(0, 0.01)) = 0.0035
        _BlurOffsetY ("Blur Offset Y", Range(0, 0.01)) = 0.0017
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _StartPointU ("Start Point U", Range(0, 1)) = 0.5
        _StartPointV ("Start Point V", Range(0, 1)) = 0.68
        _Strength("_Strength", Range(0, 5)) = 0.9
        _AttenuateRatio("Attenuate Ratio", Range(0, 1)) = 0.4
        _DistanceAttenuation("Distance Attenuation", Range(0, 2)) = 1.52

    }
    SubShader
    {
        // 在所有不透明对象之后绘制自己，更加靠近屏幕
        Tags{"Queue"="Transparent"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            half _DifferencingScaleX;
            half _DifferencingScaleY;

            struct v2f
            {
                float4 pos : POSITION;
                half2 uv : TEXCOORD0;
            };

            fixed luminance(fixed3 color)
            {
                return color.r * 0.212 + color.g * 0.715 + color.b * 0.072;
            }

            half sobel(half2 uv)
            {
                const half Gx[9] =
                {
                    -1,0,0,
                    0,1,0,
                    0,0,0
                };

                const half Gy[9]=
                {
                    0,1,0,
                    -1,0,0,
                    0,0,0
                };

                const half2 duv[9]=
                {
                    half2(-1, -1), half2(0, -1), half2(1, -1),
                    half2(-1,  0), half2(0,  0), half2(1,  0),
                    half2(-1,  1), half2(0,  1), half2(1,  1)
                };

                half edgeX = 0;
                half edgeY = 0;

                for(int i = 0; i < 9; i++)
                {
                    half lum = luminance(tex2D(_MainTex, uv + duv[i] * half2(_DifferencingScaleX, _DifferencingScaleY)).rgb);

                    edgeX += lum * Gx[i];
                    edgeY += lum * Gy[i];
                }

                return saturate(sqrt(edgeX * edgeX + edgeY * edgeY));
            }

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                half G = sobel(i.uv);
                return fixed4(G, G, G, 1.0f);
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

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            struct v2f
            {
                float4 pos : POSITION;
                half4 uv : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.texcoord, _NoiseTex);
                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                float G = tex2D(_MainTex, i.uv).r * tex2D(_NoiseTex, i.uv.zw - floor(i.uv.zw)).r;
                return fixed4(G, G, G, 1.0f);
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

            half _StartPointU;
            half _StartPointV;

            float _Strength;
            float _AttenuateRatio;

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
                float G = 0;
                half2 vec = half2(_StartPointU, _StartPointV) - i.uv;
                float len = length(vec);
                int cnt = 25;
                float stepLength = 0.0005;
                half2 step = vec / len * stepLength;

                [unroll(40)]
                for(int it = 0; it < cnt; it++)
                {
                    if(it * stepLength > len )
                        break;
                    half2 pos = i.uv + step * it;
                    float tmp = max(tex2D(_MainTex, pos) * _Strength - _AttenuateRatio * ( log(length(vec) / length(vec - step * it))), 0);
                    G = max(G, tmp);
                }

                return fixed4(G, G, G, 1.0f);

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

            half _StartPointU;
            half _StartPointV;

            float _AttenuateRatio;
            float _DistanceAttenuation;

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
                float G = 0;
                half2 vec = half2(_StartPointU, _StartPointV) - i.uv;
                float len = length(vec);
                int cnt = 25;
                float stepLength = 0.0005;
                half2 step = vec / len * stepLength;
                [unroll(40)]
                for(int it = 0; it < cnt * cnt; it += cnt)
                {
                    if(it * stepLength > len )
                        break;
                    half2 pos = i.uv + step * it;
                    float tmp = max(tex2D(_MainTex, pos).r - _AttenuateRatio * ( log(length(vec) / length(vec - step * it)) ), 0);
                    G = max(G, tmp);
                }

                G *= saturate(1 - length(vec) * _DistanceAttenuation);

                float3 tex = tex2D(_OriginTex, i.uv);
                return fixed4(tex * (1 - G) + G, 1.0f);

            }

            ENDCG
        }

    }
    FallBack "Diffuse"
}
