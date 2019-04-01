// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "InClassFishDemo"
{
	Properties
	{
		_Frequency("Frequency", Float) = 1
		_TimeOffset("TimeOffset", Float) = 1
		_Amplitude("Amplitude", Float) = 1
		_AplitudeOffset("AplitudeOffset", Float) = 1
		_BlueOffset("BlueOffset", Float) = 1
		_PositionalOffset("Positional Offset", Float) = 30
		_PositionalAmplitudeScaler("Positional Amplitude Scaler", Float) = 1
		_BluePositionalAmplitudeScaler("Blue Positional Amplitude Scaler", Float) = 1
		_BlueAmplitude("Blue Amplitude", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform float _BluePositionalAmplitudeScaler;
		uniform float _Frequency;
		uniform float _TimeOffset;
		uniform float _PositionalOffset;
		uniform float _BlueAmplitude;
		uniform float _BlueOffset;
		uniform float _Amplitude;
		uniform float _PositionalAmplitudeScaler;
		uniform float _AplitudeOffset;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float temp_output_5_0 = sin( ( ( _Frequency * _Time.y ) + _TimeOffset + ( abs( ase_vertex3Pos.z ) * _PositionalOffset ) ) );
			float4 appendResult14 = (float4(( ( v.color.b * ( ase_vertex3Pos.z * _BluePositionalAmplitudeScaler ) * temp_output_5_0 * _BlueAmplitude ) + _BlueOffset ) , ( ( _Amplitude * temp_output_5_0 * abs( ( ase_vertex3Pos.x * _PositionalAmplitudeScaler ) ) ) + _AplitudeOffset ) , 0.0 , 0.0));
			v.vertex.xyz += appendResult14.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
-1920;23;1920;1035;2380.781;834.8655;1.786523;True;False
Node;AmplifyShaderEditor.CommentaryNode;20;-1416.326,-221.2008;Float;False;937.5927;839.6923;Adding Scaled and Offset Value to the Vertex's Y Position;4;5;7;18;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;18;-1338.479,229.9619;Float;False;819.0781;344.942;Scales Vertex Y Position;4;27;17;15;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;15;-1277.88,274.9885;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;19;-1342.118,-168.369;Float;False;369.2501;359.3654;Scales and Offsets TIme Input;4;2;10;6;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1313.269,-113.7976;Float;False;Property;_Frequency;Frequency;0;0;Create;True;0;0;False;0;1;2.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-1316.974,-9.672095;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;27;-1072.865,283.5041;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1274.363,437.6578;Float;False;Property;_PositionalOffset;Positional Offset;5;0;Create;True;0;0;False;0;30;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;25;-974.6489,692.2363;Float;False;495;226.5;Uses distance from origin as scalar multiplier of amtitude;2;23;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1188.806,75.11174;Float;False;Property;_TimeOffset;TimeOffset;1;0;Create;True;0;0;False;0;1;-2.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-923.3581,804.2363;Float;False;Property;_PositionalAmplitudeScaler;Positional Amplitude Scaler;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1145.479,-115.5675;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-680.2662,390.6424;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-644.3581,742.2363;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-777.0122,-114.4596;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;37;-546.6694,-755.3232;Float;False;711.4996;399.7936;Blue;5;30;36;31;32;33;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;21;-454.6909,-219.2096;Float;False;536.6058;383.265;scaling and offseting the output;4;8;3;11;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-203.7601,388.5602;Float;False;Property;_BluePositionalAmplitudeScaler;Blue Positional Amplitude Scaler;7;0;Create;True;0;0;False;0;1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-404.6909,-169.2096;Float;False;Property;_Amplitude;Amplitude;2;0;Create;True;0;0;False;0;1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;75.23975,327.8954;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;28;-400.3845,555.5193;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;5;-633.0286,-112.1102;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-336.1231,-546.064;Float;False;Property;_BlueAmplitude;Blue Amplitude;8;0;Create;True;0;0;False;0;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;30;-496.6696,-705.3232;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-379.0383,55.67469;Float;False;Property;_AplitudeOffset;AplitudeOffset;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-161.5206,-702.5439;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-244.9356,-124.7504;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-197.4755,-470.5303;Float;False;Property;_BlueOffset;BlueOffset;4;0;Create;True;0;0;False;0;1;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;22;143.2705,-59.14374;Float;False;213;227;applying to x axis;1;14;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;10.83006,-596.6885;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-72.58499,-18.895;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;193.2705,-9.143738;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;26;211.2333,-268.9526;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;543.0498,-173.6699;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;InClassFishDemo;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;15;3
WireConnection;10;0;6;0
WireConnection;10;1;2;0
WireConnection;16;0;27;0
WireConnection;16;1;17;0
WireConnection;24;0;15;1
WireConnection;24;1;23;0
WireConnection;7;0;10;0
WireConnection;7;1;9;0
WireConnection;7;2;16;0
WireConnection;34;0;15;3
WireConnection;34;1;35;0
WireConnection;28;0;24;0
WireConnection;5;0;7;0
WireConnection;31;0;30;3
WireConnection;31;1;34;0
WireConnection;31;2;5;0
WireConnection;31;3;36;0
WireConnection;4;0;3;0
WireConnection;4;1;5;0
WireConnection;4;2;28;0
WireConnection;33;0;31;0
WireConnection;33;1;32;0
WireConnection;8;0;4;0
WireConnection;8;1;11;0
WireConnection;14;0;33;0
WireConnection;14;1;8;0
WireConnection;0;11;14;0
ASEEND*/
//CHKSM=E9226D27B562E0B9CAAAB8759B24AD1C76F1273F