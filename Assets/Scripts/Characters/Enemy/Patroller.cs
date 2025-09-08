using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller
{
    private Transform[] _waypoints;
    private Mover _mover;
    private Flipper _flipper;
    private Coroutine _patrolCoroutine;

    private float _waitTime = 2f;
    private float _reachThreshold = 0.1f;

    public bool IsPatrolling { get; private set; }
    public Vector2 CurrentDirection { get; private set; }
    public float WaitTime => _waitTime;

    public void Initialize(Transform[] waypoints, Mover mover, Flipper flipper)
    {
        SetWaypoints(waypoints);
        _mover = mover;
        _flipper = flipper;
    }

    public void StartPatrol()
    {
        if (_waypoints == null || _waypoints.Length == 0)
            return;

        if (_patrolCoroutine != null)
            StopPatrol();

        _patrolCoroutine = _mover.StartCoroutine(PatrolRoutine());
    }

    public void StopPatrol()
    {
        if (_patrolCoroutine != null)
        {
            _mover.StopCoroutine(_patrolCoroutine);
            _patrolCoroutine = null;
        }

        IsPatrolling = false;
        _mover.Stop();
        CurrentDirection = Vector2.zero;
    }

    private IEnumerator PatrolRoutine()
    {
        IsPatrolling = true;
        int currentWaypointIndex = 0;

        while (IsPatrolling && _waypoints != null && _waypoints.Length > 0)
        {
            Vector2 targetPosition = _waypoints[currentWaypointIndex].position;

            while (Vector2.Distance(_mover.transform.position, targetPosition) > _reachThreshold && IsPatrolling)
            {
                CurrentDirection = (targetPosition - (Vector2)_mover.transform.position).normalized;
                _mover.SetDirection(CurrentDirection.x);
                _mover.Move();
                _flipper.UpdateFacingDirection(CurrentDirection.x);
                yield return null;
            }

            if (IsPatrolling == false)
                yield break;

            _mover.Stop();

            yield return new WaitForSeconds(_waitTime);

            if (IsPatrolling == false)
                yield break;

            currentWaypointIndex = (currentWaypointIndex + 1) % _waypoints.Length;
        }
    }

    public void SetWaitTime(float waitTime)
    {
        _waitTime = Mathf.Max(0f, waitTime);
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        if (waypoints != null && waypoints.Length > 0)
            _waypoints = waypoints;
        else
            _waypoints = null;
    }

    public bool HasWaypoints()
    {
        return _waypoints != null && _waypoints.Length > 0;
    }
}