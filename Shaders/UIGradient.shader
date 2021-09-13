Shader "Zigurous/UI/Gradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)

        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255

        [HideInInspector] _ColorMask ("Color Mask", Float) = 15

        _Rotation ("Rotation", Range(0, 180)) = 0

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        [HideInInspector] _Color0 ("Color 0", Color) = (1, 1, 1, 1)
        [HideInInspector] _Color1 ("Color 1", Color) = (1, 1, 1, 1)
        [HideInInspector] _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        [HideInInspector] _Color3 ("Color 3", Color) = (1, 1, 1, 1)
        [HideInInspector] _Color4 ("Color 4", Color) = (1, 1, 1, 1)
        [HideInInspector] _Color5 ("Color 5", Color) = (1, 1, 1, 1)
        [HideInInspector] _Color6 ("Color 6", Color) = (1, 1, 1, 1)
        [HideInInspector] _Color7 ("Color 7", Color) = (1, 1, 1, 1)

        [HideInInspector] _ColorTime0 ("Color Time 0", Float) = 0
        [HideInInspector] _ColorTime1 ("Color Time 1", Float) = 0
        [HideInInspector] _ColorTime2 ("Color Time 2", Float) = 0
        [HideInInspector] _ColorTime3 ("Color Time 3", Float) = 0
        [HideInInspector] _ColorTime4 ("Color Time 4", Float) = 0
        [HideInInspector] _ColorTime5 ("Color Time 5", Float) = 0
        [HideInInspector] _ColorTime6 ("Color Time 6", Float) = 0
        [HideInInspector] _ColorTime7 ("Color Time 7", Float) = 0

        [HideInInspector] _Colors ("Colors", Int) = 0

        [HideInInspector] _Alpha0 ("Alpha 0", Float) = 0
        [HideInInspector] _Alpha1 ("Alpha 1", Float) = 0
        [HideInInspector] _Alpha2 ("Alpha 2", Float) = 0
        [HideInInspector] _Alpha3 ("Alpha 3", Float) = 0
        [HideInInspector] _Alpha4 ("Alpha 4", Float) = 0
        [HideInInspector] _Alpha5 ("Alpha 5", Float) = 0
        [HideInInspector] _Alpha6 ("Alpha 6", Float) = 0
        [HideInInspector] _Alpha7 ("Alpha 7", Float) = 0

        [HideInInspector] _AlphaTime0 ("Alpha Time 0", Float) = 0
        [HideInInspector] _AlphaTime1 ("Alpha Time 1", Float) = 0
        [HideInInspector] _AlphaTime2 ("Alpha Time 2", Float) = 0
        [HideInInspector] _AlphaTime3 ("Alpha Time 3", Float) = 0
        [HideInInspector] _AlphaTime4 ("Alpha Time 4", Float) = 0
        [HideInInspector] _AlphaTime5 ("Alpha Time 5", Float) = 0
        [HideInInspector] _AlphaTime6 ("Alpha Time 6", Float) = 0
        [HideInInspector] _AlphaTime7 ("Alpha Time 7", Float) = 0

        [HideInInspector] _Alphas ("Alphas", Int) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex        : SV_POSITION;
                fixed4 color         : COLOR;
                float2 texcoord      : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float  _Rotation;

            float4 _Color0, _Color1, _Color2, _Color3, _Color4, _Color5, _Color6, _Color7;
            float  _Alpha0, _Alpha1, _Alpha2, _Alpha3, _Alpha4, _Alpha5, _Alpha6, _Alpha7;
            float  _ColorTime0, _ColorTime1, _ColorTime2, _ColorTime3, _ColorTime4, _ColorTime5, _ColorTime6, _ColorTime7;
            float  _AlphaTime0, _AlphaTime1, _AlphaTime2, _AlphaTime3, _AlphaTime4, _AlphaTime5, _AlphaTime6, _AlphaTime7;
            int    _Colors, _Alphas;

            float4 get_gradient (float2 texcoord)
            {
                float4 cc[8] = { _Color0, _Color1, _Color2, _Color3, _Color4, _Color5, _Color6, _Color7 };
                float  aa[8] = { _Alpha0, _Alpha1, _Alpha2, _Alpha3, _Alpha4, _Alpha5, _Alpha6, _Alpha7 };
                float  ct[8] = { _ColorTime0, _ColorTime1, _ColorTime2, _ColorTime3, _ColorTime4, _ColorTime5, _ColorTime6, _ColorTime7 };
                float  at[8] = { _AlphaTime0, _AlphaTime1, _AlphaTime2, _AlphaTime3, _AlphaTime4, _AlphaTime5, _AlphaTime6, _AlphaTime7 };

                float4 cv1 = cc[0], cv2 = cc[_Colors - 1];
                float  ct1 = ct[0], ct2 = ct[_Colors - 1];
                float  av1 = aa[0], av2 = aa[_Alphas - 1];
                float  at1 = at[0], at2 = at[_Alphas - 1];

                float t = texcoord.x;

                for (int i = 0; i < _Colors; i++)
                {
                    if (ct[i] > t)
                        break;

                    cv1 = cc[i];
                    ct1 = ct[i];
                }

                for (int j = 0; j < _Colors; j++)
                {
                    if (ct[j] < t)
                        continue;

                    cv2 = cc[j];
                    ct2 = ct[j];
                    break;
                }

                for (int k = 0; k < _Alphas; k++)
                {
                    if (at[k] > t)
                        break;

                    av1 = aa[k];
                    at1 = at[k];
                }

                for (int l = 0; l < _Alphas; l++)
                {
                    if (at[l] < t)
                        continue;

                    av2 = aa[l];
                    at2 = at[l];
                    break;
                }

                float  lerpA = (t - at1) / (at2 - at1);
                float  lerpC = (t - ct1) / (ct2 - ct1);
                float4 finalC = lerp(cv1, cv2, lerpC);
                float4 finalA = lerp(av1, av2, lerpA);
                finalC.a = finalA;

                return finalC;
            }

            v2f vert (appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _Color;

                const float Deg2Rad = (UNITY_PI * 2.0) / 360.0;
                float rotationRadians = _Rotation * Deg2Rad;
                float s = sin(rotationRadians);
                float c = cos(rotationRadians);
                float2x2 rotationMatrix = float2x2(c, -s, s, c);
                OUT.texcoord.xy = mul(v.texcoord.xy, rotationMatrix);

                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                float2 texcoord = (IN.texcoord - _MainTex_ST.zw) / _MainTex_ST.xy;
                float4 gradient = get_gradient(texcoord);
                return color * gradient;
            }

            ENDCG

        }

    }

    CustomEditor "Zigurous.UI.Editor.UIGradientShaderGUI"
}
