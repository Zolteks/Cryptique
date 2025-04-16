Shader "Hidden/ColorBlindness"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _Mode("Mode", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "ColorBlindness"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                int _Mode;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            float3 ApplyColorBlindness(float3 color, float mode)
            {
                float3x3 mat;

                if (mode == 1.0)
                {
                    mat = float3x3(
                        0.567, 0.433, 0.0,
                        0.558, 0.442, 0.0,
                        0.0,   0.242, 0.758
                    );
                }
                else if (mode == 2.0)
                {
                    mat = float3x3(
                        0.625, 0.375, 0.0,
                        0.7,   0.3,   0.0,
                        0.0,   0.3,   0.7
                    );
                }
                else if (mode == 3.0)
                {
                    mat = float3x3(
                        0.95, 0.05,  0.0,
                        0.0,  0.433, 0.567,
                        0.0,  0.475, 0.525
                    );
                }
                else
                {
                    return color;
                }

                return mul(mat, color);
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                color.rgb = ApplyColorBlindness(color.rgb, _Mode);
                return color;
            }

            ENDHLSL
        }
    }
}
