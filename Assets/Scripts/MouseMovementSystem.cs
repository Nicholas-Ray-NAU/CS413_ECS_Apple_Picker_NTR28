using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct MouseMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var pos2D = Input.mousePosition;
        pos2D.z = -Camera.main.transform.position.z;
        var newPos = Camera.main.ScreenToWorldPoint(pos2D);

        foreach (var (transform, properties) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BasketTag>>())
        {
            var pos = transform.ValueRO.Position;
            pos.x = newPos.x;
            transform.ValueRW.Position = pos ;
        }
    }
}