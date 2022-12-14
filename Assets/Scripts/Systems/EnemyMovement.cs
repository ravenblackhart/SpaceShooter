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

    private GameSettingsComponent _settings; 
    protected override void OnStartRunning()
    {
        base.OnStartRunning();


       _settings = GetSingleton<GameSettingsComponent>();
    }

    protected override void OnUpdate()
    {
        float DeltaTime = Time.DeltaTime;

        float leftBorder = -(_settings.FieldWidth / 2); 
        float rightBorder = _settings.FieldWidth / 2;


        Entities
            .WithAll<EnemyTag>()
            .ForEach((ref Translation translation, ref TransformComponent transformComponent) =>
            {
                translation.Value.y += transformComponent.Speed * transformComponent.Direction.y * DeltaTime;

                if (transformComponent.SwitchDirection)
                {
                    translation.Value.x -= transformComponent.Speed * transformComponent.Direction.x * DeltaTime;
                }
                else if (!transformComponent.SwitchDirection)
                {
                    translation.Value.x += transformComponent.Speed * transformComponent.Direction.x * DeltaTime;
                }
                    
                if (translation.Value.x >= rightBorder && !transformComponent.SwitchDirection) transformComponent.SwitchDirection = true; 
                else if (translation.Value.x <= leftBorder && transformComponent.SwitchDirection) transformComponent.SwitchDirection = false; 
                

            }).ScheduleParallel();
    }
}


