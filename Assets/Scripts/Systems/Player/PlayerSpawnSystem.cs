using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public partial class PlayerSpawnSystem : SystemBase
{
    private EntityQuery m_PlayerQuery;
    private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
    private Entity m_PlayerPrefab;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_PlayerQuery = GetEntityQuery(ComponentType.ReadWrite<PlayerTag>());
        m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>(); 
        RequireSingletonForUpdate<GameSettingsComponent>();
    }

    protected override void OnUpdate()
    {
        var gameSettings = GetSingleton<GameSettingsComponent>(); 
        if (m_PlayerPrefab == Entity.Null)
        {
            m_PlayerPrefab = GetSingleton<PrefabsAuthoringComponent>().PlayerPrefab;
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

        Entities
            .WithAll<PlayerTag>()
            .ForEach((Entity entity, ref TransformComponent transformComponent) =>
            {
                transformComponent.Speed = gameSettings.PlayerSpeed; 
            }).ScheduleParallel(); 

    }
}
