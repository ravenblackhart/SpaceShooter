using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public partial class InputSpawnSystem : SystemBase
{
    private EntityQuery m_PlayerQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private Entity m_PlayerPrefab;
    private Entity m_ProjectilePrefab;
    private float m_NextTime = 0;
    private float m_PerSecond;
    private EntityQuery m_GameSettingsQuery; 
    

    protected override void OnCreate()
    {
        m_PlayerQuery = GetEntityQuery(ComponentType.ReadWrite<PlayerTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>()); 
    }
    
    protected override void OnUpdate()
    {
        var gameSettings = GetSingleton<GameSettingsComponent>();
        m_PerSecond = gameSettings.ProjectilesPerSecond;

        if (m_PlayerPrefab == Entity.Null || m_ProjectilePrefab == Entity.Null)
        {
            m_PlayerPrefab = GetSingleton<PrefabsAuthoringComponent>().PlayerPrefab;
            m_ProjectilePrefab = GetSingleton<PrefabsAuthoringComponent>().ProjectilePrefab; 
            return; 
        }

        byte shoot;
        shoot = 0;
        var playercount = m_PlayerQuery.CalculateEntityCountWithoutFiltering();

        if (Input.GetKey("space"))
        {
            shoot = 1; 
        }

        if (shoot == 1 && playercount < 1)
        {
            EntityManager.Instantiate(m_PlayerPrefab);
            return;
        }

        var commandBuffer = m_BeginSimECB.CreateCommandBuffer().AsParallelWriter();
        var projectilePrefab = m_ProjectilePrefab;

        bool canShoot = false;
        if (UnityEngine.Time.time >= m_NextTime)
        {
            canShoot = true; 
            m_NextTime += (1 / m_PerSecond); 
        }
        
        

        Entities
            .WithAll<PlayerTag>()
            .ForEach((Entity entity, int entityInQueryIndex,ref TransformComponent transformComponent,in Translation translation, in ProjectileSpawnComponent projectileSpawn) =>
            {
                if (shoot != 1 || !canShoot)
                {
                    return; 
                }

                var projectileEntity = commandBuffer.Instantiate(entityInQueryIndex, projectilePrefab);
                var spawnPosition = new Translation { Value = translation.Value + projectileSpawn.SpawnPosition }; 
                commandBuffer.SetComponent(entityInQueryIndex, projectileEntity,spawnPosition);
                
                
            }).ScheduleParallel();

        m_BeginSimECB.AddJobHandleForProducer(Dependency);

    }
}
