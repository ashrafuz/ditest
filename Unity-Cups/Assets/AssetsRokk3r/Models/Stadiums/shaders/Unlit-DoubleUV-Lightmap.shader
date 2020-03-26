// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Unlit/DoubleUV Lightmapped" {

Properties {
    _MainTex ("Texture1 (RGB)", 2D) = "white" {}
    _SecTex ("Texture2 (RGB)", 2D) = "white" {}
}

SubShader {

	Tags { "Queue" = "Geometry" }
	
	Pass {
		CGPROGRAM
		#pragma exclude_renderers ps3 xbox360 flash
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		
		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _SecTex;
		float4 _SecTex_ST;
		
		// sampler2D unity_Lightmap;
		// float4 unity_LightmapST;
		
		struct vertexInput
		{
			float4 vertex : POSITION;
			float4 texcoord : TEXCOORD0;
			float4 texcoord2 : TEXCOORD1;
		};
		
		struct fragmentInput
		{
			float4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
			half2 uv2 : TEXCOORD1;
			half2 uv3 : TEXCOORD2;
		};
		
		fragmentInput vert(vertexInput i)
		{
			fragmentInput o;
			o.pos = UnityObjectToClipPos (i.vertex);
			o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
			o.uv2 = TRANSFORM_TEX(i.texcoord2, _SecTex);
			o.uv3 = i.texcoord2.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			
			return o;
		}
		
		half4 frag (fragmentInput i) : COLOR
		{
			half4 firstTexVal = tex2D(_MainTex, i.uv);
			half4 secTexVal = tex2D(_SecTex, i.uv2);
			return firstTexVal * secTexVal * half4(DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv3)), 1);
		}
		ENDCG
	}
}

SubShader {
    Pass {
		Lighting Off
        BindChannels {
            Bind "texcoord", texcoord0
            Bind "texcoord1", texcoord1
        }

        SetTexture[_MainTex]
        SetTexture[_SecTex] {
            Combine texture * previous
        }
    }

}
Fallback "Diffuse"
}
