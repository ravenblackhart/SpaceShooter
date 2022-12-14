using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class AsteroidOutOfBounds : SystemBase
{
    private EndFixedStepSimulationEntityCommandBufferSystem m_EndFixedStepSimECB;

    protected override void OnCreate()
    {
        base.OnCreate();

        m_EndFixedStepSimECB = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>(); 
        RequireSingletonForUpdate<GameSettingsComponent>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_EndFixedStepSimECB.CreateCommandBuffer().AsParallelWriter();
        var settings = GetSingleton<GameSettingsComponent>(); 
        
        Entities
            .WithAll<AsteroidTag>()
            .ForEach((Entity entity, int entityInQueryIndex, in Translation translation) => {
                if (Mathf.Abs(translation.Value.x) > settings.FieldWidth / 2 ||
                    Mathf.Abs(translation.Value.y) > settings.FieldHeight / 2)
                {
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new DestroyTag());
                    return;
                }
            }).ScheduleParallel();
        
        m_EndFixedStepSimECB.AddJobHandleForProducer(Dependency);
    }
}
