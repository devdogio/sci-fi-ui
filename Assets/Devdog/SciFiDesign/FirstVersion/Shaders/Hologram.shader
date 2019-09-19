// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Hologram" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_HologramTex ("Hologram (RGBA)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_RimColor("Rim color", Color) = (1,1,1,1)
		_RimPow("Rim pow", Range(0, 10)) = 1.0

		_RimColor2("Rim color 2", Color) = (1,1,1,1)
		_RimPow2("Rim pow 2", Range(0, 10)) = 1.0
	}
	SubShader {
		
		Pass
		{
			CGPROGRAM

#pragma vertex vert
#pragma fragment frag

			sampler2D _MainTex;
			sampler2D _HologramTex;

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			fixed4 _RimColor;
			fixed _RimPow;

			fixed4 _RimColor2;
			fixed _RimPow2;

			struct VertexIn
			{
				half2 uv : TEXCOORD;
				half4 pos : POSITION;
				half3 normal : NORMAL;
			};

			struct FragmentIn
			{
				half2 uv : TEXCOORD;
				half4 pos : SV_POSITION;
				half3 normal : NORMAL;
				half2 dotProducts : TEXCOORD1; // Dot product from camera / normal
			};

			FragmentIn vert(VertexIn v)
			{
				FragmentIn f;

				f.uv = v.uv;
				f.pos = mul(UNITY_MATRIX_MVP, v.pos);
				f.normal = v.normal;

				half3 normalDir = mul(v.normal.rgbb, unity_WorldToObject).rgb;
				normalDir = normalize(normalDir);

				half3 camDir = normalize(f.pos - _WorldSpaceCameraPos.xyz);

				float d = dot(camDir, f.normal);
				f.dotProducts[0] = pow(d, _RimPow);
				f.dotProducts[1] = pow(d, _RimPow2);

				return f;
			}

			fixed4 frag(FragmentIn i) : COLOR
			{
				//fixed4 hologramTexColor = tex2D(_HologramTex, (i.uv + half2(0, _Time[1] * 0.1)) * 5);
				fixed4 hologramTexColor = fixed4(1, 1, 1, 1);

				fixed4 col1 = fixed4(_RimColor.rgb * ((i.dotProducts[0] + 0.5) * 2), 1.0 - i.dotProducts[0]);
				fixed4 col2 = fixed4(_RimColor2.rgb * ((i.dotProducts[1] + 0.5) * 2), 1.0 - i.dotProducts[1]);
				fixed4 finalCol = col1 + col2;

				return (finalCol / 2) * (hologramTexColor * hologramTexColor.a);
			}
	
			ENDCG
		}
	}
	
	//FallBack "Diffuse"
}
