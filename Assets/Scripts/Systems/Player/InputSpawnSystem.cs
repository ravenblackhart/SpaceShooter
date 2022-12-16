using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class InputSpawnSystem : SystemBase
{
    private int health;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private EntityQuery m_GameSettingsQuery;
    private float m_NextTime;
    private float m_PerSecond;
    private Entity m_PlayerPrefab;
    private EntityQuery m_PlayerQuery;
    private Entity m_ProjectilePrefab;


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

        if (Input.GetKey("space")) shoot = 1;

        if (shoot == 1 && playercount < 1)
        {
            var player = EntityManager.Instantiate(m_PlayerPrefab);
            //GetComponent<TransformComponent>(player).Health = gameSettings.PlayerHealth; 

            return;
        }

        var commandBuffer = m_BeginSimECB.CreateCommandBuffer().AsParallelWriter();
        var projectilePrefab = m_ProjectilePrefab;

        var canShoot = false;
        if (UnityEngine.Time.time >= m_NextTime)
        {
            canShoot = true;
            m_NextTime += 1 / m_PerSecond;
        }


        Entities
            .WithAll<PlayerTag>()
            .ForEach((Entity entity, int entityInQueryIndex, ref TransformComponent transformComponent,
                in Translation translation, in ProjectileSpawnComponent spawner) =>
            {
                if (shoot != 1 || !canShoot) return;

                var projectileEntity = commandBuffer.Instantiate(entityInQueryIndex, projectilePrefab);
                var spawnPosition = new Translation { Value = translation.Value + spawner.SpawnPosition };
                commandBuffer.SetComponent(entityInQueryIndex, projectileEntity, spawnPosition);
            }).ScheduleParallel();

        m_BeginSimECB.AddJobHandleForProducer(Dependency);
    }
}