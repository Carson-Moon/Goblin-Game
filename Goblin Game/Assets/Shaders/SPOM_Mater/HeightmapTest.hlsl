void ReadHeight_float(
    float2 uv,
    UnityTexture2D _Heightmap,
    float3 ViewDirTS,
    int Steps,
    float HeightScale,
    float Bias, // 0.0 is full inward, 0.5 is centered, 1.0 is outward
    out float2 HitUV,
    out float SilhouetteMask, // 1.0 visible, 0.0 clipped
    out float OutDepthOffset
)
{
    // normalize the view vector
    float3 viewDir = normalize(ViewDirTS);

    // calculate the step size for each iteration, using perspective division. 
    // multiply heightscale here for the maximum offset, representing the actual physical depth
    float2 rayDirection = (viewDir.xy / max(viewDir.z, 0.0001)) * HeightScale;

    // Step sizes must divide the total offset by the total number of steps present.
    float2 uvStep = rayDirection / (float)Steps;
    float heightStep = 1.0 / (float)Steps;

    float2 currentUV = uv + (uvStep * (Steps * Bias));
    float currentRayHeight = 1.0;

    float2 previousUV = currentUV;
    float previousRayHeight = currentRayHeight;
    float previousSurfaceHeight = 0.0;

    bool intersected = false;

    float rayDepth;

    UNITY_LOOP
    for (int i = 0; i < Steps; i++)
    {
        // Save where we were before taking this step
        previousUV = currentUV;
        previousRayHeight = currentRayHeight;
        previousSurfaceHeight = SAMPLE_TEXTURE2D(_Heightmap.tex, _Heightmap.samplerstate, previousUV).r;
        
        
        // march forward along the ray
        currentUV -= uvStep; // move the texture opposite to the view vector
        currentRayHeight -= heightStep;

        // sample the height field at new position
        float surfaceHeight = SAMPLE_TEXTURE2D(_Heightmap.tex, _Heightmap.samplerstate, currentUV).r;

        // check to see if our ray has intersected with the surface described by the heightmap.
        if (currentRayHeight <= surfaceHeight)
        {
            float previousDifference = previousRayHeight - previousSurfaceHeight;
            float currentDifference = currentRayHeight - surfaceHeight;
            float t = previousDifference / (previousDifference - currentDifference);
            currentUV = lerp(previousUV, currentUV, t);
            intersected = true;
            break;
        }
    }

    HitUV = currentUV;

    // silhouette clipping for SPOM
    
    // check if the calculated intersection UV is outside the valid 0-1 quad boundary
    if (!intersected || currentUV.x < 0.0 || currentUV.x > 1.0 || currentUV.y < 0.0 || currentUV.y > 1.0)
    {
        SilhouetteMask = 0.0;
    }
    else
    {
        SilhouetteMask = 1.0;
    }

    // PDO injection
    rayDepth = 1.0 - currentRayHeight; // how far the ray travelled.

    // convert dpeth into world space distance
    float depthOffset = (rayDepth * HeightScale) / max(viewDir.z, 0.0001); // divide by z component for grazing angles travelling further

    // convert to linear depth and add to the screen space pixel depth
    // hook into Unity's frag output if compiled inside the pass.
    #if defined(UNITY_COMPILER_HLSL) && !defined(SHADERGRAPH_PREVIEW)
    // fetch SSDepth position from interpolator & inject the depth offset into frag
        #ifdef SV_Depth
            SV_Depth = depthOffset;
        #endif
    #endif

    // output the calculated depth offset
    OutDepthOffset = depthOffset;
}