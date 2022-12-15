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

    private EntityQuery m_GameSettingsQuery;
    protected override void OnCreate()
    {
        base.OnCreate();


        m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());
        
        RequireForUpdate(m_GameSettingsQuery);
    }

    protected override void OnUpdate()
    {
        var _settings = GetSingleton<GameSettingsComponent>(); 
        float DeltaTime = Time.DeltaTime;

        float padding = 0.5f; 
        float leftBorder = -(_settings.FieldWidth / 2 - padding); 
        float rightBorder = _settings.FieldWidth / 2 - padding;
        float topBorder = _settings.FieldHeight / 2 - padding;
        float bottomBorder = -(_settings.FieldHeight / 2 - padding);


        Entities
            .WithAll<EnemyTag>()
            .ForEach((ref Translation translation, ref TransformComponent transformComponent) =>
            {

                if (!transformComponent.SwitchDirectionX && !transformComponent.SwitchDirectionY)
                {
                    translation.Value.xy += transformComponent.Speed * transformComponent.Direction.xy * DeltaTime;
                }
                else if (transformComponent.SwitchDirectionX && !transformComponent.SwitchDirectionY)
                {
                    translation.Value.x -= transformComponent.Speed * transformComponent.Direction.x * DeltaTime;
                    translation.Value.y += transformComponent.Speed * transformComponent.Direction.y * DeltaTime;
                }
                
                else if (!transformComponent.SwitchDirectionX && transformComponent.SwitchDirectionY)
                {
                    translation.Value.x += transformComponent.Speed * transformComponent.Direction.x * DeltaTime;
                    translation.Value.y -= transformComponent.Speed * transformComponent.Direction.y * DeltaTime;
                }

                else
                {
                    translation.Value.xy -= transformComponent.Speed * transformComponent.Direction.xy * DeltaTime;
                }
                    
                if (translation.Value.x >= rightBorder && !transformComponent.SwitchDirectionX) transformComponent.SwitchDirectionX = true; 
                else if (translation.Value.x <= leftBorder && transformComponent.SwitchDirectionX) transformComponent.SwitchDirectionX = false; 
                
                
                if (translation.Value.y >= topBorder && transformComponent.SwitchDirectionY) transformComponent.SwitchDirectionY = false; 
                else if (translation.Value.y <= bottomBorder && !transformComponent.SwitchDirectionY) transformComponent.SwitchDirectionY = true; 
                

            }).ScheduleParallel();
    }
}


