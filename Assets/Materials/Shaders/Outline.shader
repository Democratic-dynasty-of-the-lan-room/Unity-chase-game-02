Shader "Unlit/Outline"
{
    Properties
    {
        _Albede ("Albedo", Color) = (1,1,1,1)
        [Enum(None,0,Add,1,Multiply,2, Subtract,3)] _Blend ("Blend mode subset", Int) = 0
        _MainTex ("Texture", 2D) = "white" {}

        _Blend("Blend", Range(0,1)) = 0.0
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
           // #pragma multi_compile_fog
            // Shader model Target 3.0
            #pragma target 3.0
            // resume Unity Stuffs
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

             //Blending Variables

            half _Blend;

          

                struct Input 
                {
                     float2 uv_MainTex;
                 float2 uv_MainTex2;
                };

            // Resume Normal Texcoords
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 _Albedo;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;


            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
