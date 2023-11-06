using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct TeleportTreeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, properties) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AppleTreeProperties>>())
        {
            if (Menu_Button.isHard && properties.ValueRW.RandomChange.NextFloat() < properties.ValueRO.TreeTeleportChance)
            {
                var randTreePos = properties.ValueRW.RandomPos.NextFloat(23f);
                var pos = transform.ValueRO.Position;
                pos.x = randTreePos;
                transform.ValueRW.Position = pos;
            }
        }
    }
}