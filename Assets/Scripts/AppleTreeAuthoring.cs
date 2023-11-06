using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class AppleTreeAuthoring : MonoBehaviour
{
    public float speed = 5f;
    public float leftAndRightEdge = 25f;
    public float changeDirectionChance = 0.01f;
    public float treeTeleportChance = 0.005f;

    private class AppleTreeBaker : Baker<AppleTreeAuthoring>
    {
        public override void Bake(AppleTreeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var propertiesComponent = new AppleTreeProperties
            {
                Speed = authoring.speed,
                LeftAndRightEdge = authoring.leftAndRightEdge,
                ChangeDirectionChance = authoring.changeDirectionChance,
                TreeTeleportChance = authoring.treeTeleportChance,
                RandomChange = Random.CreateFromIndex((uint)entity.Index),
                RandomPos = Random.CreateFromIndex((uint)System.DateTime.Now.Millisecond)
            };

            AddComponent(entity, propertiesComponent);
        }
    }
}

public struct AppleTreeProperties : IComponentData
{
    public float Speed;
    public float LeftAndRightEdge;
    public float ChangeDirectionChance;
    public float TreeTeleportChance;
    public Random RandomChange;
    public Random RandomPos;
}