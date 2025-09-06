using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public bool isOccupied;
    }

    [System.Serializable]
    public class SpawnGroup
    {
        public GameObject prefab;
        public int maxCount = 5;
        public float spawnInterval = 3f;
        public SpawnPoint[] spawnPoints;
        [HideInInspector] public List<GameObject> spawnedObjects = new List<GameObject>();
        [HideInInspector] public float spawnTimer;
    }

    [Header("Spawn Settings")]
    [SerializeField] private SpawnGroup[] _spawnGroups;

    private void Update()
    {
        foreach (var group in _spawnGroups)
        {
            if (group.spawnedObjects.Count < group.maxCount)
            {
                group.spawnTimer += Time.deltaTime;

                if (group.spawnTimer >= group.spawnInterval)
                {
                    TrySpawnObject(group);
                    group.spawnTimer = 0f;
                }
            }
        }
    }

    private void TrySpawnObject(SpawnGroup group)
    {
        SpawnPoint freePoint = null;
        foreach (var point in group.spawnPoints)
        {
            if (!point.isOccupied)
            {
                freePoint = point;
                break;
            }
        }

        if (freePoint == null) return;

        GameObject spawnedObj = Instantiate(group.prefab, freePoint.point.position, freePoint.point.rotation);
        group.spawnedObjects.Add(spawnedObj);
        freePoint.isOccupied = true;

        var spawnable = spawnedObj.GetComponent<ISpawnable>();

        if (spawnable != null)
        {
            spawnable.OnDespawn += () =>
            {
                group.spawnedObjects.Remove(spawnedObj);
                freePoint.isOccupied = false;
            };
        }
        else
        {
            Debug.LogWarning($"Spawned object {spawnedObj.name} doesn't implement ISpawnable interface");
        }
    }

    public void SpawnObjectManually(int groupIndex, int pointIndex)
    {
        if (groupIndex >= 0 && groupIndex < _spawnGroups.Length)
        {
            var group = _spawnGroups[groupIndex];
            if (pointIndex >= 0 && pointIndex < group.spawnPoints.Length)
            {
                var point = group.spawnPoints[pointIndex];
                if (!point.isOccupied)
                {
                    GameObject spawnedObj = Instantiate(group.prefab, point.point.position, point.point.rotation);
                    group.spawnedObjects.Add(spawnedObj);
                    point.isOccupied = true;
                }
            }
        }
    }
}
