using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct GameSettingsComponent : IComponentData
{
    [Header("Field Boundaries")] 
    public float FieldWidth;
    public float FieldHeight; 

    [Header("Player Properties")] 
    public float PlayerForce;
    public float PlayerSpeed;
    public float PlayerDirection;
    
    [Header("Asteroids Properties")] 
    public int AsteroidSpawnRate;
    
    




}
