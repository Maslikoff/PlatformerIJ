using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundedLeeway = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private float _lastGroundedTime;
    private bool _isJumping;

    public bool IsJumping { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        CheckGroundedStatus();
    }

    public void TryJump()
    {
        bool canJump = Time.time - _lastGroundedTime <= _groundedLeeway || IsGrounded();

        if (canJump && _isJumping == false)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _isJumping = true;
            IsJumping = true;
            _lastGroundedTime = 0;
        }
    }

    public bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(_collider.bounds.size.x * 0.9f, 0.1f);
        Vector2 boxCenter = _collider.bounds.center - new Vector3(0, _collider.bounds.extents.y);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0, _groundLayer);
    }

    private void CheckGroundedStatus()
    {
        bool wasGrounded = IsGrounded();

        if (wasGrounded)
        {
            _lastGroundedTime = Time.time;
            IsJumping = false;
            _isJumping = false;
        }
    }
}
