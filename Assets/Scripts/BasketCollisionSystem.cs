using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct CollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (BasketTransform, BasketProperties, BasketEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BasketTag>>().WithEntityAccess())
        {
            var basketPos = BasketTransform.ValueRO.Position;

            foreach (var (AppleTransform, AppleProperties, AppleEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AppleProperties>>().WithEntityAccess())
            {
                var applePos = AppleTransform.ValueRO.Position;
                if (applePos.x >= basketPos.x - 2
                    && applePos.x <= basketPos.x + 2 
                    && applePos.y <= basketPos.y + 1
                    && applePos.y >= basketPos.y + 0.5)
                {
                    ecb.DestroyEntity(AppleEntity);
                }
            }
        }
    }
}