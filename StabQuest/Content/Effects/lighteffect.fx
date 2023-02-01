sampler s0;

texture lightMask;
sampler lightSampler = sampler_state 
{ 
    texture = <lightMask>;
};

float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);
    float4 lightColor = tex2D(lightSampler, coords);
    return color * lightColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderLight();
    }
}

//
//#if OPENGL
//#define SV_POSITION POSITION
//#define VS_SHADERMODEL vs_3_0
//#define PS_SHADERMODEL ps_3_0
//#else
//#define VS_SHADERMODEL vs_4_0_level_9_1
//#define PS_SHADERMODEL ps_4_0_level_9_1
//#endif
//
//Texture2D Texture : register(t0);
//sampler TextureSampler : register(s0)
//{
//	Texture = (Texture);
//};
//
//Texture2D LightMap;
//sampler LightMapSampler
//{
//	Texture = <LightMap>;
//};
//
//float4 Main(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
//{
//	float4 mainColor = tex2D(TextureSampler, texCoord);
//	float4 lightColor = tex2D(LightMapSampler, texCoord);
//	return mainColor * lightColor;
//}
//
//technique BasicColorDrawing
//{
//	pass P0
//	{
//		PixelShader = compile PS_SHADERMODEL Main();
//	}
//}