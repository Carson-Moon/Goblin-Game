Shader "Custom/DepthBufferMask"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        LOD 100
        
        Pass
        {           
            Ztest Greater
            Zwrite off
            Cull Off
            Colormask 0
            Stencil
            {
                Comp Greater
                Ref 1
                Pass Replace
            }
        }
        
        
    }
}
