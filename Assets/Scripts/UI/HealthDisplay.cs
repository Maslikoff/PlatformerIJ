using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthDisplay : MonoBehaviour
{
    [SerializeField] protected Health _health;

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
        _health.HealthChanged += OnHealthChanged;
    }

    protected virtual void UnsubscribeFromHealthEvents()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    protected abstract void OnHealthChanged(int currentHealth);
    protected abstract void InitializeDisplay();
}
