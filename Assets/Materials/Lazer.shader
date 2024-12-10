Shader "Custom/Lazer"
{
    Properties
    {
        _TintColor("Tint Color", Color) = (1, 1, 1, 1)
        _FadeSpeed("Fade Speed", Range(0, 10)) = 1.0
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                    float4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _TintColor;
                float _FadeSpeed;

                struct Input {
                    float2 uv_MainTex;
                };

                void serf (Input IN, inout SurfaceOutputStandard o) 
                {

                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = _TintColor;
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 texColor = tex2D(_MainTex, i.uv) * i.color;
                    float alpha = max(1.0 - _FadeSpeed * (_Time - _Time.y), 0.0);
                    texColor.a *= alpha;
                    return texColor;
                }
                ENDCG
            }
        }
        FallBack "Diffuse"
}