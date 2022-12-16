using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class AsteroidDestruction : SystemBase
{
    private EndSimulationEntityCommandBufferSystem m_EndSimEcb;

    protected override void OnCreate()
    {
        m_EndSimEcb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_EndSimEcb.CreateCommandBuffer().AsParallelWriter();

        Entities
            .WithAll<DestroyTag, AsteroidTag>()
            .ForEach((Entity entity, int entityInQueryIndex) =>
            {
                commandBuffer.DestroyEntity(entityInQueryIndex, entity);
            }).ScheduleParallel();

        m_EndSimEcb.AddJobHandleForProducer(Dependency);
    }
}