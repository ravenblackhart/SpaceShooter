using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ProjectileAgeComponent : IComponentData
{
    public ProjectileAgeComponent(float maxAge)
    {
        this.maxAge = maxAge;
        age = 0;
        hasHit = false; 
    }

    public float age;
    public float maxAge;
    public bool hasHit;

}
