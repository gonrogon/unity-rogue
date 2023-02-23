Shader "Unlit/Shadow2"
{
    Properties
    {
        _Sun("Sun", Vector) = (1, 1, 1)
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float3 _Sun;

            v2f vert (appdata v)
            {
                float3 p = mul(unity_ObjectToWorld, v.vertex);
                float3 q = normalize(p - _Sun);
                float3 n = float3(0, 0,  1);

                float num = -dot(n, p);
                float den =  dot(n, q);
                float t   =  num / den;

                float4 sol = float4(p, 1) + float4(q, 0) * t;

                v2f o;
                o.vertex = mul(UNITY_MATRIX_VP, sol);//UnityObjectToClipPos(sol);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = fixed4(1.0, 0.0, 0.0, 1.0);

                return col;
            }
            ENDCG
        }
    }
}
