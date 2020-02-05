Shader "Custom/VertexColor"
{
    SubShader
    {
        CGPROGRAM

        #pragma surface surf Lambert

        struct Input
        {
            float4 vertColor : COLOR;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = IN.vertColor;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
