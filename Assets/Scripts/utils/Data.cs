using Unity.Jobs;
using Unity.Mathematics;

public struct BaseData
{
    public byte mode;
    public byte res;
    public float posBase;
    public float steps;
}

public struct PI
{
    public float value;
}

public struct TimeSin
{
    public half x;
    public half z;
}

public struct FreqSin
{
    public half x;
    public half z;
}

public struct MagnSin
{
    public half x;
    public half z;
}

public struct FpsData
{
    public byte fpsAvg;
    public byte fpscount;
    public half fpsSum;
    public bool fpsClock;
}

public struct FPSJob : IJob
{
    public FpsData fpsData;
    public byte time;
    public float uTime;

    public void Execute()
    {
        fpsData.fpscount++;
        fpsData.fpsSum += math.half(1f / uTime);
        if (math.floor(time) % 2 == 0 && fpsData.fpsClock) //possible break on math.ceil()
        {
            fpsData.fpsClock = false;
            fpsData.fpsAvg = (byte)math.clamp(fpsData.fpsSum / fpsData.fpscount, 0, 99);
            fpsData.fpscount = 0;
            fpsData.fpsSum = math.half(0f);
        }
        else if (time % 2 != 0 && !fpsData.fpsClock)
        {
            fpsData.fpsClock = true;
        }
    } 
}