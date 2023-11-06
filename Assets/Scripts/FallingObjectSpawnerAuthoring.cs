using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class FallingObjectSpawnerAuthoring : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bombPrefab;
    public float spawnTimer = 2f;
    public float bombDropChance = 0.25f;
    public class FallingObjectSpawnerAuthoringBaker : Baker<FallingObjectSpawnerAuthoring> 
    {
        public float randSeed = System.DateTime.Now.Millisecond;
        public override void Bake(FallingObjectSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var propertiesComponent = new SpawnProperties
            {
                spawnTimer = authoring.spawnTimer,
                bombDropChance = authoring.bombDropChance,
                RandomDropChoice = Random.CreateFromIndex((uint)randSeed),
                RandomTimerUpdate = Random.CreateFromIndex((uint)randSeed),
                applePrefab = GetEntity(authoring.applePrefab, TransformUsageFlags.Dynamic),
                bombPrefab = GetEntity(authoring.bombPrefab, TransformUsageFlags.Dynamic)
            };
            AddComponent(entity, propertiesComponent);
        }
    }
}

public struct SpawnProperties : IComponentData
{
    public float spawnTimer;
    public float bombDropChance;
    public Random RandomDropChoice;
    public Random RandomTimerUpdate;
    public Entity applePrefab;
    public Entity bombPrefab;
}