using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] private float _directionThreshold = 0.1f;

    private float _lastNonZeroDirection = 1f;
    private bool _facingRight = true;

    public void UpdateFacingDirection(float direction)
    {
        if (Mathf.Abs(direction) > _directionThreshold)
            _lastNonZeroDirection = direction;

        bool shouldFaceRight = _lastNonZeroDirection > 0;

        if (shouldFaceRight != _facingRight)
        {
            _facingRight = shouldFaceRight;
            transform.rotation = Quaternion.Euler(0, _facingRight ? 0 : 180, 0);
        }
    }
}