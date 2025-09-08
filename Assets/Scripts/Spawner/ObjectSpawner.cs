using System;
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
    }

    [Header("Spawn Settings")]
    [SerializeField] private SpawnGroup[] _spawnGroups;

    private Dictionary<GameObject, SpawnPoint> _spawnPointMap = new Dictionary<GameObject, SpawnPoint>();
    private Dictionary<GameObject, System.Action> _despawnHandlers = new Dictionary<GameObject, System.Action>();

    private void Start()
    {
        foreach (var group in _spawnGroups)
        {
            StartCoroutine(SpawnRoutine(group));
        }
    }

    private IEnumerator SpawnRoutine(SpawnGroup group)
    {
        while (true)
        {
            yield return new WaitForSeconds(group.spawnInterval);

            if (group.spawnedObjects.Count < group.maxCount)
                TrySpawnObject(group);
        }
    }

    private void TrySpawnObject(SpawnGroup group)
    {
        SpawnPoint freePoint = null;

        foreach (var point in group.spawnPoints)
        {
            if (point.isOccupied == false)
            {
                freePoint = point;
                break;
            }
        }

        if (freePoint == null) 
            return;

        GameObject spawnedObj = Instantiate(group.prefab, freePoint.point.position, freePoint.point.rotation);
        group.spawnedObjects.Add(spawnedObj);
        freePoint.isOccupied = true;
        _spawnPointMap.Add(spawnedObj, freePoint);

        var spawnable = spawnedObj.GetComponent<ISpawnable>();

        if (spawnable != null)
        {
            Action<ISpawnable> despawnHandler = (spawnableObj) => HandleDespawn(spawnableObj, group, spawnedObj);
            spawnable.OnDespawn += despawnHandler;

            _despawnHandlers.Add(spawnedObj, () => spawnable.OnDespawn -= despawnHandler);
        }
    }

    private void HandleDespawn(ISpawnable spawnable, SpawnGroup group, GameObject spawnedObj)
    {
        if (_spawnPointMap.TryGetValue(spawnedObj, out SpawnPoint point))
        {
            point.isOccupied = false;
            _spawnPointMap.Remove(spawnedObj);
        }

        group.spawnedObjects.Remove(spawnedObj);

        if (_despawnHandlers.TryGetValue(spawnedObj, out System.Action unsubscribeAction))
        {
            unsubscribeAction();
            _despawnHandlers.Remove(spawnedObj);
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

                if (point.isOccupied == false)
                    TrySpawnObject(group);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var handler in _despawnHandlers.Values)
            handler();
  
        _despawnHandlers.Clear();
        _spawnPointMap.Clear();
    }
}
