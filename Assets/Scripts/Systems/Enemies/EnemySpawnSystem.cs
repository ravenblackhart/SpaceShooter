using System.Diagnostics;
using Unity.Burst;
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

    [BurstCompile]
    protected override void OnUpdate()
    {
        if (m_EnemyPrefab == Entity.Null)
        {
            m_EnemyPrefab = GetSingleton<PrefabsAuthoringComponent>().EnemyPrefab;
            return;
        }
        
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer().AsParallelWriter();
        var count = m_EnemyQuery.CalculateChunkCountWithoutFiltering();
        var enemyPrefab = m_EnemyPrefab;
        var rand = new Random((uint)Stopwatch.GetTimestamp());
        float DeltaTime = Time.DeltaTime;


        Entities
            .ForEach((int entityInQueryIndex, ref GameSettingsComponent settings) =>
            {
                settings.EnemySpawnTimer -= DeltaTime;
                if (settings.EnemySpawnTimer <= 0)
                {
                    if (count < settings.EnemyDensity)
                    {
                        for (var i = 0; i <= settings.EnemySpawnRate; i++)
                        {
                            var padding = 0.5f;
                            var xPosition = rand.NextFloat(-1f * (settings.FieldWidth / 2 - padding),
                                settings.FieldWidth / 2 - padding);
                            var yPosition = settings.FieldHeight / 2 - padding;

                            var pos = new Translation { Value = new float3(xPosition, yPosition, 0f) };
                            var e = commandBuffer.Instantiate(entityInQueryIndex, enemyPrefab);

                            commandBuffer.SetComponent(entityInQueryIndex, e, pos);

                            var transform = new TransformComponent { 
                                Speed = settings.EnemySpeed, 
                                Direction = new float2(1f, -0.25f), 
                                Health = settings.EnemyHealth
                            };
                            commandBuffer.SetComponent(entityInQueryIndex, e, transform);
                        }

                    }
                    
                    settings.EnemySpawnTimer = settings.ESpawnSetting; 
                }
                
            }).ScheduleParallel();
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}