Shader "FX/Additive (Billboard)" {
	Properties {
		_MainTex ( "Texture (RGBA)", 2D ) = "white" {}
	}

    SubShader {
        Tags { "Queue"="Transparent+1" "RenderType"="Transparent" }
        ZTest Always
        LOD 100
        Cull Off ZWrite On Fog { Mode Off }   
        Blend One One

        Pass {  
	        CGPROGRAM
			#pragma exclude_renderers ps3 xbox360 flash
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			}; 
			
			fragmentInput vert(vertexInput i)
			{
				fragmentInput o;
				o.pos = mul(UNITY_MATRIX_P,  mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1)) + float4(i.vertex.x, i.vertex.y, 0.0, 0.0));	
			    o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
				return o;
			}
			
			half4 frag (fragmentInput i) : COLOR
			{ 
				return tex2D(_MainTex, i.uv);
			}
			
			ENDCG
        }
    }
}