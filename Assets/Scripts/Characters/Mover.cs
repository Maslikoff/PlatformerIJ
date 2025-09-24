using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private const float MinSpeed = 0.1f;

    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rigidbody;
    private float _direction;

    public float CurrentSpeed => _rigidbody.velocity.x;
    public bool IsMoving => Mathf.Abs(_direction) > MinSpeed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public float GetMoveSpeed() => _moveSpeed;

    public void SetDirection(float direction)
    {
        _direction = Mathf.Clamp(direction, -1f, 1f);
    }

    public void SetMoveSpeed(float speed)
    {
        _moveSpeed = Mathf.Max(0f, speed);
    }

    public void Move()
    {
        _rigidbody.velocity = new Vector2(_direction * _moveSpeed, _rigidbody.velocity.y);
    }

    public void Stop()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _direction = 0;
    }
}