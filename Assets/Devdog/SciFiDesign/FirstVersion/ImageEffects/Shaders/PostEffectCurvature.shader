Shader "Hidden/PostEffectCurvature" {
	Properties {
		_MainTex ("Base", 2D) = "" {}
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
		
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		
		return o;
	} 
	
	half4 fragDs(v2f i) : SV_Target 
	{
		half4 c = tex2D (_MainTex, i.uv.xy + _MainTex_TexelSize.xy * 0.5);
		c += tex2D (_MainTex, i.uv.xy - _MainTex_TexelSize.xy * 0.5);
		c += tex2D (_MainTex, i.uv.xy + _MainTex_TexelSize.xy * float2(0.5,-0.5));
		c += tex2D (_MainTex, i.uv.xy - _MainTex_TexelSize.xy * float2(0.5,-0.5));
		return c/4.0;
	}

	ENDCG 
	
Subshader {

 // 0: box downsample
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      
      #pragma vertex vert
      #pragma fragment fragDs
      
      ENDCG
  }
//// 1: simple chrom aberration
//Pass {
//	  ZTest Always Cull Off ZWrite Off
//
//      CGPROGRAM
//      
//      #pragma vertex vert
//      #pragma fragment frag
//      
//      ENDCG
//  }
//// 2: simulates more chromatic aberration effects
//Pass {
//	  ZTest Always Cull Off ZWrite Off
//
//      CGPROGRAM
//      
//      #pragma vertex vert
//      #pragma fragment fragComplex
//      
//      ENDCG
//  }  
}

Fallback off
	
} // shader
