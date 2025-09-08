using System;
using UnityEngine;
using UnityEngine.Events;

public class InputReader : MonoBehaviour
{
    public event Action<float> MoveInput;
    public event Action JumpInput;
    public event Action AttackInput;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw(InputConstants.HORIZONTAL_AXIS);
        MoveInput?.Invoke(horizontal);

        if (Input.GetButtonDown(InputConstants.JUMP_BUTTON))
            JumpInput?.Invoke();

        if (Input.GetButtonDown(InputConstants.ATTACK_BUTTON))
            AttackInput?.Invoke();
    }
}
