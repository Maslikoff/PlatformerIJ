using UnityEngine;

[RequireComponent(typeof(InputReader), typeof(Mover), typeof(Jumper))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(Flipper))]
public class Player : MonoBehaviour
{
    private InputReader _inputReader;
    private Mover _mover;
    private Jumper _jumper;
    private Flipper _flipper;
    private PlayerAnimator _playerAnimator;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _mover = GetComponent<Mover>();
        _jumper = GetComponent<Jumper>();
        _flipper = GetComponent<Flipper>();
        _playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void OnEnable()
    {
        _inputReader.OnMoveInput += HandleMoveInput;
        _inputReader.OnJumpInput += HandleJumpInput;
    }

    private void OnDisable()
    {
        _inputReader.OnMoveInput -= HandleMoveInput;
        _inputReader.OnJumpInput -= HandleJumpInput;
    }

    private void FixedUpdate()
    {
        _mover.Move();
        _jumper.CheckGroundedStatus();
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
}