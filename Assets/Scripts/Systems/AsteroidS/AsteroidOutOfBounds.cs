using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
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
            .ForEach((Entity entity, int entityInQueryIndex, in Translation translation) =>
            {
                if (Mathf.Abs(translation.Value.x) > settings.FieldWidth / 2 + 3 ||
                    Mathf.Abs(translation.Value.y) > settings.FieldHeight / 2 + 3)
                {
                    commandBuffer.AddComponent(entityInQueryIndex, entity, new DestroyTag());
                }
            }).ScheduleParallel();

        m_EndFixedStepSimECB.AddJobHandleForProducer(Dependency);
    }
}