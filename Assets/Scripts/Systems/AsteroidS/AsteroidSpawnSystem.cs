using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = Unity.Mathematics.Random;

public partial class AsteroidSpawnSystem : SystemBase
{
    private Entity m_AsteroidPrefab;
    private EntityQuery m_AsteroidQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private EntityQuery m_GameSettingsQuery;
    private float spawnWaitTime; 

    protected override void OnCreate()
    {
        m_AsteroidQuery = GetEntityQuery(ComponentType.ReadWrite<AsteroidTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());
        RequireForUpdate(m_GameSettingsQuery);
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        //var settings = GetSingleton<GameSettingsComponent>();
        
        if (m_AsteroidPrefab == Entity.Null)
        {
            m_AsteroidPrefab = GetSingleton<PrefabsAuthoringComponent>().AsteroidPrefab;
            return;
        }

        var commandBuffer = m_BeginSimECB.CreateCommandBuffer().AsParallelWriter();
        var count = m_AsteroidQuery.CalculateChunkCountWithoutFiltering();
        var asteroidPrefab = m_AsteroidPrefab;
        var rand = new Random((uint)Stopwatch.GetTimestamp());
        float DeltaTime = Time.DeltaTime; 
        

        Entities
            .ForEach((
                int entityInQueryIndex, ref GameSettingsComponent settings) =>
            {
                settings.AsteroidSpawnTimer -= DeltaTime;
                if (settings.AsteroidSpawnTimer <= 0)
                {
                    for (var i = count; i < settings.AsteroidDensity; i++)
                    {
                    
                        var padding = 1f;
                        var xPosition = rand.NextFloat(-1f * (settings.FieldWidth / 2 - padding),
                            settings.FieldWidth / 2 - padding);
                        var yPosition = settings.FieldHeight / 2 + 3;

                        var pos = new Translation { Value = new float3(xPosition, yPosition, 0f) };
                        var e = commandBuffer.Instantiate(entityInQueryIndex, asteroidPrefab);

                        commandBuffer.SetComponent(entityInQueryIndex, e, pos);

                        var randomDir = new Vector2(rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, -0.1f));
                        randomDir.Normalize();
                        randomDir = randomDir * settings.AsteroidSpeed;
                        var randomSpeed = rand.NextFloat(0.5f, settings.AsteroidSpeed);
                        var randomRotation = rand.NextFloat(-settings.AsteroidRotation, settings.AsteroidRotation);
                        var transform = new TransformComponent
                        {
                            Direction = new float2(randomDir.x, randomDir.y), Speed = randomSpeed,
                            RotationSpeed = randomRotation, Health = settings.AsteroidHealth
                        };

                        commandBuffer.SetComponent(entityInQueryIndex, e, transform);
                    }
                    settings.AsteroidSpawnTimer = 5f; 
                }
                

                


            }).ScheduleParallel();

        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}