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

                float Sunrise = 0.23;
                float SunMid = 0.25;
                float SunSet = 0.77;
                float SunFall = 0.79;
                float FallDown = 0.81;

                float4 skyColor = texColor;

                if(timeOfDay <= Sunrise || timeOfDay >= FallDown){
                    texColor.rgb = lerp(texColor, _SkyColorNight * texColor, 1);
                }

                else if(timeOfDay >= Sunrise && timeOfDay <= SunMid){
                    
                    texColor.rgb = lerp(_SkyColorMorning * texColor, _SkyColorNoon * texColor, saturate((timeOfDay - Sunrise) / (SunMid - Sunrise)));
                    }

                else if(timeOfDay >= SunMid && timeOfDay <= SunSet){
                    
                    texColor.rgb = lerp(texColor, _SkyColorNoon * texColor, 1);
                    }

                else if(timeOfDay >= SunSet && timeOfDay <= SunFall){
                    texColor.rgb = lerp(_SkyColorNoon * texColor, _SkyColorEvening * texColor, saturate((timeOfDay - SunSet) / (SunFall - SunSet)));
                    
                    }

                else {
                    texColor.rgb = lerp(_SkyColorEvening * texColor, _SkyColorNight * texColor, saturate((timeOfDay - SunFall) / (FallDown - SunFall)));
                    
                    }



                if(skyColor.a > 0.1){
                    texColor.a = texColor.a * skyColor.a;
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
