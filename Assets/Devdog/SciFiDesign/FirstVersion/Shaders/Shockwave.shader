Shader "Unlit/Shockwave"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_HighlightColor("HighlightColor", Color) = (1,1,1,1)
		_Speed("Speed", float) = 1
		_Amplitude("Amplitude", float) = 10
		_Radius("Radius", Range(0.0, 3.0)) = 1.0
		_Intensity("Intensity", Range(0, 50)) = 5.0
	}
	SubShader
	{
		LOD 100

		Tags
		{
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
		}

		Cull Off
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			fixed4 _Color;
			fixed4 _HighlightColor;
			half2 _MousePosition;
			fixed _Speed;
			fixed _Amplitude;
			fixed _Radius;
			fixed _Intensity;
			fixed _MouseMovementSpeed;


			bool _UseClipRect;
			float4 _ClipRect;

			bool _UseAlphaClip;

			v2f vert (appdata v)
			{
				v2f o;

				float lengthOffset = length(v.uv - float2(0.5, 0.5));
				
				float distanceToMouse = (_Radius - length(v.uv - _MousePosition)) * _Intensity * (_MouseMovementSpeed * 50);
				float wave = distanceToMouse * (cos(distanceToMouse * -_Amplitude - _Time[1] * _Speed)) * 2;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertex.xy += wave * distanceToMouse;
				o.vertex.z = 1.0;

				o.color = lerp(_Color, _HighlightColor, distanceToMouse);

				float t = wave;
				o.color += float4(t, t, t, 0);
				o.color.a = 1.0;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}
	}
}
