using UnityEngine;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Attacker))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Jumper))]
[RequireComponent(typeof(Flipper))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(VampireAbility))]
public class Player : MonoBehaviour
{
    private InputReader _inputReader;
    private Attacker _attacker;
    private Mover _mover;
    private Jumper _jumper;
    private Flipper _flipper;
    private PlayerAnimator _playerAnimator;
    private GroundDetector _groundDetector;
    private VampireAbility _vampireAbility;

    public bool IsMoving => _mover.IsMoving;
    public bool IsJumping => _jumper.IsJumping;
    public bool IsGrounded => _groundDetector.IsGrounded;
    public bool IsAttacking { get; private set; }
    public bool IsUsingVampireAbility => _vampireAbility.IsAbilityActive;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _attacker = GetComponent<Attacker>();
        _mover = GetComponent<Mover>();
        _jumper = GetComponent<Jumper>();
        _flipper = GetComponent<Flipper>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _groundDetector = GetComponent<GroundDetector>();
        _vampireAbility = GetComponent<VampireAbility>();
    }

    private void OnEnable()
    {
        _inputReader.MoveInput += HandleMoveInput;
        _inputReader.JumpInput += HandleJumpInput;
        _inputReader.AttackInput += HandleAttackInput;
        _inputReader.VampireAbilityInput += HandleVampireAbilityInput;
        _attacker.OnAttack += HandleAttackInput;
    }

    private void OnDisable()
    {
        _inputReader.MoveInput -= HandleMoveInput;
        _inputReader.JumpInput -= HandleJumpInput;
        _inputReader.AttackInput -= HandleAttackInput;
        _inputReader.VampireAbilityInput -= HandleVampireAbilityInput;
        _attacker.OnAttack -= HandleAttackInput;
    }

    private void FixedUpdate()
    {
        _mover.Move();
    }

    private void HandleMoveInput(float direction)
    {
        _mover.SetDirection(direction);
        _flipper.UpdateFacingDirection(direction);
        UpdateAnimation();
    }

    private void HandleJumpInput()
    {
        _jumper.CheckGroundedStatus();
        _jumper.TryJump();
        UpdateAnimation();
    }

    private void HandleAttackInput()
    {
        if (IsUsingVampireAbility) 
            return;

        IsAttacking = true;
        _playerAnimator.PlayAttackAnimation();
        UpdateAnimation();
    }

    private void HandleVampireAbilityInput()
    {
        if (IsAttacking || IsUsingVampireAbility) 
            return;

        _vampireAbility.TryActivateAbility();

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        _playerAnimator.UpdateAnimations(IsMoving, IsJumping, IsGrounded, IsAttacking);
    }
}