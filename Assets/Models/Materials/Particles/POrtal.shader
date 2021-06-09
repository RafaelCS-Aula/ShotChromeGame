// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "POrtal"
{
	Properties
	{
		[HDR][Gamma]_Color0("Color 0", Color) = (0.1792453,0.01099146,0.1101967,1)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float4 screenPos;
		};

		uniform float4 _Color0;


		float2 voronoihash12( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi12( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash12( n + g );
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
			float time12 = _Time.y;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float simplePerlin2D1 = snoise( ase_screenPosNorm.xy*2.11 );
			simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
			float2 temp_cast_1 = (simplePerlin2D1).xx;
			float2 coords12 = temp_cast_1 * 400.0;
			float2 id12 = 0;
			float2 uv12 = 0;
			float voroi12 = voronoi12( coords12, time12, id12, uv12, 0 );
			float2 temp_cast_2 = (voroi12).xx;
			float temp_output_2_0_g1 = 1.09;
			float temp_output_3_0_g1 = 0.25;
			float2 appendResult21_g1 = (float2(temp_output_2_0_g1 , temp_output_3_0_g1));
			float Radius25_g1 = max( min( min( abs( ( 0.86 * 2 ) ) , abs( temp_output_2_0_g1 ) ) , abs( temp_output_3_0_g1 ) ) , 1E-05 );
			float2 temp_cast_3 = (0.0).xx;
			float temp_output_30_0_g1 = ( length( max( ( ( abs( (temp_cast_2*2.0 + -1.0) ) - appendResult21_g1 ) + Radius25_g1 ) , temp_cast_3 ) ) / Radius25_g1 );
			o.Emission = ( _Color0 + ( saturate( ( ( 1.0 - temp_output_30_0_g1 ) / fwidth( temp_output_30_0_g1 ) ) ) * 30.2 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
484;266;1495;833;1488.659;371.74;1.21;True;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;3;-1197.05,-301.5602;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;16;-806.6097,369.5999;Inherit;False;232;166;Comment;1;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1011.92,71.12003;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-1013.131,-196.2901;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2.11;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-756.6097,419.5999;Inherit;False;Constant;_Float9;Float 9;0;0;Create;True;0;0;0;False;0;False;0.86;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-878.8201,259.88;Inherit;False;Constant;_Float7;Float 7;0;0;Create;True;0;0;0;False;0;False;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-836.4702,170.3398;Inherit;False;Constant;_Float8;Float 8;0;0;Create;True;0;0;0;False;0;False;1.09;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;12;-579.9502,-228.9601;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0.33;False;2;FLOAT;400;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;22;-512.1905,342.16;Inherit;False;Constant;_Float10;Float 10;0;0;Create;True;0;0;0;False;0;False;30.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;4;-410.5499,-9.950035;Inherit;True;Rounded Rectangle;-1;;1;8679f72f5be758f47babb3ba1d5f51d3;0;4;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-400.8697,-256.7901;Inherit;False;Property;_Color0;Color 0;0;2;[HDR];[Gamma];Create;True;0;0;0;False;0;False;0.1792453,0.01099146,0.1101967,1;0.008321469,0.02877546,0.1037736,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-295.5999,195.75;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-172.1805,-189.0298;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;POrtal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;3;0
WireConnection;12;0;1;0
WireConnection;12;1;20;0
WireConnection;4;1;12;0
WireConnection;4;2;6;0
WireConnection;4;3;5;0
WireConnection;4;4;7;0
WireConnection;25;0;4;0
WireConnection;25;1;22;0
WireConnection;23;0;13;0
WireConnection;23;1;25;0
WireConnection;0;2;23;0
ASEEND*/
//CHKSM=1A666ACA0EEFF5D233AEB8A51340F9576AF79901