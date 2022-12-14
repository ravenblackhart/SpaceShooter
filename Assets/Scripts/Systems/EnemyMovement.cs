using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine; 

public partial class EnemyMovement : SystemBase
{

    private FieldBounds _fieldBounds; 
    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        
        //_fieldBounds = Get
    }

    protected override void OnUpdate()
    {
        float DeltaTIme = Time.DeltaTime;
        int leftBorder = -4;
        int rightBorder = 4;

        Entities
            .WithAll<EnemyTag>()
            .ForEach((ref Translation translation, ref TransformComponent transformComponent) =>
            {
                translation.Value.y += transformComponent.Speed * transformComponent.Direction.y * DeltaTIme;

                if (transformComponent.SwitchDirection)
                {
                    translation.Value.x -= transformComponent.Speed * transformComponent.Direction.x * DeltaTIme;
                }
                else if (!transformComponent.SwitchDirection)
                {
                    translation.Value.x += transformComponent.Speed * transformComponent.Direction.x * DeltaTIme;
                }
                    
                if (translation.Value.x >= rightBorder && !transformComponent.SwitchDirection) transformComponent.SwitchDirection = true; 
                else if (translation.Value.x <= leftBorder && transformComponent.SwitchDirection) transformComponent.SwitchDirection = false; 
                

            }).ScheduleParallel();
    }
}


