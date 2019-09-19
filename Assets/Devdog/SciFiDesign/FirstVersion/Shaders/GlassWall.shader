Shader "Devdog/Unlit/GlassWall(old)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NormalTex("Normal Texture", 2D) = "bump" {}
		_OverlayIntensity("Overlay Intensity", Range(0,1)) = 0.5 
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}

		// Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 grabPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed _OverlayIntensity;

			sampler2D _NormalTex;
            sampler2D _BackgroundTexture;

            sampler2D _CameraGBufferTexture2;
		    sampler2D_float _CameraDepthNormalsTexture;


		    float3 SampleNormal(float4 uv)
		    {
		        float4 cdn = tex2D(_CameraDepthNormalsTexture, uv);
		        return DecodeViewNormalStereo(cdn) * float3(1, 1, -1);
		    }

		    // Normal vector comparer (for geometry-aware weighting)
		    half CompareNormal(half3 d1, half3 d2)
		    {
		        return (dot(d1, d2) + 1) * 0.5;
		    }

			// Geometry-aware separable blur filter (large kernel)
		    fixed3 SeparableBlurLarge(sampler2D tex, float4 uv, half4 delta)
		    {
		    #if !SHADER_API_MOBILE
		        // 9-tap Gaussian blur with adaptive sampling
		        float4 uv1a = uv - delta;
		        float4 uv1b = uv + delta;
		        float4 uv2a = uv - delta * 2;
		        float4 uv2b = uv + delta * 2;
		        float4 uv3a = uv - delta * 3.2307692308;
		        float4 uv3b = uv + delta * 3.2307692308;

		        half3 n0 = SampleNormal(uv);

		        half w0 = 0.37004405286;
		        half w1a = CompareNormal(n0, SampleNormal(uv1a)) * 0.31718061674;
		        half w1b = CompareNormal(n0, SampleNormal(uv1b)) * 0.31718061674;
		        half w2a = CompareNormal(n0, SampleNormal(uv2a)) * 0.19823788546;
		        half w2b = CompareNormal(n0, SampleNormal(uv2b)) * 0.19823788546;
		        half w3a = CompareNormal(n0, SampleNormal(uv3a)) * 0.11453744493;
		        half w3b = CompareNormal(n0, SampleNormal(uv3b)) * 0.11453744493;

		        half3 s = tex2Dproj(tex, uv).rgb * w0;
		        s += tex2Dproj(tex, uv1a).rgb * w1a;
		        s += tex2Dproj(tex, uv1b).rgb * w1b;
		        s += tex2Dproj(tex, uv2a).rgb * w2a;
		        s += tex2Dproj(tex, uv2b).rgb * w2b;
		        s += tex2Dproj(tex, uv3a).rgb * w3a;
		        s += tex2Dproj(tex, uv3b).rgb * w3b;

		        return s / (w0 + w1a + w1b + w2a + w2b + w3a + w3b);
		    #else
		        // 9-tap Gaussian blur with linear sampling
		        // (less quality but slightly fast)
		        float4 uv1a = uv - delta * 1.3846153846;
		        float4 uv1b = uv + delta * 1.3846153846;
		        float4 uv2a = uv - delta * 3.2307692308;
		        float4 uv2b = uv + delta * 3.2307692308;

		        half3 n0 = SampleNormal(uv);

		        half w0 = 0.2270270270;
		        half w1a = CompareNormal(n0, SampleNormal(uv1a)) * 0.3162162162;
		        half w1b = CompareNormal(n0, SampleNormal(uv1b)) * 0.3162162162;
		        half w2a = CompareNormal(n0, SampleNormal(uv2a)) * 0.0702702703;
		        half w2b = CompareNormal(n0, SampleNormal(uv2b)) * 0.0702702703;

		        half s = tex2Dproj(tex, uv).rgb * w0;
		        s += tex2Dproj(tex, uv1a).rgb * w1a;
		        s += tex2Dproj(tex, uv1b).rgb * w1b;
		        s += tex2Dproj(tex, uv2a).rgb * w2a;
		        s += tex2Dproj(tex, uv2b).rgb * w2b;

		        return s / (w0 + w1a + w1b + w2a + w2b);
		    #endif
		    }


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.normal = v.normal;

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

				fixed3 col = tex2D(_MainTex, i.uv).rgb;
				fixed3 normal = UnpackNormal(tex2D(_NormalTex, i.uv)).rgb;

				float3 lightAtten = dot(lightDir, i.normal);
				lightAtten *= _OverlayIntensity;

				fixed3 bgColor = SeparableBlurLarge(_BackgroundTexture, i.grabPos, half4(0.002, -0.002, 0, 0));
				bgColor.rgb += col;

				UNITY_APPLY_FOG(i.fogCoord, col);

				return fixed4(bgColor.rgb * lightAtten, 1);
			}

			ENDCG
		}
	}
}
