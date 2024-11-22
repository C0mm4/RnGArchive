Shader "Custom/TimeBasedCloue"
{
    Properties
    {
        _SkyColorMorning ("Morning Sky Color", Color) = (0.145098, 0.3686274, 0.5764706, 1)
        _SkyColorNoon ("Noon Sky Color", Color) = (0.7647059, 0.8784314, 0.972549, 1)
        _SkyColorEvening ("Evening Sky Color", Color) = (0.8705882, 0.6117647, 0.5137254, 1)
        _SkyColorNight ("Night Sky Color", Color) = (0.145098, 0.3686274, 0.5764706, 1)
        
        _CustomTime ("Custom Time (0-1)", Range(0, 1)) = 0

        _MainTex("Sprite texture", 2D) = "white" {}
        _UVRange("UV Range", Vector) = (0, 0, 1, 1)

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            sampler2D _MainTex;
            float4 _UVRange;
            fixed4 _SkyColorEvening, _SkyColorMorning, _SkyColorNoon, _SkyColorNight;
            float _CustomTime;

            struct appData{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                };
                
            struct v2f{
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                };


            v2f vert(appData v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
                }

            float4 frag(v2f i) : SV_Target{

                float timeOfDay = _CustomTime;
                float4 texColor = tex2D(_MainTex, i.uv);


                float morningEnd = 0.2;
                float noonEnd = 0.75;    
                float eveningEnd = 0.85;
                
                fixed4 skyColor = lerp(_SkyColorNight, _SkyColorMorning, saturate(timeOfDay / morningEnd));
                
                if (timeOfDay > morningEnd)
                {
                    skyColor = lerp(_SkyColorMorning, _SkyColorNoon, saturate((timeOfDay - morningEnd) / (noonEnd - morningEnd)));
                }
                
                if (timeOfDay > noonEnd)
                {
                    skyColor = lerp(_SkyColorNoon, _SkyColorEvening, saturate((timeOfDay - noonEnd) / (eveningEnd - noonEnd)));
                }

                if (timeOfDay > eveningEnd)
                {
                    skyColor = lerp(_SkyColorEvening, _SkyColorNight, saturate((timeOfDay - eveningEnd) / (1.0 - eveningEnd)));
                }

                if(texColor.a > 0.1){
                    texColor.rgb = lerp(texColor.rgb, skyColor * texColor.rgb, skyColor.a);

                    }
                else{
                    discard;
                    }


                return float4(texColor.rgb, texColor.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
