using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public struct Index : IComponentData
{
    public int index;
}

public class JobSystem : JobComponentSystem
{


    //  ----WaveMode Job----

        
    private struct WaveMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public SinXData sinXj;
        [ReadOnly] public SinZData sinZj;
        [ReadOnly] public float3 scalej;
        private float3 vector;
        private float2 iXZ;

        public void Execute(ref Position pos, ref Index i, ref Scale scale)
        {
            iXZ.x = i.index / baseDataj.res;
            iXZ.y = i.index % baseDataj.res;
            if ((iXZ.x * baseDataj.res) + iXZ.y < baseDataj.res * baseDataj.res)
            {
                vector.x = baseDataj.posBase + (baseDataj.steps * iXZ.x);
                vector.z = baseDataj.posBase + (baseDataj.steps * iXZ.y);
                vector.y = ((math.sin(sinXj.pi * sinXj.freqXSine * (vector.x + sinXj.timeXSine)) * sinXj.magXSine)
                          + (math.sin(sinXj.pi * sinZj.freqZSine * (vector.z + sinZj.timeZSine)) * sinZj.magZSine)) * 0.2f;
                vector.x += 10f;
            }
            else
            {
                vector = float3.zero;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----RippleMode Job----

        
    private struct RippleMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public SinXData sinXj;
        [ReadOnly] public SinZData sinZj;
        [ReadOnly] public float3 scalej;
        private float3 vector;
        private float s;
        private float2 iXZ;

        public void Execute(ref Position pos, ref Index i, ref Scale scale)
        {
            iXZ.x = i.index / baseDataj.res;
            iXZ.y = i.index % baseDataj.res;
            if ((iXZ.x * baseDataj.res) + iXZ.y < baseDataj.res * baseDataj.res)
            {
                vector.x = baseDataj.posBase + (baseDataj.steps * iXZ.x);
                vector.z = baseDataj.posBase + (baseDataj.steps * iXZ.y);
                s = math.sqrt((vector.x * vector.x) + (vector.z * vector.z));
                vector.y = ((math.sin(sinXj.pi * ((sinXj.freqXSine * s) - sinXj.timeXSine)) / (1f + (sinXj.magXSine * 10f * s)))
                          + (math.sin(sinXj.pi * ((sinZj.freqZSine * s) - sinZj.timeZSine)) / (1f + (sinZj.magZSine * 10f * s)))) * 0.2f;
                vector.x += 10f;
            }
            else
            {
                vector = float3.zero;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----CylinderMode Job----

        
    private struct CylinderMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public SinXData sinXj;
        [ReadOnly] public SinZData sinZj;
        [ReadOnly] public float3 scalej;
        private float3 vector;
        private float u;
        private float v;
        private float r;
        private float2 iXZ;

        public void Execute(ref Position pos, ref Index i, ref Scale scale)
        {
            iXZ.x = i.index / baseDataj.res;
            iXZ.y = i.index % baseDataj.res;
            if ((iXZ.x * baseDataj.res) + iXZ.y < baseDataj.res * baseDataj.res)
            {
                v = ((iXZ.y + 0.5f) * baseDataj.steps) - 1f;
                u = ((iXZ.x + 0.5f) * baseDataj.steps) - 1f;
                r = sinXj.magXSine + (math.sin(sinXj.pi * ((math.floor(sinXj.freqXSine * 3f) * u) + (v * sinZj.freqZSine) + sinXj.timeXSine)) * 0.2f);
                vector.x = (math.sin(sinXj.pi * u) * r) + 10f;
                vector.y = math.sin(sinXj.pi * 0.5f * v) * 0.5f * sinZj.magZSine;
                vector.z = math.cos(sinXj.pi * u) * r;
            }
            else
            {
                vector = float3.zero;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----SphearMode Job----

        
    private struct SphearMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public SinXData sinXj;
        [ReadOnly] public SinZData sinZj;
        [ReadOnly] public float3 scalej;
        private float3 vector;
        private float u;
        private float v;
        private float r;
        private float s;
        private float2 iXZ;

        public void Execute(ref Position pos, ref Index i, ref Scale scale)
        {
            iXZ.x = i.index / baseDataj.res;
            iXZ.y = i.index % baseDataj.res;
            if ((iXZ.x * baseDataj.res) + iXZ.y < baseDataj.res * baseDataj.res)
            {
                v = ((iXZ.y + 0.5f) * baseDataj.steps) - 1f;
                u = ((iXZ.x + 0.5f) * baseDataj.steps) - 1f;
                r = sinXj.magXSine
                    + (math.sin(sinXj.pi * (((int)(sinZj.freqZSine * 3f) * v) + sinZj.timeZSine)) * 0.1f)
                    + (math.sin(sinXj.pi * (((int)(sinXj.freqXSine * 3f) * u) + sinXj.timeXSine)) * 0.1f);
                s = math.cos(sinXj.pi * 0.5f * v) * r;
                vector.x = (math.sin(sinXj.pi * u) * s) + 10f;
                vector.y = math.sin(sinXj.pi * 0.5f * v) * sinZj.magZSine * r;
                vector.z = math.cos(sinXj.pi * u) * s;
            }
            else
            {
                vector = float3.zero;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----TorusMode Job----

        
    private struct TorusMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public SinXData sinXj;
        [ReadOnly] public SinZData sinZj;
        [ReadOnly] public float3 scalej;
        private float3 vector;
        private float u;
        private float v;
        private float r;
        private float s;
        private float2 iXZ;

        public void Execute(ref Position pos, ref Index i, ref Scale scale)
        {
            iXZ.x = i.index / baseDataj.res;
            iXZ.y = i.index % baseDataj.res;
            if ((iXZ.x * baseDataj.res) + iXZ.y < baseDataj.res * baseDataj.res)
            {
                v = ((iXZ.y + 0.5f) * baseDataj.steps) - 1f;
                u = ((iXZ.x + 0.5f) * baseDataj.steps) - 1f;
                r = 0.2f + (math.sin(sinXj.pi * (((int)(sinZj.freqZSine * 3f) * v) + sinZj.timeZSine)) * 0.05f);
                s = (r * math.cos(sinXj.pi * v)) + (sinXj.magXSine + (math.sin(sinXj.pi * (((int)(sinXj.freqXSine * 3f) * u) + sinXj.timeXSine)) * 0.1f));
                vector.x = (math.sin(sinXj.pi * u) * s) + 10f;
                vector.y = math.sin(sinXj.pi * v) * sinZj.magZSine * r;
                vector.z = math.cos(sinXj.pi * u) * s;
            }
            else
            {
                vector = float3.zero;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----Default Job----

        
    private struct DefaultMode : IJobProcessComponentData<Position, Index, Scale>
    {
        public void Execute(ref Position pos, ref Index i, ref Scale scale)
        {
            pos.Value = float3.zero;
            scale.Value = new float3(1f, 1f, 1f); 
        }
    }


    //  ----Update Call----


    private JobHandle jobHandle;
    private WaveMode waveMode;
    private RippleMode rippleMode;
    private CylinderMode cylinderMode;
    private SphearMode sphearMode;
    private TorusMode torusMode;
    private DefaultMode defaultMode;
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        switch (GameObj.baseData.mode)
        {
            case 1:
                waveMode = new WaveMode
                {
                    baseDataj = GameObj.baseData,
                    sinXj = GameObj.sinX,
                    sinZj = GameObj.sinZ,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps
                };
                jobHandle = waveMode.Schedule(this, inputDeps);
                break;

            case 2:
                rippleMode = new RippleMode
                {
                    baseDataj = GameObj.baseData,
                    sinXj = GameObj.sinX,
                    sinZj = GameObj.sinZ,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps
                };
                jobHandle = rippleMode.Schedule(this, inputDeps);
                break;

            case 3:
                cylinderMode = new CylinderMode
                {
                    baseDataj = GameObj.baseData,
                    sinXj = GameObj.sinX,
                    sinZj = GameObj.sinZ,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps
                };
                jobHandle = cylinderMode.Schedule(this, inputDeps);
                break;

            case 4:
                sphearMode = new SphearMode
                {
                    baseDataj = GameObj.baseData,
                    sinXj = GameObj.sinX,
                    sinZj = GameObj.sinZ,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps
                };
                jobHandle = sphearMode.Schedule(this, inputDeps);
                break;

            case 5:
                torusMode = new TorusMode
                {
                    baseDataj = GameObj.baseData,
                    sinXj = GameObj.sinX,
                    sinZj = GameObj.sinZ,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps
                };
                jobHandle = torusMode.Schedule(this, inputDeps);
                break;

            default:
                defaultMode = new DefaultMode { };
                jobHandle = defaultMode.Schedule(this, inputDeps);
                break;
        }
        return jobHandle;
    }
}
