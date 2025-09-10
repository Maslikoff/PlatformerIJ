using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnGroup
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public int MaxCount { get; private set; } = 5;
    [field: SerializeField] public float SpawnInterval { get; private set; } = 3f;
    [field: SerializeField] public SpawnPoint[] SpawnPoints { get; private set; }

    [HideInInspector] public List<ISpawnable> SpawnedObjects = new List<ISpawnable>();
}
