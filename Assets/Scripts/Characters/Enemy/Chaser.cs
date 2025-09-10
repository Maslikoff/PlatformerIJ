using System;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private float _chaseStopDistance = 2f;
    [SerializeField] private float _giveUpDistance = 10f;

    private PlayerDetector _playerDetector;
    public bool IsChasing { get; private set; }
    public float Direction { get; private set; }
    public Vector2 TargetPosition { get; private set; }

    public Action<bool> ChaseStateChanged;
    public Action<float> DirectionChanged;
    public Action<float> SpeedChanged;

    private void Awake()
    {
        _playerDetector = GetComponent<PlayerDetector>();
    }

    public void UpdateChase()
    {
        if (_playerDetector.IsPlayerDetected)
            HandleChase();
        else if (IsChasing)
            StopChase();
    }

    private void HandleChase()
    {
        IsChasing = true;
        float distanceToPlayer = _playerDetector.DistanceToPlayer;

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
            Vector2 directionToPlayer = (_playerDetector.PlayerPosition - (Vector2)transform.position).normalized;
            Direction = Mathf.Sign(directionToPlayer.x);
            TargetPosition = _playerDetector.PlayerPosition;

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