Shader "Custom/Terrain_Shader"
{
	Properties
	{
		_MultiplySand("Multiply", Range(0, 1)) = 1
		_Sand("Sand", 2D) = "white" {}
		_SandNormal("SandNormal", 2D) = "bump" {}
		_SandHeight("Sand Height", Range(-50, 50)) = 0
		_BlendSharpness("Blend Sharpness", Range(0.06, 4)) = 0.75

		[Space(30)]
			_MultiplyGrass("Multiply", Range(0, 1)) = 1
			_Grass("Grass", 2D) = "white" {}
			_GrassNormal("SandNormal", 2D) = "bump" {}
			_GrassHeight("Grass Height", Range(0, 20)) = 1
		[Space(20)]
			_MultiplyStone("Multiply", Range(0, 1)) = 1
			_Stone("Stone", 2D) = "white" {}
			_StoneNormal("SandNormal", 2D) = "bump" {}
			_StoneHeight("Stone Height", Range(0, 20)) = 1
	}
		SubShader
			{
				Tags { "RenderType" = "Opaque" }
				LOD 200

				CGPROGRAM
				#pragma surface surf Standard
				#pragma target 3.0

				float _MultiplySand;
				sampler2D _Sand;
				sampler2D _SandNormal;
				float _SandHeight;
				float _BlendSharpness;

				float _MultiplyGrass;
				sampler2D _Grass;
				sampler2D _GrassNormal;
				float _GrassHeight;

				float _MultiplyStone;
				sampler2D _Stone;
				sampler2D _StoneNormal;
				float _StoneHeight;

				struct Input
				{
					float2 uv_Sand;
					float2 uv_SandNormal;
					float2 uv_Grass;
					float2 uv_GrassNormal;
					float2 uv_Stone;
					float2 uv_StoneNormal;
					float3 worldPos; //Unity will automatically fill this with vertex world position when using Surface shaders
				};

				UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_INSTANCING_BUFFER_END(Props)

				struct blendingData
				{
					float height;
					float4 result;
					float4 resultNormal;
				};

				blendingData BlendLayer(float4 layer, float4 normal, float layerHeight, blendingData bd)
				{
					bd.height = max(0, bd.height - layerHeight);
					float t = min(1, bd.height * _BlendSharpness);
					bd.result = lerp(bd.result, layer, t);
					bd.resultNormal = lerp(bd.resultNormal, normal, t);
					return bd;
				}

				void surf(Input IN, inout SurfaceOutputStandard o)
				{
					float2 multSand = float2(IN.worldPos.x * _MultiplySand, IN.worldPos.z * _MultiplySand);
					float2 multGrass = float2(IN.worldPos.x * _MultiplyGrass, IN.worldPos.z * _MultiplyGrass);
					float2 multStone = float2(IN.worldPos.x * _MultiplyStone, IN.worldPos.z * _MultiplyStone);

					blendingData bdata;
					bdata.height = IN.worldPos.y - _SandHeight;
					bdata.result = tex2D(_Sand, IN.uv_Sand * multSand);
					bdata.resultNormal = tex2D(_SandNormal, IN.uv_Sand * multSand);
					float4 grass = tex2D(_Grass, IN.uv_Grass * multGrass);
					float4 grassNormal = tex2D(_GrassNormal, IN.uv_GrassNormal * multGrass);
					float4 stone = tex2D(_Stone, IN.uv_Stone * multStone);
					float4 stoneNormal = tex2D(_StoneNormal, IN.uv_StoneNormal * multStone);

					bdata = BlendLayer(grass, grassNormal, _GrassHeight, bdata);
					bdata = BlendLayer(stone, stoneNormal, _StoneHeight, bdata);

					o.Albedo = bdata.result;
					o.Normal = bdata.resultNormal;
				}
				ENDCG
			}
				FallBack "Diffuse"
}
