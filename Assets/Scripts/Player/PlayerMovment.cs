using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidbody;
    private float _direction;

    public float CurrentSpeed => _rigidbody.velocity.x;
    public bool IsMoving => Mathf.Abs(_direction) > 0.1f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction * _moveSpeed, _rigidbody.velocity.y);
    }

    public void SetDirection(float newDirection)
    {
        _direction = newDirection;

        if (_direction != 0)
            _spriteRenderer.flipX = _direction < 0;
    }
}
