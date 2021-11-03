// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BaseTree"
{
	Properties
	{
		[NoScaleOffset]_BaseColor("Base Color", 2D) = "white" {}
		[NoScaleOffset]_Noraml("Noraml", 2D) = "bump" {}
		[NoScaleOffset]_Metallic("Metallic", 2D) = "white" {}
		[NoScaleOffset]_Roughness("Roughness", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Noraml;
		uniform sampler2D _BaseColor;
		uniform sampler2D _Metallic;
		uniform sampler2D _Roughness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Noraml2 = i.uv_texcoord;
			o.Normal = UnpackNormal( tex2D( _Noraml, uv_Noraml2 ) );
			float2 uv_BaseColor1 = i.uv_texcoord;
			o.Albedo = tex2D( _BaseColor, uv_BaseColor1 ).rgb;
			float2 uv_Metallic3 = i.uv_texcoord;
			o.Metallic = tex2D( _Metallic, uv_Metallic3 ).r;
			float2 uv_Roughness4 = i.uv_texcoord;
			float4 temp_cast_2 = (0.36).xxxx;
			o.Smoothness = ( ( 1.0 - tex2D( _Roughness, uv_Roughness4 ) ) - temp_cast_2 ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
1381;860;1721;898;1019.025;436.2258;1.3;True;False
Node;AmplifyShaderEditor.SamplerNode;4;-486.8229,514.4772;Inherit;True;Property;_Roughness;Roughness;3;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;63aa69c90be130c45a416f7b308c2fbc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;5;-150.1698,509.0709;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-146.4264,595.3612;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;False;0.36;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-481.7265,-107.8676;Inherit;True;Property;_Noraml;Noraml;1;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;84e7a49a8e6c18d48a2b1c4495a22a9e;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-482.6324,84.98236;Inherit;True;Property;_Metallic;Metallic;2;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;9862f15ee6e258d4ca7ad61ec90883e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;32.10627,510.2342;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;8;-492.5251,286.5741;Inherit;True;Property;_AmbientOcclusion;Ambient Occlusion;4;0;Create;True;0;0;False;0;False;-1;None;8ade0a093b382f64cb6f6705b3c50bd1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-479.5,-300;Inherit;True;Property;_BaseColor;Base Color;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;3bbda0cc041466e4f978436ac3cf095a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;273.3146,-28.50444;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;BaseTree;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;4;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;0;0;1;0
WireConnection;0;1;2;0
WireConnection;0;3;3;0
WireConnection;0;4;6;0
ASEEND*/
//CHKSM=CD537FF4FDE4FE60F76E5C9C7B97D1611D8175CB