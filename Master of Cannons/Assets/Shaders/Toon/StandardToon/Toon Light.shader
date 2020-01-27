Shader "URP/ToonLight"
{
    Properties
    {
		_AmbientColor("Ambient Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
        _BaseColor("Base Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
		_SpecColor("Specular Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
		_Shininess("Shininess", Range(1, 15)) = 1

		_RimColor("Rim Color", COLOR) = (1.0, 1.0, 1.0, 1.0)
		_RimTreshold("Rim Power", Range(0, 1)) = 0.5


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
			float _Shininess;
			float _RimTreshold;

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
				float NdotL = saturate(dot(lightDirection, input.normal));
				float diffuseReflection = smoothstep(0, 0.01, NdotL);
				float3 diffuseToon = diffuseReflection * _MainLightColor.rgb; 


				float3 halfVector = SafeNormalize(lightDirection + input.viewDir);
				float NdotH = saturate(dot(input.normal, halfVector));
				float specularReflection = pow(NdotH, _Shininess * _Shininess);

				float3 specularToon = smoothstep(0.005, 0.01, specularReflection) * _SpecColor.rgb;
				
				float3 albedo = diffuseToon + specularToon;

				float rim = 1 - saturate(dot(input.viewDir, input.normal));				
				float rimIntensity = rim * pow(NdotL, _RimTreshold);
				rimIntensity = smoothstep(0.6 - 0.01, 0.6 + 0.01, rimIntensity);
				float3 rimColor = rimIntensity * _RimColor * 3.0;

				albedo += rimColor + _AmbientColor;				
				return float4(albedo * _BaseColor.rgb, 1.0);

			}

			ENDHLSL
        }
    }
}
