using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public class SetProjectileSpawnPoint : UnityEngine.MonoBehaviour, IConvertGameObjectToEntity
{
    public GameObject ProjectileSpawner;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var projectileOffset = default(ProjectileSpawnComponent);

        var offsetVector = ProjectileSpawner.transform.position;
        projectileOffset.SpawnPosition = new float3(offsetVector.x, offsetVector.y,offsetVector.z);
        
        dstManager.AddComponentData(entity, projectileOffset);
       
    }
}
