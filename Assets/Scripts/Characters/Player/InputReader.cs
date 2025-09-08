using UnityEngine;
using UnityEngine.Events;

public class InputReader : MonoBehaviour
{
    public event UnityAction<float> OnMoveInput;
    public event UnityAction OnJumpInput;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        OnMoveInput?.Invoke(horizontal);

        if (Input.GetButtonDown("Jump"))
            OnJumpInput?.Invoke();
    }
}
