using Unity.Entities;


public struct SpawnCube : IComponentData
{
    public int CountX;
    public int CountY;
    public Entity Prefab;
}
