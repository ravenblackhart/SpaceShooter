using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor.Rendering;

public partial class AsteroidSpawnSystem : SystemBase
{
    private EntityQuery m_AsteroidQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private EntityQuery m_GameSettingsQuery;
    private Entity m_AsteroidPrefab;

    protected override void OnCreate()
    {
        m_AsteroidQuery = GetEntityQuery(ComponentType.ReadWrite<AsteroidTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());
        
        RequireForUpdate(m_GameSettingsQuery);
    }
    
    protected override void OnUpdate()
    {
        if (m_AsteroidPrefab == Entity.Null)
        {
            m_AsteroidPrefab = GetSingleton<PrefabsAuthoringComponent>().AsteroidPrefab;
            return; 
        }

        var settings = GetSingleton<GameSettingsComponent>();
        var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
        var count = m_AsteroidQuery.CalculateChunkCountWithoutFiltering();
        var asteroidPrefab = m_AsteroidPrefab; 
        var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());


        Job
            .WithCode(() =>
            {
                for (int i = count; i < settings.AsteroidSpawnRate; i++)
                {
                    var padding = 1f; 
                    var xPosition = rand.NextFloat(-1f*((settings.FieldWidth)/2 -padding), (settings.FieldWidth)/2 - padding);
                    var yPosition = rand.NextFloat(-1f*((settings.FieldHeight)/2-padding), (settings.FieldHeight)/2-padding);

                    var pos = new Translation { Value = new float3(xPosition, yPosition, 0f) };
                    var e = commandBuffer.Instantiate(asteroidPrefab); 
                    
                    commandBuffer.SetComponent(e, pos);
                }
                
            }).Schedule(); 
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}


