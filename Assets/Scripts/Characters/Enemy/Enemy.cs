using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Flipper))]
[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(EnemyAnimator))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;
    private Flipper _flipper;
    private Patroller _patroller;
    private EnemyAnimator _enemyAnimator;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _flipper = GetComponent<Flipper>();
        _patroller = GetComponent<Patroller>();
        _enemyAnimator = GetComponent<EnemyAnimator>();
    }

    private void Update()
    {
        if (_patroller.HasPoints == false) 
            return;

        _patroller.UpdatePatrol();
        HandleMovement();
        UpdateAnimations();
    }

    private void HandleMovement()
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

    private void UpdateAnimations()
    {
        bool isMoving = !_patroller.IsWaiting && _patroller.HasPoints;
        _enemyAnimator.UpdateAnimations(isMoving);
    }
}