using Unity.Entities;
using UnityEngine;

public class BombAuthoring : MonoBehaviour
{
    public GameObject bombPrefab;
    public float bottomY = -20f;
    public float dropSpeed = 1f;
    public float dropQueries = 0.005f;
    public class BombAuthoringBaker : Baker<BombAuthoring>
    {
        public override void Bake(BombAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var propertiesComponent = new BombProperties
            {
                bottomY = authoring.bottomY,
                dropSpeed = authoring.dropSpeed,
                dropQueries = authoring.dropQueries,
                bombPrefab = GetEntity(authoring.bombPrefab, TransformUsageFlags.Dynamic)
            };
            AddComponent(entity, propertiesComponent);
        }
    }
}

public struct BombProperties : IComponentData
{
    public float bottomY;
    public float dropSpeed;
    public float dropQueries;
    public Entity bombPrefab;
}