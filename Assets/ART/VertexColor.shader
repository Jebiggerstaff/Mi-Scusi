Shader "Unlit/VertexColor"
{
	
		SubShader{
			ZWrite Off

			Pass {
				Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "LightMode" = "ForwardBase"}
				CGPROGRAM
				#pragma vertex vert 
				#pragma fragment frag 
				#include "UnityCG.cginc" 
				#pragma multi_compile_fwdbase 
				#include "AutoLight.cginc" 
				sampler2D _MainTex;
				float4 _MainTex_ST;

				struct v2f {
					float4 pos : SV_POSITION;
					LIGHTING_COORDS(0,1)
					float2 uv : TEXCOORD2;
					fixed4 color : COLOR;
				};

				v2f vert(appdata_full v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);	
					o.color = v.color;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					return o;
				}
				float4 _Color;
				fixed4 frag(v2f i) : COLOR
				{
					float attenuation = LIGHT_ATTENUATION(i);
					return attenuation * i.color;
				}
				ENDCG
			}
		}
			Fallback "VertexLit"
}
