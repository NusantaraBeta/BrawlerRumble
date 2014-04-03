Shader "Swift Shadows/Shadow Multiply"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	Category {
		Tags { "Queue"="Transparent-1" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off 
		Fog { Color (0,0,0,0) }

		Cull Back 
		ZWrite Off
		Offset -1, -1
		
		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader {
			Pass {
				SetTexture [_MainTex] {
					Combine texture * primary
				}
			}
		}
	}
}