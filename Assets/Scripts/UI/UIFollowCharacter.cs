using UnityEngine;

public class UIFollowCharacter : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}