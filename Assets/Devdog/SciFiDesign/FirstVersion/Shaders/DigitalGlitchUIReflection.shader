Shader "Devdog/UI/DigitalGlitchUIReflection"
{
	Properties
	{
		[PerRendererData]
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		
		_NoiseTex("Noise", 2D) = "white" {}
		_ShakeStrength("Shake Strength", Float) = 0.2

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
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
		float4 worldPosition : TEXCOORD1;
	};

	fixed4 _Color;
	fixed4 _TextureSampleAdd;
	
	sampler2D _MainTex;
	sampler2D _NoiseTex;

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


	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.worldPosition = IN.vertex;

		float3 noiseOffset = tex2Dlod(_NoiseTex, float4(IN.texcoord * ((_Time[1] % 5) + 5), 0.0, 0.0)).rgb;
		noiseOffset -= float3(0.5, 0.5, 0.5);
		//noiseOffset += random(IN.vertex.xy / 10);
		noiseOffset.y += _NormalizedTime * 4;

		float strength = _ShakeStrength;
		noiseOffset *= strength;

		OUT.worldPosition.xy += noiseOffset.xy * 15;
		OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);
		

		OUT.texcoord = IN.texcoord;

#ifdef UNITY_HALF_TEXEL_OFFSET
		OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
#endif

		OUT.color = IN.color * _Color;
		OUT.color.r += noiseOffset.x * strength * 3;
		OUT.color.bg -= noiseOffset.x * strength * 3;
		OUT.color.a *= _NormalizedTime;

		return OUT;
	}


	fixed4 frag(v2f IN) : SV_Target
	{
		half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

		if (_UseClipRect)
			color *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

		if (_UseAlphaClip)
			clip(color.a - 0.001);

		return color;
	}
		ENDCG
	}




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
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
		float4 worldPosition : TEXCOORD1;
	};

	fixed4 _Color;
	fixed4 _TextureSampleAdd;

	sampler2D _MainTex;
	sampler2D _NoiseTex;

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


	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.worldPosition = IN.vertex;

		float3 noiseOffset = tex2Dlod(_NoiseTex, float4(IN.texcoord * ((_Time[1] % 5) + 5), 0.0, 0.0)).rgb;
		noiseOffset -= float3(0.5, 0.5, 0.5);
		//noiseOffset += random(IN.vertex.xy / 10);
		noiseOffset.y += _NormalizedTime * 4;

		float strength = _ShakeStrength;
		noiseOffset *= strength;

		OUT.worldPosition.xy += noiseOffset.xy * 15;
		OUT.worldPosition.z = -OUT.worldPosition.z + 60;
		OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);


		OUT.texcoord = IN.texcoord;

#ifdef UNITY_HALF_TEXEL_OFFSET
		OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
#endif

		OUT.color = IN.color * _Color;
		float len = length(OUT.color.rgb);
		OUT.color.r = 0;
		OUT.color.g = 0.6 * len;
		OUT.color.b = 1 * len;
		OUT.color.a = (OUT.color.a * _NormalizedTime) / 8;

		return OUT;
	}


	fixed4 frag(v2f IN) : SV_Target
	{
		half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

		if (_UseClipRect)
			color *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

		if (_UseAlphaClip)
			clip(color.a - 0.001);

		return color;
	}
		ENDCG
	}




	}
}
