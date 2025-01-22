Shader "Custom/RawImageShader"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            sampler2D _MainTex; // La RenderTexture
            float4 _MainTex_TexelSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Leer el valor de la RenderTexture (R16G16B16A16_SFLOAT)
                float4 color = tex2D(_MainTex, i.uv);
                
                // Convertir el valor de punto flotante a un rango de 0 a 1
                color = saturate(color);  // Asegurarse de que esté en el rango [0, 1]
                return color;
            }
            ENDCG
        }
    }
}
