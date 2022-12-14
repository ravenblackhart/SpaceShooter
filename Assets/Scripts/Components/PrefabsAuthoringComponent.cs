using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


[GenerateAuthoringComponent]
public struct PrefabsAuthoringComponent : IComponentData
{
    public Entity AsteroidPrefab;
    public Entity EnemyPrefab;
    public Entity ProjectilePrefab;
    public Entity PlayerPrefab;
}

