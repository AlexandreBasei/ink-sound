Shader "Custom/MetaballEffect"
{
    Properties
    {
        _MainTex ("Render Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(0, 2)) = 1
        _Smooth ("Smoothness", Range(0, 1)) = 0.1
        _BlobColor ("Color", Color) = (0.2, 0.8, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Threshold;
            float _Smooth;
            float4 _BlobColor;

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float field = tex2D(_MainTex, i.uv).r;
                float alpha = smoothstep(_Threshold - _Smooth, _Threshold + _Smooth, field);
                return float4(_BlobColor.rgb, alpha);
            }
            ENDCG
        }
    }
}
