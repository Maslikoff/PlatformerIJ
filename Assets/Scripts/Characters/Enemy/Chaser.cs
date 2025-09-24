using System;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private float _chaseStopDistance = 2f;
    [SerializeField] private float _giveUpDistance = 10f;

    private TargetDetector _playerDetector;

    public bool IsChasing { get; private set; }
    public float Direction { get; private set; }
    public Vector2 TargetPosition { get; private set; }

    public Action<bool> ChaseStateChanged;
    public Action<float> DirectionChanged;
    public Action<float> SpeedChanged;

    private void Awake()
    {
        _playerDetector = GetComponent<TargetDetector>();
    }

    public void UpdateChase()
    {
        _playerDetector.DetectTargets();

        if (_playerDetector.HasTargets)
            HandleChase();
        else if (IsChasing)
            StopChase();
    }

    private void HandleChase()
    {
        IsChasing = true;
        float distanceToPlayer = _playerDetector.DistanceToClosestTarget;

        if (distanceToPlayer > _giveUpDistance)
        {
            StopChase();
            return;
        }

        if (distanceToPlayer <= _chaseStopDistance)
        {
            Direction = 0f;
            TargetPosition = transform.position;
            DirectionChanged?.Invoke(Direction);
            SpeedChanged?.Invoke(0f);
        }
        else
        {
            Vector2 directionToPlayer = (_playerDetector.ClosestTarget.position - transform.position).normalized;
            Direction = Mathf.Sign(directionToPlayer.x);
            TargetPosition = _playerDetector.ClosestTarget.position;

            DirectionChanged?.Invoke(Direction);
            SpeedChanged?.Invoke(_chaseSpeed);
        }

        ChaseStateChanged?.Invoke(true);
    }

    private void StopChase()
    {
        IsChasing = false;
        Direction = 0f;
        TargetPosition = transform.position;

        SpeedChanged?.Invoke(-1f);
        ChaseStateChanged?.Invoke(false);
        DirectionChanged?.Invoke(0f);
    }
}