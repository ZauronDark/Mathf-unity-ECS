public struct BlittableBool
{
    public readonly byte Value;

    public BlittableBool(byte value)
    {
        Value = value;
    }

    public BlittableBool(bool value)
    {
        Value = value ? (byte)1 : (byte)0;
    }

    public static implicit operator bool(BlittableBool bb)
    {
        return bb.Value != 0;
    }

    public static implicit operator BlittableBool(bool b)
    {
        return new BlittableBool(b);
    }
}

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
    public BlittableBool fpsClock;
    public int fpscount;
}