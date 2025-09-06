using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(GroundDetector))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundedLeeway = 0.1f;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private GroundDetector _groundDetector;

    private float _direction;
    private bool _facingRight = true;

    private float _lastGroundedTime;
    private bool _isJumping;
    private bool _wasGrounded;

    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    public float CurrentSpeed => _rigidbody.velocity.x;
    public bool IsMoving => Mathf.Abs(_direction) > 0.1f;
    public bool IsJumping => _isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _groundDetector = GetComponent<GroundDetector>();

        _wasGrounded = _groundDetector.IsGrounded;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        CheckGroundedStatus();
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    private void HandleInput()
    {
        _direction = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            TryJump();
    }

    private void HandleMovement()
    {
        _rigidbody.velocity = new Vector2(_direction * _moveSpeed, _rigidbody.velocity.y);

        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        if (_direction == 0) 
            return;

        bool shouldFaceRight = _direction > 0;

        if (shouldFaceRight != _facingRight)
        {
            _facingRight = shouldFaceRight;
            transform.rotation = Quaternion.Euler(0, _facingRight ? 0 : 180, 0);
        }
    }

    private void TryJump()
    {
        bool canJump = Time.time - _lastGroundedTime <= _groundedLeeway || _groundDetector.IsGrounded;

        if (canJump && !_isJumping)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _isJumping = true;
            _lastGroundedTime = 0;
            OnJumpStarted();
        }
    }

    private void CheckGroundedStatus()
    {
        if (_groundDetector.IsGrounded)
        {
            _lastGroundedTime = Time.time;

            if (_isJumping)
                _isJumping = false;
        }
    }

    private void UpdateAnimations()
    {
        _animator.SetBool(IsRun, IsMoving);
        _animator.SetBool(IsGrounded, _groundDetector.IsGrounded);
    }

    private void OnJumpStarted()
    {
        _animator.SetTrigger(IsJump);
    }
}