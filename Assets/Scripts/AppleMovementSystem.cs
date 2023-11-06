using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct AppleMovementSystem : ISystem
{    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (transform, properties, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AppleProperties>>().WithEntityAccess())
        {
            var pos = transform.ValueRO.Position;
            var posDelta = properties.ValueRO.dropSpeed;

            pos.y -= posDelta * SystemAPI.Time.DeltaTime * properties.ValueRO.dropQueries;
            transform.ValueRW.Position = pos;
            properties.ValueRW.dropQueries += 0.05f;

            if (pos.y < properties.ValueRO.bottomY)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}
