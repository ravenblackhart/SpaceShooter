using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GameSettingsComponent : IComponentData
{
    [Header("Field Boundaries")] 
    public float FieldWidth;
    public float FieldHeight; 

    [Header("Player Properties")]
    public float PlayerSpeed;

    [Header("Asteroids Properties")] 
    public int AsteroidDensity;
    public float AsteroidSpawnTimer; 
    public int AsteroidSpeed;
    public int AsteroidRotation;

    [Header("Enemy Properties")] 
    public int EnemySpawnDensity; 
    public int EnemySpeed;

    [Header("Projectile Properties")] 
    public int ProjectileSpeed;
    public float ProjectilesPerSecond;


}
