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
            _waitIntervals[group] = new WaitForSeconds(group.SpawnInterval);
            StartCoroutine(SpawnRoutine(group));
        }
    }

    private IEnumerator SpawnRoutine(SpawnGroup group)
    {
        while (true)
        {
            yield return _waitIntervals[group];

            if (group.SpawnedObjects.Count < group.MaxCount)
                TrySpawnObject(group);
        }
    }

    private bool TrySpawnObject(SpawnGroup group)
    {
        SpawnPoint freePoint = FindFreeSpawnPoint(group.SpawnPoints);

        if (freePoint == null) 
            return false;

        GameObject spawnedObj = Instantiate(group.Prefab, freePoint.Point.position, freePoint.Point.rotation);
        ISpawnable spawnableComponent = spawnedObj.GetComponent<ISpawnable>();

        if (spawnableComponent == null)
        {
            Destroy(spawnedObj);
            return false;
        }

        group.SpawnedObjects.Add(spawnableComponent);
        freePoint.IsOccupied = true;
        _spawnPointMap.Add(spawnableComponent, freePoint);

        RegisterSpawnableObject(spawnableComponent, group);
        return true;
    }

    private SpawnPoint FindFreeSpawnPoint(SpawnPoint[] spawnPoints)
    {
        foreach (var point in spawnPoints)
            if (point.IsOccupied == false)
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
            point.IsOccupied = false;
            _spawnPointMap.Remove(spawnable);
        }

        group.SpawnedObjects.Remove(spawnable);

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

        if (pointIndex < 0 || pointIndex >= group.SpawnPoints.Length)
            return false;

        var point = group.SpawnPoints[pointIndex];

        if (point.IsOccupied)
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