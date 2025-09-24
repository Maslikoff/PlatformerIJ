using UnityEngine;

public abstract class HealthDisplay : MonoBehaviour
{
    [SerializeField] protected Health Health;

    protected virtual void Start()
    {
        SubscribeToHealthEvents();
        InitializeDisplay();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeFromHealthEvents();
    }

    protected virtual void SubscribeToHealthEvents()
    {
        Health.Changed += OnHealthChanged;
    }

    protected virtual void UnsubscribeFromHealthEvents()
    {
        Health.Changed -= OnHealthChanged;
    }

    protected abstract void OnHealthChanged(float currentHealth);
    protected abstract void InitializeDisplay();
}