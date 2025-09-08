using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckExtraWidth = 0.9f;

    private Collider2D _collider;
    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        Vector2 boxSize = new Vector2(_collider.bounds.size.x * _groundCheckExtraWidth, 0.1f);
        Vector2 boxCenter = _collider.bounds.center - new Vector3(0, _collider.bounds.extents.y);

        _isGrounded = Physics2D.OverlapBox(boxCenter, boxSize, 0, _groundLayer);
    }
}
