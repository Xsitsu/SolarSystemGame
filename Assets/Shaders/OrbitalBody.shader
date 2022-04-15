Shader "Unlit/OrbitalBody"
{
    Properties
    {
        _LightDirection ("Light Source Direction", Vector) = (0, 0, 0, 0)
        _TerrainTexture ("Texture", 2D) = "white" {}
        _LightTexture ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Ambience ("Ambience", Float) = 0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPosition : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _TerrainTexture;
            float4 _TerrainTexture_ST;

            sampler2D _LightTexture;
            float4 _LightTexture_ST;

            fixed4 _Color;
            float _Ambience;
            float4 _LightDirection;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _TerrainTexture);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;

                fixed3 lightDirection = normalize(_LightDirection.xyz);
                float intensity = -1 * dot(lightDirection, i.worldNormal);
                if (intensity > 0) // Day side
                {
                    intensity = ((1 - _Ambience) * intensity * intensity) + _Ambience;
                    col = tex2D(_TerrainTexture, i.uv);
                }
                else // Night side
                {
                    intensity = _Ambience;
                    col = tex2D(_TerrainTexture, i.uv);
                }

                col = col * intensity;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
