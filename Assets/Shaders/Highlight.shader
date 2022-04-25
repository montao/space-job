Shader "Unlit/Highlight"
{
    Properties
    {
        _HighlightColor("Color", Color) = (0.6, 1, 0.6, 0.7)
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha  // Enable transparency
        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            fixed4 _HighlightColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _HighlightColor;

                uint ypos = uint(i.vertex.y);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                if ((ypos >> 2) % 2 == 0) {
                    return 1.3 * col;
                } else {
                    return col;
                }
            }
            ENDCG
        }
    }
}
