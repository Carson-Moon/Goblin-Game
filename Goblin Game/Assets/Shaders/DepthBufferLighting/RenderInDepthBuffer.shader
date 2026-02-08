Shader "Custom/RenderInDepthBuffer"
{
    Properties
    {
        [HDR]_Color("Color",Color) = (1,1,1,1)
        _RotationSpeed("Rotation Speed (deg/s)", Range(-180,180)) = 30
        _ScaleAmplitude("Scale Amplitude", Range(0,1)) = 0.1
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _NoiseTiling("Noise Tiling", Range(0.1,10)) = 1
        _NoiseScrollSpeed("Noise Scroll Speed", Range(0,5)) = 0
    }
    HLSLINCLUDE
    
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"        
    struct appdata
    {
        float4 vertex : POSITION;
    };
    
    struct v2f
    {
        float4 vertex : SV_POSITION;
    };

    TEXTURE2D(_NoiseTex);
    SAMPLER(sampler_NoiseTex);

    CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _RotationSpeed;
        float _ScaleAmplitude;
        float _NoiseTiling;
        float _NoiseScrollSpeed;
    CBUFFER_END

    v2f vert(appdata v)
    {
        v2f o;
        float3 p = v.vertex.xyz;

        // Uniform scale from tiling noise texture
        float2 noiseUv = p.xz * _NoiseTiling + _Time.y * _NoiseScrollSpeed;
        float noise = SAMPLE_TEXTURE2D_LOD(_NoiseTex, sampler_NoiseTex, noiseUv, 0).r;
        float scale = 1.0 + (noise * 2.0 - 1.0) * _ScaleAmplitude;
        p *= scale;

        // Clockwise rotation around object Y axis
        float angle = radians(-_RotationSpeed) * _Time.y;
        float s, c;
        sincos(angle, s, c);
        p = float3(c * p.x - s * p.z, p.y, s * p.x + c * p.z);

        o.vertex = TransformObjectToHClip(p);
        return o;
    }
    
    float4 frag(v2f i) : SV_Target
    {
        return _Color * _Color.a;
    }
    ENDHLSL
 
    SubShader
    {
        Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}  
        Pass
        {
            Tags
            {
                "RenderType" = "Transparent"          
                "RenderPipeline" = "UniversalPipeline"            
            }         
            Zwrite off
            Ztest Lequal
            Cull Back
            Blend DstColor One
            
            Stencil
            {
                comp equal
                ref 1
                pass zero
                fail zero
                zfail zero
            }         
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag         
            ENDHLSL
        }      
        Pass
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "LightMode" = "UniversalForward"
            }
            ZTest always
            ZWrite off
            Cull Front
            Blend DstColor One
 
            Stencil
            {
                Ref 1
                Comp equal
                Pass Zero 
                fail zero
                Zfail zero
            }         
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }     
    }
}
