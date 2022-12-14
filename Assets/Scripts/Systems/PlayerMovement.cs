using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class PlayerMovement : SystemBase
{

    
    protected override void OnUpdate()
    {

        float DeltaTime = Time.DeltaTime; 
        
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in TransformComponent transformComponent) =>
            {

                translation.Value.xy += transformComponent.Speed * transformComponent.Direction * DeltaTime; 

                //on input left => translate left
            }).ScheduleParallel();
    }
}
