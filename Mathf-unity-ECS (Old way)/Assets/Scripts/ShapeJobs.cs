using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace MathfECS
{
    public class JobSystem : JobComponentSystem
    {
        //JobSystem is a Job class to calculate Position of each cube based on user input accross CPU cores


        //  ▀▄▀▄▀▄ WaveMode Job ▄▀▄▀▄▀


        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct WaveMode : IJobForEach<Translation, Index, Scale>
        {
            [ReadOnly] public BaseData baseData;
            [ReadOnly] public TransformData transformData;
            [ReadOnly] public FreqSin freq;
            [ReadOnly] public MagnSin magn;
            [ReadOnly] public TimeSin time;
            [ReadOnly] public float scalej;
            public float3 vector;

            public void Execute(ref Translation pos, [ReadOnly] ref Index i, ref Scale scale)
            {
                vector.x = transformData.posBase + (transformData.size * (i.index / baseData.res));
                vector.z = transformData.posBase + (transformData.size * (i.index % baseData.res));
                vector.y = ((math.sin(math.PI * freq.x * (vector.x + time.x)) * magn.x)
                          + (math.sin(math.PI * freq.z * (vector.z + time.z)) * magn.z)) * 0.2f;
                vector.x += 10f;

                pos.Value = vector;
                scale.Value = scalej;
            }
        }



        //  ▀▄▀▄▀▄ RippleMode Job ▄▀▄▀▄▀


        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct RippleMode : IJobForEach<Translation, Index, Scale>
        {
            [ReadOnly] public BaseData baseData;
            [ReadOnly] public TransformData transformData;
            [ReadOnly] public FreqSin freq;
            [ReadOnly] public MagnSin magn;
            [ReadOnly] public TimeSin time;
            [ReadOnly] public float scalej;
            public float3 vector;
            private float s;

            public void Execute(ref Translation pos, [ReadOnly] ref Index i, ref Scale scale)
            {
                vector.x = transformData.posBase + (transformData.size * (i.index / baseData.res));
                vector.z = transformData.posBase + (transformData.size * (i.index % baseData.res));
                s = math.sqrt((vector.x * vector.x) + (vector.z * vector.z));
                vector.y = ((math.sin(math.PI * ((freq.x * s) - time.x)) / (1f + (magn.x * 10f * s)))
                          + (math.sin(math.PI * ((freq.z * s) - time.z)) / (1f + (magn.z * 10f * s)))) * 0.2f;
                vector.x += 10f;

                pos.Value = vector;
                scale.Value = scalej;
            }
        }



        //  ▀▄▀▄▀▄ CylinderMode Job ▄▀▄▀▄▀


        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct CylinderMode : IJobForEach<Translation, Index, Scale>
        {
            [ReadOnly] public BaseData baseData;
            [ReadOnly] public TransformData transformData;
            [ReadOnly] public FreqSin freq;
            [ReadOnly] public MagnSin magn;
            [ReadOnly] public TimeSin time;
            [ReadOnly] public float scalej;
            public float3 vector;
            private float u;
            private float v;
            private float r;

            public void Execute(ref Translation pos, [ReadOnly] ref Index i, ref Scale scale)
            {
                v = (((i.index % baseData.res) + 0.5f) * transformData.size) - 1f;
                u = (((i.index / baseData.res) + 0.5f) * transformData.size) - 1f;
                r = magn.x + (math.sin(math.PI * ((math.floor(freq.x * 3f) * u) + (v * freq.z) + time.x)) * 0.2f);
                vector.x = (math.sin(math.PI * u) * r) + 10f;
                vector.y = math.sin(math.PI * 0.5f * v) * 0.5f * magn.z;
                vector.z = math.cos(math.PI * u) * r;

                pos.Value = vector;
                scale.Value = scalej;
            }
        }



        //  ▀▄▀▄▀▄ SphereMode Job ▄▀▄▀▄▀


        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct SphereMode : IJobForEach<Translation, Index, Scale>
        {
            [ReadOnly] public BaseData baseData;
            [ReadOnly] public TransformData transformData;
            [ReadOnly] public FreqSin freq;
            [ReadOnly] public MagnSin magn;
            [ReadOnly] public TimeSin time;
            [ReadOnly] public float scalej;
            public float3 vector;
            private float u;
            private float v;
            private float r;
            private float s;

            public void Execute(ref Translation pos, [ReadOnly] ref Index i, ref Scale scale)
            {
                v = (((i.index % baseData.res) + 0.5f) * transformData.size) - 1f;
                u = (((i.index / baseData.res) + 0.5f) * transformData.size) - 1f;
                r = magn.x + (math.sin(math.PI * ((math.floor(freq.z * 3f) * v) + time.z)) * 0.1f)
                           + (math.sin(math.PI * ((math.floor(freq.x * 3f) * u) + time.x)) * 0.1f);
                s = math.cos(math.PI * 0.5f * v) * r;
                vector.x = (math.sin(math.PI * u) * s) + 10f;
                vector.y = math.sin(math.PI * 0.5f * v) * magn.z * r;
                vector.z = math.cos(math.PI * u) * s;

                pos.Value = vector;
                scale.Value = scalej;
            }
        }



        //  ▀▄▀▄▀▄ TorusMode Job ▄▀▄▀▄▀


        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct TorusMode : IJobForEach<Translation, Index, Scale>
        {
            [ReadOnly] public BaseData baseData;
            [ReadOnly] public TransformData transformData;
            [ReadOnly] public FreqSin freq;
            [ReadOnly] public MagnSin magn;
            [ReadOnly] public TimeSin time;
            [ReadOnly] public float scalej;
            public float3 vector;
            private float u;
            private float v;
            private float r;
            private float s;

            public void Execute(ref Translation pos, [ReadOnly] ref Index i, ref Scale scale)
            {
                v = (((i.index % baseData.res) + 0.5f) * transformData.size) - 1f;
                u = (((i.index / baseData.res) + 0.5f) * transformData.size) - 1f;
                r = 0.2f + (math.sin(math.PI * ((math.floor(freq.z * 3f) * v) + time.z)) * 0.05f);
                s = (r * math.cos(math.PI * v)) + (magn.x + (math.sin(math.PI * ((math.floor(freq.x * 3f) * u) + time.x)) * 0.1f));
                vector.x = (math.sin(math.PI * u) * s) + 10f;
                vector.y = math.sin(math.PI * v) * magn.z * r;
                vector.z = math.cos(math.PI * u) * s;

                pos.Value = vector;
                scale.Value = scalej;
            }
        }



        //  ▀▄▀▄▀▄ Default Job ▄▀▄▀▄▀


        [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
        private struct DefaultMode : IJobForEach<Translation, Index, Scale>
        {
            [ReadOnly] public float scalej;
            public float3 vector;
            public void Execute(ref Translation pos, [ReadOnly] ref Index i, ref Scale scale)
            {
                pos.Value = vector;
                scale.Value = scalej;
            }
        }



        //  ▀▄▀▄▀▄ Update Call ▄▀▄▀▄▀


        private JobHandle jobHandle;
        private WaveMode waveMode;
        private RippleMode rippleMode;
        private CylinderMode cylinderMode;
        private SphereMode sphereMode;
        private TorusMode torusMode;
        private DefaultMode defaultMode;
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            switch (GameObj.baseData.mode)
            {
                case 1:
                    waveMode = new WaveMode
                    {
                        baseData = GameObj.baseData,
                        transformData = GameObj.transformData,
                        freq = GameObj.freq,
                        magn = GameObj.magn,
                        time = GameObj.time,
                        scalej = GameObj.transformData.size,
                        vector = float3.zero,
                    };
                    jobHandle = waveMode.Schedule(this, inputDeps);
                    break;

                case 2:
                    rippleMode = new RippleMode
                    {
                        baseData = GameObj.baseData,
                        transformData = GameObj.transformData,
                        freq = GameObj.freq,
                        magn = GameObj.magn,
                        time = GameObj.time,
                        scalej = GameObj.transformData.size,
                        vector = float3.zero
                    };
                    jobHandle = rippleMode.Schedule(this, inputDeps);
                    break;

                case 3:
                    cylinderMode = new CylinderMode
                    {
                        baseData = GameObj.baseData,
                        transformData = GameObj.transformData,
                        freq = GameObj.freq,
                        magn = GameObj.magn,
                        time = GameObj.time,
                        scalej = GameObj.transformData.size,
                        vector = float3.zero
                    };
                    jobHandle = cylinderMode.Schedule(this, inputDeps);
                    break;

                case 4:
                    sphereMode = new SphereMode
                    {
                        baseData = GameObj.baseData,
                        transformData = GameObj.transformData,
                        freq = GameObj.freq,
                        magn = GameObj.magn,
                        time = GameObj.time,
                        scalej = GameObj.transformData.size,
                        vector = float3.zero
                    };
                    jobHandle = sphereMode.Schedule(this, inputDeps);
                    break;

                case 5:
                    torusMode = new TorusMode
                    {
                        baseData = GameObj.baseData,
                        transformData = GameObj.transformData,
                        freq = GameObj.freq,
                        magn = GameObj.magn,
                        time = GameObj.time,
                        scalej = GameObj.transformData.size,
                        vector = float3.zero
                    };
                    jobHandle = torusMode.Schedule(this, inputDeps);
                    break;

                default:
                    defaultMode = new DefaultMode
                    {
                        scalej = 1f,
                        vector = float3.zero
                    };
                    jobHandle = defaultMode.Schedule(this, inputDeps);
                    break;
            }
            return jobHandle;
        }
    }
}