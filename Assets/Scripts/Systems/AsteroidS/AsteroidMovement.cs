using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class AsteroidMovement : SystemBase
{
    private GameSettingsComponent _settings;

    protected override void OnCreate()
    {
        base.OnCreate();

        _settings = GetSingleton<GameSettingsComponent>(); 
    }

    protected override void OnUpdate()
    {
        float DeltaTime = Time.DeltaTime;

        float bottomBorder = -(_settings.FieldHeight / 2); 
        
        Entities
            .WithAll<AsteroidTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in TransformComponent transformComponent) =>
            {
                translation.Value.y -= transformComponent.Speed * transformComponent.Direction.y * DeltaTime;


                if (translation.Value.y <= bottomBorder + 1f)
                {
                    //Remove Movement System & Destroy Object 
                }
                
            }).ScheduleParallel();
    }
}
