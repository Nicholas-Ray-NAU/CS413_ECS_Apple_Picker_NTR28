using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.SceneManagement;

public partial struct BasketLiveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var collisionOccured = false;

        foreach (var (BasketTagTransform, BasketTagProperties, BasketTagEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BasketTag>>().WithEntityAccess())
        {
            var basketPos = BasketTagTransform.ValueRO.Position;

            foreach (var (BasketTransform, BasketProperties) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BasketProperties>>())
            {
                foreach (var (AppleTransform, AppleProperties, AppleEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AppleProperties>>().WithEntityAccess())
                {
                    var applePos = AppleTransform.ValueRO.Position;
                    if (applePos.y <= basketPos.y - (3 * BasketProperties.ValueRO.numbaskets))
                    {
                        ecb.DestroyEntity(AppleEntity);
                        ecb.DestroyEntity(BasketTagEntity);
                        BasketProperties.ValueRW.numbaskets--;

                        foreach (var (RemAppleTransform, RemAppleProperties, RemAppleEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AppleProperties>>().WithEntityAccess())
                        {
                            ecb.DestroyEntity(RemAppleEntity);
                        }

                        collisionOccured = true;

                    }
                }
                foreach (var (BombTransform, BombProperties, BombEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BombProperties>>().WithEntityAccess())
                {
                    var bombPos = BombTransform.ValueRO.Position;

                    if (bombPos.x >= basketPos.x - 2
                        && bombPos.x <= basketPos.x + 2
                        && bombPos.y <= basketPos.y + 1
                        && bombPos.y >= basketPos.y + 0.5)
                    {
                        ecb.DestroyEntity(BombEntity);
                        ecb.DestroyEntity(BasketTagEntity);
                        BasketProperties.ValueRW.numbaskets--;

                        foreach (var (RemBombTransform, RemBombProperties, RemBombEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BombProperties>>().WithEntityAccess())
                        {
                            ecb.DestroyEntity(RemBombEntity);
                        }

                        collisionOccured = true;
                    }
                }
                if (collisionOccured)
                {
                    foreach (var (SpawnnerTransform, SpawnnerProperties) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<SpawnProperties>>())
                    {
                        if (BasketProperties.ValueRO.numbaskets <= 0)
                        {
                            BasketProperties.ValueRW.spawnReq = true;
                            SceneManager.LoadScene("_Scene_Game_Over");
                        }
                        else
                        {
                            SpawnnerProperties.ValueRW.spawnTimer = 2f;
                        }
                    }
                }
            }
        }
    }
}