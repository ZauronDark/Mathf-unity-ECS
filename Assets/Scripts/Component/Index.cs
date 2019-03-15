using System;
using Unity.Entities;

[Serializable]
public struct Index : IComponentData
{
    public int index;
}

public class IndexComponent : ComponentDataProxy<Index> { }
