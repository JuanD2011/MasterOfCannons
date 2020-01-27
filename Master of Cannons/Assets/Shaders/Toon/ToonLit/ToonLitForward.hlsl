#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes
{
    float4 positionOS    : POSITION;
    float3 normalOS      : NORMAL;
    float4 tangentOS     : TANGENT;
    float2 texcoord      : TEXCOORD0;
    float2 lightmapUV    : TEXCOORD1;
};

struct Varyings
{
    float2 uv                       : TEXCOORD0;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

    float3 posWS                    : TEXCOORD2;    // xyz: posWS

    float4 normal                   : TEXCOORD3;    // xyz: normal, w: viewDir.x
    float4 tangent                  : TEXCOORD4;    // xyz: tangent, w: viewDir.y
    float4 bitangent                : TEXCOORD5;    // xyz: bitangent, w: viewDir.z

    half4 fogFactorAndVertexLight   : TEXCOORD6; // x: fogFactor, yzw: vertex light

    float4 shadowCoord              : TEXCOORD7;

    float4 positionCS               : SV_POSITION;
};

void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
    inputData.positionWS = input.posWS;

    half3 viewDirWS = half3(input.normal.w, input.tangent.w, input.bitangent.w);
    inputData.normalWS = TransformTangentToWorld(normalTS,
        half3x3(input.tangent.xyz, input.bitangent.xyz, input.normal.xyz));


    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    viewDirWS = SafeNormalize(viewDirWS);

    inputData.viewDirectionWS = viewDirWS;
    inputData.shadowCoord = input.shadowCoord;

    inputData.fogCoord = input.fogFactorAndVertexLight.x;
    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
    inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
}


half4 ToonLight(InputData inputData, half3 diffuse, half4 specularGloss, half smoothness, half alpha)
{
	float3 finalColor;
    Light mainLight = GetMainLight(inputData.shadowCoord);
    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));
    half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);

	//DIFFUSE
	float NdotL = dot(mainLight.direction, inputData.normalWS);
	float halfLambert = NdotL * 0.45 + 0.45;
	float diffuseReflection = smoothstep(0.65, 0.66, halfLambert);	
	float3 diffuseColor = inputData.bakedGI + (diffuseReflection * attenuatedLightColor); 

	//SPECULAR
	float3 halfVector = SafeNormalize(mainLight.direction + inputData.viewDirectionWS);
	float NdotH = saturate(dot(inputData.normalWS, halfVector));
	float specularReflection = pow(NdotH, _Shininess * _Shininess);
	float specularIntensity = smoothstep(0.0, 0.01, specularReflection);
	specularIntensity = (NdotL > 0.95) ? specularIntensity *= 3.0 : 0;
	float3 specularToon = specularIntensity * _SpecColor.rgb;

	//RIM
	float rim = 1 - saturate(dot(inputData.viewDirectionWS, inputData.normalWS));			
	float rimIntensity = pow(rim, _RimPower) * pow(NdotL, _RimTreshold);
	rimIntensity = smoothstep(0.6 - 0.01, 0.6 + 0.01, rimIntensity);
	rimIntensity = floor(rimIntensity * 5.0);
	float3 rimColor = rimIntensity * _RimColor;

	finalColor = (diffuseColor * diffuse) + specularToon + rimColor;
    return half4(finalColor, alpha);
}


Varyings LitPassVertexSimple(Attributes input)
{
    Varyings output = (Varyings)0;

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    half3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
    half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.posWS.xyz = vertexInput.positionWS;
    output.positionCS = vertexInput.positionCS;

    output.normal = half4(normalInput.normalWS, viewDirWS.x);
    output.tangent = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangent = half4(normalInput.bitangentWS, viewDirWS.z);


    OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
    OUTPUT_SH(output.normal.xyz, output.vertexSH);

    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

    output.shadowCoord = GetShadowCoord(vertexInput);

    return output;
}

half4 LitPassFragmentSimple(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.uv;
    half4 diffuseAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    half3 diffuse = diffuseAlpha.rgb * _BaseColor.rgb;

    half alpha = diffuseAlpha.a * _BaseColor.a;

    half3 normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
    half4 specular = _SpecColor;
    half smoothness = pow(_Shininess, 2);

    InputData inputData;
    InitializeInputData(input, normalTS, inputData);

    half4 color = ToonLight(inputData, diffuse, specular, smoothness, alpha);
    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    return color;
};

