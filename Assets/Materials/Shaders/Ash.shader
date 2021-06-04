// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Ash"
{
	Properties
	{
		_Float0("Float 0", Float) = 0
		_Color1("Color 1", Color) = (0.2093272,0.9056604,0.8349391,0)
		_GlowForce("GlowForce", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Float0;
		uniform float4 _Color1;
		uniform float _GlowForce;


		float2 voronoihash21( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi21( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash21( n + g );
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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float time21 = 0.74;
			float simplePerlin2D1 = snoise( v.texcoord.xy*_Float0 );
			simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
			float2 temp_cast_0 = (simplePerlin2D1).xx;
			float2 coords21 = temp_cast_0 * 1.32;
			float2 id21 = 0;
			float2 uv21 = 0;
			float voroi21 = voronoi21( coords21, time21, id21, uv21, 0 );
			float3 temp_cast_1 = (voroi21).xxx;
			v.normal = temp_cast_1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float time21 = 0.74;
			float simplePerlin2D1 = snoise( i.uv_texcoord*_Float0 );
			simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
			float2 temp_cast_0 = (simplePerlin2D1).xx;
			float2 coords21 = temp_cast_0 * 1.32;
			float2 id21 = 0;
			float2 uv21 = 0;
			float voroi21 = voronoi21( coords21, time21, id21, uv21, 0 );
			float4 temp_output_17_0 = ( voroi21 * _Color1 );
			o.Albedo = temp_output_17_0.rgb;
			o.Emission = ( float4( 0,0,0,0 ) + ( _GlowForce * temp_output_17_0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
730;147;1240;812;728.1387;512.2228;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-705.2381,-214.6601;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-763.3184,126.82;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0;1.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-520.8185,-88.49976;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;-100.2371,313.9261;Inherit;False;Property;_Color1;Color 1;1;0;Create;True;0;0;0;False;0;False;0.2093272,0.9056604,0.8349391,0;0.07186722,0.8473445,0.8962264,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;21;-323.8377,172.8767;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0.74;False;2;FLOAT;1.32;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;19;-30.03774,-16.27302;Inherit;False;Property;_GlowForce;GlowForce;2;0;Create;True;0;0;0;False;0;False;0;6.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;93.46238,128.0263;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;154.5622,-50.07304;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;15;-253.0798,-149.1199;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;-0.08;False;2;FLOAT;1;False;3;FLOAT;2.51;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;10;-19.87921,-526.3588;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-264.499,-336.8192;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;0;False;0;False;0.1792453,0.1792453,0.1792453,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;6;-602.0583,-559.4991;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;108.59;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;27.58075,-426.7596;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;245.5624,-284.073;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;371.5193,-222.9203;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Ash;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;4;0
WireConnection;1;1;3;0
WireConnection;21;0;1;0
WireConnection;17;0;21;0
WireConnection;17;1;16;0
WireConnection;20;0;19;0
WireConnection;20;1;17;0
WireConnection;10;0;6;0
WireConnection;6;0;4;0
WireConnection;11;0;10;0
WireConnection;11;1;7;0
WireConnection;18;1;20;0
WireConnection;0;0;17;0
WireConnection;0;2;18;0
WireConnection;0;12;21;0
ASEEND*/
//CHKSM=FA3A4AD8D6D363AE39B7F608E9A2AC74F48FA6DA