using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mover), typeof(Flipper), typeof(Chaser))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _waitTimeAtPoint = 2f;

    private Mover _mover;
    private Flipper _flipper;
    private Patroller _patroller;

    public float WaitTimeAtPoint
    {
        get => _waitTimeAtPoint;
        set
        {
            _waitTimeAtPoint = value;

            if (_patroller != null)
                _patroller.SetWaitTime(_waitTimeAtPoint);
        }
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _flipper = GetComponent<Flipper>();
        _patroller = new Patroller();
    }

    private void Start()
    {
        InitializePatroller();
    }

    private void InitializePatroller()
    {
        if (_waypoints != null && _waypoints.Length > 0)
        {
            _patroller.Initialize(_waypoints, _mover, _flipper);
            _patroller.SetWaitTime(_waitTimeAtPoint);
            _patroller.StartPatrol();
        }
    }

    private void OnEnable()
    {
        if (_patroller != null && _patroller.HasWaypoints())
        {
            _patroller.StartPatrol();
        }
    }

    private void OnDisable()
    {
        if (_patroller != null)
            _patroller.StopPatrol();
    }

    private void OnDestroy()
    {
        if (_patroller != null)
            _patroller.StopPatrol();
    }
}