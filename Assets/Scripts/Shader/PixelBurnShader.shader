Shader "Unlit/PixelBurnEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "black" {}
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
	}

		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			Blend SrcAlpha OneMinusSrcAlpha
			Fog { Mode Off }
			Lighting Off

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 noise_uv : TEXCOORD1;
					float4 vertex : SV_POSITION;
					float4 objPos : TEXCOORD2;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _NoiseTex;
				float4 _NoiseTex_ST;
				fixed4 _GlowColor;

				// Simple function to grab the brightness out of an image without an alpha channel. 
				float tex_brightness(fixed4 c)
				{
					return c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.objPos = v.vertex;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.noise_uv = TRANSFORM_TEX(v.uv, _NoiseTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float max_brightness = 0;
					float min_brightness = -1;
					float end_y = 1;
					float start_y = 0.5;
					float tex_cutoff = 0.5;
					float glow_cutoff = 0.3;
					float speed = 0.4;
					float pixel_level = 80;

					// Set the y coordinate to [0, 1] in object space
					float y = i.objPos.y + 0.5;
					
					// Calculate the slope and offset of the fade-out equation
					float slope = (min_brightness - max_brightness) / (end_y - start_y);
					float offset = max_brightness - slope * start_y;
					
					float t = fmod(_Time.y * speed, .45);
					float2 noise_uv = float2(i.noise_uv.x, i.noise_uv.y - t);

					noise_uv = floor(pixel_level * noise_uv) / pixel_level;
					float noise_alpha = tex_brightness(tex2D(_NoiseTex, noise_uv));

					float brightness = clamp(noise_alpha + slope * y + offset, 0, 1);

					float tex_on = step(tex_cutoff, brightness);
					float glow_on = step(glow_cutoff, brightness);
					

					return (tex2D(_MainTex, i.uv) * tex_on + glow_on * (1 - tex_on)) * _GlowColor;
				}

				ENDCG
			}
		}
}