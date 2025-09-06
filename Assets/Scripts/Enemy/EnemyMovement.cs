using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private float _reachThreshold = 0.1f;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementDirection;
    private int _currentWaypointIndex = 0;
    private bool _isWaiting = false;

    public Vector2 MovementDirection => _movementDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (_waypoints.Length > 0)
            transform.position = _waypoints[0].position;
    }

    private void Update()
    {
        if (_isWaiting || _waypoints.Length == 0) 
            return;

        MoveToWaypoint();
        CheckWaypointReached();
    }

    private void MoveToWaypoint()
    {
        Vector2 targetPosition = _waypoints[_currentWaypointIndex].position;
        _movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        _rigidbody.velocity = _movementDirection * _moveSpeed;
    }

    private void CheckWaypointReached()
    {
        if (Vector2.Distance(transform.position, _waypoints[_currentWaypointIndex].position) <= _reachThreshold)
            StartCoroutine(WaitAtWaypoint());
    }

    private IEnumerator WaitAtWaypoint()
    {
        _isWaiting = true;
        _rigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(_waitTime);

        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        _isWaiting = false;
    }
}
