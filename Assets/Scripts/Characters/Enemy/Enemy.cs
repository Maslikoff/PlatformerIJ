using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Flipper))]
[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(PlayerDetector))]
[RequireComponent(typeof(Chaser))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;
    private Flipper _flipper;
    private Patroller _patroller;
    private EnemyAnimator _enemyAnimator;
    private PlayerDetector _playerDetector;
    private Chaser _chaser;

    private float _originalMoveSpeed;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _flipper = GetComponent<Flipper>();
        _patroller = GetComponent<Patroller>();
        _enemyAnimator = GetComponent<EnemyAnimator>();
        _playerDetector = GetComponent<PlayerDetector>();
        _chaser = GetComponent<Chaser>();

        _originalMoveSpeed = _mover.GetMoveSpeed();
    }

    private void OnEnable()
    {
        _chaser.ChaseStateChanged += HandleChaseStateChanged;
        _chaser.DirectionChanged += HandleDirectionChanged;
        _chaser.SpeedChanged += HandleSpeedChanged;
    }

    private void OnDisable()
    {
        _chaser.ChaseStateChanged -= HandleChaseStateChanged;
        _chaser.DirectionChanged -= HandleDirectionChanged;
        _chaser.SpeedChanged -= HandleSpeedChanged;
    }

    private void Update()
    {
        if (_patroller.HasPoints == false)
            return;

        _playerDetector.DetectPlayer();

        _chaser.UpdateChase();

        if (_chaser.IsChasing == false)
        {
            _patroller.UpdatePatrol();
            HandlePatrolMovement();
        }

        UpdateAnimations();
    }

    private void HandlePatrolMovement()
    {
        if (_patroller.IsWaiting)
        {
            _mover.Stop();
        }
        else
        {
            _mover.SetDirection(_patroller.Direction);
            _mover.Move();
            _flipper.UpdateFacingDirection(_patroller.Direction);
        }
    }

    private void HandleChaseStateChanged(bool isChasing)
    {
        if (isChasing == false)
            _mover.SetMoveSpeed(_originalMoveSpeed);
    }

    private void HandleDirectionChanged(float direction)
    {
        if (_chaser.IsChasing)
        {
            _mover.SetDirection(direction);
            _mover.Move();
            _flipper.UpdateFacingDirection(direction);
        }
    }

    private void HandleSpeedChanged(float speed)
    {
        if (speed < 0)
            _mover.SetMoveSpeed(_originalMoveSpeed);
        else if (_chaser.IsChasing)
            _mover.SetMoveSpeed(speed);
    }

    private void UpdateAnimations()
    {
        bool isMoving = !_patroller.IsWaiting && _patroller.HasPoints;
        _enemyAnimator.UpdateAnimations(isMoving);
    }
}