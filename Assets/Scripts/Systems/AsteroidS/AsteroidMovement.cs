using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class AsteroidMovement : SystemBase
{
    protected override void OnUpdate()
    {
        
        float DeltaTime = Time.DeltaTime;
      
        Entities
            .WithAll<AsteroidTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in TransformComponent transformComponent) =>
            {
                translation.Value.xy += transformComponent.Speed * transformComponent.Direction.xy * DeltaTime;
                rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(transformComponent.RotationSpeed * DeltaTime)));
            }).ScheduleParallel();
    }
}
