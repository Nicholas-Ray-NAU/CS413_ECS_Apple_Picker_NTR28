using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct AppleSpawner : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, properties) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<SpawnProperties>>())
        {
            if (properties.ValueRO.spawnTimer <= 0)
            {
                if (Menu_Button.isHard && properties.ValueRO.RandomDropChoice.NextFloat(1f) < properties.ValueRO.bombDropChance)
                {
                    var bomb = ecb.Instantiate(properties.ValueRO.bombPrefab);
                    ecb.SetComponent(bomb, LocalTransform.FromPosition(transform.ValueRO.Position));

                }
                else
                {
                    var apple = ecb.Instantiate(properties.ValueRO.applePrefab);
                    ecb.SetComponent(apple, LocalTransform.FromPosition(transform.ValueRO.Position));
                }
                
                if (Menu_Button.isMedium)
                {
                    properties.ValueRW.spawnTimer = properties.ValueRO.RandomTimerUpdate.NextFloat(0.4f, 3f);
                }
                else if (Menu_Button.isHard) 
                {
                    properties.ValueRW.spawnTimer = properties.ValueRO.RandomTimerUpdate.NextFloat(0.2f, 2f);
                }
                else
                {
                    properties.ValueRW.spawnTimer = 2f;
                }
            }
            else 
            {
                properties.ValueRW.spawnTimer = properties.ValueRO.spawnTimer - SystemAPI.Time.DeltaTime;
            }
        }
    }
}

