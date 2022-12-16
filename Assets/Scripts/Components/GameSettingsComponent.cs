using Unity.Entities;
using UnityEngine;
using UnityEditor;

[GenerateAuthoringComponent]
public struct GameSettingsComponent : IComponentData
{
    [Header("Field Boundaries")] public float FieldWidth;
    public float FieldHeight;

    [Header("Player Properties")] public float PlayerSpeed;
    public int PlayerHealth;

    [Header("Asteroids Properties")] public int AsteroidDensity;
    public float AsteroidSpawnTimer;
    public int AsteroidSpeed;
    public int AsteroidRotation;
    public int AsteroidHealth;

    [Header("Enemy Properties")] public int EnemySpawnDensity;
    public int EnemySpeed;
    public int EnemyHealth;

    [Header("Projectile Properties")] public int ProjectileSpeed;
    public float ProjectilesPerSecond;
}