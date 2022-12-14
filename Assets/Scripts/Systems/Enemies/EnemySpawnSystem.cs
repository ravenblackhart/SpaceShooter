using System.Diagnostics;
using System.Numerics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor.Rendering;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public partial class EnemySpawnSystem : SystemBase
{
    private EntityQuery m_EnemyQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private EntityQuery m_GameSettingsQuery;
    private Entity m_EnemyPrefab;

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
        var asteroidPrefab = m_EnemyPrefab; 
        var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());
        


        Job
            .WithCode(() =>
            {
                for (int i = count; i < settings.EnemySpawnDensity; i++)
                {
                    var padding = 0.5f; 
                    var xPosition = rand.NextFloat(-1f*((settings.FieldWidth)/2 -padding), (settings.FieldWidth)/2 - padding);
                    var yPosition = (settings.FieldHeight / 2) - padding; 

                    var pos = new Translation { Value = new float3(xPosition, yPosition, 0f) };
                    var e = commandBuffer.Instantiate(asteroidPrefab);

                    commandBuffer.SetComponent(e, pos);

                }

            }).Schedule(); 
        
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
    
    
}
