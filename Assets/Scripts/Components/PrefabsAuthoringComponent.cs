using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabsAuthoringComponent : IComponentData
{
    public Entity AsteroidPrefab;
    public Entity EnemyPrefab;
    public Entity ProjectilePrefab;
    public Entity PlayerPrefab;
}