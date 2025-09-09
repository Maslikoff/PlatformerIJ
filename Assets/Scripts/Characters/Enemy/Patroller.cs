using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _waitTimeAtPoint = 2f;
    [SerializeField] private float _reachThreshold = 0.1f;
    [SerializeField] private bool _loopPatrol = true;

    private int _currentPointIndex = 0;
    private bool _isWaiting = false;
    private bool _isMovingForward = true;
    private float _direction;

    public float Direction => _direction;
    public bool IsWaiting => _isWaiting;
    public bool HasPoints => _patrolPoints != null && _patrolPoints.Length > 0;
    public Transform CurrentTarget => HasPoints ? _patrolPoints[_currentPointIndex] : null;

    private void Start()
    {
        if (HasPoints == false)
            enabled = false;
        else
        {
            foreach (var point in _patrolPoints)
            {
                if (point == null)
                {
                    Debug.LogError("One of patrol points is null!", this);
                    enabled = false;
                    return;
                }
            }
        }
    }

    public void UpdatePatrol()
    {
        if (_isWaiting || HasPoints == false) 
            return;

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = _patrolPoints[_currentPointIndex].position;

        float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        if (distanceToTarget <= _reachThreshold)
        {
            StartCoroutine(WaitAtPoint());
            return;
        }

        _direction = Mathf.Sign(targetPosition.x - currentPosition.x);
    }

    private IEnumerator WaitAtPoint()
    {
        _isWaiting = true;
        _direction = 0f;

        yield return new WaitForSeconds(_waitTimeAtPoint);

        MoveToNextPoint();
        _isWaiting = false;
    }

    private void MoveToNextPoint()
    {
        if (_loopPatrol)
        {
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
        }
        else
        {
            // Движение вперед-назад
            if (_isMovingForward)
            {
                _currentPointIndex++;
                if (_currentPointIndex >= _patrolPoints.Length - 1)
                    _isMovingForward = false;
            }
            else
            {
                _currentPointIndex--;
                if (_currentPointIndex <= 0)
                    _isMovingForward = true;
            }
        }
    }
}