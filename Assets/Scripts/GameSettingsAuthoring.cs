using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

public class GameSettingsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{

    [Header("Field Boundaries")] 
    [SerializeField] private float FieldWidth;
    [SerializeField] private float FieldHeight;

    [Header("Player Properties")] 
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private int PlayerHealth;

    [Header("Asteroids Properties")] 
    [SerializeField] private int AsteroidDensity;
    [SerializeField] private float AsteroidSpawnTimer;
    [SerializeField] private int AsteroidSpeed;
    [SerializeField] private int AsteroidRotation;
    [SerializeField] private int AsteroidHealth;

    [Header("Enemy Properties")] private  int EnemySpawnDensity;
    [SerializeField] private int EnemySpeed;
    [SerializeField] private int EnemyHealth;

    [Header("Projectile Properties")] 
    [SerializeField] private int ProjectileSpeed;
    [SerializeField] private float ProjectilesPerSecond;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var gameSettings = default(GameSettingsComponent);

        gameSettings.FieldWidth = FieldWidth;


        dstManager.AddComponentData(entity, gameSettings); 

    }
}
