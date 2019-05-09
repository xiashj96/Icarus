Shader "Custom/Trail" 
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
        _Opacity ("Opacity", Float) = 1.0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }

		Pass
		{

            Tags{"LightMode" = "ForwardBase"}
            zWrite off
            Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 vertColor : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float _Opacity;

			v2f vert(appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.vertColor = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
                fixed4 tex = tex2D(_MainTex, i.uv);
                tex.a = tex.a * _Opacity;
				return tex * i.vertColor;
			}
			ENDCG
		}

	}
}
