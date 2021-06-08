// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderBolt"
{
	Properties
	{
		_CoreBoltWidth("CoreBoltWidth", Range( 0 , 1)) = 1
		_BoltWidth("BoltWidth", Range( 0 , 3)) = 2.905882
		_CoreRadius("CoreRadius", Float) = 0
		_Radius("Radius", Float) = 0
		_CoreColour("CoreColour", Color) = (0,0,0,0)
		_GlowPower("GlowPower", Float) = 10
		_ShineColour("ShineColour", Color) = (1,0.8213265,0.1745283,1)
		_NoiseScale("NoiseScale", Float) = 0
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

		uniform float _BoltWidth;
		uniform float _NoiseScale;
		uniform float _Radius;
		uniform float4 _ShineColour;
		uniform float _CoreBoltWidth;
		uniform float _CoreRadius;
		uniform float4 _CoreColour;
		uniform float _GlowPower;


		float2 voronoihash73( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi73( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash73( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.500 * pow( ( pow( abs( r.x ), 1 ) + pow( abs( r.y ), 1 ) ), 1.000 );
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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float clampResult51 = clamp( ( _SinTime.z * 1.0 ) , -1.0 , 1.0 );
			float time73 = 3.6;
			float2 panner62 = ( _Time.y * float2( 0,-1 ) + i.uv_texcoord);
			float simplePerlin2D71 = snoise( panner62 );
			simplePerlin2D71 = simplePerlin2D71*0.5 + 0.5;
			float2 temp_cast_0 = (simplePerlin2D71).xx;
			float2 coords73 = temp_cast_0 * _NoiseScale;
			float2 id73 = 0;
			float2 uv73 = 0;
			float voroi73 = voronoi73( coords73, time73, id73, uv73, 0 );
			float2 appendResult10_g15 = (float2(voroi73 , 1.0));
			float2 temp_output_11_0_g15 = ( abs( (i.uv_texcoord*2.0 + -1.0) ) - appendResult10_g15 );
			float2 break16_g15 = ( 1.0 - ( temp_output_11_0_g15 / fwidth( temp_output_11_0_g15 ) ) );
			float temp_output_63_0 = saturate( min( break16_g15.x , break16_g15.y ) );
			float temp_output_2_0_g18 = ( (0.5 + (clampResult51 - -1.0) * (1.0 - 0.5) / (1.0 - -1.0)) * _BoltWidth * temp_output_63_0 );
			float temp_output_3_0_g18 = 2.905882;
			float2 appendResult21_g18 = (float2(temp_output_2_0_g18 , temp_output_3_0_g18));
			float Radius25_g18 = max( min( min( abs( ( _Radius * 2 ) ) , abs( temp_output_2_0_g18 ) ) , abs( temp_output_3_0_g18 ) ) , 1E-05 );
			float2 temp_cast_1 = (0.0).xx;
			float temp_output_30_0_g18 = ( length( max( ( ( abs( (i.uv_texcoord*2.0 + -1.0) ) - appendResult21_g18 ) + Radius25_g18 ) , temp_cast_1 ) ) / Radius25_g18 );
			float4 temp_output_56_0 = ( saturate( ( ( 1.0 - temp_output_30_0_g18 ) / fwidth( temp_output_30_0_g18 ) ) ) * _ShineColour );
			float clampResult36 = clamp( ( _SinTime.w * 1.0 ) , -1.0 , 1.0 );
			float temp_output_2_0_g17 = ( (0.5 + (clampResult36 - -1.0) * (1.0 - 0.5) / (1.0 - -1.0)) * _CoreBoltWidth * temp_output_63_0 );
			float temp_output_3_0_g17 = 1.0;
			float2 appendResult21_g17 = (float2(temp_output_2_0_g17 , temp_output_3_0_g17));
			float Radius25_g17 = max( min( min( abs( ( _CoreRadius * 2 ) ) , abs( temp_output_2_0_g17 ) ) , abs( temp_output_3_0_g17 ) ) , 1E-05 );
			float2 temp_cast_2 = (0.0).xx;
			float temp_output_30_0_g17 = ( length( max( ( ( abs( (i.uv_texcoord*2.0 + -1.0) ) - appendResult21_g17 ) + Radius25_g17 ) , temp_cast_2 ) ) / Radius25_g17 );
			o.Emission = ( ( temp_output_56_0 + ( saturate( ( ( 1.0 - temp_output_30_0_g17 ) / fwidth( temp_output_30_0_g17 ) ) ) * _CoreColour ) ) * _GlowPower ).rgb;
			o.Alpha = temp_output_56_0.r;
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
485;194;1240;794;455.6122;745.8888;1;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;68;-607.3375,568.9089;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;67;-658.84,366.945;Inherit;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;0;False;0;False;0,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-719.0612,171.5888;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;62;-418.3445,315.6807;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;1,2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinTimeNode;32;-1244.106,-649.7139;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinTimeNode;49;-1201.737,-1417.049;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;84;-256.3207,194.5486;Inherit;False;Property;_NoiseScale;NoiseScale;7;0;Create;True;0;0;0;False;0;False;0;179.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1047.844,-1303.11;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;71;-90.2153,337.5381;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1099.092,-577.9544;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;73;149.444,193.9361;Inherit;True;0;4;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;3.6;False;2;FLOAT;1556.2;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.ClampOpNode;51;-812.6049,-1369.889;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;36;-922.6827,-595.8943;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-851.2343,-355.071;Inherit;False;Property;_CoreBoltWidth;CoreBoltWidth;0;0;Create;True;0;0;0;False;0;False;1;0.474;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;-746.2722,-603.3688;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-726.3359,-1051.26;Inherit;False;Property;_BoltWidth;BoltWidth;1;0;Create;True;0;0;0;False;0;False;2.905882;3;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;52;-636.1948,-1377.364;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;63;142.6481,-65.41171;Inherit;True;Rectangle;-1;;15;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-589.3937,-763.3842;Inherit;False;Property;_Radius;Radius;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-774.2313,-971.0213;Inherit;False;Constant;_BoltHeight;BoltHeight;1;0;Create;True;0;0;0;False;0;False;2.905882;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-441.9434,-1317.585;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-906.2742,-193.1999;Inherit;False;Constant;_CoreBoltHeight;CoreBoltHeight;1;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-668.5352,15.56067;Inherit;False;Property;_CoreRadius;CoreRadius;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-526.0201,-547.5893;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1;-396.3635,-408.251;Inherit;True;Rounded Rectangle;-1;;17;8679f72f5be758f47babb3ba1d5f51d3;0;4;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;-291.793,-756.0851;Inherit;False;Property;_ShineColour;ShineColour;6;0;Create;True;0;0;0;False;0;False;1,0.8213265,0.1745283,1;1,0.8538916,0.1650943,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;53;-291.8136,-1057.222;Inherit;True;Rounded Rectangle;-1;;18;8679f72f5be758f47babb3ba1d5f51d3;0;4;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;-314.5705,-159.3864;Inherit;False;Property;_CoreColour;CoreColour;4;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.1913492,0.9148287,0.9433962,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;8.701789,-898.1095;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-130.7826,-583.8119;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-36.8588,-301.8899;Inherit;False;Property;_GlowPower;GlowPower;5;0;Create;True;0;0;0;False;0;False;10;40;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;66.58906,-667.8298;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;82;884.2369,187.7377;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;74;378.437,342.5381;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;216.9839,-519.1042;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;83;511.6376,94.13771;Inherit;False;1;0;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;727.6355,203.9369;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;488.8298,-636.7499;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ThunderBolt;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;62;0;70;0
WireConnection;62;2;67;0
WireConnection;62;1;68;0
WireConnection;50;0;49;3
WireConnection;71;0;62;0
WireConnection;37;0;32;4
WireConnection;73;0;71;0
WireConnection;73;2;84;0
WireConnection;51;0;50;0
WireConnection;36;0;37;0
WireConnection;33;0;36;0
WireConnection;52;0;51;0
WireConnection;63;2;73;0
WireConnection;47;0;52;0
WireConnection;47;1;44;0
WireConnection;47;2;63;0
WireConnection;18;0;33;0
WireConnection;18;1;2;0
WireConnection;18;2;63;0
WireConnection;1;2;18;0
WireConnection;1;3;3;0
WireConnection;1;4;26;0
WireConnection;53;2;47;0
WireConnection;53;3;46;0
WireConnection;53;4;45;0
WireConnection;56;0;53;0
WireConnection;56;1;54;0
WireConnection;60;0;1;0
WireConnection;60;1;59;0
WireConnection;55;0;56;0
WireConnection;55;1;60;0
WireConnection;82;0;80;0
WireConnection;57;0;55;0
WireConnection;57;1;58;0
WireConnection;80;0;74;2
WireConnection;80;1;83;0
WireConnection;80;2;74;3
WireConnection;0;2;57;0
WireConnection;0;9;56;0
ASEEND*/
//CHKSM=D77FE6786039E7381B55D5C3F67054DFE4BD2ED1