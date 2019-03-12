﻿using Unity.Burst;
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

    [BurstCompile]
    private struct WaveMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public FreqSin freq;
        [ReadOnly] public MagnSin magn;
        [ReadOnly] public TimeSin time;
        [ReadOnly] public PI pi;
        [ReadOnly] public float3 scalej;
        public float3 vector;

        public void Execute(ref Position pos, [ReadOnly] ref Index i, ref Scale scale)
        {
            if (i.index + 1 < baseDataj.res * baseDataj.res)
            {
                vector.x = baseDataj.posBase + (baseDataj.steps * i.index / baseDataj.res);
                vector.z = baseDataj.posBase + (baseDataj.steps * i.index % baseDataj.res);
                vector.y = ((math.sin(pi.value * freq.x * (vector.x + time.x)) * magn.x)
                          + (math.sin(pi.value * freq.z * (vector.z + time.z)) * magn.z)) * 0.2f;
                vector.x += 10f;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----RippleMode Job----


    [BurstCompile]
    private struct RippleMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public FreqSin freq;
        [ReadOnly] public MagnSin magn;
        [ReadOnly] public TimeSin time;
        [ReadOnly] public PI pi;
        [ReadOnly] public float3 scalej;
        public float3 vector;
        private float s;

        public void Execute(ref Position pos, [ReadOnly] ref Index i, ref Scale scale)
        {
            if (i.index + 1 < baseDataj.res * baseDataj.res)
            {
                vector.x = baseDataj.posBase + (baseDataj.steps * i.index / baseDataj.res);
                vector.z = baseDataj.posBase + (baseDataj.steps * i.index % baseDataj.res);
                s = math.sqrt((vector.x * vector.x) + (vector.z * vector.z));
                vector.y = ((math.sin(pi.value * ((freq.x * s) - time.x)) / (1f + (magn.x * 10f * s)))
                          + (math.sin(pi.value * ((freq.z * s) - time.z)) / (1f + (magn.z * 10f * s)))) * 0.2f;
                vector.x += 10f;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----CylinderMode Job----


    [BurstCompile]
    private struct CylinderMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public FreqSin freq;
        [ReadOnly] public MagnSin magn;
        [ReadOnly] public TimeSin time;
        [ReadOnly] public PI pi;
        [ReadOnly] public float3 scalej;
        public float3 vector;
        private float u;
        private float v;
        private float r;

        public void Execute(ref Position pos, [ReadOnly] ref Index i, ref Scale scale)
        {
            if (i.index + 1 < baseDataj.res * baseDataj.res)
            {
                v = ((i.index % baseDataj.res + 0.5f) * baseDataj.steps) - 1f;
                u = ((i.index / baseDataj.res + 0.5f) * baseDataj.steps) - 1f;
                r = magn.x + (math.sin(pi.value * ((math.floor(freq.x * 3f) * u) + (v * freq.z) + time.x)) * 0.2f);
                vector.x = (math.sin(pi.value * u) * r) + 10f;
                vector.y = math.sin(pi.value * 0.5f * v) * 0.5f * magn.z;
                vector.z = math.cos(pi.value * u) * r;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----SphearMode Job----


    [BurstCompile]
    private struct SphearMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public FreqSin freq;
        [ReadOnly] public MagnSin magn;
        [ReadOnly] public TimeSin time;
        [ReadOnly] public PI pi;
        [ReadOnly] public float3 scalej;
        public float3 vector;
        private float u;
        private float v;
        private float r;
        private float s;

        public void Execute(ref Position pos, [ReadOnly] ref Index i, ref Scale scale)
        {
            if (i.index + 1 < baseDataj.res * baseDataj.res)
            {
                v = ((i.index % baseDataj.res + 0.5f) * baseDataj.steps) - 1f;
                u = ((i.index / baseDataj.res + 0.5f) * baseDataj.steps) - 1f;
                r = magn.x + (math.sin(pi.value * ((math.floor(freq.z * 3f) * v) + time.z)) * 0.1f)
                           + (math.sin(pi.value * ((math.floor(freq.x * 3f) * u) + time.x)) * 0.1f);
                s = math.cos(pi.value * 0.5f * v) * r;
                vector.x = (math.sin(pi.value * u) * s) + 10f;
                vector.y = math.sin(pi.value * 0.5f * v) * magn.z * r;
                vector.z = math.cos(pi.value * u) * s;
            }

            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----TorusMode Job----


    [BurstCompile]
    private struct TorusMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public BaseData baseDataj;
        [ReadOnly] public FreqSin freq;
        [ReadOnly] public MagnSin magn;
        [ReadOnly] public TimeSin time;
        [ReadOnly] public PI pi;
        [ReadOnly] public float3 scalej;
        public float3 vector;
        private float u;
        private float v;
        private float r;
        private float s;

        public void Execute(ref Position pos, [ReadOnly] ref Index i, ref Scale scale)
        {
            if (i.index + 1 < baseDataj.res * baseDataj.res)
            {
                v = ((i.index % baseDataj.res + 0.5f) * baseDataj.steps) - 1f;
                u = ((i.index / baseDataj.res + 0.5f) * baseDataj.steps) - 1f;
                r = 0.2f + (math.sin(pi.value * ((math.floor(freq.z * 3f) * v) + time.z)) * 0.05f);
                s = (r * math.cos(pi.value * v)) + (magn.x + (math.sin(pi.value * ((math.floor(freq.x * 3f) * u) + time.x)) * 0.1f));
                vector.x = (math.sin(pi.value * u) * s) + 10f;
                vector.y = math.sin(pi.value * v) * magn.z * r;
                vector.z = math.cos(pi.value * u) * s;
            }
            pos.Value = vector;
            scale.Value = scalej;
        }
    }


    //  ----Default Job----


    [BurstCompile]
    private struct DefaultMode : IJobProcessComponentData<Position, Index, Scale>
    {
        [ReadOnly] public float3 scalej;
        public float3 vector;
        public void Execute(ref Position pos, [ReadOnly] ref Index i, ref Scale scale)
        {
            pos.Value = vector;
            scale.Value = scalej; 
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
                    freq = GameObj.freq,
                    magn = GameObj.magn,
                    time = GameObj.time,
                    pi = GameObj.pi,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps,
                    vector = float3.zero
                };
                jobHandle = waveMode.Schedule(this, inputDeps);
                break;

            case 2:
                rippleMode = new RippleMode
                {
                    baseDataj = GameObj.baseData,
                    freq = GameObj.freq,
                    magn = GameObj.magn,
                    time = GameObj.time,
                    pi = GameObj.pi,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps,
                    vector = float3.zero
                };
                jobHandle = rippleMode.Schedule(this, inputDeps);
                break;

            case 3:
                cylinderMode = new CylinderMode
                {
                    baseDataj = GameObj.baseData,
                    freq = GameObj.freq,
                    magn = GameObj.magn,
                    time = GameObj.time,
                    pi = GameObj.pi,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps,
                    vector = float3.zero
                };
                jobHandle = cylinderMode.Schedule(this, inputDeps);
                break;

            case 4:
                sphearMode = new SphearMode
                {
                    baseDataj = GameObj.baseData,
                    freq = GameObj.freq,
                    magn = GameObj.magn,
                    time = GameObj.time,
                    pi = GameObj.pi,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps,
                    vector = float3.zero
                };
                jobHandle = sphearMode.Schedule(this, inputDeps);
                break;

            case 5:
                torusMode = new TorusMode
                {
                    baseDataj = GameObj.baseData,
                    freq = GameObj.freq,
                    magn = GameObj.magn,
                    time = GameObj.time,
                    pi = GameObj.pi,
                    scalej = new float3(1f, 1f, 1f) * GameObj.baseData.steps,
                    vector = float3.zero
                };
                jobHandle = torusMode.Schedule(this, inputDeps);
                break;

            default:
                defaultMode = new DefaultMode {
                    scalej = new float3(1f, 1f, 1f),
                    vector = float3.zero
                };
                jobHandle = defaultMode.Schedule(this, inputDeps);
                break;
        }
        return jobHandle;
    }
}
