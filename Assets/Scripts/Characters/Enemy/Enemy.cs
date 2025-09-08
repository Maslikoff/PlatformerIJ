using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mover), typeof(Flipper))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private float _reachThreshold = 0.1f;

    private int _currentWaypointIndex = 0;
    private Mover _mover;
    private Flipper _flipper;
    private Coroutine _movementCoroutine;
    private bool _isMoving;
    private Vector2 _currentDirection;
    private WaitForSeconds _waitForSeconds;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _flipper = GetComponent<Flipper>();


        _waitForSeconds = new WaitForSeconds(_waitTime);
    }

    private void Start()
    {
        if (_waypoints != null && _waypoints.Length > 0)
        {
            transform.position = _waypoints[0].position;
            StartMovement();
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            _mover.SetDirection(_currentDirection.x);
            _mover.Move();
        }
    }

    private void OnEnable()
    {
        if (_waypoints != null && _waypoints.Length > 0 && !_isMoving)
            StartMovement();
    }

    private void OnDisable()
    {
        StopMovement();
    }

    public void StartMovement()
    {
        if (_movementCoroutine != null)
            StopCoroutine(_movementCoroutine);

        _movementCoroutine = StartCoroutine(MovementRoutine());
    }

    public void StopMovement()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }

        _isMoving = false;
        _mover.Stop();
        _currentDirection = Vector2.zero;
    }

    private IEnumerator MovementRoutine()
    {
        if (_waypoints == null || _waypoints.Length == 0)
            yield break;

        _isMoving = true;
        _currentWaypointIndex = 0;

        while (_isMoving)
        {
            Vector2 targetPosition = _waypoints[_currentWaypointIndex].position;

            while (Vector2.Distance(transform.position, targetPosition) > _reachThreshold && _isMoving)
            {
                _currentDirection = (targetPosition - (Vector2)transform.position).normalized;
                _flipper.UpdateFacingDirection(_currentDirection);
                yield return null;
            }

            if (_isMoving == false) 
                yield break;

            _currentDirection = Vector2.zero;
            yield return _waitForSeconds;

            if (_isMoving == false)
                yield break;

            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        }
    }
}