Shader "Custom/NPCHolo"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _OverlayColor("Overlay Color", Color) = (0.5, 0.7, 1.0, 0.2)
        _LineColor("Line Color", Color) = (0.0, 0.2, 0.5, 0.3)
        _LineThickness("Line Thickness", Float) = 1
        _LineSpacing("Line Spacing", Float) = 20
        _DisplayPercentage("Display Percentage", Range(0, 1)) = 0.0
        _UVRange("UV Range", Vector) = (0, 0, 1, 1)
        _Color("Color", Color) = (1, 1, 1, 1)
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0

                sampler2D _MainTex;
                float4 _OverlayColor;
                float4 _LineColor;
                float _LineThickness;
                float _LineSpacing;
                float _DisplayPercentage;
                float4 _UVRange; // (x, y, width, height)

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    // Atlas ���� ��������Ʈ UV ���� ���� ��ǥ ��ȯ
                    float2 adjustedUV = i.uv * _UVRange.zw + _UVRange.xy;


                    // �ؽ�ó ���� �� ���� �� ��������
                    float4 texColor = tex2D(_MainTex, i.uv);

                    // ��� ������ ���� ����ŷ ó��
                    float mappedY = ((i.uv.y - _UVRange.y)/_UVRange.w - (1.0 - _DisplayPercentage)) / _DisplayPercentage;
                    if (mappedY < 0)
                    {
                        discard;
                    }


                    // ���� ���� ����� ū ��쿡�� ȿ�� ����
                    if (texColor.a > 0.1)
                    {
                        texColor.rgb = lerp(texColor.rgb, _OverlayColor.rgb * texColor.rgb, _OverlayColor.a);

                        // ��ĵ���� ȿ�� �߰�
                        float linePattern = step(_LineThickness, fmod(i.uv.y * _LineSpacing, 1.0));
                        texColor.rgb = lerp(texColor.rgb, _LineColor.rgb * texColor.rgb, linePattern * _LineColor.a);
                    }



                    return texColor;
                }

                ENDCG
            }
        }
}