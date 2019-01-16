using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Jobs;

public struct XLoop : IJobParallelFor
{
    public NativeArray<float3> posJ;
    [ReadOnly] public BaseData baseDataj;
    [ReadOnly] public SinXData sinXj;
    [ReadOnly] public SinZData sinZj;
    private float x;
    private float y;
    private float z;
    private float v;
    private float u;
    private float r;
    private float s;

    public void Execute(int i)
    {
        if (i % 100 < baseDataj.res || i / 100 < baseDataj.res)
        {
            switch (baseDataj.mode)
            {
                case 0: //wave
                    x = baseDataj.posBase + (baseDataj.steps * (i / baseDataj.res));
                    z = baseDataj.posBase + (baseDataj.steps * (i % baseDataj.res));
                    y = ((math.sin(sinXj.pi * sinXj.freqXSine * (x + sinXj.timeXSine))
                            * sinXj.magXSine)
                        + (math.sin(sinXj.pi * sinZj.freqZSine * (z + sinZj.timeZSine))
                            * sinZj.magZSine)
                        ) * 0.2f;
                    x += 10f;
                    break;

                case 1: //ripple
                    x = baseDataj.posBase + (baseDataj.steps * (i / baseDataj.res));
                    z = baseDataj.posBase + (baseDataj.steps * (i % baseDataj.res));
                    s = math.sqrt((x * x) + (z * z));
                    y = ((math.sin(sinXj.pi * ((sinXj.freqXSine * s) - sinXj.timeXSine))
                            / (1f + (sinXj.magXSine * 10f * s))
                            )
                        + (math.sin(sinXj.pi * ((sinZj.freqZSine * s) - sinZj.timeZSine))
                            / (1f + (sinZj.magZSine * 10f * s))
                            )
                        ) * 0.2f;
                    x += 10f;
                    break;

                case 2: //cylinder
                    v = (((i % baseDataj.res) + 0.5f) * baseDataj.steps) - 1f;
                    u = (((i / baseDataj.res) + 0.5f) * baseDataj.steps) - 1f;
                    r = sinXj.magXSine
                        + (math.sin(sinXj.pi * ((math.floor(sinXj.freqXSine * 3f) * u) + (v * sinZj.freqZSine) + sinXj.timeXSine)) * 0.2f);
                    x = (math.sin(sinXj.pi * u) * r)
                        + 10f;
                    y = math.sin(sinXj.pi * 0.5f * v)
                        * 0.5f
                        * sinZj.magZSine;
                    z = math.cos(sinXj.pi * u)
                        * r;
                    break;

                case 3: //sphear
                    v = (((i % baseDataj.res) + 0.5f) * baseDataj.steps) - 1f;
                    u = (((i / baseDataj.res) + 0.5f) * baseDataj.steps) - 1f;
                    r = sinXj.magXSine
                        + (math.sin(sinXj.pi * (((int)(sinZj.freqZSine * 3f) * v) + sinZj.timeZSine)) * 0.1f)
                        + (math.sin(sinXj.pi * (((int)(sinXj.freqXSine * 3f) * u) + sinXj.timeXSine)) * 0.1f);
                    s = math.cos(sinXj.pi * 0.5f * v)
                        * r;
                    x = (math.sin(sinXj.pi * u) * s)
                        + 10f;
                    y = math.sin(sinXj.pi * 0.5f * v)
                        * sinZj.magZSine
                        * r;
                    z = math.cos(sinXj.pi * u)
                        * s;
                    break;

                case 4: //torus
                    v = (((i % baseDataj.res) + 0.5f) * baseDataj.steps) - 1f;
                    u = (((i / baseDataj.res) + 0.5f) * baseDataj.steps) - 1f;
                    r = 0.2f
                        + (math.sin(sinXj.pi * (((int)(sinZj.freqZSine * 3f) * v) + sinZj.timeZSine)) * 0.05f);
                    s = (r * math.cos(sinXj.pi * v))
                        + (sinXj.magXSine
                            + (math.sin(sinXj.pi * (((int)(sinXj.freqXSine * 3f) * u) + sinXj.timeXSine)) * 0.1f)
                            );
                    x = (math.sin(sinXj.pi * u) * s)
                        + 10f;
                    y = math.sin(sinXj.pi * v)
                        * sinZj.magZSine
                        * r;
                    z = math.cos(sinXj.pi * u)
                        * s;
                    break;

                default:
                    x = 0f;
                    y = 0f;
                    z = 0f;
                    break;
            }
        }
        posJ[i] = new float3(x, y, z);
    }
}

public struct GOPos : IJobParallelForTransform
{
    [ReadOnly] public float3 scaleJ;
    [ReadOnly] public NativeArray<float3> pos;

    public void Execute(int index, TransformAccess transform)
    {
        transform.localScale = scaleJ;
        transform.position = pos[index];
    }
}
