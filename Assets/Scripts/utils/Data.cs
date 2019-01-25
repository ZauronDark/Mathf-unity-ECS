public struct BaseData
{
    public int mode;
    public int res;
    public float posBase;
    public float steps;
}

public struct SinXData
{
    public float pi;
    public float timeXSine;
    public float freqXSine;
    public float magXSine;
}

public struct SinZData
{
    public float timeZSine;
    public float freqZSine;
    public float magZSine;
}

public struct FpsData
{
    public int fpsAvg;
    public float fpsSum;
    public bool fpsClock;
    public int fpscount;
}