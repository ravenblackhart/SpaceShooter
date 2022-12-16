using Unity.Entities;
using UnityEngine;
using UnityEditor;

[GenerateAuthoringComponent]
public struct GameSettingsComponent : IComponentData
{
    public float FieldWidth;
    public float FieldHeight;

    public float PlayerSpeed;
    public int PlayerHealth;
    
    public int AsteroidSpeed;
    public int AsteroidRotation;
    public int AsteroidHealth;
    public int AsteroidDensity;
    public int AsteroidSpawnRate; 
    public float AsteroidSpawnTimer;
    public float ASpawnSetting; 
    
    public int EnemySpeed;
    public int EnemyHealth;
    public int EnemySpawnRate; 
    public int EnemyDensity;
    public float EnemySpawnTimer; 
    public float ESpawnSetting;

    public int ProjectileSpeed;
    public float ProjectilesPerSecond;
}