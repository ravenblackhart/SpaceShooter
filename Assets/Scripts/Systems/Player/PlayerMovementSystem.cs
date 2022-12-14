using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Input = UnityEngine.Input;

public partial class PlayerMovementSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<GameSettingsComponent>();
    }

    protected override void OnUpdate()
    {
        var gameSettings = GetSingleton<GameSettingsComponent>(); 
        float DeltaTime = Time.DeltaTime;
        
        byte right, left, thrust, reverseThrust;
        right = left = thrust = reverseThrust = 0;

        //Movement Control via Keyboard; 
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) right = 1; 
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) left = 1; 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) thrust = 1; 
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) reverseThrust = 1;

        Entities
            .WithAll<PlayerTag>()
            .ForEach((Entity entity, ref Translation translation, ref Rotation rotation, in TransformComponent transformComponent) =>
            {
                var translate = transformComponent.Direction;

                
                
             if (right == 1 && translation.Value.x + 1 <= gameSettings.FieldWidth / 2)
             {
                 translate.x = 1; 
                 translation.Value.x +=  transformComponent.Speed * translate.x * DeltaTime;
             }
             if (left == 1 && translation.Value.x - 1 >= -gameSettings.FieldWidth / 2)
             {
                 translate.x = -1; 
                 translation.Value.x +=  transformComponent.Speed * translate.x  * DeltaTime;
             }
             if (thrust == 1 && translation.Value.y + 1 <= gameSettings.FieldHeight / 2)
             {   
                 translate.y = 1; 
                 translation.Value.y +=  transformComponent.Speed * translate.y  * DeltaTime;
             }
             if (reverseThrust == 1 && translation.Value.y - 1 >= -gameSettings.FieldHeight / 2)
             {
                 translate.y = -1; 
                 translation.Value.y += transformComponent.Speed * translate.y  * DeltaTime;
             }
            
                
                

              
            }).ScheduleParallel();
    }
}
