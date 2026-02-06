float3 GerstnerWave(float3 position, float steepness, float wavelength, float speed, float direction,
                    inout float3 tangent, inout float3 binormal)
{
    direction = direction * 2 - 1;

    float2 d = normalize(float2(cos(3.14159265 * direction), sin(3.14159265 * direction)));

    float k = 2.0 * 3.14159265 / max(wavelength, 1e-5);   // avoid div-by-zero
    float f = k * (dot(d, position.xz) - speed * _Time.y);
    float a = steepness / k;

    float s = sin(f);
    float c = cos(f);

    tangent += float3(-d.x * d.x * (steepness * s),
                       d.x          * (steepness * c),
                      -d.x * d.y * (steepness * s));

    binormal += float3(-d.x * d.y * (steepness * s),
                        d.y          * (steepness * c),
                       -d.y * d.y * (steepness * s));

    return float3(d.x * (a * c),
                  a * s,
                  d.y * (a * c));
}

void GerstnerWaves_float(float3 position, float steepness, float wavelength, float speed, float4 directions,
                         out float3 Offset, out float3 normal)
{
    // local temps to guarantee full initialization
    float3 offsetTmp = float3(0,0,0);
    float3 normalTmp = float3(0,1,0);

    float3 tangent  = float3(1,0,0);
    float3 binormal = float3(0,0,1);

    float3 disp = float3(0,0,0);
    disp += GerstnerWave(position, steepness, wavelength, speed, directions.x, tangent, binormal);
    disp += GerstnerWave(position, steepness, wavelength, speed, directions.y, tangent, binormal);
    disp += GerstnerWave(position, steepness, wavelength, speed, directions.z, tangent, binormal);
    disp += GerstnerWave(position, steepness, wavelength, speed, directions.w, tangent, binormal);

    offsetTmp = disp;
    normalTmp = normalize(cross(binormal, tangent));

    Offset = offsetTmp;
    normal = normalTmp;
}
