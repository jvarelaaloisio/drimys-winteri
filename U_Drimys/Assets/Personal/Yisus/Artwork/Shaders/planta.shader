// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Planta"
{
	Properties
	{
		[NoScaleOffset]_BaseColor("Base Color", 2D) = "white" {}
		_BaseColorHue("Base Color Hue", Range( -1 , 1)) = 0
		_BaseColorSaturation("Base Color Saturation", Range( -1 , 1)) = 0
		_BaseColorValue("Base Color Value", Range( -1 , 1)) = 0
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_TessMaxDisp( "Max Displacement", Float ) = 25
		_MaxOpacityMask("Max Opacity Mask", Float) = 1.65
		_LocalWindMask("Local Wind Mask", Range( -1 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float WindSpeed;
		uniform float WindTurbulence;
		uniform float WindIntensity;
		uniform float WindDirX;
		uniform float WindDirZ;
		uniform float _LocalWindMask;
		uniform float WindMaskGlobal;
		uniform float WindSpeedGlobal;
		uniform float WindIntensityGlobal;
		uniform sampler2D _BaseColor;
		uniform float _BaseColorHue;
		uniform float _BaseColorSaturation;
		uniform float _BaseColorValue;
		SamplerState sampler_BaseColor;
		uniform float _MaxOpacityMask;
		uniform float _EdgeLength;
		uniform float _TessMaxDisp;


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

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTessCull (v0.vertex, v1.vertex, v2.vertex, _EdgeLength , _TessMaxDisp );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime311 = _Time.y * WindSpeed;
			float4 appendResult312 = (float4(mulTime311 , 0.0 , 0.0 , 0.0));
			float2 uv_TexCoord310 = v.texcoord.xy + appendResult312.xy;
			float simplePerlin2D309 = snoise( uv_TexCoord310*(0.5 + (WindTurbulence - 0.0) * (0.7 - 0.5) / (1.0 - 0.0)) );
			simplePerlin2D309 = simplePerlin2D309*0.5 + 0.5;
			float temp_output_306_0 = (0.0 + (simplePerlin2D309 - 0.0) * (WindIntensity - 0.0) / (1.0 - 0.0));
			float3 appendResult257 = (float3(WindDirX , ( ( ( abs( WindDirX ) + abs( WindDirZ ) ) / 4.0 ) * -1.0 ) , WindDirZ));
			float3 break318 = appendResult257;
			float3 appendResult300 = (float3(( temp_output_306_0 + break318.x ) , break318.y , ( break318.z + temp_output_306_0 )));
			float4 transform262 = mul(unity_WorldToObject,float4( ( appendResult300 * (0.0 + (v.color.r - 0.0) * (_LocalWindMask - 0.0) / (1.0 - 0.0)) ) , 0.0 ));
			float2 temp_cast_2 = (WindSpeedGlobal).xx;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult249 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner10_g1 = ( 1.0 * _Time.y * temp_cast_2 + ( appendResult249 * WindIntensityGlobal ));
			float simplePerlin2D11_g1 = snoise( panner10_g1 );
			simplePerlin2D11_g1 = simplePerlin2D11_g1*0.5 + 0.5;
			float4 lerpResult322 = lerp( transform262 , float4( 0,0,0,0 ) , ( WindMaskGlobal * simplePerlin2D11_g1 ));
			v.vertex.xyz += lerpResult322.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BaseColor347 = i.uv_texcoord;
			float4 tex2DNode347 = tex2D( _BaseColor, uv_BaseColor347 );
			float3 hsvTorgb367 = RGBToHSV( tex2DNode347.rgb );
			float3 hsvTorgb371 = HSVToRGB( float3(( hsvTorgb367.x + _BaseColorHue ),( hsvTorgb367.y + _BaseColorSaturation ),( hsvTorgb367.z + _BaseColorValue )) );
			o.Albedo = saturate( hsvTorgb371 );
			o.Metallic = 0.0;
			o.Smoothness = 0.0;
			o.Alpha = saturate( ( tex2DNode347.a * _MaxOpacityMask ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows noshadow vertex:vertexDataFunc tessellate:tessFunction 

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
				vertexDataFunc( v );
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
1090;1116;1542;905;918.4571;658.9345;2.587255;True;False
Node;AmplifyShaderEditor.CommentaryNode;292;-1113.231,1425.61;Inherit;False;994.9999;442.4537;Wind direction;8;268;285;284;280;282;283;257;267;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;321;-1527.098,1033.344;Inherit;False;1415.002;363.6343;Indiviudual Animation;9;341;320;306;308;309;310;312;311;295;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;268;-1065.849,1753.373;Inherit;False;Global;WindDirZ;Wind Dir Z;4;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;267;-1058.231,1619.064;Inherit;False;Global;WindDirX;Wind Dir X;3;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;285;-902.0017,1540.61;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;295;-1477.098,1169.576;Inherit;False;Global;WindSpeed;Wind Speed;6;0;Create;True;0;0;False;0;False;1;0.05;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;284;-903.0017,1475.61;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;280;-759.231,1496.064;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;311;-1201.233,1170.586;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;282;-615.2304,1494.064;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;320;-1474.614,1279.88;Inherit;False;Global;WindTurbulence;Wind Turbulence;7;0;Create;True;0;0;False;0;False;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;312;-1003.302,1083.344;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;310;-855.2327,1092.586;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;341;-1003.207,1225.076;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;283;-470.2308,1495.064;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;257;-279.2307,1592.064;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;309;-645.9563,1097.864;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;308;-735.2812,1252.978;Inherit;False;Global;WindIntensity;Wind Intensity;5;0;Create;True;0;0;False;0;False;0.2;0.6;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;306;-409.0943,1088.344;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;318;256,1152;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;250;489.7549,1651.447;Inherit;False;917;338;World Noise;7;330;333;246;204;191;249;248;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;248;539.7551,1845.445;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;319;535,1260;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;352;194.1643,1320.776;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;355;170.6791,1504.616;Inherit;False;Property;_LocalWindMask;Local Wind Mask;12;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;347;84.57139,490.729;Inherit;True;Property;_BaseColor;Base Color;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;23279189a4d82cf4b949df7bf696cad7;ec0f44e1f482f954bb14bcbe8345af71;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;317;544,1072;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;367;808.2773,411.349;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;370;807.5984,690.8396;Inherit;False;Property;_BaseColorValue;Base Color Value;3;0;Create;True;0;0;False;0;False;0;0.469;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;368;807.0977,550.494;Inherit;False;Property;_BaseColorHue;Base Color Hue;1;0;Create;True;0;0;False;0;False;0;-0.227;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;369;807.7153,621.2889;Inherit;False;Property;_BaseColorSaturation;Base Color Saturation;2;0;Create;True;0;0;False;0;False;0;0.38;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;300;768,1152;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;249;747.7558,1893.445;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;353;710.6791,1352.648;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;191;540.7554,1765.447;Inherit;False;Global;WindIntensityGlobal;Wind Intensity(Global);2;0;Create;True;0;0;False;0;False;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;540.7554,1689.447;Inherit;False;Global;WindSpeedGlobal;Wind Speed (Global);1;0;Create;True;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;333;929.6054,1688.847;Inherit;False;Global;WindMaskGlobal;Wind Mask (Global);0;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;349;1012.91,1134.705;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;357;989.2782,904.1811;Inherit;False;Property;_MaxOpacityMask;Max Opacity Mask;10;0;Create;True;0;0;False;0;False;1.65;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;246;939.7548,1781.445;Inherit;False;WorldNoise;-1;;1;f17c4c9c155f92e4cb0e3f353a46552a;0;3;14;FLOAT;0;False;13;FLOAT;0;False;2;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;374;1224.483,445.6674;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;372;1224.483,637.6673;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;373;1224.483,541.6674;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;371;1368.483,509.6674;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;358;1268.397,832.07;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;330;1209.604,1689.847;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;262;1470.832,1146.098;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;322;1726.832,1146.098;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;351;1507.262,946.9794;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;348;1717.775,619.9866;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;361;755.5266,-195.2165;Inherit;True;Global;_CameraGBufferTexture0;_CameraGBufferTexture0;11;0;Create;True;0;0;False;0;False;-1;None;;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;363;1073.546,-434.6656;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;366;1731.296,-32.52161;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;365;566.0129,-421.7383;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;362;568.4066,-188.6353;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;375;800.1105,-328.4589;Inherit;False;Property;_GroundBlend;GroundBlend;9;0;Create;True;0;0;False;0;False;0;4.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;359;1497.337,841.8551;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;350;1508.817,1026.635;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2102.467,866.4319;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Planta;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;3;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;4;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;285;0;268;0
WireConnection;284;0;267;0
WireConnection;280;0;284;0
WireConnection;280;1;285;0
WireConnection;311;0;295;0
WireConnection;282;0;280;0
WireConnection;312;0;311;0
WireConnection;310;1;312;0
WireConnection;341;0;320;0
WireConnection;283;0;282;0
WireConnection;257;0;267;0
WireConnection;257;1;283;0
WireConnection;257;2;268;0
WireConnection;309;0;310;0
WireConnection;309;1;341;0
WireConnection;306;0;309;0
WireConnection;306;4;308;0
WireConnection;318;0;257;0
WireConnection;319;0;318;2
WireConnection;319;1;306;0
WireConnection;317;0;306;0
WireConnection;317;1;318;0
WireConnection;367;0;347;0
WireConnection;300;0;317;0
WireConnection;300;1;318;1
WireConnection;300;2;319;0
WireConnection;249;0;248;1
WireConnection;249;1;248;3
WireConnection;353;0;352;1
WireConnection;353;4;355;0
WireConnection;349;0;300;0
WireConnection;349;1;353;0
WireConnection;246;14;204;0
WireConnection;246;13;191;0
WireConnection;246;2;249;0
WireConnection;374;0;367;1
WireConnection;374;1;368;0
WireConnection;372;0;367;3
WireConnection;372;1;370;0
WireConnection;373;0;367;2
WireConnection;373;1;369;0
WireConnection;371;0;374;0
WireConnection;371;1;373;0
WireConnection;371;2;372;0
WireConnection;358;0;347;4
WireConnection;358;1;357;0
WireConnection;330;0;333;0
WireConnection;330;1;246;0
WireConnection;262;0;349;0
WireConnection;322;0;262;0
WireConnection;322;2;330;0
WireConnection;348;0;371;0
WireConnection;361;1;362;0
WireConnection;363;0;365;1
WireConnection;363;4;375;0
WireConnection;366;0;361;0
WireConnection;366;1;371;0
WireConnection;366;2;363;0
WireConnection;359;0;358;0
WireConnection;0;0;348;0
WireConnection;0;3;351;0
WireConnection;0;4;350;0
WireConnection;0;9;359;0
WireConnection;0;11;322;0
ASEEND*/
//CHKSM=ABC109522F39E34A41CC41F7706F311250CE688C