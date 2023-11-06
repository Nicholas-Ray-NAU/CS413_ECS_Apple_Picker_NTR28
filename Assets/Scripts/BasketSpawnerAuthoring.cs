using Unity.Entities;
using UnityEngine;

public class BasketSpawnerAuthoring : MonoBehaviour
{
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public GameObject basketPrefab;
    public bool spawnReq = true;
    public class BasketAuthoringBaker : Baker<BasketSpawnerAuthoring>
    {
        public override void Bake(BasketSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var propertiesComponent = new BasketProperties
            {
                numbaskets = authoring.numBaskets,
                basketBottomY = authoring.basketBottomY,
                basketSpacingY = authoring.basketSpacingY,
                spawnReq = authoring.spawnReq,
                basketPrefab = GetEntity(authoring.basketPrefab, TransformUsageFlags.Dynamic)
            };

            AddComponent(entity, propertiesComponent);

        }
    }
}

public struct BasketProperties : IComponentData
{
    public int numbaskets;
    public float basketBottomY;
    public float basketSpacingY;
    public bool spawnReq;
    public Entity basketPrefab;
}