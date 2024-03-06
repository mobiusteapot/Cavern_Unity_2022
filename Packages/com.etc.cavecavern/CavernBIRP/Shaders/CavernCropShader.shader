Shader "Hidden/CropScreenOutput"
{
    Properties
    {
        [MainTexture] _MainTex("Texture", 2D) = "white" {}
        _CropRegion("CropRegion", Vector) = (1,1,1,1)
        _StretchRegion("StretchRegion", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque"}
        LOD 100
        ZWrite Off Cull Off
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
    
            #pragma multi_compile_fog
			#pragma multi_compile_local _ DEBUG_COLOR

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _DebugTint;
			float4 _CropRegion;
            float4 _StretchRegion;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                i.uv = i.uv * _CropRegion.zw + _CropRegion.xy;
                
                float2 oguv = i.uv;
    
    if (i.uv.y > 0.5)
    {
        i.uv.y += _StretchRegion.y;
        i.uv.y *= _StretchRegion.w;
    }
    else
    {
        i.uv.y += _StretchRegion.x;
        i.uv.y *= _StretchRegion.z;
    }
                fixed4 col = tex2D(_MainTex, i.uv);
                if (DEBUG_COLOR)
                {
                    fixed4 uvCropTest = fixed4(0,1,0,1);
                    fixed4 redDebugTint = fixed4(1, 0, 0, 1);
                    fixed4 blueDebugTint = fixed4(0, 0, 1, 1);
                    if (oguv.y > 0.5)
                    {
                        // return uvCropTest;
                        col *= redDebugTint;

                    }
                    else
                    {
                        col *= blueDebugTint;            
                    }
                }
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}