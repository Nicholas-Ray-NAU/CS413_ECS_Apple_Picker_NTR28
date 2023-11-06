using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct BasketSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, properties, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BasketProperties>>().WithEntityAccess())
        {
            if (properties.ValueRO.spawnReq)
            {
                for (var i = 0; i < properties.ValueRO.numbaskets; i++)
                {
                    var basket = ecb.Instantiate(properties.ValueRO.basketPrefab);
                    var pos = new float3
                    {
                        y = properties.ValueRO.basketBottomY + (properties.ValueRO.basketSpacingY * i)
                    };
                    
                    ecb.SetComponent(basket, LocalTransform.FromPosition(pos));
                }
                properties.ValueRW.spawnReq = false;
            }
        }
    }
}