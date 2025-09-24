using System;
using UnityEngine;
using UnityEngine.Events;

public class InputReader : MonoBehaviour
{
    public event Action<float> MoveInput;
    public event Action JumpInput;
    public event Action AttackInput;
    public event Action VampireAbilityInput;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw(InputConstants.HorizontalAxis);
        MoveInput?.Invoke(horizontal);

        if (Input.GetButtonDown(InputConstants.JumpButton))
            JumpInput?.Invoke();

       /* if (Input.GetButtonDown(InputConstants.AttackButton))
            AttackInput?.Invoke();*/

        if (Input.GetMouseButtonDown(0))
            VampireAbilityInput?.Invoke();
    }
}
