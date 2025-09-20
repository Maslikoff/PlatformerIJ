using UnityEngine;

public abstract class ButtonHealth : MonoBehaviour
{
    [SerializeField] protected Health Health;
    [SerializeField] protected int Amount = 10;

    public abstract void Execute();
}