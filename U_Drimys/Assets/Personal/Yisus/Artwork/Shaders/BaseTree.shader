// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BaseTree"
{
	Properties
	{
		_BaseColorHue("Base Color Hue", Range( -1 , 1)) = 0
		_BaseColorSat("Base Color Sat", Range( -1 , 1)) = 0
		_BasecolorValue("Base color Value", Range( -1 , 1)) = 0
		_Tilling("Tilling", Float) = 0
		[NoScaleOffset]_BaseColor("Base Color", 2D) = "white" {}
		[NoScaleOffset][Normal]_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_Metallic("Metallic", 2D) = "white" {}
		[NoScaleOffset]_Roughness("Roughness", 2D) = "white" {}
		[NoScaleOffset]_AmbientOcclusion("Ambient Occlusion", 2D) = "white" {}
		[NoScaleOffset]_Heigth("Heigth", 2D) = "white" {}
		_TesselationFactor("Tesselation Factor", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Heigth;
		SamplerState sampler_Heigth;
		uniform float _Tilling;
		uniform float _TesselationFactor;
		uniform sampler2D _Normal;
		uniform sampler2D _BaseColor;
		uniform float _BaseColorHue;
		uniform float _BaseColorSat;
		uniform float _BasecolorValue;
		uniform sampler2D _Metallic;
		uniform sampler2D _Roughness;
		uniform sampler2D _AmbientOcclusion;


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
			float2 temp_cast_0 = (_Tilling).xx;
			float2 uv_TexCoord19 = v.texcoord.xy * temp_cast_0;
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( tex2Dlod( _Heigth, float4( uv_TexCoord19, 0, 0.0) ).r * _TesselationFactor ) * ase_vertexNormal );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Tilling).xx;
			float2 uv_TexCoord19 = i.uv_texcoord * temp_cast_0;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_TexCoord19 ) );
			float3 hsvTorgb2_g2 = RGBToHSV( tex2D( _BaseColor, uv_TexCoord19 ).rgb );
			float3 hsvTorgb8_g2 = HSVToRGB( float3(( hsvTorgb2_g2.x + _BaseColorHue ),( hsvTorgb2_g2.y + _BaseColorSat ),( hsvTorgb2_g2.z + _BasecolorValue )) );
			o.Albedo = hsvTorgb8_g2;
			o.Metallic = tex2D( _Metallic, uv_TexCoord19 ).r;
			float4 temp_cast_3 = (0.36).xxxx;
			o.Smoothness = ( ( 1.0 - tex2D( _Roughness, uv_TexCoord19 ) ) - temp_cast_3 ).r;
			o.Occlusion = tex2D( _AmbientOcclusion, uv_TexCoord19 ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
832;1033;1561;728;1491.251;25.58936;1.643266;True;False
Node;AmplifyShaderEditor.RangedFloatNode;20;-1210.74,110.6801;Inherit;False;Property;_Tilling;Tilling;3;0;Create;True;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-966.5625,111.5955;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-358.9782,1089.188;Inherit;False;Property;_TesselationFactor;Tesselation Factor;15;0;Create;True;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-486.8229,514.4772;Inherit;True;Property;_Roughness;Roughness;7;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;63aa69c90be130c45a416f7b308c2fbc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;-505.3106,897.2993;Inherit;True;Property;_Heigth;Heigth;9;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;3e04237c0523c1240bf8d94aeab62ff2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;5;-150.1698,509.0709;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-472.4659,-380.1536;Inherit;False;Property;_BasecolorValue;Base color Value;2;0;Create;True;0;0;False;0;False;0;-0.36;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;24;-102.2398,1133.689;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-117.6441,938.5687;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-472.349,-449.7043;Inherit;False;Property;_BaseColorSat;Base Color Sat;1;0;Create;True;0;0;False;0;False;0;-0.31;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-479.5,-300;Inherit;True;Property;_BaseColor;Base Color;4;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;3bbda0cc041466e4f978436ac3cf095a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-146.4264,595.3612;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;False;0.36;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-472.9666,-520.4992;Inherit;False;Property;_BaseColorHue;Base Color Hue;0;0;Create;True;0;0;False;0;False;0;-0.16;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;18;-90.09219,-507.1901;Inherit;False;HSV;-1;;2;1d09c0b080619d84f85fef3df119befc;0;4;13;FLOAT;0;False;14;FLOAT;0;False;15;FLOAT;0;False;9;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;8;-492.5251,286.5741;Inherit;True;Property;_AmbientOcclusion;Ambient Occlusion;8;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;8ade0a093b382f64cb6f6705b3c50bd1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-481.7265,-107.8676;Inherit;True;Property;_Normal;Normal;5;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;False;-1;None;84e7a49a8e6c18d48a2b1c4495a22a9e;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;192.1535,933.4341;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;3;-482.6324,84.98236;Inherit;True;Property;_Metallic;Metallic;6;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;9862f15ee6e258d4ca7ad61ec90883e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;32.10627,510.2342;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;565.9963,-31.92762;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;BaseTree;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;20;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;10;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;20;0
WireConnection;4;1;19;0
WireConnection;21;1;19;0
WireConnection;5;0;4;0
WireConnection;26;0;21;1
WireConnection;26;1;25;0
WireConnection;1;1;19;0
WireConnection;18;13;11;0
WireConnection;18;14;9;0
WireConnection;18;15;12;0
WireConnection;18;9;1;0
WireConnection;8;1;19;0
WireConnection;2;1;19;0
WireConnection;23;0;26;0
WireConnection;23;1;24;0
WireConnection;3;1;19;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;0;0;18;0
WireConnection;0;1;2;0
WireConnection;0;3;3;0
WireConnection;0;4;6;0
WireConnection;0;5;8;0
WireConnection;0;11;23;0
ASEEND*/
//CHKSM=63D82C90620DFE0B921E619B43437541E19483AD