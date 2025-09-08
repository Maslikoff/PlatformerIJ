using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimations(bool isMoving, bool isJumping, bool isGrounded)
    {
        _animator.SetBool(IsRun, isMoving);
        _animator.SetBool(IsGrounded, isGrounded);

        if (isJumping)
            _animator.SetTrigger(IsJump);
    }
}
