using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class AsteroidMovement : SystemBase
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
        var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());
        
        
        Entities
            .WithAll<AsteroidTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in TransformComponent transformComponent) =>
            {
                translation.Value.xy += transformComponent.Speed * transformComponent.Direction.xy * DeltaTime;
                rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(transformComponent.RotationSpeed * DeltaTime)));
            }).ScheduleParallel();
    }
}
