using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ProjectileSpawnComponent : IComponentData
{
    public float3 SpawnPosition;
}