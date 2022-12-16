using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class EnemySpawnSystem : SystemBase
{
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private Entity m_EnemyPrefab;
    private EntityQuery m_EnemyQuery;
    private EntityQuery m_GameSettingsQuery;

    protected override void OnCreate()
    {
        m_EnemyQuery = GetEntityQuery(ComponentType.ReadWrite<EnemyTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());

        RequireForUpdate(m_GameSettingsQuery);
    }

    protected override void OnUpdate()
    {
        if (m_EnemyPrefab == Entity.Null)
        {
            m_EnemyPrefab = GetSingleton<PrefabsAuthoringComponent>().EnemyPrefab;
            return;
        }

        var settings = GetSingleton<GameSettingsComponent>();
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        var count = m_EnemyQuery.CalculateChunkCountWithoutFiltering();
        var enemyPrefab = m_EnemyPrefab;
        var rand = new Random((uint)Stopwatch.GetTimestamp());


        Job
            .WithCode(() =>
            {
                for (var i = count; i < settings.EnemySpawnDensity; i++)
                {
                    var padding = 0.5f;
                    var xPosition = rand.NextFloat(-1f * (settings.FieldWidth / 2 - padding),
                        settings.FieldWidth / 2 - padding);
                    var yPosition = settings.FieldHeight / 2 - padding;

                    var pos = new Translation { Value = new float3(xPosition, yPosition, 0f) };
                    var e = commandBuffer.Instantiate(enemyPrefab);

                    commandBuffer.SetComponent(e, pos);

                    var transform = new TransformComponent
                    {
                        Speed = settings.EnemySpeed, Direction = new float2(1f, -0.25f), Health = settings.EnemyHealth
                    };
                    commandBuffer.SetComponent(e, transform);
                }
            }).Schedule();

        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}