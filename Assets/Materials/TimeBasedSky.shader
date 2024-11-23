Shader "Custom/TimeBasedSky"
{
    Properties
    {
        _SkyColorMorning ("Morning Sky Color", Color) = (0.53, 0.81, 0.92, 1)
        _SkyColorNoon ("Noon Sky Color", Color) = (0.00, 0.75, 1.00, 1)
        _SkyColorEvening ("Evening Sky Color", Color) = (1.00, 0.27, 0.00, 1)
        _SkyColorNight ("Night Sky Color", Color) = (0.10, 0.10, 0.44, 1)


        _CustomTime ("Custom Time (0-1)", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _SkyColorMorning, _SkyColorNoon, _SkyColorEvening, _SkyColorNight;
            float _CustomTime;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float timeOfDay = _CustomTime;

                float Sunrise = 0.23;
                float SunMid = 0.25;
                float SunSet = 0.77;
                float SunFall = 0.79;
                float FallDown = 0.81;

                float4 skyColor;

                if(timeOfDay <= Sunrise || timeOfDay >= FallDown){
                    skyColor.rgb = _SkyColorNight;
                }

                else if(timeOfDay >= Sunrise && timeOfDay <= SunMid){
                    
                    skyColor.rgb = lerp(_SkyColorMorning, _SkyColorNoon, saturate((timeOfDay - Sunrise) / (SunMid - Sunrise)));
                    }

                else if(timeOfDay >= SunMid && timeOfDay <= SunSet){
                    
                    skyColor.rgb = _SkyColorNoon;
                    }

                else if(timeOfDay >= SunSet && timeOfDay <= SunFall){
                    skyColor.rgb = lerp(_SkyColorNoon, _SkyColorEvening, saturate((timeOfDay - SunSet) / (SunFall - SunSet)));
                    
                    }

                else {
                    skyColor.rgb = lerp(_SkyColorEvening, _SkyColorNight, saturate((timeOfDay - SunFall) / (FallDown - SunFall)));
                    
                    }


                return skyColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
