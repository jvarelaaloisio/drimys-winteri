// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drimys/Nature/Water"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_WatherColor1("Wather Color 1", Color) = (0.510235,0.8939884,0.9245283,0)
		_WatherColor2("Wather Color 2", Color) = (0.510235,0.8939884,0.9245283,0)
		_WaveTransform("Wave Transform", Vector) = (1,1,1,0)
		_SpeedAndDirWaves("Speed And Dir Waves", Vector) = (0,0,0,0)
		_VertexOffsetFactor("Vertex Offset Factor", Float) = 0
		_DepthColor("Depth Color", Color) = (0.990566,0.1170263,0,0)
		_DepthDistance("Depth Distance", Float) = 0
		_FoamBorder("Foam Border", Float) = 0
		_FoamDistance("Foam Distance", Float) = 0
		_FoamColor("Foam Color", Color) = (0.990566,0.1170263,0,0)
		_FoamScale("Foam Scale", Float) = 0
		_NormalFactor("Normal Factor", Float) = 0
		_FoamRangeMin("Foam Range Min", Range( 0 , 1)) = 0
		_FoamRangeMax("Foam Range Max", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_OpacityBase("Opacity Base", Range( -1 , 1)) = 0
		_ReflectionSwich("Reflection Swich", Range( 0 , 1)) = 1
		_ScaleRefraction("Scale Refraction", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform float2 _SpeedAndDirWaves;
		uniform float3 _WaveTransform;
		uniform float _VertexOffsetFactor;
		uniform float _NormalFactor;
		uniform float4 _WatherColor1;
		uniform float4 _WatherColor2;
		uniform float4 _DepthColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float4 _FoamColor;
		uniform float _FoamDistance;
		uniform float _FoamBorder;
		uniform float _FoamScale;
		uniform float _FoamRangeMin;
		uniform float _FoamRangeMax;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _ScaleRefraction;
		uniform float _ReflectionSwich;
		uniform float _Smoothness;
		uniform float _OpacityBase;
		uniform float _EdgeLength;


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


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 SpeedDirWaves158 = _SpeedAndDirWaves;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult15 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceUV16 = appendResult15;
			float4 temp_output_24_0 = ( ( WorldSpaceUV16 * float4( _WaveTransform , 0.0 ) ) * 1.0 );
			float2 panner11 = ( 1.0 * _Time.y * SpeedDirWaves158 + temp_output_24_0.xy);
			float simplePerlin2D9 = snoise( panner11 );
			simplePerlin2D9 = simplePerlin2D9*0.5 + 0.5;
			float2 panner49 = ( 1.0 * _Time.y * ( 1.0 - SpeedDirWaves158 ) + temp_output_24_0.xy);
			float simplePerlin2D48 = snoise( panner49 );
			simplePerlin2D48 = simplePerlin2D48*0.5 + 0.5;
			float WavesPattern66 = ( simplePerlin2D9 + simplePerlin2D48 );
			float3 temp_cast_4 = ((0.0 + (WavesPattern66 - 0.0) * (_VertexOffsetFactor - 0.0) / (1.0 - 0.0))).xxx;
			v.vertex.xyz += temp_cast_4;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_16_0_g3 = ( ase_worldPos * 100.0 );
			float3 crossY18_g3 = cross( ase_worldNormal , ddy( temp_output_16_0_g3 ) );
			float3 worldDerivativeX2_g3 = ddx( temp_output_16_0_g3 );
			float dotResult6_g3 = dot( crossY18_g3 , worldDerivativeX2_g3 );
			float crossYDotWorldDerivX34_g3 = abs( dotResult6_g3 );
			float2 SpeedDirWaves158 = _SpeedAndDirWaves;
			float4 appendResult15 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceUV16 = appendResult15;
			float4 temp_output_24_0 = ( ( WorldSpaceUV16 * float4( _WaveTransform , 0.0 ) ) * 1.0 );
			float2 panner11 = ( 1.0 * _Time.y * SpeedDirWaves158 + temp_output_24_0.xy);
			float simplePerlin2D9 = snoise( panner11 );
			simplePerlin2D9 = simplePerlin2D9*0.5 + 0.5;
			float2 panner49 = ( 1.0 * _Time.y * ( 1.0 - SpeedDirWaves158 ) + temp_output_24_0.xy);
			float simplePerlin2D48 = snoise( panner49 );
			simplePerlin2D48 = simplePerlin2D48*0.5 + 0.5;
			float WavesPattern66 = ( simplePerlin2D9 + simplePerlin2D48 );
			float temp_output_20_0_g3 = ( WavesPattern66 * _NormalFactor );
			float3 crossX19_g3 = cross( ase_worldNormal , worldDerivativeX2_g3 );
			float3 break29_g3 = ( sign( crossYDotWorldDerivX34_g3 ) * ( ( ddx( temp_output_20_0_g3 ) * crossY18_g3 ) + ( ddy( temp_output_20_0_g3 ) * crossX19_g3 ) ) );
			float3 appendResult30_g3 = (float3(break29_g3.x , -break29_g3.y , break29_g3.z));
			float3 normalizeResult39_g3 = normalize( ( ( crossYDotWorldDerivX34_g3 * ase_worldNormal ) - appendResult30_g3 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir42_g3 = mul( ase_worldToTangent, normalizeResult39_g3);
			o.Normal = worldToTangentDir42_g3;
			float4 lerpResult58 = lerp( _WatherColor1 , _WatherColor2 , saturate( WavesPattern66 ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth60 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth60 = abs( ( screenDepth60 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float DepthWaterMask94 = saturate( ( 1.0 - distanceDepth60 ) );
			float4 lerpResult77 = lerp( lerpResult58 , _DepthColor , DepthWaterMask94);
			float screenDepth70 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth70 = abs( ( screenDepth70 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamDistance ) );
			float temp_output_71_0 = ( 1.0 - distanceDepth70 );
			float2 panner102 = ( 1.0 * _Time.y * SpeedDirWaves158 + WorldSpaceUV16.xy);
			float simplePerlin2D98 = snoise( panner102*_FoamScale );
			simplePerlin2D98 = simplePerlin2D98*0.5 + 0.5;
			float2 panner105 = ( 1.0 * _Time.y * ( 1.0 - SpeedDirWaves158 ) + WorldSpaceUV16.xy);
			float simplePerlin2D104 = snoise( panner105*_FoamScale );
			simplePerlin2D104 = simplePerlin2D104*0.5 + 0.5;
			float BorderWatherMask91 = saturate( (0.0 + (( ( temp_output_71_0 - _FoamBorder ) + ( temp_output_71_0 * ( simplePerlin2D98 * simplePerlin2D104 ) ) ) - _FoamRangeMin) * (1.0 - 0.0) / (_FoamRangeMax - _FoamRangeMin)) );
			float4 lerpResult79 = lerp( lerpResult77 , _FoamColor , BorderWatherMask91);
			float3 temp_output_16_0_g2 = ( ase_worldPos * 100.0 );
			float3 crossY18_g2 = cross( ase_worldNormal , ddy( temp_output_16_0_g2 ) );
			float3 worldDerivativeX2_g2 = ddx( temp_output_16_0_g2 );
			float dotResult6_g2 = dot( crossY18_g2 , worldDerivativeX2_g2 );
			float crossYDotWorldDerivX34_g2 = abs( dotResult6_g2 );
			float temp_output_20_0_g2 = WavesPattern66;
			float3 crossX19_g2 = cross( ase_worldNormal , worldDerivativeX2_g2 );
			float3 break29_g2 = ( sign( crossYDotWorldDerivX34_g2 ) * ( ( ddx( temp_output_20_0_g2 ) * crossY18_g2 ) + ( ddy( temp_output_20_0_g2 ) * crossX19_g2 ) ) );
			float3 appendResult30_g2 = (float3(break29_g2.x , -break29_g2.y , break29_g2.z));
			float3 normalizeResult39_g2 = normalize( ( ( crossYDotWorldDerivX34_g2 * ase_worldNormal ) - appendResult30_g2 ) );
			float3 worldToTangentDir42_g2 = mul( ase_worldToTangent, normalizeResult39_g2);
			float4 screenColor133 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_screenPosNorm + float4( ( worldToTangentDir42_g2 * _ScaleRefraction ) , 0.0 ) ).xy);
			float4 lerpResult155 = lerp( lerpResult79 , screenColor133 , _ReflectionSwich);
			o.Albedo = saturate( lerpResult155 ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = ( DepthWaterMask94 + BorderWatherMask91 + _OpacityBase );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

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
				float4 screenPos : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
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
16;820;1900;1279;-29.57861;1708.582;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;87;1196.371,-1666.849;Inherit;False;700.8978;243.0804;WordSpaceUV;3;14;15;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;163;-2475.02,-1002.826;Inherit;False;535.6846;214;Variables;2;13;158;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;14;1246.371,-1616.849;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;15;1503.422,-1606.769;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;13;-2425.02,-952.8258;Inherit;False;Property;_SpeedAndDirWaves;Speed And Dir Waves;8;0;Create;True;0;0;False;0;False;0,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;158;-2169.335,-951.0333;Inherit;False;SpeedDirWaves;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;85;-2483.979,-744.7645;Inherit;False;1761.699;1029.931;Border Objects;22;97;91;81;101;98;72;71;106;104;70;102;105;69;100;110;112;113;115;116;119;159;160;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;1672.269,-1590.388;Inherit;False;WorldSpaceUV;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;86;-2470.815,357.7284;Inherit;False;1725.758;537.4464;Waves patern;14;66;38;9;48;162;161;121;49;11;25;26;24;18;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;26;-2438.815,517.7283;Inherit;False;Property;_WaveTransform;Wave Transform;7;0;Create;True;0;0;False;0;False;1,1,1;0.4,0.4,0.4;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;17;-2438.815,421.7284;Inherit;False;16;WorldSpaceUV;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;159;-2459.696,-41.19388;Inherit;False;158;SpeedDirWaves;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;-2098.707,769.764;Inherit;False;158;SpeedDirWaves;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2214.815,421.7284;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-2214.815,549.7283;Inherit;False;Constant;_AuxEx;AuxEx;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-2455.898,-119.8135;Inherit;False;16;WorldSpaceUV;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;160;-2252.802,51.52219;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;105;-2063.131,2.691421;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-2022.816,421.7284;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-2469.409,-688.1318;Inherit;False;Property;_FoamDistance;Foam Distance;13;0;Create;True;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;102;-2117.754,-270.0441;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-1957.488,-117.5928;Inherit;False;Property;_FoamScale;Foam Scale;15;0;Create;True;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;121;-1878.816,773.728;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-2087.815,667.728;Inherit;False;158;SpeedDirWaves;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;98;-1728.973,-256.9348;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;104;-1740.538,-18.2953;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;11;-1702.816,421.7284;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;84;-1593.57,-1424;Inherit;False;867.402;386.1866;Depth Water;6;61;60;65;82;63;94;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DepthFade;70;-2432.623,-483.3344;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;49;-1702.816,661.728;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;-1382.816,421.7284;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;48;-1382.816,661.728;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1565.57,-1375;Inherit;False;Property;_DepthDistance;Depth Distance;11;0;Create;True;0;0;False;0;False;0;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-2090.155,-401.1576;Inherit;False;Property;_FoamBorder;Foam Border;12;0;Create;True;0;0;False;0;False;0;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-1445.674,-72.75952;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;71;-2161.936,-482.9304;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;60;-1543.57,-1184;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-1094.816,533.7283;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-1692.851,-470.9874;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;-1938.48,-558.1832;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;157;-595.7002,-425.9892;Inherit;False;1055.193;472.4989;Reflection;7;141;139;142;134;143;135;133;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;65;-1305.57,-1184;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;83;-1593.57,-2064;Inherit;False;886.8557;570.4091;WavesBaseColor;5;68;59;56;57;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;112;-1311.755,-324.339;Inherit;False;Property;_FoamRangeMin;Foam Range Min;17;0;Create;True;0;0;False;0;False;0;0.24;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-1300.53,-217.8876;Inherit;False;Property;_FoamRangeMax;Foam Range Max;18;0;Create;True;0;0;False;0;False;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;119;-1521.63,-516.3461;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-950.8157,533.7283;Inherit;False;WavesPattern;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-1545.57,-1616;Inherit;False;66;WavesPattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;82;-1142.57,-1182;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;-545.7002,-186.7676;Inherit;False;66;WavesPattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;110;-980.5506,-394.3234;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;142;-346.7567,-181.6461;Inherit;False;Normal From Height;-1;;2;1942fe2c5f1a1f94881a33d532e4afeb;0;1;20;FLOAT;0;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;59;-1209.57,-1600;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;57;-1305.57,-1792;Inherit;False;Property;_WatherColor2;Wather Color 2;6;0;Create;True;0;0;False;0;False;0.510235,0.8939884,0.9245283,0;0.145098,0.7254902,0.7921569,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;56;-1289.57,-2000;Inherit;False;Property;_WatherColor1;Wather Color 1;5;0;Create;True;0;0;False;0;False;0.510235,0.8939884,0.9245283,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;139;-268.9955,-69.49028;Inherit;False;Property;_ScaleRefraction;Scale Refraction;22;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-960.5694,-1190;Inherit;False;DepthWaterMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;81;-1188.935,-532.722;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-969.5694,-1856;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;63;-1369.57,-1376;Inherit;False;Property;_DepthColor;Depth Color;10;0;Create;True;0;0;False;0;False;0.990566,0.1170263,0,0;0.2392157,0.427451,0.8588236,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;-1055.182,-532.6478;Inherit;False;BorderWatherMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;245.8183,-1317.673;Inherit;False;94;DepthWaterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-57.07473,-163.2536;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;134;-130.6481,-375.9892;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;135;118.0905,-216.0398;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;77;484.2932,-1411.691;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;379.2879,-632.3784;Inherit;False;91;BorderWatherMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;72;-2240.957,-683.376;Inherit;False;Property;_FoamColor;Foam Color;14;0;Create;True;0;0;False;0;False;0.990566,0.1170263,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;164;1291.391,-947.2305;Inherit;False;66;WavesPattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;172;1293.603,-847.8861;Inherit;False;Property;_NormalFactor;Normal Factor;16;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;672.2856,-724.8596;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;133;263.4923,-225.6351;Inherit;False;Global;_GrabScreen0;Grab Screen 0;17;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;156;609,-166;Inherit;False;Property;_ReflectionSwich;Reflection Swich;21;0;Create;True;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;155;905.8055,-469.5194;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;1493.55,-277.196;Inherit;False;94;DepthWaterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;1520.657,126.5852;Inherit;False;66;WavesPattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;1517.91,222.2821;Inherit;False;Property;_VertexOffsetFactor;Vertex Offset Factor;9;0;Create;True;0;0;False;0;False;0;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;1491.533,-108.5983;Inherit;False;Property;_OpacityBase;Opacity Base;20;0;Create;True;0;0;False;0;False;0;0.31;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;1495.603,-917.8861;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;92;1493.55,-197.4769;Inherit;False;91;BorderWatherMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;1798.568,-222.1563;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;168;1640.555,-969.8514;Inherit;True;Normal From Height;-1;;3;1942fe2c5f1a1f94881a33d532e4afeb;0;1;20;FLOAT;0;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;154;1075.698,-454.6747;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;27;1758.647,131.3962;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;170;1495.052,-361.9894;Inherit;False;Property;_Smoothness;Smoothness;19;0;Create;True;0;0;False;0;False;1;0.93;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2266.444,-452.9503;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Drimys/Nature/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;14;1
WireConnection;15;1;14;3
WireConnection;158;0;13;0
WireConnection;16;0;15;0
WireConnection;18;0;17;0
WireConnection;18;1;26;0
WireConnection;160;0;159;0
WireConnection;105;0;100;0
WireConnection;105;2;160;0
WireConnection;24;0;18;0
WireConnection;24;1;25;0
WireConnection;102;0;100;0
WireConnection;102;2;159;0
WireConnection;121;0;161;0
WireConnection;98;0;102;0
WireConnection;98;1;101;0
WireConnection;104;0;105;0
WireConnection;104;1;101;0
WireConnection;11;0;24;0
WireConnection;11;2;162;0
WireConnection;70;0;69;0
WireConnection;49;0;24;0
WireConnection;49;2;121;0
WireConnection;9;0;11;0
WireConnection;48;0;49;0
WireConnection;106;0;98;0
WireConnection;106;1;104;0
WireConnection;71;0;70;0
WireConnection;60;0;61;0
WireConnection;38;0;9;0
WireConnection;38;1;48;0
WireConnection;97;0;71;0
WireConnection;97;1;106;0
WireConnection;115;0;71;0
WireConnection;115;1;116;0
WireConnection;65;0;60;0
WireConnection;119;0;115;0
WireConnection;119;1;97;0
WireConnection;66;0;38;0
WireConnection;82;0;65;0
WireConnection;110;0;119;0
WireConnection;110;1;112;0
WireConnection;110;2;113;0
WireConnection;142;20;141;0
WireConnection;59;0;68;0
WireConnection;94;0;82;0
WireConnection;81;0;110;0
WireConnection;58;0;56;0
WireConnection;58;1;57;0
WireConnection;58;2;59;0
WireConnection;91;0;81;0
WireConnection;143;0;142;40
WireConnection;143;1;139;0
WireConnection;135;0;134;0
WireConnection;135;1;143;0
WireConnection;77;0;58;0
WireConnection;77;1;63;0
WireConnection;77;2;95;0
WireConnection;79;0;77;0
WireConnection;79;1;72;0
WireConnection;79;2;93;0
WireConnection;133;0;135;0
WireConnection;155;0;79;0
WireConnection;155;1;133;0
WireConnection;155;2;156;0
WireConnection;171;0;164;0
WireConnection;171;1;172;0
WireConnection;90;0;96;0
WireConnection;90;1;92;0
WireConnection;90;2;88;0
WireConnection;168;20;171;0
WireConnection;154;0;155;0
WireConnection;27;0;67;0
WireConnection;27;4;28;0
WireConnection;0;0;154;0
WireConnection;0;1;168;40
WireConnection;0;4;170;0
WireConnection;0;9;90;0
WireConnection;0;11;27;0
ASEEND*/
//CHKSM=E3145F9638DBACAB45D7F29E77BAC71129E50A8C