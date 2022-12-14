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
                for (int i = count; i < settings.AsteroidDensity; i++)
                {
                    var padding = 1f; 
                    var xPosition = rand.NextFloat(-1f*((settings.FieldWidth)/2 -padding), (settings.FieldWidth)/2 - padding);
                    var yPosition = (settings.FieldHeight / 2) - padding; 

                    var pos = new Translation { Value = new float3(xPosition, yPosition, 0f) };
                    var e = commandBuffer.Instantiate(asteroidPrefab);

                    commandBuffer.SetComponent(e, pos);
                    
                    var randomDir = new Vector2(rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, -0.1f));
                    randomDir.Normalize();
                    randomDir = randomDir * settings.AsteroidSpeed;
                    var randomSpeed = rand.NextFloat(0.5f, settings.AsteroidSpeed);
                    var randomRotation = rand.NextFloat(-settings.AsteroidRotation , settings.AsteroidRotation); 
                    var transform = new TransformComponent { Direction = new float2(randomDir.x, randomDir.y), Speed = randomSpeed, RotationSpeed = randomRotation};
                    
                    commandBuffer.SetComponent(e, transform);
                    
                }

            }).Schedule(); 
        
        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}


