using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimations(bool isMoving, bool isJumping, bool isGrounded, bool isAttacking = false)
    {
        _animator.SetBool(IsRun, isMoving);
        _animator.SetBool(IsGrounded, isGrounded);

        if (isJumping)
            _animator.SetTrigger(IsJump);

        _animator.SetBool(IsAttacking, isAttacking);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(IsAttacking);
    }
}
