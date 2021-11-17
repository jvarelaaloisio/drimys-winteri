// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DrimmysShaders/Leaf-Instancing"
{
	Properties
	{
		[NoScaleOffset]_BaseColor1("Base Color", 2D) = "white" {}
		_BaseColorHue1("Base Color Hue", Range( -1 , 1)) = 0
		_BaseColorSaturation1("Base Color Saturation", Range( -1 , 1)) = 0
		_BaseColorValue1("Base Color Value", Range( -1 , 1)) = 0
		_MaxOpacityMask1("Max Opacity Mask", Float) = 1.65
		_LocalWindMask1("Local Wind Mask", Range( -1 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float WindSpeed1;
		uniform float WindTurbulence1;
		uniform float WindIntensity1;
		uniform float WindDirX1;
		uniform float WindDirZ1;
		uniform float _LocalWindMask1;
		uniform float WindMaskGlobal1;
		uniform float WindSpeedGlobal1;
		uniform float WindIntensityGlobal1;
		uniform sampler2D _BaseColor1;
		uniform float _BaseColorHue1;
		uniform float _BaseColorSaturation1;
		uniform float _BaseColorValue1;
		SamplerState sampler_BaseColor1;
		uniform float _MaxOpacityMask1;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime12 = _Time.y * WindSpeed1;
			float4 appendResult14 = (float4(mulTime12 , 0.0 , 0.0 , 0.0));
			float2 uv_TexCoord18 = v.texcoord.xy + appendResult14.xy;
			float simplePerlin2D19 = snoise( uv_TexCoord18*(0.5 + (WindTurbulence1 - 0.0) * (0.7 - 0.5) / (1.0 - 0.0)) );
			simplePerlin2D19 = simplePerlin2D19*0.5 + 0.5;
			float temp_output_24_0 = (0.0 + (simplePerlin2D19 - 0.0) * (WindIntensity1 - 0.0) / (1.0 - 0.0));
			float3 appendResult20 = (float3(WindDirX1 , ( ( ( abs( WindDirX1 ) + abs( WindDirZ1 ) ) / 4.0 ) * -1.0 ) , WindDirZ1));
			float3 break23 = appendResult20;
			float3 appendResult34 = (float3(( temp_output_24_0 + break23.x ) , break23.y , ( break23.z + temp_output_24_0 )));
			float4 transform49 = mul(unity_WorldToObject,float4( ( appendResult34 * (0.0 + (v.color.r - 0.0) * (_LocalWindMask1 - 0.0) / (1.0 - 0.0)) ) , 0.0 ));
			float2 temp_cast_2 = (WindSpeedGlobal1).xx;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult38 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner10_g1 = ( 1.0 * _Time.y * temp_cast_2 + ( appendResult38 * WindIntensityGlobal1 ));
			float simplePerlin2D11_g1 = snoise( panner10_g1 );
			simplePerlin2D11_g1 = simplePerlin2D11_g1*0.5 + 0.5;
			float4 lerpResult52 = lerp( transform49 , float4( 0,0,0,0 ) , ( WindMaskGlobal1 * simplePerlin2D11_g1 ));
			v.vertex.xyz += lerpResult52.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BaseColor125 = i.uv_texcoord;
			float4 tex2DNode25 = tex2D( _BaseColor1, uv_BaseColor125 );
			float3 hsvTorgb33 = RGBToHSV( tex2DNode25.rgb );
			float3 hsvTorgb48 = HSVToRGB( float3(( hsvTorgb33.x + _BaseColorHue1 ),( hsvTorgb33.y + _BaseColorSaturation1 ),( hsvTorgb33.z + _BaseColorValue1 )) );
			o.Albedo = saturate( hsvTorgb48 );
			float temp_output_60_0 = 0.0;
			o.Metallic = temp_output_60_0;
			o.Smoothness = temp_output_60_0;
			o.Alpha = saturate( ( tex2DNode25.a * _MaxOpacityMask1 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
2076;1128;1561;734;2039.293;869.9689;2.044383;True;False
Node;AmplifyShaderEditor.CommentaryNode;4;-3922.589,498.2214;Inherit;False;994.9999;442.4537;Wind direction;8;20;16;13;11;10;8;6;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-3867.589,691.6754;Inherit;False;Global;WindDirX1;Wind Dir X;3;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-3875.207,825.9844;Inherit;False;Global;WindDirZ1;Wind Dir Z;4;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;7;-4336.457,105.9554;Inherit;False;1415.002;363.6343;Indiviudual Animation;9;24;21;19;18;17;15;14;12;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.AbsOpNode;8;-3712.36,548.2214;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-4286.457,242.1875;Inherit;False;Global;WindSpeed1;Wind Speed;6;0;Create;True;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;10;-3711.36,613.2214;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-3568.589,568.6754;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;12;-4010.592,243.1975;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-3424.588,566.6754;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-3812.66,155.9554;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-4283.973,352.4914;Inherit;False;Global;WindTurbulence1;Wind Turbulence;7;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-3279.589,567.6754;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;17;-3812.565,297.6875;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-3664.591,165.1975;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;20;-3088.589,664.6754;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-3544.639,325.5894;Inherit;False;Global;WindIntensity1;Wind Intensity;5;0;Create;True;0;0;False;0;False;0.2;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;19;-3455.314,170.4754;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;22;-2319.603,724.0584;Inherit;False;917;338;World Noise;7;47;44;40;39;38;37;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.BreakToComponentsNode;23;-2553.358,224.6114;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCRemapNode;24;-3218.452,160.9554;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-2724.787,-436.6596;Inherit;True;Property;_BaseColor1;Base Color;5;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;23279189a4d82cf4b949df7bf696cad7;a94c433b4af76994cb7d7b5ceb0ebb47;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-2638.679,577.2274;Inherit;False;Property;_LocalWindMask1;Local Wind Mask;12;0;Create;True;0;0;False;0;False;0;0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;27;-2615.194,393.3874;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-2274.358,332.6114;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;29;-2269.603,918.0563;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-2265.358,144.6114;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;33;-2001.081,-516.0397;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCRemapNode;35;-2098.679,425.2594;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2001.643,-306.0997;Inherit;False;Property;_BaseColorSaturation1;Base Color Saturation;7;0;Create;True;0;0;False;0;False;0;-0.26;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-2041.358,224.6114;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2268.603,838.0584;Inherit;False;Global;WindIntensityGlobal1;Wind Intensity(Global);2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-2002.261,-376.8946;Inherit;False;Property;_BaseColorHue1;Base Color Hue;6;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-2061.602,966.0563;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2268.603,762.0584;Inherit;False;Global;WindSpeedGlobal1;Wind Speed (Global);1;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2001.76,-236.549;Inherit;False;Property;_BaseColorValue1;Base Color Value;8;0;Create;True;0;0;False;0;False;0;0.04;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1879.753,761.4584;Inherit;False;Global;WindMaskGlobal1;Wind Mask (Global);0;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1796.449,207.3164;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1584.876,-385.7212;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1820.08,-23.20747;Inherit;False;Property;_MaxOpacityMask1;Max Opacity Mask;10;0;Create;True;0;0;False;0;False;1.65;1.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;44;-1869.604,854.0563;Inherit;False;WorldNoise;-1;;1;f17c4c9c155f92e4cb0e3f353a46552a;0;3;14;FLOAT;0;False;13;FLOAT;0;False;2;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-1584.876,-481.7213;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1584.876,-289.7213;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-1599.755,762.4584;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;48;-1440.876,-417.7213;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldToObjectTransfNode;49;-1338.527,218.7095;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1540.962,-95.31856;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-797.6595,-114.7914;Inherit;False;Constant;_Float2;Float 1;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;-1735.813,-1362.054;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;-1082.527,218.7095;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;53;-1078.063,-959.9101;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2009.248,-1255.847;Inherit;False;Property;_GroundBlend1;GroundBlend;9;0;Create;True;0;0;False;0;False;0;4.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;55;-2243.345,-1349.127;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;56;-2053.832,-1122.605;Inherit;True;Global;_CameraGBufferTexture1;_CameraGBufferTexture0;11;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;57;-2240.951,-1116.024;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;58;-1121.941,-288.8503;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;59;-1299.208,-85.53346;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-516.717,-195.1304;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;DrimmysShaders/Leaf-Instancing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;5;0
WireConnection;10;0;6;0
WireConnection;11;0;8;0
WireConnection;11;1;10;0
WireConnection;12;0;9;0
WireConnection;13;0;11;0
WireConnection;14;0;12;0
WireConnection;16;0;13;0
WireConnection;17;0;15;0
WireConnection;18;1;14;0
WireConnection;20;0;5;0
WireConnection;20;1;16;0
WireConnection;20;2;6;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;23;0;20;0
WireConnection;24;0;19;0
WireConnection;24;4;21;0
WireConnection;28;0;23;2
WireConnection;28;1;24;0
WireConnection;30;0;24;0
WireConnection;30;1;23;0
WireConnection;33;0;25;0
WireConnection;35;0;27;1
WireConnection;35;4;26;0
WireConnection;34;0;30;0
WireConnection;34;1;23;1
WireConnection;34;2;28;0
WireConnection;38;0;29;1
WireConnection;38;1;29;3
WireConnection;41;0;34;0
WireConnection;41;1;35;0
WireConnection;42;0;33;2
WireConnection;42;1;31;0
WireConnection;44;14;37;0
WireConnection;44;13;39;0
WireConnection;44;2;38;0
WireConnection;45;0;33;1
WireConnection;45;1;36;0
WireConnection;46;0;33;3
WireConnection;46;1;32;0
WireConnection;47;0;40;0
WireConnection;47;1;44;0
WireConnection;48;0;45;0
WireConnection;48;1;42;0
WireConnection;48;2;46;0
WireConnection;49;0;41;0
WireConnection;50;0;25;4
WireConnection;50;1;43;0
WireConnection;51;0;55;1
WireConnection;51;4;54;0
WireConnection;52;0;49;0
WireConnection;52;2;47;0
WireConnection;53;0;56;0
WireConnection;53;1;48;0
WireConnection;53;2;51;0
WireConnection;56;1;57;0
WireConnection;58;0;48;0
WireConnection;59;0;50;0
WireConnection;0;0;58;0
WireConnection;0;3;60;0
WireConnection;0;4;60;0
WireConnection;0;9;59;0
WireConnection;0;11;52;0
ASEEND*/
//CHKSM=A66985FA8F69CC7B67C489033233A0322680FC30