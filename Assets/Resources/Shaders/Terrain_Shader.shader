Shader "Custom/Terrain_Shader"
{
    Properties
    {
		_Metalness("Metalness", Range(0, 1)) = 0.1
		_Smoothness("Smoothness", Range(0, 1)) = 0.1
		_MainTex("Base Texture", 2D) = "white" {}
		_BaseHeight("Base Height", Range(-50, 50)) = 0
		_BlendSharpness("Blend Sharpness", Range(0.06, 4)) = 0.75

		[Space(30)]
			_Layer1("Layer 1", 2D) = "white" {}
			_Layer1Height("Layer 1 Height", Range(0, 20)) = 1
		[Space(20)]
			_Layer2("Layer 2", 2D) = "white" {}
			_Layer2Height("Layer 2 Height", Range(0, 20)) = 1
		[Space(20)]
			_Layer3("Layer 3", 2D) = "white" {}
			_Layer3Height("Layer 3 Height", Range(0, 20)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "LightweightPipeline" }
        LOD 200
		Pass
			{

				CGPROGRAM
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 3.5
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag


				float _Metalness;
				float _Smoothness;

				UNITY_DECLARE_TEX2D(_MainTex);
				float _BaseHeight;
				float _BlendSharpness;

				UNITY_DECLARE_TEX2D(_Layer1);
				float _Layer1Height;

				UNITY_DECLARE_TEX2D(_Layer2);
				float _Layer2Height;

				UNITY_DECLARE_TEX2D(_Layer3);
				float _Layer3Height;

				struct Input
				{
					float2 uv_MainTex;
					float2 uv_Layer1;
					float2 uv_Layer2;
					float2 uv_Layer3;
					float3 worldPos; //Unity will automatically fill this with vertex world position when using Surface shaders
				};

				UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_INSTANCING_BUFFER_END(Props)

				struct blendingData
				{
					float height;
					float4 result;
				};

				blendingData BlendLayer(float4 layer, float layerHeight, blendingData bd)
				{
					bd.height = max(0, bd.height - layerHeight);
					float t = min(1, bd.height * _BlendSharpness);
					bd.result = lerp(bd.result, layer, t);
					return bd;
				}

				void surf(Input IN, inout SurfaceOutputStandard o)
				{
					blendingData bdata;
					bdata.height = IN.worldPos.y - _BaseHeight;
					bdata.result = UNITY_SAMPLE_TEX2D(_MainTex, IN.uv_MainTex);
					float4 layer1 = UNITY_SAMPLE_TEX2D(_Layer1, IN.uv_Layer1);
					float4 layer2 = UNITY_SAMPLE_TEX2D(_Layer2, IN.uv_Layer2);
					float4 layer3 = UNITY_SAMPLE_TEX2D(_Layer3, IN.uv_Layer3);

					bdata = BlendLayer(layer1, _Layer1Height, bdata);
					bdata = BlendLayer(layer2, _Layer2Height, bdata);
					bdata = BlendLayer(layer3, _Layer3Height, bdata);

					o.Albedo = bdata.result;
					o.Metallic = _Metalness;
					o.Smoothness = _Smoothness;
				}

				float4 vert(float4 vertexPos : POSITION) : SV_POSITION
					// vertex shader 
				{
				   return UnityObjectToClipPos(vertexPos);
				// this line transforms the vertex input parameter 
				// and returns it as a nameless vertex output parameter 
				// (with semantic SV_POSITION)
				}


				float4 frag(void) : COLOR // fragment shader
				{
				   return float4(1.0, 0.0, 0.0, 1.0);
				// this fragment shader returns a nameless fragment
				// output parameter (with semantic COLOR) that is set to
				// opaque red (red = 1, green = 0, blue = 0, alpha = 1)
				}

				ENDCG
			}	
    }
	FallBack "VertexLit"
}
