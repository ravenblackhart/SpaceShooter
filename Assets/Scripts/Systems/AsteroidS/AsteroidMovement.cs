using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class AsteroidMovement : SystemBase
{
    protected override void OnUpdate()
    {
        var DeltaTime = Time.DeltaTime;

        Entities
            .WithAll<AsteroidTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in TransformComponent transformComponent) =>
            {
                translation.Value.xy += transformComponent.Speed * transformComponent.Direction.xy * DeltaTime;
                rotation.Value = math.mul(rotation.Value,
                    quaternion.RotateZ(math.radians(transformComponent.RotationSpeed * DeltaTime)));
            }).ScheduleParallel();
    }
}