using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public struct JobData
{
    public int modeJ;
    public int resJ;
    public float posBaseJ;
    public float stepsJ;
    public float piRefJ;
    public float timeXSineJ;
    public float timeZSineJ;
    public float freqXSineJ;
    public float freqZSineJ;
    public float magXSineJ;
    public float magZSineJ;
}



public struct XLoop : IJobParallelFor
{
    public NativeArray<Vector3> posJ;
    [ReadOnly] public JobData jobDataJ;
    float x;
    float y;
    float z;
    float v;
    float u;
    float r;
    float s;

    public void Execute(int i)
    {
        
        if (i % 100 < jobDataJ.resJ || i / 100 < jobDataJ.resJ)
        {
            
            switch (jobDataJ.modeJ)
            {
                case 0:
                    x = jobDataJ.posBaseJ + (jobDataJ.stepsJ * (i / jobDataJ.resJ));
                    z = jobDataJ.posBaseJ + (jobDataJ.stepsJ * (i % jobDataJ.resJ));
                    y = ((Mathf.Sin(jobDataJ.freqXSineJ * jobDataJ.piRefJ * (x + jobDataJ.timeXSineJ)) * jobDataJ.magXSineJ)
                        + (Mathf.Sin(jobDataJ.freqZSineJ * jobDataJ.piRefJ * (z + jobDataJ.timeZSineJ)) * jobDataJ.magZSineJ))
                        * 0.2f;
                    x += 10f;
                    break;

                case 1:
                    x = jobDataJ.posBaseJ + (jobDataJ.stepsJ * (i / jobDataJ.resJ));
                    z = jobDataJ.posBaseJ + (jobDataJ.stepsJ * (i % jobDataJ.resJ));
                    s = Mathf.Sqrt((x * x) + (z * z));
                    y = ((Mathf.Sin(jobDataJ.piRefJ * ((jobDataJ.freqXSineJ * s) - jobDataJ.timeXSineJ)) / (1f + (jobDataJ.magXSineJ * 10f * s)))
                        + (Mathf.Sin(jobDataJ.piRefJ * ((jobDataJ.freqZSineJ * s) - jobDataJ.timeZSineJ)) / (1f + (jobDataJ.magZSineJ * 10f * s))))
                        * 0.2f;
                    x += 10f;
                    break;

                case 2:
                    v = (((i % jobDataJ.resJ) + 0.5f) * jobDataJ.stepsJ) - 1f;
                    u = (((i / jobDataJ.resJ) + 0.5f) * jobDataJ.stepsJ) - 1f;
                    r = jobDataJ.magXSineJ + (Mathf.Sin(jobDataJ.piRefJ * (((int)(jobDataJ.freqXSineJ * 3f) * u) + (v * jobDataJ.freqZSineJ) + jobDataJ.timeXSineJ)) * 0.2f);
                    x = (Mathf.Sin(jobDataJ.piRefJ * u) * r) + 10f;
                    y = Mathf.Sin(jobDataJ.piRefJ * 0.5f * v) * 0.5f * jobDataJ.magZSineJ;
                    z = Mathf.Cos(jobDataJ.piRefJ * u) * r;
                    break;

                case 3:
                    v = (((i % jobDataJ.resJ) + 0.5f) * jobDataJ.stepsJ) - 1f;
                    u = (((i / jobDataJ.resJ) + 0.5f) * jobDataJ.stepsJ) - 1f;
                    r = jobDataJ.magXSineJ
                        + (Mathf.Sin(jobDataJ.piRefJ * (((int)(jobDataJ.freqZSineJ * 3f) * v) + jobDataJ.timeZSineJ)) * 0.1f)
                        + (Mathf.Sin(jobDataJ.piRefJ * (((int)(jobDataJ.freqXSineJ * 3f) * u) + jobDataJ.timeXSineJ)) * 0.1f);
                    s = r * Mathf.Cos(jobDataJ.piRefJ * 0.5f * v);
                    x = (Mathf.Sin(jobDataJ.piRefJ * u) * s) + 10f;
                    y = Mathf.Sin(jobDataJ.piRefJ * 0.5f * v) * r * jobDataJ.magZSineJ;
                    z = Mathf.Cos(jobDataJ.piRefJ * u) * s;
                    break;

                case 4:
                    v = (((i % jobDataJ.resJ) + 0.5f) * jobDataJ.stepsJ) - 1f;
                    u = (((i / jobDataJ.resJ) + 0.5f) * jobDataJ.stepsJ) - 1f;
                    r = 0.2f + (Mathf.Sin(jobDataJ.piRefJ * (((int)(jobDataJ.freqZSineJ * 3f) * v) + jobDataJ.timeZSineJ)) * 0.05f);
                    s = (r * Mathf.Cos(jobDataJ.piRefJ * v))
                        + (jobDataJ.magXSineJ + (Mathf.Sin(jobDataJ.piRefJ * (((int)(jobDataJ.freqXSineJ * 3f) * u) + jobDataJ.timeXSineJ)) * 0.1f));
                    x = (Mathf.Sin(jobDataJ.piRefJ * u) * s) + 10f;
                    y = Mathf.Sin(jobDataJ.piRefJ * v) * r * jobDataJ.magZSineJ;
                    z = Mathf.Cos(jobDataJ.piRefJ * u) * s;
                    break;

                default:
                    x = 0f;
                    y = 0f;
                    z = 0f;
                    break;
            }
        }
        posJ[i] = new Vector3(x, y, z);
    }
}

public struct GOPos : IJobParallelForTransform
{
    [ReadOnly] public Vector3 scaleJ;
    [ReadOnly] public NativeArray<Vector3> pos;

    public void Execute(int index, TransformAccess transform)
    {
        transform.localScale = scaleJ;
        transform.position = pos[index];
    }
}