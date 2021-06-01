// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderBolt"
{
	Properties
	{
		_CoreBoltWidth("CoreBoltWidth", Range( 0 , 1)) = 1
		_BoltWidth1("BoltWidth", Range( 0 , 3)) = 2.905882
		_CoreRadius("CoreRadius", Float) = 0
		_Radius1("Radius", Float) = 0
		_CoreColour("CoreColour", Color) = (0,0,0,0)
		_GlowPower("GlowPower", Float) = 10
		_ShineColour("ShineColour", Color) = (1,0.8213265,0.1745283,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _BoltWidth1;
		uniform float _Radius1;
		uniform float4 _ShineColour;
		uniform float _CoreBoltWidth;
		uniform float _CoreRadius;
		uniform float4 _CoreColour;
		uniform float _GlowPower;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_2_0_g5 = 2.905882;
			float clampResult51 = clamp( ( _SinTime.z * 1.0 ) , -1.0 , 1.0 );
			float temp_output_3_0_g5 = ( (0.5 + (clampResult51 - -1.0) * (1.0 - 0.5) / (1.0 - -1.0)) * _BoltWidth1 );
			float2 appendResult21_g5 = (float2(temp_output_2_0_g5 , temp_output_3_0_g5));
			float Radius25_g5 = max( min( min( abs( ( _Radius1 * 2 ) ) , abs( temp_output_2_0_g5 ) ) , abs( temp_output_3_0_g5 ) ) , 1E-05 );
			float2 temp_cast_0 = (0.0).xx;
			float temp_output_30_0_g5 = ( length( max( ( ( abs( (i.uv_texcoord*2.0 + -1.0) ) - appendResult21_g5 ) + Radius25_g5 ) , temp_cast_0 ) ) / Radius25_g5 );
			float temp_output_2_0_g6 = 0.44834;
			float clampResult36 = clamp( ( _SinTime.w * 1.0 ) , -1.0 , 1.0 );
			float temp_output_3_0_g6 = ( (0.5 + (clampResult36 - -1.0) * (1.0 - 0.5) / (1.0 - -1.0)) * _CoreBoltWidth );
			float2 appendResult21_g6 = (float2(temp_output_2_0_g6 , temp_output_3_0_g6));
			float Radius25_g6 = max( min( min( abs( ( _CoreRadius * 2 ) ) , abs( temp_output_2_0_g6 ) ) , abs( temp_output_3_0_g6 ) ) , 1E-05 );
			float2 temp_cast_1 = (0.0).xx;
			float temp_output_30_0_g6 = ( length( max( ( ( abs( (i.uv_texcoord*2.0 + -1.0) ) - appendResult21_g6 ) + Radius25_g6 ) , temp_cast_1 ) ) / Radius25_g6 );
			float4 temp_output_55_0 = ( ( saturate( ( ( 1.0 - temp_output_30_0_g5 ) / fwidth( temp_output_30_0_g5 ) ) ) * _ShineColour ) + ( saturate( ( ( 1.0 - temp_output_30_0_g6 ) / fwidth( temp_output_30_0_g6 ) ) ) * _CoreColour ) );
			o.Emission = ( temp_output_55_0 * _GlowPower ).rgb;
			o.Alpha = temp_output_55_0.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
Version=18900
696;156;1415;771;1348.194;1575.669;1.11;False;False
Node;AmplifyShaderEditor.SinTimeNode;32;-1244.106,-649.7139;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;49;-1201.737,-1417.049;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1047.844,-1303.11;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1099.092,-577.9544;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;51;-812.6049,-1369.889;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;36;-922.6827,-595.8943;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-836.4136,-277.2652;Inherit;False;Property;_CoreBoltWidth;CoreBoltWidth;0;0;Create;True;0;0;0;False;0;False;1;0.824;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;-746.2722,-603.3688;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-726.3359,-1051.26;Inherit;False;Property;_BoltWidth1;BoltWidth;1;0;Create;True;0;0;0;False;0;False;2.905882;1;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;52;-636.1948,-1377.364;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-558.4579,-758.4344;Inherit;False;Property;_Radius1;Radius;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-668.5352,15.56067;Inherit;False;Property;_CoreRadius;CoreRadius;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-659.1127,-893.0949;Inherit;False;Constant;_BoltHeight1;BoltHeight;1;0;Create;True;0;0;0;False;0;False;2.905882;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-526.0201,-547.5893;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-769.1902,-119.0999;Inherit;False;Constant;_CoreBoltHeight;CoreBoltHeight;1;0;Create;True;0;0;0;False;0;False;0.44834;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-415.9434,-1321.585;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;-291.793,-756.0851;Inherit;False;Property;_ShineColour;ShineColour;6;0;Create;True;0;0;0;False;0;False;1,0.8213265,0.1745283,1;1,0.8213265,0.1745283,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;1;-432.8846,-370.6898;Inherit;True;Rounded Rectangle;-1;;6;8679f72f5be758f47babb3ba1d5f51d3;0;4;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;53;-263.9779,-1060.325;Inherit;True;Rounded Rectangle;-1;;5;8679f72f5be758f47babb3ba1d5f51d3;0;4;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;-398.0325,-138.2222;Inherit;False;Property;_CoreColour;CoreColour;4;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;8.701789,-898.1095;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-164.9325,-405.7321;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-28.67379,-185.225;Inherit;False;Property;_GlowPower;GlowPower;5;0;Create;True;0;0;0;False;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-80.64327,-565.7598;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;87.91627,-258.7654;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;341.0196,-538.2399;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ThunderBolt;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;0;0;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;0;49;3
WireConnection;37;0;32;4
WireConnection;51;0;50;0
WireConnection;36;0;37;0
WireConnection;33;0;36;0
WireConnection;52;0;51;0
WireConnection;18;0;33;0
WireConnection;18;1;2;0
WireConnection;47;0;52;0
WireConnection;47;1;44;0
WireConnection;1;2;3;0
WireConnection;1;3;18;0
WireConnection;1;4;26;0
WireConnection;53;2;46;0
WireConnection;53;3;47;0
WireConnection;53;4;45;0
WireConnection;56;0;53;0
WireConnection;56;1;54;0
WireConnection;60;0;1;0
WireConnection;60;1;59;0
WireConnection;55;0;56;0
WireConnection;55;1;60;0
WireConnection;57;0;55;0
WireConnection;57;1;58;0
WireConnection;0;2;57;0
WireConnection;0;9;55;0
ASEEND*/
//CHKSM=F3AB81EEBB63C744C5A6053C4D32AF336B858FDD