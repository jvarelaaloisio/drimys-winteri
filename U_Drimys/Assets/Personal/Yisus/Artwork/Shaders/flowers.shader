// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drimys/Nature/flowers"
{
	Properties
	{
		[NoScaleOffset]_GrassAtlas("GrassAtlas", 2D) = "white" {}
		_Saturation("Saturation", Range( 0 , 2)) = 1
		_Hue("Hue", Range( 0 , 2)) = 0.06
		_Value("Value", Range( 0 , 2)) = 1
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

		uniform float WindSpeed;
		uniform float WindTurbulence;
		uniform float WindIntensity;
		uniform float WindDirX;
		uniform float WindDirZ;
		uniform float WindMaskGlobal;
		uniform float WindSpeedGlobal;
		uniform float WindIntensityGlobal;
		uniform float _Hue;
		uniform sampler2D _GrassAtlas;
		uniform float _Saturation;
		uniform float _Value;
		SamplerState sampler_GrassAtlas;


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
			float mulTime11 = _Time.y * WindSpeed;
			float4 appendResult14 = (float4(mulTime11 , 0.0 , 0.0 , 0.0));
			float2 uv_TexCoord17 = v.texcoord.xy + appendResult14.xy;
			float simplePerlin2D19 = snoise( uv_TexCoord17*(0.5 + (WindTurbulence - 0.0) * (0.7 - 0.5) / (1.0 - 0.0)) );
			simplePerlin2D19 = simplePerlin2D19*0.5 + 0.5;
			float temp_output_21_0 = (0.0 + (simplePerlin2D19 - 0.0) * (WindIntensity - 0.0) / (1.0 - 0.0));
			float3 appendResult18 = (float3(WindDirX , ( ( ( abs( WindDirX ) + abs( WindDirZ ) ) / 4.0 ) * -1.0 ) , WindDirZ));
			float3 break22 = appendResult18;
			float3 appendResult30 = (float3(( temp_output_21_0 + break22.x ) , break22.y , ( break22.z + temp_output_21_0 )));
			float4 transform34 = mul(unity_WorldToObject,float4( ( appendResult30 * v.color.r ) , 0.0 ));
			float2 temp_cast_2 = (WindSpeedGlobal).xx;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult26 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner10_g1 = ( 1.0 * _Time.y * temp_cast_2 + ( appendResult26 * WindIntensityGlobal ));
			float simplePerlin2D11_g1 = snoise( panner10_g1 );
			simplePerlin2D11_g1 = simplePerlin2D11_g1*0.5 + 0.5;
			float4 lerpResult38 = lerp( transform34 , float4( 0,0,0,0 ) , ( WindMaskGlobal * simplePerlin2D11_g1 ));
			v.vertex.xyz += lerpResult38.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_GrassAtlas35 = i.uv_texcoord;
			float4 tex2DNode35 = tex2D( _GrassAtlas, uv_GrassAtlas35 );
			float3 hsvTorgb93 = RGBToHSV( tex2DNode35.rgb );
			float3 hsvTorgb107 = HSVToRGB( float3(( _Hue * hsvTorgb93.x ),( hsvTorgb93.y * _Saturation ),( hsvTorgb93.z * _Value )) );
			o.Albedo = saturate( hsvTorgb107 );
			o.Metallic = 0.0;
			o.Smoothness = 0.0;
			float BaseColorOp110 = tex2DNode35.a;
			o.Alpha = BaseColorOp110;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows noshadow vertex:vertexDataFunc 

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
217;208;1561;728;2866.984;1077.218;3.339724;True;False
Node;AmplifyShaderEditor.CommentaryNode;1;-3046.242,251.0551;Inherit;False;994.9999;442.4537;Wind direction;8;18;15;12;10;9;8;6;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2;-3460.109,-141.2107;Inherit;False;1415.002;363.6343;Indiviudual Animation;9;21;20;19;17;16;14;13;11;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2991.242,444.5092;Inherit;False;Global;WindDirX;Wind Dir X;3;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-2998.86,578.8185;Inherit;False;Global;WindDirZ;Wind Dir Z;4;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;8;-2836.013,301.0552;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;9;-2823.013,370.0552;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-3410.109,-4.978752;Inherit;False;Global;WindSpeed;Wind Speed;0;0;Create;True;0;0;False;0;False;1;0.05;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;11;-3134.244,-3.968742;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-2692.242,321.5091;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;12;-2548.242,319.5091;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-3407.625,105.3251;Inherit;False;Global;WindTurbulence;Wind Turbulence;0;0;Create;True;0;0;False;0;False;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-2936.313,-91.21068;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-2788.244,-81.96861;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2404.242,321.5091;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;16;-2936.218,50.52122;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;0.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;19;-2578.968,-76.69066;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2668.292,78.42316;Inherit;False;Global;WindIntensity;Wind Intensity;0;0;Create;True;0;0;False;0;False;0.2;0.6;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-2212.242,417.5092;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;22;-1819.439,253.3982;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCRemapNode;21;-2342.105,-86.21068;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;3;-1387.339,745.7778;Inherit;False;917;338;World Noise;7;36;33;32;29;28;26;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;35;-1325.028,-301.6592;Inherit;True;Property;_GrassAtlas;GrassAtlas;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;23279189a4d82cf4b949df7bf696cad7;7b2e72f2c7346714bb919a43630e918e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1531.439,173.3982;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1531.439,381.3982;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;24;-1337.339,939.7767;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VertexColorNode;145;-1074.289,335.8098;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-1336.339,859.7778;Inherit;False;Global;WindIntensityGlobal;Wind Intensity(Global);2;0;Create;True;0;0;False;0;False;0;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;26;-1129.338,987.7767;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-1307.439,253.3982;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1336.339,783.7778;Inherit;False;Global;WindSpeedGlobal;Wind Speed (Global);1;0;Create;True;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-899.5805,-534.9209;Inherit;False;Property;_Hue;Hue;2;0;Create;True;0;0;False;0;False;0.06;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-899.5805,-377.5606;Inherit;False;Property;_Value;Value;3;0;Create;True;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;93;-856.5805,-291.5606;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;104;-899.5805,-454.5605;Inherit;False;Property;_Saturation;Saturation;1;0;Create;True;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-523.5807,-361.5606;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-780.572,255.728;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-947.4885,783.1777;Inherit;False;Global;WindMaskGlobal;Wind Mask (Global);0;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-523.5807,-265.5605;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-523.5807,-457.5605;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;32;-937.3391,875.7767;Inherit;False;WorldNoise;-1;;1;f17c4c9c155f92e4cb0e3f353a46552a;0;3;14;FLOAT;0;False;13;FLOAT;0;False;2;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;34;-562.9089,253.3982;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;107;-335.2329,-385.1131;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-667.4898,784.1777;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-852.1785,-143.6118;Inherit;False;BaseColorOp;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;25.82068,310.3538;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-115.8791,-58.84619;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;38;-306.909,253.3982;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-117.1791,-134.2462;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-347.4337,113.6106;Inherit;False;110;BaseColorOp;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;108;-115.8888,-385.9675;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;182.6768,-216.706;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Drimys/Nature/flowers;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;6;0
WireConnection;9;0;5;0
WireConnection;11;0;7;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;12;0;10;0
WireConnection;14;0;11;0
WireConnection;17;1;14;0
WireConnection;15;0;12;0
WireConnection;16;0;13;0
WireConnection;19;0;17;0
WireConnection;19;1;16;0
WireConnection;18;0;6;0
WireConnection;18;1;15;0
WireConnection;18;2;5;0
WireConnection;22;0;18;0
WireConnection;21;0;19;0
WireConnection;21;4;20;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;25;0;22;2
WireConnection;25;1;21;0
WireConnection;26;0;24;1
WireConnection;26;1;24;3
WireConnection;30;0;23;0
WireConnection;30;1;22;1
WireConnection;30;2;25;0
WireConnection;93;0;35;0
WireConnection;103;0;93;2
WireConnection;103;1;104;0
WireConnection;92;0;30;0
WireConnection;92;1;145;1
WireConnection;105;0;93;3
WireConnection;105;1;106;0
WireConnection;101;0;102;0
WireConnection;101;1;93;1
WireConnection;32;14;29;0
WireConnection;32;13;28;0
WireConnection;32;2;26;0
WireConnection;34;0;92;0
WireConnection;107;0;101;0
WireConnection;107;1;103;0
WireConnection;107;2;105;0
WireConnection;36;0;33;0
WireConnection;36;1;32;0
WireConnection;110;0;35;4
WireConnection;38;0;34;0
WireConnection;38;2;36;0
WireConnection;108;0;107;0
WireConnection;0;0;108;0
WireConnection;0;3;150;0
WireConnection;0;4;149;0
WireConnection;0;9;111;0
WireConnection;0;11;38;0
ASEEND*/
//CHKSM=EBE51D18C13078F91D71A57CA02165EE1EB83DF5