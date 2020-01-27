Shader "URP/ToonCuts"
{
    Properties
    {
		_AmbientColor("Ambient Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
        _BaseColor("Base Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
		_SpecColor("Specular Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
		_Shininess("Shininess", Range(1, 15)) = 1

		_RimColor("Rim Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
		_RimPower("Rim Power", Range(0, 5)) = 0.5
		_RimTreshold("Rim Treshold", Range(0, 5)) = 0.5


		_Cuts("Cuts", Range(1, 15)) = 3.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "RenderPipeline"="UniversalPipeline"}
        LOD 100

        Pass
        {
		    Tags { "LightMode" = "UniversalForward" }
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


			float4 _BaseColor, _AmbientColor;
			float4 _SpecColor;
			float4 _RimColor;
			float _RimPower;
			float _Shininess;
			float _RimTreshold;
			float _Cuts;

			struct Attributes
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;				
			
			};

			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
			};

			Varyings vert (Attributes input)
			{
				Varyings output = (Varyings)0;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS);
				VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
				float3 viewDirWS = SafeNormalize(GetCameraPositionWS() - vertexInput.positionWS);

				output.positionCS = vertexInput.positionCS;
				output.normal = NormalizeNormalPerVertex(normalInput.normalWS);
				output.viewDir = viewDirWS;

				return output;
			}

			float4 frag(Varyings input) : SV_TARGET
			{
				float3 lightDirection = normalize(_MainLightPosition.xyz);
				float NdotL = dot(lightDirection, input.normal);
				float diffuseReflection = smoothstep(0.0, 0.01, NdotL);
				float3 diffuseToon = floor(NdotL * _Cuts) * _MainLightColor.rgb;
				float3 albedo = diffuseToon;


				float3 halfVector = SafeNormalize(lightDirection + input.viewDir);
				float NdotH = saturate(dot(input.normal, halfVector));
				float specularReflection = pow(NdotH, _Shininess * _Shininess);
				float specularIntensity = smoothstep(0.005, 0.01, specularReflection);
				float3 specularToon = specularIntensity * _SpecColor.rgb;		
				albedo += specularToon;

				float rim = 1 - saturate(dot(input.viewDir, input.normal));				
				float rimIntensity = pow(rim, _RimPower) * pow(NdotL, _RimTreshold);
				rimIntensity = smoothstep(0.59, 0.61, rimIntensity);
				rimIntensity = floor(rimIntensity * _Cuts);
				float3 rimColor = rimIntensity * _RimColor;
				albedo += rimColor;

				albedo += _AmbientColor;
				return float4(albedo * _BaseColor.rgb, 1.0);

			}

			ENDHLSL
        }
    }
}
