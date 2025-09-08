using UnityEngine;

[RequireComponent(typeof(GroundDetector))]
public class Jumper : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundedLeeway = 0.1f;

    private Rigidbody2D _rigidbody;
    private GroundDetector _groundDetector;
    private float _lastGroundedTime;
    private bool _isJumping;

    public bool IsJumping => _isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    public void TryJump()
    {
        bool canJump = Time.time - _lastGroundedTime <= _groundedLeeway || _groundDetector.IsGrounded;

        if (canJump && !_isJumping)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _isJumping = true;
            _lastGroundedTime = 0;
        }
    }

    public void CheckGroundedStatus()
    {
        if (_groundDetector.IsGrounded)
        {
            _lastGroundedTime = Time.time;

            if (_isJumping)
                _isJumping = false;
        }
    }
}
