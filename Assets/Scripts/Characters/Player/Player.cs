using UnityEngine;

[RequireComponent(typeof(InputReader), typeof(Mover), typeof(Jumper))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(Flipper))]
public class Player : MonoBehaviour
{
    private InputReader _inputReader;
    private Attacker _attacker;
    private Mover _mover;
    private Jumper _jumper;
    private Flipper _flipper;
    private PlayerAnimator _playerAnimator;
    private GroundDetector _groundDetector;

    public bool IsMoving => _mover.IsMoving;
    public bool IsJumping => _jumper.IsJumping;
    public bool IsGrounded => _groundDetector.IsGrounded;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _mover = GetComponent<Mover>();
        _jumper = GetComponent<Jumper>();
        _flipper = GetComponent<Flipper>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void OnEnable()
    {
        _inputReader.MoveInput += HandleMoveInput;
        _inputReader.JumpInput += HandleJumpInput;
        _inputReader.AttackInput += HandleAttackInput;
    }

    private void OnDisable()
    {
        _inputReader.MoveInput -= HandleMoveInput;
        _inputReader.JumpInput -= HandleJumpInput;
        _inputReader.AttackInput -= HandleAttackInput;
    }

    private void FixedUpdate()
    {
        _mover.Move();
        _jumper.CheckGroundedStatus();
        _playerAnimator.UpdateAnimations(IsMoving, IsJumping, IsGrounded);
    }

    private void HandleMoveInput(float direction)
    {
        _mover.SetDirection(direction);
        _flipper.UpdateFacingDirection(direction);
    }

    private void HandleJumpInput()
    {
        _jumper.TryJump();
    }

    private void HandleAttackInput()
    {
        //_attacker.Attack()
    }
}