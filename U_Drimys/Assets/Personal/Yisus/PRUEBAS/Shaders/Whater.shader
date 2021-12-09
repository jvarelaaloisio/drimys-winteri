// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Drimys/Nature/Wather"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_WatherColor_1("WatherColor_1", Color) = (0.510235,0.8939884,0.9245283,0)
		_WatherColor_2("WatherColor_2", Color) = (0.510235,0.8939884,0.9245283,0)
		_WaveTransform("WaveTransform", Vector) = (1,1,1,0)
		_SpeedDirWaves("SpeedDirWaves", Vector) = (0,0,0,0)
		_VertexOffsetFactor("VertexOffsetFactor", Float) = 0
		_DepthColor("Depth Color", Color) = (0.990566,0.1170263,0,0)
		_DepthDistance("DepthDistance", Float) = 0
		_BorderScale("Border Scale", Float) = 0
		_BorderDistance("BorderDistance", Float) = 0
		_FoamColor("Foam Color", Color) = (0.990566,0.1170263,0,0)
		_FoamScale("FoamScale", Float) = 0
		_FoamRangeMin("Foam Range Min", Range( 0 , 1)) = 0
		_FoamRangeMax("Foam Range Max", Range( 0 , 1)) = 0
		_OpacityBase("OpacityBase", Range( -1 , 1)) = 0
		_ReflectionSwich("ReflectionSwich", Range( 0 , 1)) = 1
		_ScaleRefraction("ScaleRefraction", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		GrabPass{ }
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float2 _SpeedDirWaves;
		uniform float3 _WaveTransform;
		uniform float _VertexOffsetFactor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float _BorderDistance;
		uniform float _BorderScale;
		uniform float _FoamScale;
		uniform float _FoamRangeMin;
		uniform float _FoamRangeMax;
		uniform float _OpacityBase;
		uniform float4 _WatherColor_1;
		uniform float4 _WatherColor_2;
		uniform float4 _DepthColor;
		uniform float4 _FoamColor;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _ScaleRefraction;
		uniform float _ReflectionSwich;
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
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult15 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceUV16 = appendResult15;
			float4 temp_output_24_0 = ( ( WorldSpaceUV16 * float4( _WaveTransform , 0.0 ) ) * 1.0 );
			float2 panner11 = ( 1.0 * _Time.y * _SpeedDirWaves + temp_output_24_0.xy);
			float simplePerlin2D9 = snoise( panner11 );
			simplePerlin2D9 = simplePerlin2D9*0.5 + 0.5;
			float2 temp_output_121_0 = ( 1.0 - _SpeedDirWaves );
			float2 panner49 = ( 1.0 * _Time.y * temp_output_121_0 + temp_output_24_0.xy);
			float simplePerlin2D48 = snoise( panner49 );
			simplePerlin2D48 = simplePerlin2D48*0.5 + 0.5;
			float WavesPattern66 = ( simplePerlin2D9 + simplePerlin2D48 );
			float3 temp_cast_4 = ((0.0 + (WavesPattern66 - 0.0) * (_VertexOffsetFactor - 0.0) / (1.0 - 0.0))).xxx;
			v.vertex.xyz += temp_cast_4;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth60 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth60 = abs( ( screenDepth60 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float DepthWaterMask94 = saturate( ( 1.0 - distanceDepth60 ) );
			float screenDepth70 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth70 = abs( ( screenDepth70 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _BorderDistance ) );
			float temp_output_71_0 = ( 1.0 - distanceDepth70 );
			float3 ase_worldPos = i.worldPos;
			float4 appendResult15 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceUV16 = appendResult15;
			float2 panner102 = ( 1.0 * _Time.y * _SpeedDirWaves + WorldSpaceUV16.xy);
			float simplePerlin2D98 = snoise( panner102*_FoamScale );
			simplePerlin2D98 = simplePerlin2D98*0.5 + 0.5;
			float2 temp_output_121_0 = ( 1.0 - _SpeedDirWaves );
			float2 panner105 = ( 1.0 * _Time.y * temp_output_121_0 + WorldSpaceUV16.xy);
			float simplePerlin2D104 = snoise( panner105*_FoamScale );
			simplePerlin2D104 = simplePerlin2D104*0.5 + 0.5;
			float BorderWatherMask91 = saturate( (0.0 + (( ( temp_output_71_0 - _BorderScale ) + ( temp_output_71_0 * ( simplePerlin2D98 * simplePerlin2D104 ) ) ) - _FoamRangeMin) * (1.0 - 0.0) / (_FoamRangeMax - _FoamRangeMin)) );
			float4 temp_output_24_0 = ( ( WorldSpaceUV16 * float4( _WaveTransform , 0.0 ) ) * 1.0 );
			float2 panner11 = ( 1.0 * _Time.y * _SpeedDirWaves + temp_output_24_0.xy);
			float simplePerlin2D9 = snoise( panner11 );
			simplePerlin2D9 = simplePerlin2D9*0.5 + 0.5;
			float2 panner49 = ( 1.0 * _Time.y * temp_output_121_0 + temp_output_24_0.xy);
			float simplePerlin2D48 = snoise( panner49 );
			simplePerlin2D48 = simplePerlin2D48*0.5 + 0.5;
			float WavesPattern66 = ( simplePerlin2D9 + simplePerlin2D48 );
			float4 lerpResult58 = lerp( _WatherColor_1 , _WatherColor_2 , saturate( WavesPattern66 ));
			float4 lerpResult77 = lerp( lerpResult58 , _DepthColor , DepthWaterMask94);
			float4 lerpResult79 = lerp( lerpResult77 , _FoamColor , BorderWatherMask91);
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
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
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir42_g2 = mul( ase_worldToTangent, normalizeResult39_g2);
			float4 screenColor133 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_screenPosNorm + float4( ( worldToTangentDir42_g2 * _ScaleRefraction ) , 0.0 ) ).xy);
			float4 lerpResult155 = lerp( lerpResult79 , screenColor133 , _ReflectionSwich);
			c.rgb = saturate( lerpResult155 ).rgb;
			c.a = ( DepthWaterMask94 + BorderWatherMask91 + _OpacityBase );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
15;818;1900;1280;293.73;773.2646;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;87;1963.473,-147.5538;Inherit;False;700.8978;243.0804;WordSpaceUV;3;14;15;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;14;2013.473,-97.55391;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;15;2270.525,-87.47348;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;2439.371,-71.09258;Inherit;False;WorldSpaceUV;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;86;-3861.679,762.9509;Inherit;False;2569.579;955.6666;Waves patern;11;26;17;18;25;24;49;11;9;48;38;66;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;85;-1889.392,-759.6293;Inherit;False;1761.699;1029.931;Border Objects;20;97;91;81;101;98;72;71;106;104;70;102;105;69;100;110;112;113;115;116;119;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;26;-3808.819,1012.175;Inherit;False;Property;_WaveTransform;WaveTransform;7;0;Create;True;0;0;False;0;False;1,1,1;0.3,0.3,0.3;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;17;-3811.679,849.1777;Inherit;False;16;WorldSpaceUV;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;13;-3138.72,186.3612;Inherit;False;Property;_SpeedDirWaves;SpeedDirWaves;8;0;Create;True;0;0;False;0;False;0,0;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;121;-2822.478,234.8975;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;-1861.312,-134.6782;Inherit;False;16;WorldSpaceUV;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-3465.819,1105.175;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-3512.678,923.4516;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-1362.9,-132.4575;Inherit;False;Property;_FoamScale;FoamScale;15;0;Create;True;0;0;False;0;False;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-3266.019,937.4747;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;105;-1537.031,-10.4172;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;102;-1523.167,-284.9088;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1874.823,-702.9965;Inherit;False;Property;_BorderDistance;BorderDistance;13;0;Create;True;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;98;-1149.385,-270.7995;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;11;-2454.785,812.9507;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;84;-945.1098,-1455.61;Inherit;False;867.402;386.1866;Depth Water;6;61;60;65;82;63;94;;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;104;-1145.95,-33.15998;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;70;-1838.036,-498.1992;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;49;-2558.035,1299.382;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-1495.568,-416.0223;Inherit;False;Property;_BorderScale;Border Scale;12;0;Create;True;0;0;False;0;False;0;0.583;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;-2097.164,907.0179;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;71;-1567.349,-497.7951;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-851.0858,-87.62421;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;48;-2200.414,1393.449;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-934.3366,-1406.864;Inherit;False;Property;_DepthDistance;DepthDistance;11;0;Create;True;0;0;False;0;False;0;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;-1343.892,-573.0479;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-1782.027,1099.198;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;60;-906.4083,-1210.924;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-1098.263,-485.8521;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;112;-717.1671,-339.2037;Inherit;False;Property;_FoamRangeMin;Foam Range Min;16;0;Create;True;0;0;False;0;False;0;0.24;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;119;-927.0416,-531.2108;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;83;-942.6139,-2089.407;Inherit;False;886.8557;570.4091;WavesBaseColor;5;68;59;56;57;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;65;-649.3384,-1212.691;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-705.942,-232.7523;Inherit;False;Property;_FoamRangeMax;Foam Range Max;17;0;Create;True;0;0;False;0;False;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-1516.1,1164.322;Inherit;False;WavesPattern;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;157;-25.54233,-456.928;Inherit;False;1055.193;472.4989;Reflection;7;141;139;142;134;143;135;133;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;110;-385.9626,-409.1881;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;24.45767,-217.7064;Inherit;False;66;WavesPattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-892.6139,-1644.811;Inherit;False;66;WavesPattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;82;-485.6618,-1213.085;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;57;-645.5944,-1821.439;Inherit;False;Property;_WatherColor_2;WatherColor_2;6;0;Create;True;0;0;False;0;False;0.510235,0.8939884,0.9245283,0;0.1457814,0.7252304,0.7924528,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;56;-644.1169,-2025.406;Inherit;False;Property;_WatherColor_1;WatherColor_1;5;0;Create;True;0;0;False;0;False;0.510235,0.8939884,0.9245283,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;59;-564.8916,-1629.997;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;81;-594.3475,-547.5868;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-310.9961,-1209.885;Inherit;False;DepthWaterMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;142;223.4014,-212.5849;Inherit;False;Normal From Height;-1;;2;1942fe2c5f1a1f94881a33d532e4afeb;0;1;20;FLOAT;0;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;139;301.1626,-100.4291;Inherit;False;Property;_ScaleRefraction;ScaleRefraction;20;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-320.7581,-1880.758;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;245.8183,-1317.673;Inherit;False;94;DepthWaterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;63;-721.2562,-1405.61;Inherit;False;Property;_DepthColor;Depth Color;10;0;Create;True;0;0;False;0;False;0.990566,0.1170263,0,0;0.2389196,0.4267553,0.8584906,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;513.0834,-194.1924;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;134;439.51,-406.928;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;-460.5944,-547.5125;Inherit;False;BorderWatherMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;379.2879,-632.3784;Inherit;False;91;BorderWatherMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;688.2486,-246.9786;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;72;-1646.37,-698.2408;Inherit;False;Property;_FoamColor;Foam Color;14;0;Create;True;0;0;False;0;False;0.990566,0.1170263,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;77;484.2932,-1411.691;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;133;833.6503,-256.5739;Inherit;False;Global;_GrabScreen0;Grab Screen 0;17;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;156;1053.732,-154.4319;Inherit;False;Property;_ReflectionSwich;ReflectionSwich;19;0;Create;True;0;0;False;0;False;1;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;672.2856,-724.8596;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-370.2003,1009.484;Inherit;False;66;WavesPattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;92;-413.5994,477.9717;Inherit;False;91;BorderWatherMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-300.9469,1138.181;Inherit;False;Property;_VertexOffsetFactor;VertexOffsetFactor;9;0;Create;True;0;0;False;0;False;0;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-413.5991,398.2524;Inherit;False;94;DepthWaterMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;155;1322.043,-369.4623;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-467.6165,566.8503;Inherit;False;Property;_OpacityBase;OpacityBase;18;0;Create;True;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;122;-932.1804,-1030.661;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-74.31644,446.4458;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;154;1513.28,112.3152;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;27;-132.2102,1020.295;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1706.614,-138.6755;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Drimys/Nature/Wather;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;14;1
WireConnection;15;1;14;3
WireConnection;16;0;15;0
WireConnection;121;0;13;0
WireConnection;18;0;17;0
WireConnection;18;1;26;0
WireConnection;24;0;18;0
WireConnection;24;1;25;0
WireConnection;105;0;100;0
WireConnection;105;2;121;0
WireConnection;102;0;100;0
WireConnection;102;2;13;0
WireConnection;98;0;102;0
WireConnection;98;1;101;0
WireConnection;11;0;24;0
WireConnection;11;2;13;0
WireConnection;104;0;105;0
WireConnection;104;1;101;0
WireConnection;70;0;69;0
WireConnection;49;0;24;0
WireConnection;49;2;121;0
WireConnection;9;0;11;0
WireConnection;71;0;70;0
WireConnection;106;0;98;0
WireConnection;106;1;104;0
WireConnection;48;0;49;0
WireConnection;115;0;71;0
WireConnection;115;1;116;0
WireConnection;38;0;9;0
WireConnection;38;1;48;0
WireConnection;60;0;61;0
WireConnection;97;0;71;0
WireConnection;97;1;106;0
WireConnection;119;0;115;0
WireConnection;119;1;97;0
WireConnection;65;0;60;0
WireConnection;66;0;38;0
WireConnection;110;0;119;0
WireConnection;110;1;112;0
WireConnection;110;2;113;0
WireConnection;82;0;65;0
WireConnection;59;0;68;0
WireConnection;81;0;110;0
WireConnection;94;0;82;0
WireConnection;142;20;141;0
WireConnection;58;0;56;0
WireConnection;58;1;57;0
WireConnection;58;2;59;0
WireConnection;143;0;142;40
WireConnection;143;1;139;0
WireConnection;91;0;81;0
WireConnection;135;0;134;0
WireConnection;135;1;143;0
WireConnection;77;0;58;0
WireConnection;77;1;63;0
WireConnection;77;2;95;0
WireConnection;133;0;135;0
WireConnection;79;0;77;0
WireConnection;79;1;72;0
WireConnection;79;2;93;0
WireConnection;155;0;79;0
WireConnection;155;1;133;0
WireConnection;155;2;156;0
WireConnection;90;0;96;0
WireConnection;90;1;92;0
WireConnection;90;2;88;0
WireConnection;154;0;155;0
WireConnection;27;0;67;0
WireConnection;27;4;28;0
WireConnection;0;9;90;0
WireConnection;0;13;154;0
WireConnection;0;11;27;0
ASEEND*/
//CHKSM=204E7C43C2B7C6F303D4A6BA6406DA1505380163