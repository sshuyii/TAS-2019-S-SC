// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GlassEffectShader"
{
	Properties
	{
		_Specular("Specular", Color) = (0.06808473,0.2685291,0.3207547,1)
		_Distortion("Distortion", Range( 0 , 1)) = 0.7153803
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_MainTex("MainTex", 2D) = "white" {}
		_TimeScale("TimeScale", Range( 0 , 1)) = 0.9656876
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityStandardUtils.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform float4 _Specular;
			uniform sampler2D _MainTex;
			uniform float _Distortion;
			uniform sampler2D _TextureSample0;
			uniform float _TimeScale;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord = screenPos;
				
				float3 vertexValue =  float3(0,0,0) ;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float4 screenPos = i.ase_texcoord;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 temp_output_7_0 = (ase_screenPosNorm).xy;
				float mulTime27 = _Time.y * _TimeScale;
				float4 appendResult31 = (float4(sin( mulTime27 ) , 0.0 , 0.0 , 0.0));
				
				
				finalColor = ( _Specular * tex2D( _MainTex, (( float3( temp_output_7_0 ,  0.0 ) + UnpackScaleNormal( tex2D( _TextureSample0, ( float4( temp_output_7_0, 0.0 , 0.0 ) + ( appendResult31 + float4( 0,0,0,0 ) ) ).xy ), _Distortion ) )).xyz.xy ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16700
120;114;1563;817;1680.129;353.4418;1.333964;True;False
Node;AmplifyShaderEditor.RangedFloatNode;29;-1508.203,11.20945;Float;False;Property;_TimeScale;TimeScale;4;0;Create;True;0;0;False;0;0.9656876;0.9656876;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;27;-1213.499,7.094405;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;-1033.64,6.788098;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;26;-1139.199,-290.7595;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;31;-995.0739,201.6629;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;7;-920.8698,-208.655;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-747.079,216.3055;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-614.6943,156.4467;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1204.324,603.9749;Float;False;Property;_Distortion;Distortion;1;0;Create;True;0;0;False;0;0.7153803;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-715.0569,496.3285;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;f53512d44b91e954dae7bf028209df1a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-348.8089,196.4408;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;37;-191.4547,226.8894;Float;False;True;True;True;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;25;-26.26499,540.9998;Float;True;Property;_MainTex;MainTex;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-474.9677,-261.3188;Float;False;Property;_Specular;Specular;0;0;Create;True;0;0;False;0;0.06808473,0.2685291,0.3207547,1;0.06808473,0.2685291,0.3207547,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;84.44517,60.13985;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GrabScreenPosition;6;-1392.531,-292.7954;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;24;401.2009,410.0214;Float;False;True;2;Float;ASEMaterialInspector;0;1;GlassEffectShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;27;0;29;0
WireConnection;34;0;27;0
WireConnection;31;0;34;0
WireConnection;7;0;26;0
WireConnection;28;0;31;0
WireConnection;9;0;7;0
WireConnection;9;1;28;0
WireConnection;12;1;9;0
WireConnection;12;5;21;0
WireConnection;36;0;7;0
WireConnection;36;1;12;0
WireConnection;37;0;36;0
WireConnection;25;1;37;0
WireConnection;22;0;4;0
WireConnection;22;1;25;0
WireConnection;24;0;22;0
ASEEND*/
//CHKSM=F76C2DEB775B774FB859C99D1E91BEFB11584A79