// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "EvilSmoke"
{
	Properties
	{
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_NoiseAngle("NoiseAngle", Float) = 2.88
		_NoiseScale("NoiseScale", Float) = 5.22
		_RingSpeed("RingSpeed", Vector) = (1,1,0,0)
		[HDR]_Colour("Colour", Color) = (0.02352941,0.177663,0.282353,1)
		[Toggle]_Distort("Distort", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustom keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Translucency;
		};

		uniform float _Distort;
		uniform float _NoiseScale;
		uniform float _NoiseAngle;
		sampler2D _Sampler604;
		uniform float2 _RingSpeed;
		uniform float4 _Colour;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;


		float2 voronoihash1( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi1( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash1( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float time1 = _NoiseAngle;
			float2 temp_output_1_0_g1 = float2( 1,1 );
			float2 appendResult10_g1 = (float2(( (temp_output_1_0_g1).x * v.texcoord.xy.x ) , ( v.texcoord.xy.y * (temp_output_1_0_g1).y )));
			float2 temp_output_11_0_g1 = float2( 1,0 );
			float2 panner18_g1 = ( ( (temp_output_11_0_g1).x * _Time.y ) * float2( 1,0 ) + v.texcoord.xy);
			float2 panner19_g1 = ( ( _Time.y * (temp_output_11_0_g1).y ) * float2( 0,1 ) + v.texcoord.xy);
			float2 appendResult24_g1 = (float2((panner18_g1).x , (panner19_g1).y));
			float2 temp_output_47_0_g1 = _RingSpeed;
			float2 uv_TexCoord78_g1 = v.texcoord.xy * float2( 2,2 );
			float2 temp_output_31_0_g1 = ( uv_TexCoord78_g1 - float2( 1,1 ) );
			float2 appendResult39_g1 = (float2(frac( ( atan2( (temp_output_31_0_g1).x , (temp_output_31_0_g1).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g1 )));
			float2 panner54_g1 = ( ( (temp_output_47_0_g1).x * _Time.y ) * float2( 1,0 ) + appendResult39_g1);
			float2 panner55_g1 = ( ( _Time.y * (temp_output_47_0_g1).y ) * float2( 0,1 ) + appendResult39_g1);
			float2 appendResult58_g1 = (float2((panner54_g1).x , (panner55_g1).y));
			float2 coords1 = ( ( (tex2Dlod( _Sampler604, float4( ( appendResult10_g1 + appendResult24_g1 ), 0, 0.0) )).rg * 1.0 ) + ( float2( 1,1 ) * appendResult58_g1 ) ) * _NoiseScale;
			float2 id1 = 0;
			float2 uv1 = 0;
			float voroi1 = voronoi1( coords1, time1, id1, uv1, 0 );
			float ifLocalVar19 = 0;
			if( _Distort == 1.0 )
				ifLocalVar19 = voroi1;
			float3 temp_cast_0 = (ifLocalVar19).xxx;
			v.vertex.xyz += temp_cast_0;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float time1 = _NoiseAngle;
			float2 temp_output_1_0_g1 = float2( 1,1 );
			float2 appendResult10_g1 = (float2(( (temp_output_1_0_g1).x * i.uv_texcoord.x ) , ( i.uv_texcoord.y * (temp_output_1_0_g1).y )));
			float2 temp_output_11_0_g1 = float2( 1,0 );
			float2 panner18_g1 = ( ( (temp_output_11_0_g1).x * _Time.y ) * float2( 1,0 ) + i.uv_texcoord);
			float2 panner19_g1 = ( ( _Time.y * (temp_output_11_0_g1).y ) * float2( 0,1 ) + i.uv_texcoord);
			float2 appendResult24_g1 = (float2((panner18_g1).x , (panner19_g1).y));
			float2 temp_output_47_0_g1 = _RingSpeed;
			float2 uv_TexCoord78_g1 = i.uv_texcoord * float2( 2,2 );
			float2 temp_output_31_0_g1 = ( uv_TexCoord78_g1 - float2( 1,1 ) );
			float2 appendResult39_g1 = (float2(frac( ( atan2( (temp_output_31_0_g1).x , (temp_output_31_0_g1).y ) / 6.28318548202515 ) ) , length( temp_output_31_0_g1 )));
			float2 panner54_g1 = ( ( (temp_output_47_0_g1).x * _Time.y ) * float2( 1,0 ) + appendResult39_g1);
			float2 panner55_g1 = ( ( _Time.y * (temp_output_47_0_g1).y ) * float2( 0,1 ) + appendResult39_g1);
			float2 appendResult58_g1 = (float2((panner54_g1).x , (panner55_g1).y));
			float2 coords1 = ( ( (tex2D( _Sampler604, ( appendResult10_g1 + appendResult24_g1 ) )).rg * 1.0 ) + ( float2( 1,1 ) * appendResult58_g1 ) ) * _NoiseScale;
			float2 id1 = 0;
			float2 uv1 = 0;
			float voroi1 = voronoi1( coords1, time1, id1, uv1, 0 );
			float4 temp_output_8_0 = ( voroi1 * _Colour );
			o.Emission = temp_output_8_0.rgb;
			o.Smoothness = temp_output_8_0.r;
			float2 CenteredUV15_g4 = ( i.uv_texcoord - float2( 0.5,0.5 ) );
			float2 break17_g4 = CenteredUV15_g4;
			float2 appendResult23_g4 = (float2(( length( CenteredUV15_g4 ) * 0.08 * 2.0 ) , ( atan2( break17_g4.x , break17_g4.y ) * ( 1.0 / 6.28318548202515 ) * 1.0 )));
			float3 temp_cast_2 = (( ( 1.0 - (appendResult23_g4).x ) * i.vertexColor.a )).xxx;
			o.Translucency = temp_cast_2;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
600;218;1240;794;1014.465;710.9055;1.585;True;False
Node;AmplifyShaderEditor.Vector2Node;9;-709.5,75.5;Inherit;False;Property;_RingSpeed;RingSpeed;10;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;26;-442.2801,332.0247;Inherit;False;Polar Coordinates;-1;;4;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;0.08;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-530.5,-249.5;Inherit;False;Property;_NoiseAngle;NoiseAngle;8;0;Create;True;0;0;0;False;0;False;2.88;2.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;27;-168.0751,319.3445;Inherit;True;True;False;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;4;-531.2751,-40.5;Inherit;False;RadialUVDistortion;-1;;1;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler604;False;1;FLOAT2;1,1;False;11;FLOAT2;1,0;False;65;FLOAT;1;False;68;FLOAT2;1,1;False;47;FLOAT2;0.4,2;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-531.5,-148.5;Inherit;False;Property;_NoiseScale;NoiseScale;9;0;Create;True;0;0;0;False;0;False;5.22;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;29;-174.4152,557.0944;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;28;107.7148,305.0794;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-118.5,10.5;Inherit;False;Property;_Colour;Colour;11;1;[HDR];Create;True;0;0;0;False;0;False;0.02352941,0.177663,0.282353,1;0.02574177,0.02352941,0.282353,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;1;-94.49995,-334.6998;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;21;-559.2714,-565.5187;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-648.9915,-648.0627;Inherit;False;Property;_Distort;Distort;12;1;[Toggle];Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;297.9148,358.9695;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;315.7004,-550.2999;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;13;131.7001,-798.4998;Inherit;False;Constant;_Color1;Color 1;3;0;Create;True;0;0;0;False;0;False;1,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;106.8999,-82.5001;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;356.5518,-812.3212;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;19;-203.2611,-584.323;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;578,-270;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;EvilSmoke;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;TransparentCutout;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;7;0;0;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;26;0
WireConnection;4;47;9;0
WireConnection;28;0;27;0
WireConnection;1;0;4;0
WireConnection;1;1;2;0
WireConnection;1;2;3;0
WireConnection;30;0;28;0
WireConnection;30;1;29;4
WireConnection;15;0;18;0
WireConnection;15;1;8;0
WireConnection;8;0;1;0
WireConnection;8;1;7;0
WireConnection;19;0;20;0
WireConnection;19;1;21;0
WireConnection;19;3;1;0
WireConnection;0;2;8;0
WireConnection;0;4;8;0
WireConnection;0;7;30;0
WireConnection;0;11;19;0
ASEEND*/
//CHKSM=8CE5F65977AA2076968199842F7199FF19FB3BEC