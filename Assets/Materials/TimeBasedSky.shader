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

                return skyColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
