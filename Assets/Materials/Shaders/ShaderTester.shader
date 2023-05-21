Shader "Unlit/ShaderTester"
{
     Properties
    {
        _Albedo("Albedo", Color) = (1,1,1,1) 
        _Shades("Shades", Range(1,20)) = 3

        _OutlineColor("OutlineColor", Color) = (0, 0, 0, 0)
        _OutlineSize("OutlineSize", float) = 1.0
    }



    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

         Pass
        {
         /*   Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // PBR lighting model, and enabling shadows
          //  #pragma surface surf Standard fulldorwardshadows

            // Shader model Target 3.0
            #pragma target 3.0

            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            // Texture Input GUI Spots

            struct Input
            {
                
            };
            //Blending Variables


            // Shades of Range
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _OutlineColor; 
            float _OutlineSize;

            v2f vert (appdata v)
            {
                v2f o;
                // Translate the vertex along the normal vector
                // Increases the size of the model
                o.vertex = UnityObjectToClipPos(v.vertex + _OutlineSize * v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                return _OutlineColor;
            }*/


//Start of OutlineShader Sript
#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

TEXTURE2D_Sampler2D(_MainTex, sampler_MainTex);
float4 _MainTex_TexelSize;

TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
float4x4 unity_MatrixMVP;

half _MinDepth;
half _MaxDepth;
half _Thickness;
half4 _EdgeColor;

struct v2d 
{
 float2 uv : TEXCOORD0;
 float4 vertex : SV_POSITION;
 float3 screen_pos : TEXCOORD2;
};

inline float4 CumputeScreenPos(float4 pos) 
{
 float4 o = pos * 0.5f;
 o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
 o.zw = pos.zw;
 return o;
}

v2f Vert(AttributesDefualt v) 
{
 v2f o;
 o.vertex = float4(v.vertex.xy, 0.0, 1.0);
 o.uv = TransformTriangleVertexToUV(v.vertex.xy);
 o.screen_pos = ComputeScreenPos (o.vertex);
 #if UNITY_UV_STARTS_AT_TOP
 o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
 #endif

 return o;
}

float4 Frag(v2f i) : SV_Target 
{
  float4 original = SAMPLE_TEXTURE2D(_MainTex, samplet_MainTEx, i.uv);
  //For testing;
  float4 depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTExture, samplet_CameraDepthTexture, i.uv);

  // For sample UV points
  float offset_positive = +ceil(_Thickness * 0.5);
  float offset_negative = -floor(_Thickness * 0.5);
  float left = _MainTex_TexelSize.x * offset_negative;
  float right = _MainTex_TexelSize.x * offset_positive;
  float top = _MainTex_TexelSize.y * offset_negative;
  float bottom = MainTex_TexelSize.y * offset_positive;
  float2 uv0 = i.uv + float2(left, top);
  float2 uv1 = i.uv + float2(right, bottom);
  float2 uv2 = i.uv + float2(right, top);
  float2 uv3 = i.uv + float2(left,bottom);

  //Sample Depth Texture
  float d0 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv0));
  float d1 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv1));
  float d2 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv2));
  float d3 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv3));

  float d = length(float2(d1 - d0, d3 - d2));
  d = smoothstep(_MinDepth, _MaxDepth, d);
  half4 output = d;

  return output;

}



[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.AfterStack, "Outline")]

//Effect Settings
public sealed class PostProcessOutline : PostProcessEffectSettings
{
    public FloatParameter thickness = new FloatParameter { value = 1f };
    public FloatParameter depthMin = new FloatParameter { value = 0f };
    public FloatParameter depthMax = new FloatParameter { value = 1f };
}

public class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline> 
{
    public override void Render(PostProcessRenderContext context) 
    {
        PropertySheet sheet = context.propertySheets.Get(Shader.Find("Hidden/Outline"));
        sheet.properties.SetFloat("_Thickness", settings.thickness);
        sheet.properties.SetFloat("_MinDepth", settings.depthMin);
        sheet.properties.SetFloat("_MaxDepth", settings.depthMax);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

    }


            ENDCG

        }
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;

            };

            float4 _Albedo;

            float _Shades;

            v2f vert (appdata v)
            {
                v2f o;
               // o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }



             


            fixed4 frag (v2f i) : SV_Target
            {
                //Calculate the "COS" between normal vector and light Direction
                //The world space light Direction is stored in _WorldSpaceLightPos0
                //The world space normal is stored in i.worldNormal
                //normalize both vectors and Calculate the dot product
                float cosineAngle = dot(normalize(i.worldNormal), normalize(_WorldSpaceLightPos0.xyz));

                // Set the min to zero as negatives are behind the shaded points

                cosineAngle = max(cosineAngle, 0.0); 

                cosineAngle = floor(cosineAngle * _Shades) / _Shades;

                return _Albedo * cosineAngle;

                // Lighting Direction ?
                inline float3 ObjSpaceLightDir( in float4 v )
{
    float3 objSpaceLightPos = mul(unity_WorldToObject, _WorldSpaceLightPos0).xyz;
    #ifndef USING_LIGHT_MULTI_COMPILE
        return objSpaceLightPos.xyz - v.xyz * _WorldSpaceLightPos0.w;
    #else
        #ifndef USING_DIRECTIONAL_LIGHT
        return objSpaceLightPos.xyz - v.xyz;
        #else
        return objSpaceLightPos.xyz;
        #endif
    #endif
}
            }
            ENDCG
        }
    }

    Fallback "VertexLit"
}
