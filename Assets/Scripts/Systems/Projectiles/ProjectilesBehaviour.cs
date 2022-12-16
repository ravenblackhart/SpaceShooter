using Unity.Entities;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

public partial class ProjectilesBehaviour : SystemBase
{
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;


    protected override void OnCreate()
    {
        base.OnCreate();
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        RequireSingletonForUpdate<GameSettingsComponent>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer().AsParallelWriter();
        var DeltaTime = Time.DeltaTime;
        var radius = 0.5f;

        Entities
            .WithAll<ProjectileTag>()
            .ForEach((Entity entity, int entityInQueryIndex, ref Translation translation,
                ref ProjectileAgeComponent age, in TransformComponent transformComponent) =>
            {
                translation.Value.xy += transformComponent.Speed * transformComponent.Direction.xy * DeltaTime;
                age.age += DeltaTime;
                if (age.age > age.maxAge || age.hasHit) commandBuffer.DestroyEntity(entityInQueryIndex, entity);

            }).ScheduleParallel();
        
        

        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }

    
}