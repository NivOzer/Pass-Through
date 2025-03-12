Shader "Custom/PortalCircleShader"
{
    Properties
    {
        _MainTex ("Render Texture", 2D) = "white" {}
        _EdgeSmoothness ("Edge Smoothness", Range(0.01, 0.2)) = 0.05
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _EdgeSmoothness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // Smooth transition at edges
                float alpha = smoothstep(0.5, 0.5 - _EdgeSmoothness, dist);
                
                fixed4 color = tex2D(_MainTex, i.uv);
                color.a *= alpha; // Apply smooth transparency

                return color;
            }
            ENDCG
        }
    }
}
