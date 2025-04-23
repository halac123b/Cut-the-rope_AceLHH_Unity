Shader "UI/SpotlightHole"
{
    Properties
    {
        _Color("Overlay Color", Color) = (0,0,0,0.7)
        _MaskTex("Spotlight Mask", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

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

            sampler2D _MaskTex;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float maskAlpha = tex2D(_MaskTex, i.uv).a;
                float finalAlpha = _Color.a * (1.0 - maskAlpha); // subtract vùng mask
                return fixed4(_Color.rgb, finalAlpha);
            }
            ENDCG
        }
    }
}
