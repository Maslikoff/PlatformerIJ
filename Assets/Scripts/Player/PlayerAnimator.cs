using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovment _movement;
    private PlayerJump _jump;

    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovment>();
        _jump = GetComponent<PlayerJump>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        _animator.SetBool(IsRun, _movement.IsMoving);

        if (_jump.IsJumping && !_animator.GetBool(IsJump))
            _animator.SetTrigger(IsJump);

        _animator.SetBool(IsGrounded, _jump.IsGrounded());

        if (_jump.IsGrounded() && _animator.GetBool(IsJump))
            _animator.ResetTrigger(IsJump);
    }
}
