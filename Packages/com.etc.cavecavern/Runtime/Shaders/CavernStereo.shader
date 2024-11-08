Shader "ETC/CavernStereo"
{
    Properties
    {
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
        _NProjectors("Projector Count", Float) = 0
        _DisplayIdx("Display Index", Int) = 0
       // _StereoMode("Stereo Mode (0: use VR settings, 1: TnB, 2: SbS, 3: Anaglyph, 4: VR settings + distortion correction, 5: VR settings + rotation)", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        // Can I have passes for URP and BIRP in the same shader?
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            // Debug mode
            #pragma multi_compile _ DEBUG_COLOR
            // Todo: HLSL bby
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
                int eyeIdx : TEXCOORD1;
            };

            uniform int _NPanels;
            uniform float _Radius;
            uniform float _Rotation;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _NProjectors;
            int _DisplayIdx;
            // Stereo mode is only set via global shader property
            // Todo: Split each into pragma? This is a minor optimization
            int _StereoMode;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.eyeIdx = unity_StereoEyeIndex;
                return o;
            }

            float luminance(float3 rgb){
                return (rgb.r * 0.3) + (rgb.g * 0.59) + (rgb.b * 0.11);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv;
                // Adjust our u based on projector index
                if (_StereoMode == 5 || _StereoMode == 3 || _StereoMode == 1) {
                    uv.x = (_Rotation / 360.0 + (i.uv.x + _DisplayIdx) / (_NProjectors * 4 / 3)) % 1;

                    // Distortion correct
                    float b = floor(uv.x * _NPanels) / _NPanels;
                    float x0 = _NPanels * (uv.x - b);
                    float alpha = 3.14159 * (_NPanels - 2) / (2 * _NPanels);
                    float s = 2* _Radius * sin(3.14159 / _NPanels);
                    float xp = s * x0;
                    float l = sqrt(xp * xp + _Radius * _Radius - 2 * xp * _Radius * cos(alpha));
                    float theta = asin(xp * sin(alpha) / l);
                    float x1 = _NPanels * theta / (2 * 3.14159);
                    uv.x = b + x1 / _NPanels;
                }
                else
                        uv.x = (i.uv.x + _DisplayIdx) / _NProjectors;

                // Correct u if we're doing distortion correction
                if (_StereoMode == 4) {
                    float p0 = (uv.x * (_NPanels / _NProjectors)) % 1;
                    float x0 = 2 * p0 * _Radius * tan(3.14159f  / _NPanels);
                    float alpha = 3.14159 * (_NPanels - 2) / (2 * _NPanels);
                    float r2 = _Radius * sqrt(1 + pow(tan(3.14159 / _NPanels),2));
                    float theta = asin(x0 * sin(alpha) / sqrt(pow(x0, 2) + pow(r2, 2) - 2 * x0 * r2 * cos(alpha)));
                    float p1 = _NPanels * theta / (2 * 3.14159);
                    uv.x = (floor(uv.x * (_NPanels / _NProjectors)) + p1) / (_NPanels / _NProjectors);
                }


                fixed2 anaglyphCol = fixed2(0,0);
                // Adjust our v based on eye
                if (_StereoMode == 0 || _StereoMode == 4 || _StereoMode == 5){

                    if (i.eyeIdx == 1)
                        uv.y = 0.5 + 0.5 * i.uv.y;
                    else
                        uv.y = 0.5 * i.uv.y;

                    // #ifdef DEBUG_COLOR
                    // if (i.eyeIdx == 1)
                    //     col *= float(1, 0, 0, 1);
                    // else
                    //     col *= float(0, 0, 1, 1);
                    //     #endif

                }else if (_StereoMode == 3){
                    float3 leftEye = tex2D(_MainTex, fixed2(uv.x, 0.5 + 0.5 * i.uv.y));
                    float3 rightEye = tex2D(_MainTex, fixed2(uv.x, 0.5 * i.uv.y));
                    anaglyphCol = fixed2(luminance(leftEye), luminance(rightEye));
                }else{
                    uv.y = i.uv.y;
                }

                // Alternative frame packing
                if (_StereoMode == 2) {
                    uv.x = uv.x < 0.5 ? 2 * uv.x : 2 * (uv.x - 0.5);
                    uv.y = uv.x < 0.5 ? (uv.y / 2) + 0.5 : uv.y / 2;
                }

                // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                if (_StereoMode == 3)
                    col = fixed4(anaglyphCol.x, anaglyphCol.y, 0, 0);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                // If debug color is defined, render top half red and bottom half blue
                #ifdef DEBUG_COLOR
                if(i.uv.y > 0.5)
                    col *= float4(1, 0, 0, 1);
                else
                    col *= float4(0, 0, 1, 1);
                #endif

                return col;
            }
            ENDCG
        }
    }
}
