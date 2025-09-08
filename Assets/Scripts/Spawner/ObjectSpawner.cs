using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private SpawnGroup[] _spawnGroups;

    private Dictionary<ISpawnable, SpawnPoint> _spawnPointMap = new Dictionary<ISpawnable, SpawnPoint>();
    private Dictionary<ISpawnable, Action> _despawnHandlers = new Dictionary<ISpawnable, Action>();
    private Dictionary<SpawnGroup, WaitForSeconds> _waitIntervals = new Dictionary<SpawnGroup, WaitForSeconds>();

    private void Start()
    {
        foreach (var group in _spawnGroups)
        {
            _waitIntervals[group] = new WaitForSeconds(group.spawnInterval);
            StartCoroutine(SpawnRoutine(group));
        }
    }

    private IEnumerator SpawnRoutine(SpawnGroup group)
    {
        while (true)
        {
            yield return _waitIntervals[group];

            if (group.spawnedObjects.Count < group.maxCount)
                TrySpawnObject(group);
        }
    }

    private bool TrySpawnObject(SpawnGroup group)
    {
        SpawnPoint freePoint = FindFreeSpawnPoint(group.spawnPoints);

        if (freePoint == null) 
            return false;

        GameObject spawnedObj = Instantiate(group.prefab, freePoint.point.position, freePoint.point.rotation);
        ISpawnable spawnableComponent = spawnedObj.GetComponent<ISpawnable>();

        if (spawnableComponent == null)
        {
            Destroy(spawnedObj);
            return false;
        }

        group.spawnedObjects.Add(spawnableComponent);
        freePoint.isOccupied = true;
        _spawnPointMap.Add(spawnableComponent, freePoint);

        RegisterSpawnableObject(spawnableComponent, group);
        return true;
    }

    private SpawnPoint FindFreeSpawnPoint(SpawnPoint[] spawnPoints)
    {
        foreach (var point in spawnPoints)
            if (point.isOccupied == false)
                return point;

        return null;
    }

    private void RegisterSpawnableObject(ISpawnable spawnable, SpawnGroup group)
    {
        Action<ISpawnable> despawnHandler = (spawnableObj) => HandleDespawn(spawnableObj, group);
        spawnable.Spawned += despawnHandler;

        _despawnHandlers.Add(spawnable, () => spawnable.Spawned -= despawnHandler);
    }

    private void HandleDespawn(ISpawnable spawnable, SpawnGroup group)
    {
        if (_spawnPointMap.TryGetValue(spawnable, out SpawnPoint point))
        {
            point.isOccupied = false;
            _spawnPointMap.Remove(spawnable);
        }

        group.spawnedObjects.Remove(spawnable);

        if (_despawnHandlers.TryGetValue(spawnable, out Action unsubscribeAction))
        {
            unsubscribeAction();
            _despawnHandlers.Remove(spawnable);
        }
    }

    public bool SpawnObjectManually(int groupIndex, int pointIndex)
    {
        if (groupIndex < 0 || groupIndex >= _spawnGroups.Length)
            return false;

        var group = _spawnGroups[groupIndex];

        if (pointIndex < 0 || pointIndex >= group.spawnPoints.Length)
            return false;

        var point = group.spawnPoints[pointIndex];

        if (point.isOccupied)
            return false;

        return TrySpawnObject(group);
    }

    private void OnDestroy()
    {
        foreach (var handler in _despawnHandlers.Values)
            handler?.Invoke();

        _despawnHandlers.Clear();
        _spawnPointMap.Clear();
    }
}