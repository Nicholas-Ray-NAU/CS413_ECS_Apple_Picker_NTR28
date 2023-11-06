using Unity.Entities;
using UnityEngine;

public class AppleAuthoring : MonoBehaviour
{
    public GameObject applePrefab;
    public float bottomY = -20f;
    public float dropSpeed = 1f;
    public float dropQueries = 0.005f;
    public class AppleAuthoringBaker : Baker<AppleAuthoring>
    {
        public override void Bake(AppleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var propertiesComponent = new AppleProperties
            {
                bottomY = authoring.bottomY,
                dropSpeed = authoring.dropSpeed,
                dropQueries = authoring.dropQueries,      
                applePrefab = GetEntity(authoring.applePrefab, TransformUsageFlags.Dynamic)
            };
            AddComponent(entity, propertiesComponent);
        }
    }
}

public struct AppleProperties : IComponentData
{
    public float bottomY;
    public float dropSpeed;
    public float dropQueries;
    public Entity applePrefab;
}