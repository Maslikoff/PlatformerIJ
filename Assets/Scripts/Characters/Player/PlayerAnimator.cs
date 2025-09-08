using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    private Animator _animator;
    private Mover _mover;
    private Jumper _jumper;
    private GroundDetector _groundDetector;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Mover>();
        _jumper = GetComponent<Jumper>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        _animator.SetBool(IsRun, _mover.IsMoving);
        _animator.SetBool(IsGrounded, _groundDetector.IsGrounded);

        if (_jumper.IsJumping)
            _animator.SetTrigger(IsJump);
    }
}
