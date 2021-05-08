Shader "Custom/Vortex" {
Properties {
		_Color ("Vortex Arms Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_VortexDistortion( "Vortex Distortion", Range( -200, 200) ) = 20.0
		_VortexSpeed( "Vortex Speed", Range( -10,10) ) = 5
		_ArmsNumber("Vortex Arms Number", float) = 6
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
		Blend One One
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:blend
		#pragma target 3.0	

		sampler2D _MainTex;		
		fixed4 _Color;
		half _VortexSpeed;
		half _VortexDistortion;
		half _ArmsNumber;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{					
			fixed x = IN.uv_MainTex.x-0.5;
			fixed y = IN.uv_MainTex.y-0.5;
			
			float dis = sqrt(y*y + x*x);
			
			o.Albedo = sin((_Time.w * _VortexSpeed) + atan2(y , x) * floor(_ArmsNumber) - _VortexDistortion * dis) * _Color.rgb;
			o.Alpha = saturate( 1 - dis*2);

		}
		ENDCG
	} 
	FallBack "Diffuse"
}