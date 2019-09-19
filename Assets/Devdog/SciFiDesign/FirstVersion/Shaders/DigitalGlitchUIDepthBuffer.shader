Shader "Devdog/UI/DigitalGlitchUI Depth buffer"
{
	Properties
	{
		[PerRendererData]
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		
		_NoiseTex("Noise", 2D) = "white" {}
		_ShakeStrength("Shake Strength", Float) = 0.2

		_HighlightColor("Highlight Color", Color) = (1, 1, 1, .5) //Color when intersecting
		_HighlightThresholdMax("Highlight Threshold Max", Float) = 1 //Max difference for intersections
		_HighlightStrength("Highlight strength", Float) = 1
		_HighlightTex("Highlight texture", 2D) = "white" {}

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]





			Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 3.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
		half2 uv  : TEXCOORD0;

		float4 projPos : TEXCOORD1;
	};

	fixed4 _Color;
	fixed4 _TextureSampleAdd;
	
	float4 _HighlightColor;
	float _HighlightThresholdMax;
	fixed _HighlightStrength;

	sampler2D _MainTex;
	sampler2D _NoiseTex;
	sampler2D _CameraDepthTexture;
	sampler2D _HighlightTex;

	fixed _ShakeStrength;
	fixed _NormalizedTime;

	bool _UseClipRect;
	float4 _ClipRect;

	bool _UseAlphaClip;

	float random(float2 p)
	{
		float2 r = float2(23.14069, 2.6651);
		return frac(cos(dot(p, r)) * 123456.0);
	}


	v2f vert(appdata_t i)
	{
		v2f OUT;

		float3 noiseOffset = tex2Dlod(_NoiseTex, float4(i.uv * ((_Time[1] % 5) + 5), 0.0, 0.0)).rgb;
		noiseOffset -= float3(0.5, 0.5, 0.5);
		//noiseOffset += random(i.vertex.xy / 10);
		noiseOffset.y += _NormalizedTime * 4;
		noiseOffset *= _ShakeStrength;

		OUT.vertex = mul(UNITY_MATRIX_MVP, i.vertex + fixed4(noiseOffset.xyz, 0) * 15);
		OUT.projPos = ComputeScreenPos(OUT.vertex);
		OUT.uv = i.uv;
		//OUT.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, i.uv);

#ifdef UNITY_HALF_TEXEL_OFFSET
		OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
#endif

		OUT.color = i.color * _Color;
		OUT.color.r += noiseOffset.x * _ShakeStrength * 3;
		OUT.color.bg -= noiseOffset.x * _ShakeStrength * 3;
		OUT.color.a *= _NormalizedTime;

		return OUT;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float depth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.projPos.xyz / i.projPos.w)));
		float fragmentDepth = i.projPos.w;

		i.color.a = 1;
		half4 color = (tex2D(_MainTex, i.uv) + _TextureSampleAdd) * i.color;
		if (_UseClipRect)
			color *= UnityGet2DClipping(i.vertex.xy, _ClipRect);

		if (_UseAlphaClip)
			clip(color.a - 0.001);

		float diff = (abs(depth - fragmentDepth)) / _HighlightThresholdMax;
		if (diff < 1)
		{
			fixed4 highlightColor = tex2D(_HighlightTex, i.uv * 100);
			return lerp(_HighlightColor * highlightColor * color.a * _HighlightStrength, color, diff);
		}

		return color;
	}
		ENDCG
	}

	}
}
