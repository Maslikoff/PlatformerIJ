using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnGroup
{
    public GameObject prefab;
    public int maxCount = 5;
    public float spawnInterval = 3f;
    public SpawnPoint[] spawnPoints;
    [HideInInspector] public List<ISpawnable> spawnedObjects = new List<ISpawnable>();
}
